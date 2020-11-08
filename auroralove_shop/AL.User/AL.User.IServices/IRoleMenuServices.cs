using System;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.Common.Base.Services;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;

namespace AL.User.IServices
{
    public interface IRoleMenuServices : IBaseServices<RoleMenu>
    {
        /// <summary>
        /// 获取用户菜单权限
        /// </summary>
        /// <returns></returns>
        Task<AuthUserModel> GetUserAuth();
    }
}