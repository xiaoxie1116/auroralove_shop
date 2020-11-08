using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthUserModel
    {
        /// <summary>
        /// 菜单集合
        /// </summary>
        public List<AuthMenuItem> Menus { get; set; }

        /// <summary>
        /// 按钮集合
        /// </summary>
        public List<AuthMenuButton> Buttons { get; set; }
    }

    /// <summary>
    /// 菜单
    /// </summary>
    public class AuthMenuItem
    {
        public int MenuId { get; set; }

        public string Icon { get; set; }

        public string MenuName { get; set; }

        public string Url { get; set; }

        public List<AuthMenuItem> Children { get; set; }
    }

    /// <summary>
    /// 菜单按钮 关联
    /// </summary>
    public class AuthMenuButton
    {
        public int MenuId { get; set; }

        public List<AuthButton> AuthButton { get; set; }
    }

    public class AuthButton
    {
        public int BtnId { get; set; }
        public string BtnName { get; set; }
        public string BtnCode { get; set; }
    }


    public class UserAuthComplex
    {
        public int FK_Role { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }

        public int ParentMenuId { get; set; }
        public string Url { get; set; }

        public int BtnId { get; set; }
        public string BtnName { get; set; }
        public string BtnCode { get; set; }
    }
}
