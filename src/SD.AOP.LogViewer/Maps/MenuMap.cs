using SD.AOP.LogViewer.Models;
using SD.Toolkits.EasyUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.AOP.LogViewer.Maps
{
    /// <summary>
    /// 菜单映射
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
            menus = menus?.ToArray() ?? Array.Empty<MenuModel>();

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
    }
}
