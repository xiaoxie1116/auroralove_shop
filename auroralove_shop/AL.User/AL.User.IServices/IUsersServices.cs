using System;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.Common.Base.Services;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;

namespace AL.User.IServices
{
    public interface IUsersServices : IBaseServices<Users>
    {
        Task<UserInformation> LoginIn(LoginRequest request);

        Task<PageModel<UserListResponse>> GetUserList(UserListRequest request);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userType">1 B端用户 2 c端用户</param>
        /// <returns></returns>
        Task<ResponseModel<bool>> AddOrUpdateUser(AddOrUpdateUserModel user, short userType);
    }
}