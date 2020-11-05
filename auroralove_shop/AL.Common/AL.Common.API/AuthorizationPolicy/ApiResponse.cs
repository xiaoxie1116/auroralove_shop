using AL.Common.Base;
using AL.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.API
{
    public class ApiResponse
    {
        public ResponseModel<bool> Response;
        public ApiResponse(ResponseStatus status)
        {
            Response = new ResponseModel<bool>();
            switch (status)
            {
                case ResponseStatus.Code_401:
                    Response.status = 401;
                    Response.msg = "对不起，你没有访问该接口的权限，请重新登录或者联系管理员！";
                    Response.success = false;
                    break;
                case ResponseStatus.Code_403:
                    Response.status = 403;
                    Response.msg = "很抱歉，您的访问权限等级不够，请联系管理员!";
                    Response.success = false;
                    break;
                case ResponseStatus.Code_500:
                    Response.status = 500;
                    Response.msg = "啊哦，服务器开了一个小差，火速联系攻城狮小哥哥！";
                    Response.success = false;
                    break;
            }
        }
    }
}
