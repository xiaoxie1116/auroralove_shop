using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Data.Models
{
    /// <summary>
    /// 账单回写
    /// </summary>
    public class CallBackPayModelCap
    {
        /// <summary>
        /// 是否充值成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 交易类型 1 充值  2支付订单  3退款
        /// </summary>
        public int? TradeType { get; set; }

        /// <summary>
        /// 1微信支付 2支付宝支付 3现金支付 4线下汇款
        /// </summary>
        public int? SubType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal? Rate { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
        
        public GrpcUserInfo UserInfo { get; set; }

    }

    public class GrpcUserInfo
    {
        #region 用户信息
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 账号性质（1个人 2公司）
        /// </summary>
        public int Property { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int? CompanyID { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        public string FPayerAccount { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        public string ToAccountTime { get; set; }


        #endregion
    }
}
