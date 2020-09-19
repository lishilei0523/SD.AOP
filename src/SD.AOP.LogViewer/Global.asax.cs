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
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}