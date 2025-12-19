using App1.Saga;
using EasyAbp.Abp.EventBus.Cap;
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
using Volo.Abp.Modularity;
using WorkflowCore.Interface;

namespace App1
{
    [DependsOn(
        typeof(AbpEventBusCapModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreSerilogModule))]
    public class App1Module : AbpModule
    {
        private IWorkflowHost workflowHost;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
          
            context.AddCapEventBus(capOptions =>
            {
                capOptions.UseInMemoryStorage();
                capOptions.UseRabbitMQ("localhost");//UseRabbitMQ 服务器地址配置，支持配置IP地址和密码
                capOptions.UseDashboard();//CAP2.X版本以后官方提供了Dashboard页面访问。
            });
            ConfigureSwaggerServices(context, configuration);

            ConfigureWorkflowcore(context);
        }

        private static void ConfigureWorkflowcore(ServiceConfigurationContext context)
        {
            //context.Services.AddWorkflow();
            context.Services.AddWorkflow(x => x.UseSqlite(@"Data Source=database.db;", true));
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {

            context.Services.AddSwaggerGen(
               options =>
               {
                   options.SwaggerDoc("v1", new OpenApiInfo
                   {
                       Version = "v1",
                       Title = "app1",
                       Description = "app1服务api",

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
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.MapAbpStaticAssets();
            app.UseAbpSerilogEnrichers();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "App1 API");
            });
            app.UseConfiguredEndpoints();
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            workflowHost = context.ServiceProvider.GetService<IWorkflowHost>();
            workflowHost.RegisterWorkflow<CompensatingWorkflow>();
            workflowHost.Start();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            workflowHost.Stop();
        }
    }
}