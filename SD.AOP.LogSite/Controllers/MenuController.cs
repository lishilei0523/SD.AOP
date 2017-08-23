using SD.AOP.LogSite.Entities;
using SD.AOP.LogSite.Maps;
using SD.AOP.LogSite.Repositories;
using SD.AOP.LogSite.ViewModels.Formats.EasyUI;
using SD.AOP.LogSite.ViewModels.Outputs;
using SD.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace SD.AOP.LogSite.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : Controller
    {
        #region # 字段及构造器

        private readonly DbSession _dbSession;

        /// <summary>
        /// 构造器
        /// </summary>
        public MenuController()
        {
            this._dbSession = new DbSession();
        }

        #endregion


        //视图部分

        #region # 加载首页视图 —— ViewResult Index()
        /// <summary>
        /// 加载首页视图
        /// </summary>
        /// <returns>首页视图</returns>
        [HttpGet]
        [RequireAuthorization("菜单管理首页视图")]
        public ViewResult Index()
        {
            return base.View();
        }
        #endregion

        #region # 加载创建菜单视图 —— ViewResult Add()
        /// <summary>
        /// 加载创建菜单视图
        /// </summary>
        /// <returns>创建菜单视图</returns>
        [HttpGet]
        [RequireAuthorization("创建菜单视图")]
        public ViewResult Add()
        {
            return base.View();
        }
        #endregion

        #region # 加载修改菜单视图 —— ViewResult Update(Guid id)
        /// <summary>
        /// 加载修改菜单视图
        /// </summary>
        /// <param name="id">菜单Id</param>
        /// <returns>修改菜单视图</returns>
        [HttpGet]
        [RequireAuthorization("修改菜单视图")]
        public ViewResult Update(Guid id)
        {
            Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == id);
            MenuView menuView = currentMenu.ToViewModel();

            return base.View(menuView);
        }
        #endregion


        //命令部分

        #region # 创建菜单 —— void CreateMenu(string systemNo, string menuName...
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="icon">图标</param>
        /// <param name="parentId">父级菜单Id</param>
        [HttpPost]
        [RequireAuthorization("创建菜单")]
        public void CreateMenu(string menuName, int sort, string url, string icon, Guid? parentId)
        {
            Menu parentMenu = parentId.HasValue ? this._dbSession.Set<Menu>().Single(x => x.Id == parentId.Value) : null;
            Menu menu = new Menu(menuName, sort, url, icon, parentMenu);

            this._dbSession.Set<Menu>().Add(menu);
            this._dbSession.SaveChanges();

        }
        #endregion

        #region # 修改菜单 —— void UpdateMenu(Guid menuId, string menuName...
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="icon">图标</param>
        [HttpPost]
        [RequireAuthorization("修改菜单")]
        public void UpdateMenu(Guid menuId, string menuName, int sort, string url, string icon)
        {
            Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == menuId);
            currentMenu.UpdateInfo(menuName, sort, url, icon);

            this._dbSession.SaveChanges();
        }
        #endregion

        #region # 删除菜单 —— void RemoveMenu(Guid id)
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单Id</param>
        [HttpPost]
        [RequireAuthorization("删除菜单")]
        public void RemoveMenu(Guid id)
        {
            Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == id);

            this._dbSession.RemoveMenus(currentMenu);
            this._dbSession.SaveChanges();
        }
        #endregion

        #region # 批量删除菜单 —— void RemoveMenus(IEnumerable<Guid> menuIds)
        /// <summary>
        /// 批量删除菜单
        /// </summary>
        /// <param name="menuIds">菜单Id集</param>
        [HttpPost]
        [RequireAuthorization("批量删除菜单")]
        public void RemoveMenus(IEnumerable<Guid> menuIds)
        {
            menuIds = menuIds ?? new Guid[0];

            IQueryable<Menu> menus = this._dbSession.Set<Menu>().Where(x => menuIds.Contains(x.Id));

            foreach (Menu menu in menus)
            {
                this._dbSession.RemoveMenus(menu);
            }

            this._dbSession.SaveChanges();
        }
        #endregion


        //查询部分

        #region # 获取菜单树 —— JsonResult GetMenuTree()
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        [RequireAuthorization("获取菜单树")]
        public JsonResult GetMenuTree()
        {
            using (this._dbSession)
            {
                Menu[] menus = this._dbSession.Set<Menu>().OrderBy(x => x.Sort).ToArray();
                IEnumerable<MenuView> menuViews = menus.Select(x => x.ToViewModel());

                IEnumerable<Node> menuTree = menuViews.ToTree(null);

                return base.Json(menuTree, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region # 获取菜单TreeGrid —— JsonResult GetMenuTreeGrid()
        /// <summary>
        /// 获取菜单TreeGrid
        /// </summary>
        /// <returns>菜单TreeGrid</returns>
        [RequireAuthorization("获取菜单树形表格")]
        public JsonResult GetMenuTreeGrid(string keywords)
        {
            using (this._dbSession)
            {
                keywords = keywords == null ? string.Empty : keywords.Trim();

                Expression<Func<Menu, bool>> condition =
                    x =>
                        (string.IsNullOrEmpty(keywords) || x.Name.Contains(keywords));

                IEnumerable<Menu> menus = this._dbSession.Set<Menu>().Where(condition).OrderBy(x => x.Sort).ToArray();
                IEnumerable<MenuView> menuViews = menus.Select(x => x.ToViewModel());
                menuViews = menuViews.ToTreeGrid();

                Grid<MenuView> grid = new Grid<MenuView>(menus.Count(), menuViews);

                return base.Json(grid, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
