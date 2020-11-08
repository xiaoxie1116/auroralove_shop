using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;
using System.Threading.Tasks;
using System.Linq;

namespace AL.User.Services
{
    public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
        IUserRoleRepository _userRoleRepository;
        public UserRoleServices(IUserRoleRepository UserRoleRepository)
        {
            _userRoleRepository = UserRoleRepository;
            this.BaseDal = UserRoleRepository;
        }

        public async Task<int?> GetRoleTypeByUserIDs(int Id)
        {
            return (await _userRoleRepository.GetRoleListByUserIDs(Id))?.FirstOrDefault()?.RoleType;
        }

    }
}
