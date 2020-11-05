using AngleSharp.Text;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.API.Filters
{
    /// <summary>
    /// 处理前端入参：比如防止前端脚本注入 by wo 
    /// </summary>
    public class FieldFilterAttribute : Attribute, IActionFilter
    {
        private HtmlSanitizer sanitizer;
        private string _param;
        private string[] _paramList;
        //param参数
        //第一个值为是否允许特殊字符==Allow为允许，非Allow为拦截
        //其他参数为参数字段名，字段名为不作为校验参数字段
        public FieldFilterAttribute(string param = "Allow")
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");//标签属性白名单,默认没有class标签属性   
            _paramList = param.Split(",");
        }

        //在Action方法之回之后调用
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        //在调用Action方法之前调用
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //获取Action参数集合
            var ps = context.ActionDescriptor.Parameters;
            //遍历参数集合
            foreach (var p in ps)
            {
                if (context.ActionArguments[p.Name] != null)
                {
                    //当参数等于字符串
                    if (p.ParameterType.Equals(typeof(string)))
                    {
                        context.ActionArguments[p.Name] = sanitizer.Sanitize(context.ActionArguments[p.Name].ToString());
                    }
                    else if (p.ParameterType.IsClass)//当参数等于类
                    {
                        var result = ModelFieldFilter(p.Name, p.ParameterType, context.ActionArguments[p.Name]);
                        if (!result)
                            context.Result = new JsonResult("请求来源非法");
                        //context.Result = new JsonResult(new ResponseModel<bool>
                        //{
                        //    success = false,
                        //    status = 400,
                        //    msg = "请求来源非法",
                        //});
                    }
                }

            }
        }

        /// <summary>
        /// 遍历修改类的字符串属性
        /// </summary>
        /// <param name="key">类名</param>
        /// <param name="t">数据类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private bool ModelFieldFilter(string key, Type t, object obj)
        {
            //获取类的属性集合
            //var ats = t.GetCustomAttributes(typeof(FieldFilterAttribute), false);


            if (obj != null)
            {
                //获取类的属性集合
                var pps = t.GetProperties();

                foreach (var pp in pps)
                {
                    if (pp.GetValue(obj) != null)
                    {
                        //当属性等于字符串
                        if (pp.PropertyType.Equals(typeof(string)))
                        {
                            string value = pp.GetValue(obj).ToString();
                            var filterValue = sanitizer.Sanitize(value);
                            if (_paramList[0] != "Allow" && !_paramList.Contains(pp.Name) && value != filterValue)
                                return false;
                            pp.SetValue(obj, sanitizer.Sanitize(value));
                        }
                        else if (pp.PropertyType.IsClass)//当属性等于类进行递归
                        {
                            pp.SetValue(obj, ModelFieldFilter(pp.Name, pp.PropertyType, pp.GetValue(obj)));
                        }
                    }

                }
            }

            return true;
        }
    }
}
