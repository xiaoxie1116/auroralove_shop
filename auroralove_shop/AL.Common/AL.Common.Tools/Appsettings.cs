using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AL.Common.Tools
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public static class Appsettings
    {
        static IConfiguration Configuration { get; set; }

        static Appsettings()
        {
            string applicationExeDirectory = ApplicationExeDirectory();
            //ReloadOnChange = true; 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

        internal static string ApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);
            return appRoot;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string GetSettingNode(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 根据key值获取value
        /// 层级关系key表示时用 ：分割 Eg--"Ftp:FtpPwd"
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSettingNode(string key)
        {
            var ret = string.Empty;
            if (Configuration != null)
                ret = Configuration[key];
            return ret;
        }

        /// <summary>
        ///批量获取数据（类型映射）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> GetSettingNode<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
    }
}
