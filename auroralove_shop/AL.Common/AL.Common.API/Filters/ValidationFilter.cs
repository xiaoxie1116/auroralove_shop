using AL.Common.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AL.Common.API.Filters
{
    /// <summary>
    /// Required对model数据验证 返回统一的数据格式
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        error = state.Errors.First().ErrorMessage;
                        break;
                    }
                }

                ResponseModel<bool> result = new ResponseModel<bool>()
                {
                    success = false,
                    msg = error,
                    status = 400,
                    values = false
                };
                context.Result = new JsonResult(result);
            }
        }


    }
}
