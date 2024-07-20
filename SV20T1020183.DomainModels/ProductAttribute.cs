using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020183.DomainModels
{
    public class ProductAttribute
    {
        public long AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = "";
        public string AttributeValue { get; set; }= "";
        public int DisplayOrder {  get; set; }

        public static implicit operator List<object>(ProductAttribute? v)
        {
            throw new NotImplementedException();
        }
    }
}
