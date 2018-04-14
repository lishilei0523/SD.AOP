using SD.Infrastructure.EntityBase;
using SD.Toolkits.Recursion.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.AOP.LogSite.Entities
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : AggregateRootEntity, ISortable, ITree<Menu>
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Menu()
        {
            //初始化导航属性
            this.SubNodes = new HashSet<Menu>();
        }
        #endregion

        #region 02.创建菜单构造器
        /// <summary>
        /// 创建菜单构造器
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">菜单排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="icon">图标</param>
        /// <param name="parentNode">上级菜单</param>
        public Menu(string menuName, int sort, string url, string icon, Menu parentNode)
            : this()
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName", "菜单名称不可为空！");
            }

            #endregion

            base.Name = menuName;
            this.Sort = sort;
            this.Url = url;
            this.Icon = icon;
            this.ParentNode = parentNode;
            this.IsRoot = parentNode == null;
        }
        #endregion

        #endregion

        #region # 属性

        #region 链接地址 —— string Url
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; private set; }
        #endregion

        #region 图标 —— string Icon
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; private set; }
        #endregion

        #region 排序 —— int Sort
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        #endregion

        #region 是否是根级节点 —— bool IsRoot
        /// <summary>
        /// 是否是根级节点
        /// </summary>
        public bool IsRoot { get; private set; }
        #endregion

        #region 只读属性 - 是否是叶子级节点 —— bool IsLeaf
        /// <summary>
        /// 只读属性 - 是否是叶子级节点
        /// </summary>
        public bool IsLeaf
        {
            get { return !this.SubNodes.Any(); }
        }
        #endregion

        #region 导航属性 - 父级菜单 —— Menu ParentNode
        /// <summary>
        /// 导航属性 - 父级菜单
        /// </summary>
        public virtual Menu ParentNode { get; private set; }
        #endregion

        #region 导航属性 - 子级菜单集 ——ICollection<Menu> SubNodes
        /// <summary>
        /// 导航属性 - 子级菜单集
        /// </summary>
        public virtual ICollection<Menu> SubNodes { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改菜单信息 —— void UpdateInfo(string menuName, int sort...
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">菜单排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="icon">图标</param>
        public void UpdateInfo(string menuName, int sort, string url, string icon)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName", "菜单名称不可为空！");
            }

            #endregion

            base.Name = menuName;
            this.Sort = sort;
            this.Url = url;
            this.Icon = icon;
        }
        #endregion

        #region 重写ToString()方法
        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
        #endregion

        #endregion
    }
}