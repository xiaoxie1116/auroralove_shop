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
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<List<Roles>> GetRoleListByUserIDs(int Id);

        Task<List<UserAuthComplex>> GetUserAuth(int userId);

        Task<Dictionary<int, int>> GetUserCountByRoleID(List<int> roleID);

        Task<List<int>> GetUserCountByRoleID(int roleID);
    }
}


