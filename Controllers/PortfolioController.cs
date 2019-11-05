using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.AspNetCore.Middleware.Models;

namespace Portfolio.AspNetCore.Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : Controller
    {
        // GET: api/Portfolio
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new PortfolioDto[] { new PortfolioDto { id = 1 }, new PortfolioDto { id = 2 } });
        }

        // GET: api/Portfolio/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(new PortfolioDto { id = id });
        }

        // POST: api/Portfolio
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PortfolioDto value)
        {
            return Ok(value);
        }

        // PUT: api/Portfolio/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PortfolioDto value)
        {
            return Ok(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(new PortfolioDto());
        }
    }
}
