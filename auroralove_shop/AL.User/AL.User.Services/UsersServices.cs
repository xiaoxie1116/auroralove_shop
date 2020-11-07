using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;

namespace AL.User.Services
{
	public class UsersServices : BaseServices<Users>, IUsersServices
    {
        IUsersRepository _UsersRepository;
		public  UsersServices(IUsersRepository UsersRepository)
        {
            _UsersRepository =UsersRepository;           
        }
	}
}
