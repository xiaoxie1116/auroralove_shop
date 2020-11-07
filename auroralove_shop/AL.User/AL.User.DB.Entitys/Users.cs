using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///客户表 
    ///</summary>
    [SugarTable("Users")]
    public partial class Users
    {
           public Users(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:用户昵称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

           /// <summary>
           /// Desc:密码 MD5加密
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Pwd {get;set;}

           /// <summary>
           /// Desc:电话号码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Phone {get;set;}

           /// <summary>
           /// Desc:生日
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? Birthday {get;set;}

           /// <summary>
           /// Desc:头像
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string HeadImg {get;set;}

           /// <summary>
           /// Desc:性别
           /// Default:
           /// Nullable:True
           /// </summary>           
           public long? Sex {get;set;}

           /// <summary>
           /// Desc:所在城市
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? FK_Citys {get;set;}

           /// <summary>
           /// Desc:所在店铺
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? FK_Shop {get;set;}

           /// <summary>
           /// Desc:签名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Sign {get;set;}

           /// <summary>
           /// Desc:会员等级
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? Level {get;set;}

           /// <summary>
           /// Desc:是否可用
           /// Default:
           /// Nullable:True
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
           /// Desc:修改人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ModifyUser {get;set;}

           /// <summary>
           /// Desc:修改时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? ModifyTime {get;set;}

    }
}
