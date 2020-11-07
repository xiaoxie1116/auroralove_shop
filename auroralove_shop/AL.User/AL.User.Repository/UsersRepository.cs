using AL.Common.Base;
using AL.Common.Base.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using AL.User.DB.Entitys;
using AL.User.IRepository;

namespace AL.User.Repository
{
	public class UsersRepository : BaseRepository<Users>,IUsersRepository
	{
	    public UsersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
	}
}

