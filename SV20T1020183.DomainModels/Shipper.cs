using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020183.DomainModels
{
    /// <summary>
    /// Người giao hàng
    /// </summary>
    public class Shipper
    {
        public int ShipperID { get; set; }
        public string ShipperName { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
