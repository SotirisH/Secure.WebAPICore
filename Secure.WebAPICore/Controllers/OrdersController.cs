using Microsoft.AspNetCore.Mvc;

namespace Secure.WebAPICore.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        public IActionResult Get()
        {
            return Ok("100 orders retrieved!");
        }
    }

}
