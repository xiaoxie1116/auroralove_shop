using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace AL.Common.Tools.Helper
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHelper
    {

        #region Excel导出

        /// <summary>
        /// 实体类集合导出到EXCLE2003
        /// </summary>
        /// <param name="cellHeard">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>文件的下载地址</returns>
        public static MemoryStream EntityListToExcel2003(Dictionary<string, string> cellHeard, IList enList, string sheetName)
        {
            try
            {
                string fileName = sheetName; // 文件名称
                //string filePath = HttpContext.Current.Server.MapPath("\\" + urlPath); // 文件路径

                //// 1.检测是否存在文件夹，若不存在就建立个文件夹
                //string directoryName = Path.GetDirectoryName(filePath);
                //if (!Directory.Exists(directoryName))
                //{
                //    Directory.CreateDirectory(directoryName);
                //}

                // 2.解析单元格头部，设置单元头的中文名称
                HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(fileName); // 工作表
                sheet.CreateFreezePane(0, 1); //冻结列头行
                HSSFRow row_Title = (HSSFRow)sheet.CreateRow(0); //创建列头行
                row_Title.HeightInPoints = 19.5F; //设置列头行高

                List<string> keys = cellHeard.Keys.ToList();

                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < keys.Count; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                }

                #region 设置列头单元格样式                
                HSSFCellStyle cs_Title = (HSSFCellStyle)workbook.CreateCellStyle();// 创建列头样式
                cs_Title.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
                cs_Title.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
                HSSFFont cs_Title_Font = (HSSFFont)workbook.CreateFont(); //创建字体
                cs_Title_Font.IsBold = true; //字体加粗
                cs_Title_Font.FontHeightInPoints = 12; //字体大小
                cs_Title.SetFont(cs_Title_Font); //将字体绑定到样式
                #endregion

                for (int i = 0; i < keys.Count; i++)
                {
                    HSSFCell cell_Title = (HSSFCell)row_Title.CreateCell(i); //创建单元格
                    cell_Title.CellStyle = cs_Title; //将样式绑定到单元格
                    cell_Title.SetCellValue(cellHeard[keys[i]]); // 列名为Key的值
                }

                #region 设置内容单元格样式
                HSSFCellStyle cs_Content = (HSSFCellStyle)workbook.CreateCellStyle(); //创建列头样式
                cs_Content.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
                cs_Content.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
                #endregion

                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow row_Content = sheet.CreateRow(rowIndex);
                    row_Content.HeightInPoints = 16;
                    for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        System.Reflection.PropertyInfo properotyInfo = null; // 属性的信息

                        // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                        if (keys[i].IndexOf(".") >= 0)
                        {
                            // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                            string[] properotyArray = keys[i].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                            string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                            System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                            if (subClassInfo != null)
                            {
                                // 3.1.2 获取子类的实例
                                var subClassEn = en.GetType().GetProperty(subClassName).GetValue(en, null);
                                // 3.1.3 根据属性名称获取子类里的属性类型
                                properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                                if (properotyInfo != null)
                                {
                                    properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                                }
                            }
                        }
                        else
                        {
                            // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                            properotyInfo = en.GetType().GetProperty(keys[i]);
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(en, null);
                            }
                        }

                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }

                        // 3.4 填充到Excel的单元格里
                        HSSFCell cell_Conent = (HSSFCell)row_Content.CreateCell(i); //创建单元格
                        cell_Conent.CellStyle = cs_Content;
                        cell_Conent.SetCellValue(cellValue);
                    }
                    rowIndex++;
                }

                // 4.生成文件

                MemoryStream memStream = new MemoryStream();

                workbook.Write(memStream);
                memStream.Flush();
                memStream.Position = 0L;

                return memStream;
                //var curContext = HttpContext.Current;
                //curContext.Response.BufferOutput = true;
                //curContext.Response.ContentType = "application/vnd.ms-excel";
                //curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //curContext.Response.Charset = "";
                //curContext.Response.AppendHeader("Content-Disposition",
                //     "attachment;filename=" + fileName);
                //curContext.Response.BinaryWrite(memStream.GetBuffer());
                //curContext.Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Excel导出


    }
}
