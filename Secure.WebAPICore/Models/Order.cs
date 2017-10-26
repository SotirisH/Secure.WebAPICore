using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secure.WebAPICore.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }
    }
}
