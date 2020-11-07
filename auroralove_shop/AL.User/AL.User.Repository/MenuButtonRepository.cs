﻿using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;

namespace AL.User.Repository
{
	public class MenuButtonRepository : BaseRepository<MenuButton>,IMenuButtonRepository
	{
	    public MenuButtonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
	}
}

