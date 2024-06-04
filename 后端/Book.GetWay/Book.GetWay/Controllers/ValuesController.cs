using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.GetWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("TEST")]
        public IActionResult TEST()
        {
            return Ok();
        }
    }
}
