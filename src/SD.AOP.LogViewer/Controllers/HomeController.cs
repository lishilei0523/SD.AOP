using SD.AOP.LogViewer.Entities;
using SD.AOP.LogViewer.Maps;
using SD.AOP.LogViewer.ViewModels.Outputs;
using SD.FormatModel.EasyUI;
using SD.Infrastructure.Attributes;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace SD.AOP.LogViewer.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    public class HomeController : Controller
    {
        #region # 加载主页视图 —— ViewResult Index()
        /// <summary>
        /// 加载主页视图
        /// </summary>
        /// <returns>主页视图</returns>
        [HttpGet]
        public ViewResult Index()
        {
            this.ViewBag.WebName = ConfigurationManager.AppSettings["WebName"];
            this.ViewBag.TechSupport = ConfigurationManager.AppSettings["TechSupport"];

            return base.View();
        }
        #endregion

        #region # 获取菜单树 —— JsonResult GetMenuTree()
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        [RequireAuthorization("获取菜单树")]
        public JsonResult GetMenuTree()
        {
            //初始化菜单
            IList<Menu> menus = new List<Menu>();
            string rootMenuName = $"{ConfigurationManager.AppSettings["WebName"]}日志管理后台";
            Menu rootMenu = new Menu(rootMenuName, int.MinValue, null, null, null);
            Menu exceptionLogMenu = new Menu("异常日志管理", 1, "/ExceptionLog/Index", null, rootMenu);
            Menu runningLogMenu = new Menu("程序日志管理", 2, "/RunningLog/Index", null, rootMenu);
            menus.Add(rootMenu);
            menus.Add(exceptionLogMenu);
            menus.Add(runningLogMenu);

            IEnumerable<MenuView> menuViews = menus.Select(x => x.ToViewModel());
            IEnumerable<Node> menuTree = menuViews.ToTree(null);

            return base.Json(menuTree, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
