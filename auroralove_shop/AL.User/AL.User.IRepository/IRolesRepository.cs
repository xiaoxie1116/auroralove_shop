using SqlSugar;
using System;
using AL.Common.Base;
using AL.Common.Base.Repository;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AL.User.IRepository
{
	public interface IRolesRepository : IBaseRepository<Roles>
	{
		Task<PageModel<Roles>> PageRoles(RoleListRequest request);
	}
}


