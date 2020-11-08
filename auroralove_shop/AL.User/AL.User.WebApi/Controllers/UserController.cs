using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.Common.Data;
using AL.User.DTO.Models;
using AL.User.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AL.User.WebApi.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUsersServices _userServices;
        private readonly IUserContext _userContext;

        public UserController(IUsersServices userServices, IUserContext userContext)
        {
            _userServices = userServices;
            _userContext = userContext;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<PageModel<UserListResponse>>> UserListPage(UserListRequest request)
        {
            return (await _userServices.GetUserList(request)).ToResponseModel(200);
        }

        /// <summary>
        /// 添加/更新用户 B端
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseModel<bool>> AddOrModifyUserToB(AddOrUpdateUserModel request)
        {
            return await _userServices.AddOrUpdateUser(request, 1);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<bool>> ForbidUser(int UserId)
        {
            var user = (await _userServices.Query(c => c.ID == UserId)).FirstOrDefault();
            if (user == null) return false.ToResponseModel(400, "无此用户信息！");
            user.IsValid = Consts.IsValid_False;
            user.ModifyUser = _userContext.User.UserID;
            user.ModifyTime = DateTime.Now;
            return (await _userServices.Update(user)).ToResponseModel(200);
        }



    }
}
