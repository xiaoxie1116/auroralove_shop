using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Citys")]
    public partial class Citys
    {
           public Citys(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ParentID {get;set;}

    }
}
