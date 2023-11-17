using Azure;
using basic_csharp.Models;
using basic_csharp.Payload.Response;
using Microsoft.Identity.Client;
using System.Data;
using System.Data.SqlClient;

namespace basic_csharp.SQLAdapter
{
    public class ProductSQLAdapter : ISQLAdapter<ResponseObject<Product>>
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public ProductSQLAdapter()
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=BookStore;Persist Security Info=True;User ID=sa;Password=12345";
            TableName = "Product";
        }

        public ResponseObject<List<Product>> GetProductsByPageAndSearch(int page, string search)
        {
            ResponseObject<List<Product>> response = new ResponseObject<List<Product>>();
            string query;
            if (search == "")
            {
                query = "SELECT code, product_name, price, count(*) OVER() as total, quantity FROM " + TableName + " WHERE is_available = 1 ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }
            else
            {
                query = "SELECT code, product_name, price, count(*) OVER() as total, quantity FROM " + TableName + " WHERE is_available = 1 AND product_name LIKE '%" + search + "%' ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            List<Product> products = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                Product product = new Product();
                product.ProductName = (string)row["product_name"];
                product.Code = (int)row["code"];
                product.Quantity = (int)row["quantity"];
                product.Price = (long)row["price"];
                response.Log = "" + (((int)row["total"]-1)/5 + 1);
                products.Add(product);
            }
            if (products.Count > 0) {
                response.Object = products;
            } else
            {
                response.Log = "No product";
            }
            return response;
        }

        public ResponseObject<List<Product>> GetProductsByPageAndSearchForAdmin(int page, string search)
        {
            ResponseObject<List<Product>> response = new ResponseObject<List<Product>>();
            string query;
            if (search == "")
            {
                query = "SELECT code, product_name, price, is_available, quantity, count(*) OVER() as total, quantity FROM " + TableName + " ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }
            else
            {
                query = "SELECT code, product_name, price, is_available, quantity, count(*) OVER() as total, quantity FROM " + TableName + " WHERE product_name LIKE '%" + search + "%' ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            List<Product> products = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                Product product = new Product();
                product.ProductName = (string)row["product_name"];
                product.Code = (int)row["code"];
                product.Quantity = (int)row["quantity"];
                product.Price = (long)row["price"];
                product.IsAvailable = (bool)row["is_available"];
                response.Log = "" + (((int)row["total"] - 1) / 5 + 1);
                products.Add(product);
            }
            if (products.Count > 0)
            {
                response.Object = products;
            }
            else
            {
                response.Log = "No product";
            }
            return response;
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(int code, bool isAvailable)
        {
            string query = "UPDATE " + TableName + " SET is_available = '" + !isAvailable + "' WHERE code = '" + code + "'";

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.UpdateCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            int x = sqlDataAdapter.UpdateCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public ResponseObject<Product> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<List<Product>> GetByListOfCode(List<int> codes)
        {
            ResponseObject<List<Product>> response = new ResponseObject<List<Product>>();
            if (codes == null || codes.Count == 0) 
            {
                response.Log = "No product in cart.";
                return response;
            }
            string listOfCode = "(";
            foreach (int code in codes)
            {
                listOfCode += code + ",";
            }
            listOfCode = listOfCode.Remove(listOfCode.Length - 1, 1);
            listOfCode += ")";
            string query = "SELECT code, product_name, price FROM " + TableName + " WHERE code IN " + listOfCode;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            List<Product> products = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                Product product = new Product();
                product.ProductName = (string)row["product_name"];
                product.Code = (int)row["code"];
                product.Price = (long)row["price"];
                products.Add(product);
            }
            if (products.Count > 0)
            {
                response.Object = products;
            }
            else
            {
                response.Log = "No product";
            }
            return response;
        }

        public List<ResponseObject<Product>> GetData()
        {
            throw new NotImplementedException();
        }

        public int Insert(ResponseObject<Product> item)
        {
            throw new NotImplementedException();
        }

        public int Update(ResponseObject<Product> item)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<Product> GetProductByCodeAndQuantity(int page, int quantity)
        {
            ResponseObject<Product> response = new ResponseObject<Product>();
            string query = "SELECT code, quantity FROM " + TableName + " WHERE code = '" + page + "'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            Product product = new Product();
            if (dataTable.Rows.Count > 0)
            {
                int realQuantity = (int)dataTable.Rows[0]["quantity"];
                if (realQuantity >= quantity)
                {
                    product.Quantity = realQuantity;
                    product.Code = (int)dataTable.Rows[0]["code"];
                    response.Object = product;
                }
                else
                {
                    response.Log = "The quantity is out of range!";
                }
            } else
            {
                response.Log = "The code is invalid!";
            }
            return response;
        }

        public ResponseObject<Product> GetProductByCode(int getCode)
        {
            ResponseObject<Product> response = new ResponseObject<Product>();
            string query = "SELECT code, is_available FROM " + TableName + " WHERE code = '" + getCode + "'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            Product product = new Product();
            if (dataTable.Rows.Count > 0)
            {
                product.Code = (int)dataTable.Rows[0]["code"];
                product.IsAvailable = (bool)dataTable.Rows[0]["is_available"];
                response.Object = product;
            }
            else
            {
                response.Log = "The code is invalid!";
            }
            return response;
        }

        internal void ChangeQuantity(int code, int getQuantity)
        {
            string query = "UPDATE " + TableName + " SET quantity = '" + getQuantity + "' WHERE code = '" + code + "'";

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.UpdateCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            int x = sqlDataAdapter.UpdateCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void AddProduct(string productName, int price, int quantity)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("Insert Into Product(product_name, price, quantity) values('" + productName + "','" + price + "','" + quantity + "')", ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
        }
    }
}
