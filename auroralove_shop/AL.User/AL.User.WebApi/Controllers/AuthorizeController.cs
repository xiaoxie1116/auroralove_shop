using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.User.DTO.Models;
using AL.User.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AL.User.WebApi.Controllers
{
    /// <summary>
    /// 权限模块
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public class AuthorizeController : Controller
    {
        private readonly IUserContext _user;
        private readonly IRoleMenuServices _roleMenuServices;

        public AuthorizeController(IUserContext user, IRoleMenuServices roleMenuServices)
        {
            _user = user;
            _roleMenuServices = roleMenuServices;
        }

        /// <summary>
        /// 获取当前用户的权限菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<AuthUserModel>> AuthMenusList()
        {
            if (_user.User.UserID <= 0) return (new AuthUserModel()).ToResponseModel(400, "无登录用户信息！");
            return (await _roleMenuServices.GetUserAuth()).ToResponseModel(200);
        }



    }
}
