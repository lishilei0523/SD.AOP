using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SD.AOP.LogViewer
{
    /// <summary>
    /// 全局应用程序类
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// 注册路由
        /// </summary>
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
