using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;
using AL.Common.Data;
using AL.User.DTO.Models;

namespace AL.User.Repository
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        public async Task<List<Roles>> GetRoleListByUserIDs(int Id)
        {
            if (Id <= 0) return new List<Roles>();
            return await base._db.Queryable<UserRole, Roles>((ur, u) => new object[]
            {
            JoinType.Inner,ur.FK_Role==u.ID,
            }).Where((ur, u) => Id == ur.FK_User && u.IsValid == Consts.IsValid_True).Select((ur, u) => u).ToListAsync();
        }

        public async Task<List<UserAuthComplex>> GetUserAuth(int userId)
        {
            var query = base._db.Queryable<UserRole, RoleMenuButton, MenuButton, Menus>((ur, rb, b, m) => new object[] {
                JoinType.Inner, ur.FK_Role== rb.FK_Role,
                JoinType.Inner, rb.FK_MenuButton== b.ID,
                JoinType.Left, b.FK_Menu== m.ID })
            .Where((ur, rb, b, m) => ur.FK_User == userId && m.IsValid == Consts.IsValid_True && b.IsValid == Consts.IsValid_True && rb.IsValid == Consts.IsValid_True)
            .OrderBy((ur, rb, b, m) => m.Sort).OrderBy((ur, rb, b, m) => b.Sort);
            return await query.Select((ur, rb, b, m) => new UserAuthComplex { FK_Role = ur.FK_Role, MenuIcon = m.MenuIcon, MenuId = m.ID, MenuName = m.Name, ParentMenuId = m.SourceID, Url = m.Url, BtnId = b.ID, BtnName = b.Btn_Name, BtnCode = b.Btn_Code }).ToListAsync();
        }


        /// <summary>
        /// 根据角色查询角色绑定的用户数量
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async Task<Dictionary<int, int>> GetUserCountByRoleID(List<int> roleID)
        {
            var result = new Dictionary<int, int>();
            var query = base._db.Queryable<UserRole, DB.Entitys.Users>((ur, u) => new object[] {
                JoinType.Inner, ur.FK_User==u.ID,
            })
            .Where((ur, u) => roleID.Contains(ur.FK_Role) && u.IsValid == Consts.IsValid_True)
            //groupby中的别名一定要和where中的别名一样，和select 中的也一样（都是sql语句查询中的别名）
            .GroupBy(ur => ur.FK_Role)
            .Select((ur, u) => new { ur.FK_Role, Count = SqlFunc.AggregateCount(u.ID) });
            (await query.ToListAsync()).ForEach(c => result.Add(c.FK_Role, c.Count));
            return result;
        }


        /// <summary>
        /// 根据角色ID查询相关用户集合数据
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async Task<List<int>> GetUserCountByRoleID(int roleID)
        {
            var query = base._db.Queryable<UserRole, DB.Entitys.Users>((ur, u) => new object[] {
                JoinType.Inner, ur.FK_User==u.ID,
            })
            .Where((ur, u) => roleID == ur.FK_Role && u.IsValid == Consts.IsValid_True)
            .Select((ur, u) => u.ID);
            var result = await query.ToListAsync();
            return result;
        }


    }
}

