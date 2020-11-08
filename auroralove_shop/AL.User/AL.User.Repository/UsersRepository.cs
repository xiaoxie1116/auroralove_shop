using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;
using AL.User.DTO.Models;

namespace AL.User.Repository
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        public UsersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageModel<UserListResponse>> GetUserList(UserListRequest request)
        {
            RefAsync<int> Total = 0;
            var list = await base._db.Queryable<Users, UserRole, Roles, Citys, Shops>((u, ur, r, c, s) => new object[] {
                JoinType.Left,u.ID==ur.FK_User,
                JoinType.Left,ur.FK_Role==r.ID,
                JoinType.Left,u.FK_Citys==c.ID,
                JoinType.Left,u.FK_Shop==s.ID
            })
            .WhereIF(SqlFunc.HasValue(request.Name), (u, ur, r, c, s) => u.UserName.Contains(request.Name) || u.Phone == request.Name)
            .WhereIF(SqlFunc.HasNumber(request.Shop), (u, ur, r, c, s) => u.FK_Shop == request.Shop)
            .OrderBy((u, ur, r, c, s) => u.ID, OrderByType.Desc)
            .Select((u, ur, r, c, s) =>
            new UserListResponse()
            {
                ID = u.ID,
                Level = u.Level,
                IsValid = u.IsValid,
                Birthday = u.Birthday,
                CityName = c.Name,
                LoginLastTime = u.LoginLastTime,
                Phone = u.Phone,
                RoleID = r.ID,
                RoleName = r.RoleName,
                Sex = u.Sex,
                ShopName = s.Name,
                UserName = u.UserName
            })
            .ToPageListAsync(request.PageIndex, request.PageSize, Total);
            return new PageModel<UserListResponse>()
            {
                data = list,
                dataCount = Total,
                page = request.PageIndex,
                PageSize = request.PageSize,
                pageCount = request.PageSize > 0 ? Total / request.PageSize : 0
            };
        }
    }
}

