using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SD.Toolkits.AspNet;

namespace SD.AOP.LogViewer
{
    class Program
    {
        static void Main()
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

            //WebHost≈‰÷√
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options =>
                {
                    foreach (int httpPort in AspNetSetting.HttpPorts)
                    {
                        options.ListenAnyIP(httpPort);
                    }
                });
                webBuilder.UseWebRoot(AspNetSetting.StaticFilesPath);
                webBuilder.UseStartup<Startup>();
            });

            //“¿¿µ◊¢»Î≈‰÷√
            ServiceLocator serviceLocator = new ServiceLocator();
            hostBuilder.UseServiceProviderFactory(serviceLocator);

            IHost host = hostBuilder.Build();
            host.Run();
        }
    }
}
