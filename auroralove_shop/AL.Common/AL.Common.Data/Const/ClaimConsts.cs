using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Data
{
    public class ClaimConsts
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public const string CompanyID = "CompanyId";

        /// <summary>
        /// erp 公司 ID
        /// </summary>
        public const string Erp_CompanyID = "Erp_CompanyId";
        /// <summary>
        /// erp 门市 ID 
        /// </summary>
        public const string Erp_Department = "Erp_Department";

        /// <summary>
        /// 用户类型(1,公司，2个人)
        /// </summary>
        public const string UserType = "UserType";

        /// <summary>
        /// 角色类型（1超管 2默认初始化角色 3普通用户 4系管）
        /// </summary>
        public const string RoleType = "RoleType";
        /// <summary>
        /// 客户状态 二进制枚举（1 正常 2 停用 4 冻结  ）
        /// </summary>
        public const string Status = "Status";
    }
}
