using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.Payload.Response
{
    public class ResponseObject<T> where T : class, new()
    {
        public ResponseObject() { }
        public string Log { get; set; }
        public T Object { get; set; }
    }
}
