using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///用户角色表 
    ///</summary>
    [SugarTable("UserRole")]
    public partial class UserRole
    {
           public UserRole(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:外键-用户ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int FK_User {get;set;}

           /// <summary>
           /// Desc:外键-角色ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int FK_Role {get;set;}

           /// <summary>
           /// Desc:是否可用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string IsVaild {get;set;}

           /// <summary>
           /// Desc:创建人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? CreateUser {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateTime {get;set;}

           /// <summary>
           /// Desc:更新人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ModifyUser {get;set;}

           /// <summary>
           /// Desc:更新时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? ModifyTime {get;set;}

    }
}
