using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public long TotalPrice { get; set; }
        public string OrderRecord { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
