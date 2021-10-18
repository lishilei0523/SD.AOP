﻿using SD.Toolkits.EasyUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.AOP.LogViewer.Models
{
    /// <summary>
    /// 菜单映射工具类
    /// </summary>
    public static class MenuMap
    {
        #region # 菜单EasyUI树节点映射 —— static Node ToNode(this MenuModel menuModel)
        /// <summary>
        /// 菜单EasyUI树节点映射
        /// </summary>
        /// <param name="menuModel">菜单视图模型</param>
        /// <returns>EasyUI树节点</returns>
        public static Node ToNode(this MenuModel menuModel)
        {
            var attributes = new
            {
                href = menuModel.Url,
                isLeaf = menuModel.IsLeaf
            };

            return new Node(menuModel.Id, menuModel.Name, "open", false, attributes);
        }
        #endregion

        #region # 菜单EasyUI树集合映射 —— static ICollection<Node> ToTree(this IEnumerable<MenuModel...
        /// <summary>
        /// 菜单EasyUI树集合映射
        /// </summary>
        /// <param name="menus">菜单视图模型集</param>
        /// <param name="parentId">父级菜单Id</param>
        /// <returns>EasyUI树集合</returns>
        public static ICollection<Node> ToTree(this IEnumerable<MenuModel> menus, Guid? parentId)
        {
            //验证
            menus = menus == null ? new MenuModel[0] : menus.ToArray();

            //声明容器
            ICollection<Node> tree = new HashSet<Node>();

            //判断父级菜单Id是否为null
            if (!parentId.HasValue)
            {
                //从根级开始遍历
                foreach (MenuModel menu in menus.Where(x => x.IsRoot))
                {
                    Node node = menu.ToNode();
                    tree.Add(node);
                    node.children = menus.ToTree(node.id);
                }
            }
            else
            {
                //从给定Id向下遍历
                foreach (MenuModel menu in menus.Where(x => x.Parent != null && x.Parent.Id == parentId.Value))
                {
                    Node node = menu.ToNode();
                    tree.Add(node);
                    node.children = menus.ToTree(node.id);
                }
            }

            return tree;
        }
        #endregion


        //Private

        #region # 填充子节点 —— static void FillChildren(this MenuModel menu...
        /// <summary>
        /// 填充子节点
        /// </summary>
        /// <param name="menu">菜单视图模型</param>
        /// <param name="menus">菜单视图模型集</param>
        private static void FillChildren(this MenuModel menu, ICollection<MenuModel> menus)
        {
            foreach (MenuModel menuView in menus)
            {
                if (menuView.Parent != null && menuView.Parent.Id == menu.Id)
                {
                    menu.children.Add(menuView);
                    menu.type = menu.IsLeaf ? "pack" : "folder";
                    menuView.Parent = null;

                    menuView.FillChildren(menus);
                }
            }
        }
        #endregion
    }
}
