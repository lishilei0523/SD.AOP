using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SD.Toolkits.AspNet;
using System;
using System.IO;

namespace SD.AOP.LogViewer
{
    /// <summary>
    /// 应用程序启动器
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        /// <summary>
        /// 配置应用程序
        /// </summary>
        public void Configure(IApplicationBuilder appBuilder)
        {
            //配置路由
            appBuilder.UseStaticFiles();
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //配置静态文件
            string staticFilesRoot = Path.Combine(AppContext.BaseDirectory, AspNetSetting.StaticFilesPath);
            Directory.CreateDirectory(staticFilesRoot);
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesRoot)
            };
            appBuilder.UseStaticFiles(staticFileOptions);
        }
    }
}
