using SD.AOP.LogViewer.Entities;
using SD.Common.PoweredByLee;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SD.AOP.LogViewer.Repositories
{
    /// <summary>
    /// EF上下文
    /// </summary>
    public class DbSession : DbContext
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public DbSession()
            : base($"name={Common.DefaultConnectionStringName}")
        {
            this.Database.CreateIfNotExists();
        }
        #endregion

        #endregion

        #region # 属性

        #region 菜单 —— DbSet<Menu> Menus
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }
        #endregion 

        #endregion

        #region # 方法

        #region 初始化菜单 —— void InitMenus()
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitMenus()
        {
            #region # 初始化数据表

            string connectionString = ConfigurationManager.ConnectionStrings[Common.DefaultConnectionStringName].ConnectionString;
            SqlHelper sqlHelper = new SqlHelper(connectionString);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("IF OBJECT_ID('[dbo].[Menus]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[Menus]( ");
            sqlBuilder.Append("[Id] [UNIQUEIDENTIFIER] NOT NULL, ");
            sqlBuilder.Append("[Url] [NVARCHAR](MAX) NULL, ");
            sqlBuilder.Append("[Icon] [NVARCHAR](MAX) NULL, ");
            sqlBuilder.Append("[Sort] [INT] NOT NULL, ");
            sqlBuilder.Append("[IsRoot] [BIT] NOT NULL, ");
            sqlBuilder.Append("[Name] [NVARCHAR](MAX) NULL, ");
            sqlBuilder.Append("[AddedTime] [DATETIME] NOT NULL, ");
            sqlBuilder.Append("[ParentNode_Id] [UNIQUEIDENTIFIER] NULL, ");
            sqlBuilder.Append("CONSTRAINT [PK_dbo.Menus] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ");

            sqlBuilder.Append("ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Menus_dbo.Menus_ParentNode_Id] FOREIGN KEY([ParentNode_Id]) REFERENCES [dbo].[Menus] ([Id]) ");
            sqlBuilder.Append("ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [FK_dbo.Menus_dbo.Menus_ParentNode_Id] ");
            sqlBuilder.Append("END ");

            sqlHelper.ExecuteNonQuery(sqlBuilder.ToString());

            #endregion

            #region # 初始化菜单节点

            if (!this.Set<Menu>().Any())
            {
                string rootMenuName = $"{ConfigurationManager.AppSettings["WebName"]}日志管理后台";

                //初始化菜单
                Menu rootMenu = new Menu(rootMenuName, int.MinValue, null, null, null);
                Menu exceptionLogMenu = new Menu("异常日志管理", 1, "/ExceptionLog/Index", null, rootMenu);
                Menu runningLogMenu = new Menu("程序日志管理", 2, "/RunningLog/Index", null, rootMenu);

                this.Set<Menu>().Add(rootMenu);
                this.Set<Menu>().Add(exceptionLogMenu);
                this.Set<Menu>().Add(runningLogMenu);
                this.SaveChanges();
            }

            #endregion
        }
        #endregion

        #region 重写SaveChanges方法 —— override int SaveChanges()
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

        #region 递归删除菜单 —— void RemoveMenus(Menu menu)
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

        #region 模型创建事件 —— override void OnModelCreating(DbModelBuilder modelBuilder)
        /// <summary>
        /// 模型创建事件
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Menu>().Ignore(x => x.CreatorAccount);
            modelBuilder.Entity<Menu>().Ignore(x => x.OperatorAccount);
            modelBuilder.Entity<Menu>().Ignore(x => x.Deleted);
            modelBuilder.Entity<Menu>().Ignore(x => x.DeletedTime);
            modelBuilder.Entity<Menu>().Ignore(x => x.SavedTime);
            modelBuilder.Entity<Menu>().Ignore(x => x.Number);
            modelBuilder.Entity<Menu>().Ignore(x => x.Keywords);
        }
        #endregion 

        #endregion
    }
}