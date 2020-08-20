using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using EasyAbp.Abp.EventBus.Cap;
using EasyAbp.Abp.EventBus.CAP.PostgreSql;

namespace App2
{
    [DependsOn(
        typeof(AbpEventBusCapModule),
        typeof(AbpEventBusCapPostgreSqlModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreSerilogModule))]
    public class App2Module : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddAbpDbContext<AppDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also VoucherManagementMigrationsDbContextFactory for EF Core tooling. */
                options.UseNpgsql();
            });

            context.AddCapEventBus(capOptions =>
            {
                capOptions.UseInMemoryStorage();
                capOptions.UseRabbitMQ("localhost");//UseRabbitMQ 服务器地址配置，支持配置IP地址和密码
                capOptions.UseDashboard();//CAP2.X版本以后官方提供了Dashboard页面访问。
            });

            ConfigureSwaggerServices(context, configuration);
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {

            context.Services.AddSwaggerGen(
               options =>
               {
                   options.SwaggerDoc("v1", new OpenApiInfo
                   {
                       Version = "v1",
                       Title = "app2",
                       Description = "app2服务api",

                   });
                   options.DocInclusionPredicate((docName, description) => true);
               });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAbpSerilogEnrichers();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "App2 API");
            });
            app.UseConfiguredEndpoints();
        }
    }
}