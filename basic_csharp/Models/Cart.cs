using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        public string CartRecord { get; set; }
    }
}
