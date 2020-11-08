using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.Common.Data;
using AL.Common.Tools;
using AL.Common.Tools.DataConvert;
using AL.Common.Tools.Redis;
using AL.User.DTO.Models;
using AL.User.IServices;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AL.User.WebApi.Controllers
{
    /// <summary>
    /// 登录模块
    /// </summary>
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly IUsersServices _usersServices;
        private readonly IUserTokenRecordServices _userTokenRecord;
        private readonly IUserContext _userContext;
        private readonly IUserRoleServices _userRoleServices;

        public LoginController(IUsersServices usersServices,
            IUserRoleServices userRoleServices,
            IUserTokenRecordServices userTokenRecordServices,
             IUserContext userContext)
        {
            _usersServices = usersServices;
            _userTokenRecord = userTokenRecordServices;
            _userContext = userContext;
            _userRoleServices = userRoleServices;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseModel<LoginInfo>> Login(LoginRequest request)
        {
            var user = await _usersServices.LoginIn(request);
            if (user == null) return new ResponseModel<LoginInfo>() { msg = "登录失败！", status = 500, success = false };
            DateTime expires;
            var token = CreateToken(user, out expires);
            return new ResponseModel<LoginInfo>()
            {
                status = 200,
                values = new LoginInfo()
                {
                    ExpireTime = GetTimeStamp(expires),
                    Token = token,
                    UserInfo = user
                }
            };
        }

        /// <summary>
        /// app端 注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<bool>> Register(RegisterUser request)
        {
            var model = MapperExtends.Map<AddOrUpdateUserModel>(request);
            return await _usersServices.AddOrUpdateUser(model, 2);
        }

        /// <summary>
        /// 刷新token
        /// </summary>     
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ResponseModel<object>> RefreshToken()
        {
            var user = _userContext.User;
            if (user == null || user.UserID <= 0) return string.Empty.ObjToResponse(401, "token无效，请重新登录！");
            var userEnity = await _usersServices.QueryById(user.UserID);
            //用户中途重新修改信息
            if (userEnity == null || user.UserName != userEnity.UserName || user.Phone != userEnity.Phone)
                return string.Empty.ObjToResponse(401, "用户信息认证失败，请重新登录！");
            var roleType = await _userRoleServices.GetRoleTypeByUserIDs(user.UserID);
            var users = new UserInformation()
            {
                ID = userEnity.ID,
                Phone = userEnity.Phone,
                UserName = userEnity.UserName,
                RoleType = roleType,
            };
            DateTime expires;
            var jwtToken = CreateToken(users, out expires);
            return Json(new { token = jwtToken, expires = GetTimeStamp(expires) }).ObjToResponse();
        }




        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="users"></param>
        /// <param name="expires"></param>
        /// <returns></returns>     
        private string CreateToken(UserInformation users, out DateTime expires)
        {
            var guid = Guid.NewGuid().ToString();
            //用户的上下文对象信息
            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.JwtId,guid),
                new Claim(JwtClaimTypes.Id,users.ID.ToString()),
                new Claim(JwtClaimTypes.Name,users.UserName),
                new Claim(JwtClaimTypes.PhoneNumber,users.Phone),
                new Claim(ClaimConsts.RoleType,users.RoleType.ObjToString()),
                new Claim(ClaimConsts.City,users.FK_Citys.ObjToString())
            };
            var Issuer = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Issuer" });
            var Audience = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Audience" });
            string secret = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Secret" });
            if (string.IsNullOrEmpty(secret)) throw new Exception("密钥不能为空，请联系后台人员进行相关配置！");
            var minuts = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Expires" }).ObjToInt();
            if (minuts <= 0) throw new Exception("请设置token的过期时间！");
            expires = DateTime.Now.AddMinutes(minuts);
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var securityToken = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            //设置缓存
            SetReidsToken(users, guid, jwtToken, minuts);
            //添加用户记录
            Task.FromResult(_userTokenRecord.Add(new DB.Entitys.UserTokenRecord()
            {
                FK_User = users.ID,
                Guid = guid,
                ExpireTime = expires,
                CreateTime = DateTime.Now
            }));
            return jwtToken;
        }




        private void SetReidsToken(UserInformation users, string guid, string jwtToken, int minuts)
        {
            //已经登录
            if (_userContext != null && _userContext.User != null && _userContext.User.UserID > 0)
            {
                if (CSRedisHelper.Exists($"{RedisConsts.UserToken}:{_userContext.User.UserID}:{ _userContext.User.JwtId}"))
                    CSRedisHelper.Del($"{RedisConsts.UserToken}:{_userContext.User.UserID}:{_userContext.User.JwtId}");
            }
            CSRedisHelper.Set($"{RedisConsts.UserToken}:{users.ID}:{guid}", jwtToken, minuts * 60);
        }

        private long GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            long timeStamp = Convert.ToInt64((dt - dateStart).TotalSeconds);
            return timeStamp;
        }


    }
}
