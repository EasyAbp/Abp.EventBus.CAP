This is a repository integrated [CAP](https://github.com/dotnetcore/CAP/) with [ABP](https://github.com/abpframework/abp)

### EventBus

```
 public override void ConfigureServices(ServiceConfigurationContext context)
  {
       var configuration = context.Services.GetConfiguration();
      
       context.AddCapEventBus(capOptions =>
       {
             capOptions.UseInMemoryStorage();
             capOptions.UseRabbitMQ("localhost");//UseRabbitMQ 服务器地址配置，支持配置IP地址和密码
             capOptions.UseDashboard();//CAP2.X版本以后官方提供了Dashboard页面访问。
       });
 }
```