using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
