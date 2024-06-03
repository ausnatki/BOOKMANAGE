using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeathCheckController : ControllerBase
    {
        public HeathCheckController() { }

        [HttpGet()]
        public string Get()
        {
            return "OK";
        }
    }
}
