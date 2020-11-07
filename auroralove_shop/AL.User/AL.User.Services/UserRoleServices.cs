using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;

namespace AL.User.Services
{
	public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
        IUserRoleRepository _UserRoleRepository;
		public  UserRoleServices(IUserRoleRepository UserRoleRepository)
        {
            _UserRoleRepository =UserRoleRepository;           
        }
	}
}
