using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///按钮操作表 
    ///</summary>
    [SugarTable("MenuButton")]
    public partial class MenuButton
    {
           public MenuButton(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:外键-菜单ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int FK_Menu {get;set;}

           /// <summary>
           /// Desc:按钮名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Btn_Name {get;set;}

           /// <summary>
           /// Desc:按钮代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Btn_Code {get;set;}

           /// <summary>
           /// Desc:请求API接口（多个逗号分隔）
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ApiUrl {get;set;}

           /// <summary>
           /// Desc:按钮排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Sort {get;set;}

           /// <summary>
           /// Desc:是否可用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string IsValid {get;set;}

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
