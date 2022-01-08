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
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using EasyAbp.Abp.EventBus.Cap;

namespace App2
{
    [DependsOn(
        typeof(AbpEventBusCapModule),
        typeof(AbpEventBusCapEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
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
                options.UseSqlServer();
            });

            context.AddCapEventBus(capOptions =>
            {
                // Set this so that the CapDistributedEventBus can recognize the DbContext used by CAP.
                capOptions.SetDbContextForCap<AppDbContext>();
                // If you are using EF, you need to add the configuration：
                // Options, Notice: You don't need to config x.UseSqlServer(""") again! CAP can autodiscovery.
                capOptions.UseEntityFramework<AppDbContext>();
                capOptions.UseRabbitMQ("localhost");
                capOptions.UseDashboard();
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