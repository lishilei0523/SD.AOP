using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace SD.AOP.LogSite.Entities.Base
{
    /// <summary>
    /// EF上下文
    /// </summary>
    public class DbSession : DbContext
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        public DbSession()
            : base("name=LogConnection")
        {
            this.Database.CreateIfNotExists();
        }
        #endregion

        #region # 菜单 —— DbSet<Menu> Menus
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }
        #endregion

        #region # 初始化菜单 —— void InitMenu()
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitMenu()
        {
            //不存在则创建菜单节点
            if (!this.Set<Menu>().Any())
            {
                string rootMenuName = string.Format("{0}日志管理后台", ConfigurationManager.AppSettings["WebName"]);

                //初始化菜单
                Menu rootMenu = new Menu(rootMenuName, int.MinValue, null, null, null);
                Menu exceptionLogMenu = new Menu("异常日志管理", 1, "/ExceptionLog/Index", null, rootMenu);
                Menu runningLogMenu = new Menu("程序日志管理", 2, "/RunningLog/Index", null, rootMenu);
                Menu manageMenu = new Menu("菜单管理", 3, "/Menu/Index", null, rootMenu);

                this.Set<Menu>().Add(rootMenu);
                this.Set<Menu>().Add(exceptionLogMenu);
                this.Set<Menu>().Add(runningLogMenu);
                this.Set<Menu>().Add(manageMenu);
                this.SaveChanges();
            }
        }
        #endregion

        #region # 重写SaveChanges方法 —— override int SaveChanges()
        /// <summary>
        /// 重写SaveChanges方法
        /// </summary>
        /// <returns>受影响的行数</returns>
        public override int SaveChanges()
        {
            try
            {
                int count = base.SaveChanges();

                this.Dispose();

                return count;
            }
            catch
            {
                this.Dispose();
                throw;
            }
        }
        #endregion

        #region # 递归删除菜单 —— void RemoveMenus(Menu menu)
        /// <summary>
        /// 递归删除菜单
        /// </summary>
        /// <param name="menu">菜单</param>
        public void RemoveMenus(Menu menu)
        {
            foreach (Menu subMenu in menu.SubNodes)
            {
                this.RemoveMenus(subMenu);
            }

            this.Set<Menu>().Remove(menu);
        }
        #endregion
    }
}