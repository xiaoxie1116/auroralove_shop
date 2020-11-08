using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    /// <summary>
    /// 菜单权限集合
    /// </summary>
    public class AuthorityModel : MenuModel
    {
        /// <summary>
        /// 子集
        /// </summary>
        public List<AuthorityModel> Childs { get; set; }
    }

    public class MenuModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 是否已经勾选  0 未勾选   1 已勾选（按钮）（（ 菜单） 2 部分勾选  3 全部勾选  ）
        /// </summary>
        public int IsChecked { get; set; }

        /// <summary>
        /// 1 菜单  2按钮
        /// </summary>
        public int Type { get; set; }
    }
}
