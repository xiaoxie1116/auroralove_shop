using System;
using System.Threading.Tasks;
using AL.Common.Base;
using AL.Common.Base.Services;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;

namespace AL.User.IServices
{
	 public interface IRolesServices : IBaseServices<Roles>
	 {
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageModel<RoleListResponse>> RoleList(RoleListRequest request);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddOrEditRole(RoleModelRequest request);

    }
}