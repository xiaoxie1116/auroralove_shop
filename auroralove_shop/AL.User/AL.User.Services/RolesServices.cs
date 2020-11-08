using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;
using System.Threading.Tasks;
using AL.Common.Data.Enums;
using AL.Common.Data;
using System;
using AL.Common.Base;
using System.Linq;
using System.Collections.Generic;

namespace AL.User.Services
{
    public class RolesServices : BaseServices<Roles>, IRolesServices
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserContext _userContext;

        public RolesServices(IRolesRepository rolesRepository, IUserRoleRepository userRoleRepository, IUserContext userContext)
        {
            _rolesRepository = rolesRepository;
            this.BaseDal = rolesRepository;
            _userRoleRepository = userRoleRepository;
            _userContext = userContext;
        }



        /// <summary>
        /// 新增or编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<bool> AddOrEditRole(RoleModelRequest request)
        {
            var entity = new Roles()
            {
                ID = request.ID,
                RoleName = request.Name,
                Remark = request.Remark,
                RoleType = (int)RoleType.OrdinaryRole,
                IsValid = Consts.IsValid_True,
                ModifyTime = DateTime.Now
            };
            if (request.ID <= 0)
            {
                entity.CreateTime = DateTime.Now;
                entity.CreateUser = _userContext.User.UserID;
                return (await _rolesRepository.Add(entity)) > 0;
            }
            else
            {
                entity.ModifyUser = _userContext.User.UserID;
                return await _rolesRepository.Update(entity);
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PageModel<RoleListResponse>> RoleList(RoleListRequest request)
        {
            var response = await _rolesRepository.PageRoles(request);
            if (response == null || !response.data.Any()) return null;
            var roles = new List<RoleListResponse>();
            var fk_Role = response.data.Select(c => c.ID).ToList();
            var userRoles = await _userRoleRepository.GetUserCountByRoleID(fk_Role);
            response.data.ForEach(c =>
            {
                roles.Add(new RoleListResponse()
                {
                    ID = c.ID,
                    RoleName = c.RoleName,
                    Remark = c.Remark,
                    UpdateTime = c.ModifyTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Count = userRoles.ContainsKey(c.ID) ? userRoles[c.ID] : 0
                });
            });
            return new PageModel<RoleListResponse>
            {
                data = roles,
                dataCount = response.dataCount,
                page = response.page,
                pageCount = response.pageCount,
                PageSize = response.PageSize
            };
        }

    }
}
