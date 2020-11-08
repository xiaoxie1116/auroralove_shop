using AL.Common.Base.Services;
using AL.Common.Base.Base;
using AL.User.DB.Entitys;
using AL.User.DTO.Models;
using AL.User.IRepository;
using AL.User.IServices;

namespace AL.User.Services
{
    public class UserTokenRecordServices : BaseServices<UserTokenRecord>, IUserTokenRecordServices
    {
        IUserTokenRecordRepository _UserTokenRecordRepository;
        public UserTokenRecordServices(IUserTokenRecordRepository UserTokenRecordRepository)
        {
            this.BaseDal = UserTokenRecordRepository;
            _UserTokenRecordRepository = UserTokenRecordRepository;
        }
    }
}
