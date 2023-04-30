using Microsoft.AspNetCore.Mvc;
using SD.AOP.LogViewer.Models;
using SD.Infrastructure;
using SD.Infrastructure.Attributes;
using SD.Toolkits.EasyUI;
using System.Collections.Generic;

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
            this.ViewBag.WebName = FrameworkSection.Setting.ApplicationName.Value;
            this.ViewBag.TechSupport = FrameworkSection.Setting.ServiceName.Value;

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
            IList<MenuModel> menus = new List<MenuModel>();
            string rootMenuName = $"{FrameworkSection.Setting.ApplicationName.Value} 日志管理后台";
            MenuModel rootMenu = new MenuModel(rootMenuName, int.MinValue, null, null, true, false, null);
            MenuModel exceptionLogMenu = new MenuModel("异常日志管理", 1, "/ExceptionLog/Index", null, false, true, rootMenu);
            MenuModel runningLogMenu = new MenuModel("运行日志管理", 2, "/RunningLog/Index", null, false, true, rootMenu);
            menus.Add(rootMenu);
            menus.Add(exceptionLogMenu);
            menus.Add(runningLogMenu);

            IEnumerable<Node> menuTree = menus.ToTree(null);

            return base.Json(menuTree);
        }
        #endregion
    }
}
