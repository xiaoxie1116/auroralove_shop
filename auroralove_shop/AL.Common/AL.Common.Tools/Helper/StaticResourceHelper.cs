using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AL.Common.Tools.Helper
{
    /// <summary>
    /// 静态资源文件帮助类
    /// 用于生成静态资源文件
    /// </summary>
    public static class StaticResourceHelper
    {
        private  static readonly SemaphoreSlim _mutex= new SemaphoreSlim(1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsFile"></param>
        /// <param name="js">是否是静态资源js文件</param>
        /// <returns></returns>
        public static async Task SaveJsFile(JsFile jsFile,bool js=false)
        {
            if (jsFile == null || string.IsNullOrEmpty(jsFile.JsPath) || string.IsNullOrEmpty(jsFile.Url))
                return;

            //调取接口 获取静态文件数据
            var webString = await HttpHelper.CallWebPageAsync(jsFile.Url, string.Empty, Encoding.UTF8, 0);
            if (string.IsNullOrEmpty(webString))
                return;
            await SaveJsFileBase(jsFile, webString, js);
        }

        /// <summary>
        /// 保存静态资源文件
        /// </summary>
        /// <param name="webString">文件内容</param>
        /// <param name="jsFile"></param>
        /// <returns></returns>
        public static async Task<string> SaveJsFile(string webString, string jsPath, bool js = true)
        {
            if (string.IsNullOrWhiteSpace(webString) || string.IsNullOrWhiteSpace(jsPath))
                return string.Empty;

            var jsFile = new JsFile() { JsPath = jsPath };

            return await SaveJsFileBase(jsFile, webString, js);
        }

        /// <summary>
        /// 更新并上传静态资源文件
        /// </summary>
        /// <param name="webString"></param>
        /// <param name="jsPath"></param>
        /// <returns></returns>
        public async static Task<string> UpdateStaticResource(string webString, string jsPath, string folder = "Js", bool js = true)
        {
            var ret = string.Empty;
            var filePath = await SaveJsFile(webString, jsPath);

            if (!File.Exists(filePath))
                return ret;
            //上传至Ftp
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var uploadRet = await FileHelper.UploadAttach(filePath, folder, Path.GetFileName(filePath), (int)FileType.Js);
                if (!string.IsNullOrWhiteSpace(uploadRet))
                    ret = uploadRet;
            }
            return ret;
        }

        private async  static Task<string> SavaJSVersion(JsFile jsFile, string name)
        {
            string str = string.Empty;
            var path = string.Format(Config.JsVersionPath, name);

            FileInfo f = new FileInfo(path);
            if (!f.Directory.Exists)
            {
                f.Directory.Create();
            }
            using (var stream = f.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(stream);
                str = await reader.ReadToEndAsync();
                reader.Close();
            }

            str = JsonConvert.SerializeObject(new JsVersion()
            {
                JsPath = jsFile.JsPath,
                Version = DateTime.Now.ToString("yyyyMMddHHmmss")
            });

            using (StreamWriter writer = new StreamWriter(path))
            {
               await writer.WriteAsync(str);
            }

            return path;
        }

        #region Help Method

        private async static Task<string> SaveJsFileBase(JsFile jsFile, string webString, bool js = false)
        {
            var lastDic = jsFile.JsPath.Substring(0, jsFile.JsPath.LastIndexOf("/"));
            var path = string.Format("{0}{1}", Config.JsFileRootPath, lastDic);
            //资源文件本地保存--文件夹是否存在 不存在则创建
             if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //将静态资源文件写到指定地址
            var file = string.Format("{0}{1}.js", Config.JsFileRootPath, jsFile.JsPath);
            using (StreamWriter writer = new StreamWriter(file))
            {
                await writer.WriteAsync(webString);
            }

            await _mutex.WaitAsync();
            try
            {
                //生成版本号
                if (js)
                {
                  var jsPath= await SavaJSVersion(jsFile, Path.GetFileNameWithoutExtension(file));
                    if (!string.IsNullOrWhiteSpace(jsPath))
                    {
                        var uploadRet = await FileHelper.UploadAttach(jsPath, string.Empty, Path.GetFileName(jsPath), (int)FileType.JsVersion);
                        if (string.IsNullOrWhiteSpace(uploadRet))
                            LoggerHelper.Log4netHelper.Error("上传更新版本文件失败");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Info(string.Format("更新版本文件失败，异常：{0}", ex.Message));
            }
            finally
            {
                _mutex.Release();
            }

            return file;

        }
        #endregion
    }



    #region Help Class

    public class JsVersion
    {
        public string JsPath { get; set; }

        public string Version { get; set; }
    }

    internal static class Config
    {
        internal static string JsFileRootPath = "D:/HY.Config/";
        internal static string JsVersionPath = "D:/asd/asd/{0}Version.js";
    }

    public class JsFile
    {
        /// <summary>
        /// 静态资源文件存储地址 Eg："data2/common/Rotaion"
        /// </summary>
        public string JsPath
        {
            get;
            set;
        }

        /// <summary>
        /// 获取静态资源文件调用接口 Eg："https://localhost:44399/Product/SyncGetRotaionImages"
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 静态资源文件组别
        /// </summary>
        public string Group
        {
            get;
            set;
        }
    }


    public enum FileType
    {
        Image=1,
        Js=2,
        JsVersion=3
    }
    #endregion Help Class
}