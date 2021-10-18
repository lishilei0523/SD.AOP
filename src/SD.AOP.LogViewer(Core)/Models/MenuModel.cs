using SD.Infrastructure.PresentationBase;
using SD.Toolkits.EasyUI;
using System;
using System.Collections.Generic;

namespace SD.AOP.LogViewer.Models
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    public class MenuModel : ModelBase, ITreeGrid<MenuModel>
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public MenuModel()
        {
            this.Id = Guid.NewGuid();
            this.children = new HashSet<MenuModel>();
        }
        #endregion

        #region 01.创建菜单模型构造器
        /// <summary>
        /// 创建菜单模型构造器
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="icon">图标</param>
        /// <param name="isRoot">是否是根级节点</param>
        /// <param name="isLeaf">是否是叶子级节点</param>
        /// <param name="parent">上级节点</param>
        public MenuModel(string menuName, int sort, string url, string icon, bool isRoot, bool isLeaf, MenuModel parent)
            : this()
        {
            base.Name = menuName;
            this.Sort = sort;
            this.Url = url;
            this.Icon = icon;
            this.IsRoot = isRoot;
            this.IsLeaf = isLeaf;
            this.Parent = parent;
        }
        #endregion

        #endregion

        #region # 属性

        #region 链接地址 —— string Url
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        #endregion

        #region 图标 —— string Icon
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        #endregion

        #region 是否是根级节点 —— bool IsRoot
        /// <summary>
        /// 是否是根级节点
        /// </summary>
        public bool IsRoot { get; set; }
        #endregion

        #region 是否是叶子级节点 —— bool IsLeaf
        /// <summary>
        /// 是否是叶子级节点
        /// </summary>
        public bool IsLeaf { get; set; }
        #endregion

        #region 排序 —— int Sort
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        #endregion


        //Others

        #region 类型 —— string type
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        #endregion

        #region 导航属性 - 上级菜单 —— MenuModel Parent
        /// <summary>
        /// 导航属性 - 上级菜单
        /// </summary>
        public MenuModel Parent { get; set; }
        #endregion

        #region 导航属性 - 下级菜单集 —— ICollection<MenuModel> children
        /// <summary>
        /// 导航属性 - 下级菜单集
        /// </summary>
        public ICollection<MenuModel> children { get; set; }
        #endregion 

        #endregion
    }
}
