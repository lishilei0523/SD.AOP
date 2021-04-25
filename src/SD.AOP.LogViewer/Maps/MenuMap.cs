using SD.AOP.LogViewer.Entities;
using SD.AOP.LogViewer.ViewModels.Outputs;
using SD.Toolkits.EasyUI;
using SD.Toolkits.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.AOP.LogViewer.Maps
{
    /// <summary>
    /// 菜单映射工具类
    /// </summary>
    public static class MenuMap
    {
        #region # 菜单视图模型映射 —— static MenuView ToViewModel(this Menu menu)
        /// <summary>
        /// 菜单视图模型映射
        /// </summary>
        /// <param name="menu">菜单领域模型</param>
        /// <returns>菜单视图模型</returns>
        public static MenuView ToViewModel(this Menu menu)
        {
            MenuView menuView = menu.Map<Menu, MenuView>();
            menuView.Parent = menu.ParentNode?.ToViewModel();

            return menuView;
        }
        #endregion

        #region # 菜单EasyUI树节点映射 —— static Node ToNode(this MenuView menuView)
        /// <summary>
        /// 菜单EasyUI树节点映射
        /// </summary>
        /// <param name="menuView">菜单视图模型</param>
        /// <returns>EasyUI树节点</returns>
        public static Node ToNode(this MenuView menuView)
        {
            var attributes = new
            {
                href = menuView.Url,
                isLeaf = menuView.IsLeaf
            };

            return new Node(menuView.Id, menuView.Name, "open", false, attributes);
        }
        #endregion

        #region # 菜单EasyUI树集合映射 —— static ICollection<Node> ToTree(this IEnumerable<MenuView...
        /// <summary>
        /// 菜单EasyUI树集合映射
        /// </summary>
        /// <param name="menus">菜单视图模型集</param>
        /// <param name="parentId">父级菜单Id</param>
        /// <returns>EasyUI树集合</returns>
        public static ICollection<Node> ToTree(this IEnumerable<MenuView> menus, Guid? parentId)
        {
            //验证
            menus = menus == null ? new MenuView[0] : menus.ToArray();

            //声明容器
            ICollection<Node> tree = new HashSet<Node>();

            //判断父级菜单Id是否为null
            if (!parentId.HasValue)
            {
                //从根级开始遍历
                foreach (MenuView menu in menus.Where(x => x.IsRoot))
                {
                    Node node = menu.ToNode();

                    tree.Add(node);

                    node.children = menus.ToTree(node.id);
                }
            }
            else
            {
                //从给定Id向下遍历
                foreach (MenuView menu in menus.Where(x => x.Parent != null && x.Parent.Id == parentId.Value))
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

        #region # 填充子节点 —— static void FillChildren(this MenuView menu...
        /// <summary>
        /// 填充子节点
        /// </summary>
        /// <param name="menu">菜单视图模型</param>
        /// <param name="menus">菜单视图模型集</param>
        private static void FillChildren(this MenuView menu, ICollection<MenuView> menus)
        {
            foreach (MenuView menuView in menus)
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
