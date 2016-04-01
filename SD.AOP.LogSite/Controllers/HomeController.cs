using System.Configuration;
using System.Web.Mvc;

namespace SD.AOP.LogSite.Controllers
{
    /// <summary>
    /// 后台首页控制器
    /// </summary>
    public class HomeController : Controller
    {
        #region 01.测试驱动管理首页视图 —— ActionResult Index()
        /// <summary>
        /// 测试驱动管理首页视图
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            this.ViewBag.WebName = ConfigurationManager.AppSettings["WebName"];
            this.ViewBag.TechSupport = ConfigurationManager.AppSettings["TechSupport"];
            return this.View();
        }
        #endregion

        #region 02.加载菜单 —— ActionResult GetMenuList()
        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <returns>菜单树</returns>
        public ActionResult GetMenuList()
        {
            return this.Content(OperationContext.Current.MainMenuTree);
        }
        #endregion
    }
}
