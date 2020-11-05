using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.User.Services
{
    /// <summary>
    /// 创建实体映射
    /// services.AddAutoMapper是会自动找到所有继承了Profile的类然后进行配置   
    /// </summary>
    public class AddMapperExample : Profile
    {
        public AddMapperExample()
        {
            //TSource  to  TDestination   
            //CreateMap<AL.User.DB.Entitys.Users, DTO.Models.Users.Users>();
        }
    }
}
