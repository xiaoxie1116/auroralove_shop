using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;

namespace AL.User.Services
{
	public class RoleMenuButtonServices : BaseServices<RoleMenuButton>, IRoleMenuButtonServices
    {
        IRoleMenuButtonRepository _RoleMenuButtonRepository;
		public  RoleMenuButtonServices(IRoleMenuButtonRepository RoleMenuButtonRepository)
        {
            _RoleMenuButtonRepository =RoleMenuButtonRepository;           
        }
	}
}
