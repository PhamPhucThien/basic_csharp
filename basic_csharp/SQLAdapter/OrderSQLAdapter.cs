using basic_csharp.Models;
using basic_csharp.Payload.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            TableName = "[Order]";
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

        public void AddOrder(Guid accountId, string newRecords, long total)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("Insert Into " + TableName + "(account_id, order_record, total_price) values('" + accountId + "','" + newRecords + "','" + total + "')", ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
        }

        internal ResponseObject<Order> GetOrderWithPage(Guid accountId, int page)
        {
            ResponseObject<Order> response = new ResponseObject<Order>();
            string query = "SELECT order_record, total_price, create_date, COUNT(*) OVER() as total FROM " + TableName + "WHERE account_id = '" + accountId + "' ORDER BY create_date DESC OFFSET " + (page - 1) * 1 + " ROWS FETCH NEXT 1 ROWS ONLY";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            
            Order order = new Order();
            if (dataTable.Rows.Count > 0)
            {
                order.OrderRecord = (string)dataTable.Rows[0]["order_record"];
                order.TotalPrice = (long)dataTable.Rows[0]["total_price"];
                order.CreateDate = (DateTime)dataTable.Rows[0]["create_date"];
                response.Log = "" + (int)dataTable.Rows[0]["total"];
                response.Object = order;
            }
            return response;
        }
    }
}
