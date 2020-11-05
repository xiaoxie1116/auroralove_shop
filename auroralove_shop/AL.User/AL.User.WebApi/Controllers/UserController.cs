using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AL.User.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("hello word");
        }
    }
}
