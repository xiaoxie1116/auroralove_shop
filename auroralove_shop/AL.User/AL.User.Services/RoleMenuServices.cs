using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using AL.Common.Base;
using AL.Common.Tools.Redis;
using System.Linq;

namespace AL.User.Services
{
    public class RoleMenuServices : BaseServices<RoleMenu>, IRoleMenuServices
    {
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IUserContext _userContext;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleMenuServices(IRoleMenuRepository roleMenuRepository,
            IUserContext userContext,
            IUserRoleRepository userRoleRepository)
        {
            _roleMenuRepository = roleMenuRepository;
            _userContext = userContext;
            _userRoleRepository = userRoleRepository;
        }


        /// <summary>
        /// 获取当前用户的菜单权限
        /// </summary>
        /// <returns></returns>
        public async Task<AuthUserModel> GetUserAuth()
        {
            var userId = _userContext.User.UserID;
            string hKey = "UserMenuAuth";
            //获取缓存
            if (await CSRedisHelper.HExistsAsync(hKey, userId.ToString()))
                return CSRedisHelper.HGet<AuthUserModel>(hKey, userId.ToString());
            var list = await _userRoleRepository.GetUserAuth(userId);
            if (list == null || !list.Any()) return null;
            var auths = new AuthUserModel() { Menus = new List<AuthMenuItem>(), Buttons = new List<AuthMenuButton>() };
            //按钮权限
            foreach (var g in list.GroupBy(t => t.MenuId))
            {
                var first = g.FirstOrDefault();
                var menuButtons = new AuthMenuButton() { MenuId = first.MenuId, AuthButton = new List<AuthButton>() };
                foreach (var item in g)
                {
                    menuButtons.AuthButton.Add(new AuthButton { BtnId = item.BtnId, BtnName = item.BtnName, BtnCode = item.BtnCode });
                }
                auths.Buttons.Add(menuButtons);
            }
            //页面菜单权限
            await AuthUsers(list, auths, 0);
            //设置缓存
            CSRedisHelper.HSet(hKey, userId.ToString(), auths);
            return auths;
        }

        /// <summary>
        /// 用户菜单权限
        /// </summary>
        /// <param name="authComplices"></param>
        /// <param name="auths"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public async Task<List<AuthMenuItem>> AuthUsers(List<UserAuthComplex> authComplices, AuthUserModel auths, int ParentId)
        {
            var menus = new List<AuthMenuItem>();
            var btns = new List<AuthButton>();
            foreach (var item in authComplices.Where(c => c.ParentMenuId == ParentId))
            {
                if (item.MenuId > 0 && item.BtnId <= 0)
                {
                    menus.Add(new AuthMenuItem()
                    {
                        MenuId = item.MenuId,
                        Url = item.Url,
                        Icon = item.MenuIcon,
                        MenuName = item.MenuName,
                        Children = await AuthUsers(authComplices, auths, item.MenuId)
                    });
                }
            }
            auths.Menus.AddRange(menus);
            return menus;
        }
    }
}
