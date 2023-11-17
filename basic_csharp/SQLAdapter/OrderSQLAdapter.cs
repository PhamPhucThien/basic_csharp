using basic_csharp.Models;
using basic_csharp.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.SQLAdapter
{
    public class OrderSQLAdapter : ISQLAdapter<ResponseObject<Order>>
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public OrderSQLAdapter()
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=BookStore;Persist Security Info=True;User ID=sa;Password=12345";
            TableName = "Order";
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<Order> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<ResponseObject<Order>> GetData()
        {
            throw new NotImplementedException();
        }

        public int Insert(ResponseObject<Order> item)
        {
            throw new NotImplementedException();
        }

        public int Update(ResponseObject<Order> item)
        {
            throw new NotImplementedException();
        }
    }
}
