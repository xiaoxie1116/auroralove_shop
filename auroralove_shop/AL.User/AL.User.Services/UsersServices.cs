using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;
using AL.Common.Base;
using System.Threading.Tasks;
using AL.Common.Tools.Helper;
using AL.Common.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using AL.Common.Tools.DataConvert;

namespace AL.User.Services
{
    public class UsersServices : BaseServices<Users>, IUsersServices
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserContext _userContext;

        public UsersServices(IUsersRepository usersRepository,
            IUserRoleRepository userRoleRepository,
            IUserContext userContext)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            this.BaseDal = usersRepository;
            _userContext = userContext;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserInformation> LoginIn(LoginRequest request)
        {
            var md5Pwd = MD5Helper.MD5Encrypt64(MD5Helper.MD5Encrypt32(request.Pwd));
            var entity = (await _usersRepository.Query(c => c.Phone == request.Phone && c.UserType == request.LoginType && c.IsValid == Consts.IsValid_True))?.FirstOrDefault();
            if (entity == null)
                throw new Exception("该账号不存在");
            if (entity.Pwd != md5Pwd)
                throw new Exception("密码输入错误！");
            //更新最后登录时间
            var user = new Users()
            {
                ID = entity.ID,
                LoginLastTime = DateTime.Now
            };
            await _usersRepository.Update(user, new List<string> { "LoginLastTime" });
            var userInfo = MapperExtends.Map<UserInformation>(entity);
            userInfo.RoleType = (await _userRoleRepository.GetRoleListByUserIDs(entity.ID)).FirstOrDefault()?.RoleType;
            return userInfo;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PageModel<UserListResponse>> GetUserList(UserListRequest request)
        {
            return await _usersRepository.GetUserList(request);
        }

        [UseTransaction]
        public async Task<ResponseModel<bool>> AddOrUpdateUser(AddOrUpdateUserModel user, short userType)
        {
            if (!user.RoleId.HasValue)
                user.RoleId = userType == 1 ? 2 : 3;
            if ((await _usersRepository.Query(c => c.Phone == user.Phone && c.UserType == userType && c.IsValid == Consts.IsValid_True)).Any())
                return false.ToResponseModel(500, "手机号已经被注册过，请更换手机号！");
            var userId = _userContext.User.UserID;
            Users entity = null;
            if (user.UserID > 0)
                entity = (await _usersRepository.Query(c => c.ID == user.UserID)).FirstOrDefault();
            if (entity == null) entity = new Users();
            entity.Phone = user.Phone;
            entity.UserName = user.Pwd;
            entity.Birthday = user.Birthday;
            entity.UserType = userType;
            entity.HeadImg = "默认头像";
            entity.Sex = user.Sex;
            entity.Level = 0;
            entity.FK_Citys = user.CityId;
            entity.FK_Shop = user.ShopId;
            int result = 0;
            if (user.UserID == 0)
            {
                entity.Pwd = MD5Helper.MD5Encrypt64(MD5Helper.MD5Encrypt32(user.Pwd));
                entity.CreateUser = userId;
                entity.CreateTime = DateTime.Now;
                entity.IsValid = user.IsValid ?? Consts.IsValid_True;
                result = await _usersRepository.Add(entity);
                var userRole = new UserRole
                {
                    FK_User = result,
                    FK_Role = user.RoleId.Value,
                    CreateTime = DateTime.Now,
                    CreateUser = _userContext.User.UserID,
                    IsValid = Consts.IsValid_True
                };
                return (await _userRoleRepository.Add(userRole) > 0).ToResponseModel(200, "添加成功！");
            }
            else
            {
                entity.ModifyTime = DateTime.Now;
                entity.ModifyUser = userId;
                if (await _usersRepository.Update(entity) && userType == 1)
                {
                    var userRole = (await _userRoleRepository.Query(c => c.FK_User == user.UserID))?.FirstOrDefault();
                    if (userRole == null) return false.ToResponseModel(500, "操作失败！");
                    userRole.FK_Role = user.RoleId.Value;
                    (await _userRoleRepository.Update(userRole)).ToResponseModel(200, "操作成功");
                }
            }
            return false.ToResponseModel(500, "操作失败！");
        }
    }
}
