﻿using SD.IdentitySystem.IPresentation.ViewModels.Formats.EasyUI;
using SD.Infrastructure.MVC;
using System.Collections.Generic;

namespace SD.AOP.LogSite.ViewModels.Outputs
{
    /// <summary>
    /// 菜单视图模型
    /// </summary>
    public class MenuView : ViewModel, ITreeGrid<MenuView>
    {
        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        public MenuView()
        {
            this.children = new HashSet<MenuView>();
        }
        #endregion

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

        #region 导航属性 - 父级菜单 —— MenuView Parent
        /// <summary>
        /// 导航属性 - 父级菜单
        /// </summary>
        public MenuView Parent { get; set; }
        #endregion

        #region 导航属性 - 子级菜单集 —— ICollection<MenuView> children
        /// <summary>
        /// 导航属性 - 子级菜单集
        /// </summary>
        public ICollection<MenuView> children { get; set; }
        #endregion
    }
}
