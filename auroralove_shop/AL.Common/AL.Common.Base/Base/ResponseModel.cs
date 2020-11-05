using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base
{
    /// <summary>
    /// 响应实体
    /// </summary>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private int _status = 500;
        public int status
        {
            get { return success ? 200 : _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T values { get; set; }

    }



    public static class ObjToReponseHelper
    {
        public static ResponseModel<int> IntToResponse(this int result, int status = 200, string msg = "操作成功!")
        {
            return new ResponseModel<int>
            {
                status = status,
                success = status == 200,
                msg = msg,
                values = result
            };
        }


        /// <summary>
        /// Object 类型 (值类型不要传，避免装箱)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static ResponseModel<object> ObjToResponse(this object result, int status = 200)
        {
            return new ResponseModel<object>
            {
                success = status == 200,
                status = status,
                values = result,
                msg = status == 200 ? "操作成功！" : "操作失败！"
            };
        }

        public static ResponseModel<object> ObjToResponse(this object result, int status, string msg)
        {
            return new ResponseModel<object>
            {
                success = status == 200,
                status = status,
                values = result,
                msg = msg
            };
        }

        public static ResponseModel<T> ToResponseModel<T>(this T result, int status = 200)
        {
            return new ResponseModel<T>
            {
                success = status == 200,
                status = status,
                values = result,
                msg = status == 200 ? "操作成功!" : "操作失败!"
            };
        }


        public static ResponseModel<T> ToResponseModel<T>(this T result, string msg, int status = 500)
        {
            return new ResponseModel<T>
            {
                success = status == 200,
                status = status,
                values = result,
                msg = msg
            };
        }

        public static ResponseModel<T> ToResponseModel<T>(this T result, int status, string msg)
        {
            return new ResponseModel<T>
            {
                success = status == 200,
                status = status,
                values = result,
                msg = msg
            };
        }

    }


}
