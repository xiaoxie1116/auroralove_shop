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
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRolesServices _rolesServices;
        private readonly IUserRoleServices _userRoleServices;

        public RoleController(IRolesServices rolesServices, IUserRoleServices userRoleServices)
        {
            _rolesServices = rolesServices;
            _userRoleServices = userRoleServices;
        }

        /// <summary>
        /// 角色新增/编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResponseModel<bool>> AddOrEdit(RoleModelRequest request)
        {
            if ((await _rolesServices.Query(c => c.RoleName == request.Name && c.ID != request.ID && c.IsValid == Consts.IsValid_True)).Any())
                return false.ToResponseModel("角色名称不允许重复！", 400);
            return (await _rolesServices.AddOrEditRole(request)).ToResponseModel();
        }

        /// <summary>
        /// 根据角色ID获取角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<object>> GetRoleInfoById(int Id)
        {
            if (Id <= 0) return false.ObjToResponse(400, "角色ID不能为空！");
            var role = await _rolesServices.QueryById(Id);
            if (role != null)
                return (new { role.ID, Name = role.RoleName, role.Remark }).ObjToResponse();
            else
                return string.Empty.ObjToResponse();
        }

        /// <summary>
        /// 角色数据源
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<object>> RoleResource()
        {
            var list = await _rolesServices.Query(c => c.IsValid == Consts.IsValid_True);
            var result = list.Select(c => new { c.ID, Value = c.RoleName });
            return result.ObjToResponse();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResponseModel<bool>> DeleteRole(int RoleID)
        {
            bool result = false;
            if (RoleID <= 0) return result.ToResponseModel("ID不能为空", 400);
            if ((await _userRoleServices.Query(c => c.FK_Role == RoleID && c.IsValid == Consts.IsValid_True)).Any())
                return result.ToResponseModel("已有用户关联该角色，请先解除！", 400);
            var roleType = (await _rolesServices.Query(c => c.ID == RoleID)).FirstOrDefault()?.RoleType;
            if (roleType == 1)
                return result.ToResponseModel("超级管理员不允许删除！", 400);
            if (roleType == 2)
                return result.ToResponseModel("默认角色不允许删除！", 400);
            return (await _rolesServices.DeleteById(RoleID)).ToResponseModel();
        }
    }
}
