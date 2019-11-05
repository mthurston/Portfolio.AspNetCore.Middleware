using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.AspNetCore.Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : Controller
    {
        // GET: api/Ping
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }
    }
}
