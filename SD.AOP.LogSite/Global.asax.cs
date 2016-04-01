using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SD.AOP.LogSite.Model.Base;

namespace SD.AOP.LogSite
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

            //初始化菜单
            DbSession.Current.InitMenu();
        }
    }
}