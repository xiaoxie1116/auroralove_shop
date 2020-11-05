using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace AL.Common.Tools.Util
{
    public static class ERPClientUtil
    {
        private static HttpClient client;
        private static object _lockHelper = new object();

        private static HttpClient GetClient()
        {
            if (client == null)
            {
                lock (_lockHelper)
                {
                    if (client == null)
                    {
                        client = new HttpClient();
                    }
                }
            }
            return client;
        }

        /// <summary>
        /// 向ETIBooking提交Get请求
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <returns></returns>
        public async static Task<ERPResult> ClientGet(string action)
        {
            string url = GetUrl(action);
            var ResponseMessage = GetClient().GetAsync(url).Result;
            return await GetResult(ResponseMessage);
        }
        /// <summary>
        /// 向ETIBooking提交Get请求
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <returns></returns>
        public async static Task<ERPResult> ClientGetByUrl(string url)
        {
            var ResponseMessage = GetClient().GetAsync(url).Result;
            return await GetResult(ResponseMessage);
        }

        /// <summary>
        /// 以Post方式向ETIBooking提交请求
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async static Task<ERPResult> ClientPost(string action, dynamic parameter)
        {
            string url = GetUrl(action);
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
            var ResponseMessage = await GetClient().PostAsync(url, stringContent);
            return await GetResult(ResponseMessage);
        }

        /// <summary>
        /// 获取Url
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static string GetUrl(string action)
        {
            //ERP主域名
            string masterDomain = LocalAppsetting.GetSettingNode(new string[] { "ERPMasterDomain", "Url" });
            if (string.IsNullOrEmpty(masterDomain))
            {
                throw new Exception("ERP主域名未配置,无法发起请求！");
            }
            return $"{masterDomain}{action}";
        }

        /// <summary>
        /// 获取返回结果
        /// </summary>
        /// <param name="ResponseMessage"></param>
        /// <returns></returns>
        private async static Task<ERPResult> GetResult(HttpResponseMessage ResponseMessage)
        {
            if (ResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var response = await ResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ERPResult>(response);
            }
            else
            {
                return new ERPResult { code = (int)ResponseMessage.StatusCode, msg = "调用ERP接口失败" };
            }
        }

        public class ERPResult
        {
            public int code { get; set; }
            public string msg { get; set; }
            public dynamic value { get; set; }
        }
    }
}
