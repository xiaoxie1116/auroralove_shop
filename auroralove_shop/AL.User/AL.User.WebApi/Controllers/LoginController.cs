using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AL.User.WebApi.Controllers
{
    /// <summary>
    /// 登录模块
    /// </summary>
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("黑hi");
        }
    }
}
