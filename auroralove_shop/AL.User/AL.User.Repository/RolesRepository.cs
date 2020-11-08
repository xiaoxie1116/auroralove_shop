using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;
using AL.User.DTO.Models;
using AL.Common.Tools.Helper;
using AL.Common.Data.Enums;

namespace AL.User.Repository
{
	public class RolesRepository : BaseRepository<Roles>,IRolesRepository
	{
	    public RolesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageModel<Roles>> PageRoles(RoleListRequest request)
        {
            var builder = FilterQueryBuilder.Create<Roles>();
            if (SqlFunc.HasValue(request.Name))
                builder.Add(a => a.RoleName.Contains(request.Name));
            //排除超级管理员的角色
            builder.Add(a => a.RoleType != (int)RoleType.SuperRole);
            return await base.QueryPage(builder.Expression, request.PageIndex, request.PageSize);
        }
    }
}

