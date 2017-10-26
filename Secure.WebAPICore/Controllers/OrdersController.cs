using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Controllers
{
    [Route("api/Orders")]
    public class OrdersController:Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }

}
