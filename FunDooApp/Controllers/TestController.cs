using Microsoft.AspNetCore.Mvc;

namespace FunDooApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public string Hello()
        {
            return "FunDoo API is working!";
        }
    }
}
