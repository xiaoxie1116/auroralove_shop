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
    /// 本地磁盘appsetting操作类
    /// </summary>
    public static class LocalAppsetting
    {
        static IConfiguration Configuration { get; set; }

        static LocalAppsetting()
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
            //本地磁盘上面的配置文件
            string location = Appsettings.GetSettingNode(new string[] { "Startup", "LocalSettings" });
            if (string.IsNullOrEmpty(location))
                throw new Exception("配置全局的settings文件路径不存在，请配置相关路径！");
            if (!File.Exists($"{location}\\appsettings.json"))
                throw new Exception($"appsettings.json 文件不存在{location}文件下，请联系相关人员进行文件的配置！");
            return location;
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
                //var val = string.Empty;
                //for (int i = 0; i < sections.Length; i++)
                //{
                //    val = $"{val}{sections[i]}:";
                //}
                //return Configuration[val.TrimEnd(':')];
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
