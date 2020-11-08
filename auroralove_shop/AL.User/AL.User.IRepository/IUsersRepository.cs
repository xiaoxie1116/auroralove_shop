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
    public interface IUsersRepository : IBaseRepository<Users>
    {
        /// <summary>
        ///  用户列表分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageModel<UserListResponse>> GetUserList(UserListRequest request);
    }
}


