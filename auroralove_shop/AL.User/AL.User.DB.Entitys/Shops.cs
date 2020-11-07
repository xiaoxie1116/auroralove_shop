using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AL.User.DB.Entitys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Shops")]
    public partial class Shops
    {
           public Shops(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:店铺名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:所在城市
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int FK_Citys {get;set;}

    }
}
