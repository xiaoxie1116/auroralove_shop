using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.Tools.Util
{
    public class HttpClientUtil
    {

        /// <summary>
        /// 以Post方式提交请求
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static async Task<string> ClientPost(string action, dynamic parameter)
        {
            HttpClient client = new HttpClient();
            var str = JsonConvert.SerializeObject(parameter);
            StringContent stringContent = new StringContent(str, Encoding.UTF8, "application/json");
            var ReponseMessage = await client.PostAsync(action, stringContent);
            return await ReponseMessage.Content.ReadAsStringAsync(); ;
        }
    }
}
