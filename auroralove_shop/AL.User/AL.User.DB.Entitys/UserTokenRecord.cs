using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("usertokenrecord")]
    public partial class UserTokenRecord
    {
           public UserTokenRecord(){


           }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID {get;set;}

           /// <summary>
           /// Desc:用户ID(外键)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? FK_User {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Guid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? ExpireTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateTime {get;set;}

    }
}
