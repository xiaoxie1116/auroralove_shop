﻿using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;

namespace AL.User.Repository
{
	public class RoleMenuButtonRepository : BaseRepository<RoleMenuButton>,IRoleMenuButtonRepository
	{
	    public RoleMenuButtonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
	}
}

