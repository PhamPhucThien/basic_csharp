using Azure;
using basic_csharp.Models;
using basic_csharp.Payload.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.SQLAdapter
{
    public class CartSQLAdapter : ISQLAdapter<ResponseObject<Cart>>
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public CartSQLAdapter()
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=BookStore;Persist Security Info=True;User ID=sa;Password=12345";
            TableName = "Cart";
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<Cart> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<ResponseObject<Cart>> GetData()
        {
            throw new NotImplementedException();
        }

        public int Insert(ResponseObject<Cart> item)
        {
            throw new NotImplementedException();
        }

        public int Update(ResponseObject<Cart> item)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<Cart> GetCartByAccountId(Guid accountId)
        {
            ResponseObject<Cart> response = new ResponseObject<Cart>();
            string query = "SELECT cart_record FROM " + TableName + " WHERE account_id = '" + accountId + "'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            Cart cart = new Cart();
            if (dataTable.Rows.Count > 0)
            {
                cart.CartRecord = (string)dataTable.Rows[0]["cart_record"];
                response.Object = cart;
            }
            else
            {
                response.Log = "The id is invalid!";
            }
            return response;
        }

        public int UpdateCartRecord(Guid accountId, string newRecords)
        {
            string query = "UPDATE " + TableName + " SET cart_record = '" + newRecords + "' WHERE account_id = '" + accountId + "'";

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.UpdateCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            int x = sqlDataAdapter.UpdateCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return x;
        }
    }
}
