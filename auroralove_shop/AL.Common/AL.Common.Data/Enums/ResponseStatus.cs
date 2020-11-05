using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data
{
    public enum ResponseStatus
    {
        [Description("OK：请求成功")]
        [EnumMember]
        Code_200,
        [Description("Created:常用于POST、PUT请求,表明请求已经成功,并新建了一个资源,并在响应体中返回路径。")]
        [EnumMember]
        Code_201,
        [Description("Accepted:请求已经接收到,但没有响应,稍后也不会返回一个异步请求结果")]
        [EnumMember]
        Code_202,
        [Description("Multiple Choice：返回多个响应，需要浏览器或者用户选择")]
        [EnumMember]
        Code_300,
        [Description("Moved Permanently: 请求资源的URL被永久的改变，新的URL会在响应的Location中给出")]
        [EnumMember]
        Code_301,
        [Description("Found: 重定向，请求资源的URL被暂时修改到Location提供的URL")]
        [EnumMember]
        Code_302,
        [Description("Bad Request：请求语法有问题，服务器无法识别")]
        [EnumMember]
        Code_400,
        [Description("UnAuthorized: 客户端未授权该请求。缺乏有效的身份认证凭证")]
        [EnumMember]
        Code_401,
        [Description("Forbidden: 服务器拒绝响应。权限不足")]
        [EnumMember]
        Code_403,
        [Description("Not Found: URL无效或者URL有效但是没有资源")]
        [EnumMember]
        Code_404,
        [Description("Method Not Allowed: 请求方式Method不允许。但是GET和HEAD属于强制方式，不能返回这个状态码")]
        [EnumMember]
        Code_405,
        [Description("Not Acceptable: 资源类型不符合服务器要求")]
        [EnumMember]
        Code_406,
        [Description("Internal Server Error: 服务器内部错误，未捕获")]
        [EnumMember]
        Code_500,
        [Description("Bad Gateway: 服务器作为网关使用时，收到上游服务器返回的无效响应")]
        [EnumMember]
        Code_502,
        [Description("Service Unavailable: 无法服务。一般发生在因维护而停机或者服务过载")]
        [EnumMember]
        Code_503,
    }
}
