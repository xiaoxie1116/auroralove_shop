using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.Tools.Helper
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class FileHelper
    {
        private static string ftpUser = Appsettings.GetSettingNode("Ftp:FtpUser");
        private static string ftpPwd = Appsettings.GetSettingNode("Ftp:FtpPwd");
        private static string ftpPath = Appsettings.GetSettingNode("Ftp:FtpPath");
        public static readonly string ContractTypeString = "gif,jpg,png,bmp,rar,pdf,doc,docx,xlsx,txt";
        public static readonly string[] ContractType = new string[] { "gif", "jpg", "png", "bmp", "rar", "pdf", "doc", "docx", "xlsx", "txt" };
        public static readonly string[] ImgType = new string[] { "gif", "jpg", "png", "bmp" };

        /// <summary>
        /// ftp下载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DownLoadFile(string path, string fileName)
        {
            bool isSuccess = true;
            var trueFilePath = path + "\\" + fileName;

            var directoryPath = Path.GetDirectoryName(trueFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (File.Exists(trueFilePath))
            {
                return isSuccess;
            }

            FtpWebRequest reqFTP;
            FileStream outputStream = new FileStream(path + "\\" + fileName, FileMode.Create);

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + fileName));
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = false;
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            FtpWebResponse response = null;
            Stream ftpStream = null;
            try
            {
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Error(ex.Message);
                isSuccess = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <param name="folder">目标文件夹名称</param>
        /// <param name="fileName">源文件名</param>
        /// <param name="fileType">文件类型：1-图片 2-文件 3-jsVersion</param>
        /// Task<string> -- 返回值--对应ftp路径Eg：Files/ETIBooking/2020/15/Rotatins.js
        /// <returns></returns>
        public async static Task<string> UploadAttach(string filePath, string folder, string fileName, int fileType)
        {
            var url = string.Empty;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：未获取到媒体文件");
                return url;
            }
            var ftp = GetFtpServerPath(folder, fileName, fileType);

            if (!File.Exists(filePath))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：本地未找到上传文件");
                return url;
            }
            //判断文件夹是否存在 不存在则创建
            if (!ValidateDirectory(ftp.Item2))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：FTP创建对应文件夹失败");
                return url;
            }
            var ftpPath = Appsettings.GetSettingNode($"Ftp:FtpPath")
             + ftp.Item2 + ftp.Item1;
            var ret = await Upload(ftpPath, filePath);
            if (ret)
                url = ftp.Item2 + ftp.Item1;

            return url;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="ftpPath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async static Task<bool> Upload(string ftpPath, string filePath)
        {
            if (string.IsNullOrWhiteSpace(ftpPath) || string.IsNullOrWhiteSpace(filePath))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：FTP/源文件路径为空");
                return false;
            }
            FtpWebRequest reqFTP;
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            //如果客户端应用程序的数据传输过程不侦听数据端口上的连接
            reqFTP.UsePassive = true;
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // 缓冲大小设置为2kb
            //int buffLength = 2048;

            try
            {
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(filePath))
                {
                    fileContents = Encoding.UTF8.GetBytes(await sourceStream.ReadToEndAsync());
                }

                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileContents.Length;
                using (Stream requestStream = reqFTP.GetRequestStream())
                {
                    await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    if (response.StatusCode != FtpStatusCode.ClosingData)
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：{ex.Message}");
                return false;
            }

            return true;
        }

        #region Help Methods

        #region FTPServer端--递归判断文件夹是否存在 不存在则创建

        private static bool ValidateDirectory(string imagePath)
        {
            try
            {
                var imagePathArr = imagePath.Split('/');

                var imgDirectory = string.Empty;
                if (ftpPath.LastOrDefault() == '/')
                    imgDirectory = ftpPath.Remove(ftpPath.Length - 1);
                else
                    imgDirectory = ftpPath;

                foreach (var path in imagePathArr)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        imgDirectory = imgDirectory + "/" + path;
                        if (!DirectoryExist(imgDirectory))
                        {
                            DirectoryCreate(imgDirectory);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void DirectoryCreate(string ftpPath)
        {
            try
            {
                FtpWebRequest reqFTP;
                // 根据uri创建FtpWebRequest对象
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                // 指定数据传输类型
                reqFTP.UseBinary = true;

                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);

                //如果客户端应用程序的数据传输过程不侦听数据端口上的连接
                reqFTP.UsePassive = true;

                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Error("FileHelper--创建文件夹失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// Todo：当文件夹存在 但文件夹下为空时 判断结果为--文件夹不存在；且已存在的文件夹不能重复创建，否则报错550；
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <returns></returns>
        private static bool DirectoryExist(string ftpPath)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string line = reader.ReadLine();

                if (line == null)
                {
                    string e = "目录不存在";
                    return false;
                }
                reader.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Error("FileHelper--判断文件夹是否存在：" + ex.Message);
                return false;
            }
        }

        #endregion FTPServer端--递归判断文件夹是否存在 不存在则创建

        /// <summary>
        /// 获取ftp对应上传文件夹 及文件名
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType">1-图片 2-js-静态资源文件 3-jsVersion文件</param>
        /// Tuple<文件名, 文件夹>
        /// <returns></returns>
        private static Tuple<string, string> GetFtpServerPath(string folder, string fileName, int fileType = (int)FileType.Image)
        {
            int RandKey = new Random().Next(100, 999);

            string ftpFileName = string.Empty, dictory = string.Empty;
            if (!string.IsNullOrWhiteSpace(folder))
                folder = $"/{folder}";
            switch (fileType)
            {
                case (int)FileType.Image:
                    ftpFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{RandKey}{fileName}";
                    dictory = $"{Appsettings.GetSettingNode($"Ftp:FtpAttachFolder{fileType}")}{folder}/{DateTime.Now.Year}/{DateTime.Now.Day}/";
                    break;
                case (int)FileType.Js:
                    ftpFileName = fileName;
                    dictory = $"{Appsettings.GetSettingNode($"Ftp:FtpAttachFolder{fileType}")}{folder}/";
                    break;
                case (int)FileType.JsVersion:
                    ftpFileName = fileName;
                    dictory = $"{Appsettings.GetSettingNode($"Ftp:FtpAttachFolder{2}")}{Appsettings.GetSettingNode($"Ftp:FtpJsVersionFolder")}/"; // Files//JsVersion
                    break;
                default:
                    ftpFileName = fileName;
                    dictory = folder;
                    break ;
            }
            return new Tuple<string, string>(ftpFileName, dictory);
        }

        #endregion Help Methods

        /// <summary>
        /// 递归创建本地文件路径
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static bool CreateNativeDir(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                return true;
            }

            try
            {
                string dirpath = fileFullPath.Substring(0, fileFullPath.LastIndexOf('\\'));
                string[] pathes = dirpath.Split('\\');
                pathes = pathes.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
                if (pathes.Length > 1)
                {
                    string path = pathes[0];
                    for (int i = 1; i < pathes.Length; i++)
                    {
                        path += "\\" + pathes[i];
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Info(string.Format("创建文件夹{0}失败，原因：{1}", fileFullPath, ex.Message));
                return false;
            }
        }

        #region New 拓展
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="stream">附件流</param>
        /// <param name="folder">自定义的文件夹目录，可以空</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileType">1：图片  2：文件</param>
        /// <returns></returns>
        public async static Task<string> UploadAttach(Stream stream, string folder, string fileName, int fileType,bool js = false)
        {
            var url = string.Empty;
            if (string.IsNullOrEmpty(fileName))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，文件名字不能空");
                return url;
            }

            var ftp = GetFtpServerPathNew(folder, fileName, fileType, js);

            ////判断文件夹是否存在 不存在则创建
            if (!ValidateDirectoryNew(ftp.Item2))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：FTP创建对应文件夹失败");
                return url;
            }
            var ftpPath = Appsettings.GetSettingNode($"Ftp:FtpPath") + ftp.Item2;
            
            var ret = await Upload(ftpPath, ftp.Item1, stream);
            if (ret)
                url = ftp.Item2 + ftp.Item1;

            return url;
        }
        public async static Task<bool> Upload(string ftpPath, string fileName, Stream sourceStream)
        {
            if (string.IsNullOrWhiteSpace(ftpPath))
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：FTP/源文件路径为空");
                return false;
            }

            FtpWebRequest reqFTP;
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)WebRequest.Create(ftpPath + fileName);
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            //如果客户端应用程序的数据传输过程不侦听数据端口上的连接
            reqFTP.UsePassive = true;
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // 缓冲大小设置为2kb
            //int buffLength = 2048;

            try
            {
                sourceStream.Position = 0;
                byte[]
                   fileContents = ((MemoryStream)sourceStream).ToArray();



                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileContents.Length;

                using (Stream requestStream = reqFTP.GetRequestStream())
                {

                    await requestStream
                        .WriteAsync(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    if (response.StatusCode != FtpStatusCode.ClosingData)
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Info($"FTP上传失败，原因：{ex.Message}");
                return false;
            }

            return true;
        }
        private static bool ValidateDirectoryNew(string imagePath)
        {
            try
            {
                var imagePathArr = imagePath.Split('/');

                var imgDirectory = string.Empty;
                if (ftpPath.LastOrDefault() == '/')
                    imgDirectory = ftpPath.Remove(ftpPath.Length - 1);
                else
                    imgDirectory = ftpPath;

                foreach (var path in imagePathArr)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {

                        if (!DirectoryExist(imgDirectory, path))
                        {
                            imgDirectory = imgDirectory + "/" + path;
                            DirectoryCreate(imgDirectory);
                        }
                        else
                        {
                            imgDirectory = imgDirectory + "/" + path;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool DirectoryExist(string ftpPath, string folderName)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string line = reader.ReadToEnd();
                        string[] nameArr = line.Replace("\r\n", "#").Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                        if (nameArr.Contains(folderName))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static Tuple<string, string> GetFtpServerPathNew(string folder, string fileName, int fileType = 1, bool js = false)
        {
            string ftpFileName = string.Empty, dictory = string.Empty;
            //名字
            if (js)
                ftpFileName = fileName;
            else
            {
                int RandKey = new Random().Next(100, 999);
                //获得上传文件的类型(后缀名)
                string extensionName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                
                ftpFileName = string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmssffff"), RandKey, extensionName);
            }

            if (!string.IsNullOrWhiteSpace(folder))
                folder = $"/{folder}";
            if (js)
                dictory = $"{Appsettings.GetSettingNode($"Ftp:FtpAttachFolder{fileType}")}{folder}/";
            else
                dictory = $"{Appsettings.GetSettingNode($"Ftp:FtpAttachFolder{fileType}")}{folder}/{DateTime.Now.Year}/{DateTime.Now.Day}/";

            return new Tuple<string, string>(ftpFileName, dictory);
        }
        #endregion
    }
}