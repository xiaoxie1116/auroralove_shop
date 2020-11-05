using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Data
{
    /// <summary>
    /// ERP web站点配置
    /// </summary>
    public class ERPConfigModel
    {
        /// <summary>
        /// ERP web服务器地址
        /// </summary>
        public string hytours { get; set; }

        /// <summary>
        /// Erp API服务器地址
        /// </summary>
        public string apihytours { get; set; }

        /// <summary>
        /// ERP 文件服务器地址
        /// </summary>
        public string hyfiles { get; set; }

        /// <summary>
        /// Erp 图片服务器地址
        /// </summary>
        public string imageServer { get; set; }
    }
}
