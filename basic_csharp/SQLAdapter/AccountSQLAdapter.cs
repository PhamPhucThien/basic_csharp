using Azure;
using basic_csharp.Models;
using basic_csharp.Payload.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_csharp.SQLAdapter
{
    public class AccountSQLAdapter : ISQLAdapter<ResponseObject<Account>>
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public AccountSQLAdapter()
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=BookStore;Persist Security Info=True;User ID=sa;Password=12345";
            TableName = "Account";
        }

        /*ResponseObject<Account> response = new ResponseObject<Account>();

            return response;*/

        public ResponseObject<Account> Login (string username, string password)
        {
            
            ResponseObject<Account> response = new ResponseObject<Account>();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                "SELECT id, name, role, is_available FROM " + TableName + " WHERE username = '" + username + "' AND password = '" + password + "'", ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                Account account = new Account();
                account.Name = (string)dataTable.Rows[0]["name"];
                account.Role = (string)dataTable.Rows[0]["role"];
                account.Id = (Guid)dataTable.Rows[0]["id"];
                if ((bool)dataTable.Rows[0]["is_available"])
                {
                    response.Object = account;
                    response.Log = "Login Successfully";
                }
                else
                {
                    response.Log = "Your account has been banned!";
                }
            }
            else response.Log = "Wrong username or password";
            return response;
        }
        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool GetByUsername(string username)
        {
            ResponseObject<Account> response = new ResponseObject<Account>();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                "SELECT id FROM " + TableName + " WHERE username = '" + username + "'", ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
                return true;
            return false;
        }

        public ResponseObject<Account> Get(Guid id)
        {
            ResponseObject<Account> response = new ResponseObject<Account> ();

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("Select * from Account", sqlConnection);

            DataTable dt = new DataTable();
            da.Fill(dt);
            Account account = new Account();
            if (dt.Rows.Count > 0)
            {
                account.Id = (Guid)dt.Rows[0]["id"];
                account.Name = (string)dt.Rows[0]["name"];
                account.Role = (string)dt.Rows[0]["role"];
            }
            return response;
        }

        public List<ResponseObject<Account>> GetData()
        {
            throw new NotImplementedException();
        }

        public int Insert(ResponseObject<Account> item)
        {
            throw new NotImplementedException();
        }

        public int Update(ResponseObject<Account> item)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<Account> Register(string createUsername, string createPassword, string createName)
        {
            ResponseObject<Account> response = new ResponseObject<Account>();

            SqlConnection con = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("Insert Into Account(username, password, name, role) values('" + createUsername + "','" + createPassword + "','" + createName + "','User')", ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);

            da = new SqlDataAdapter("SELECT id, name, role FROM " + TableName + " WHERE username = '" + createUsername + "' AND password = '" + createPassword + "'", ConnectionString);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                Account account = new Account();
                account.Name = (string)dt2.Rows[0]["name"];
                account.Role = (string)dt2.Rows[0]["role"];
                account.Id = (Guid)dt2.Rows[0]["id"];
                response.Object = account;
                response.Log = "Login Successfully";
            }
            else response.Log = "Can not create account";
            return response;
        }

        public ResponseObject<List<Account>> GetAllActiveUser(int page, string search)
        {
            ResponseObject<List<Account>> response = new ResponseObject<List<Account>>();
            
            string query;
            if (search == "")
            {
                query = "SELECT code, name, username, is_available, count(*) OVER() as total FROM " + TableName + " WHERE role = 'User' ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }
            else
            {
                query = "SELECT code, name, username, is_available, count(*) OVER() as total FROM " + TableName + " WHERE role = 'User' AND username LIKE '%" + search + "%' ORDER BY code OFFSET " + (page - 1) * 5 + " ROWS FETCH NEXT 5 ROWS ONLY";
            }

            SqlDataAdapter da = new SqlDataAdapter(query, ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Account> list = new List<Account>();
            foreach (DataRow row in dt.Rows)
            {
                Account account = new Account();
                account.Code = (int)row["code"];
                account.Name = (string)row["name"];
                account.UserName = (string)row["username"];
                account.IsAvailable = (bool)row["is_available"];
                response.Log = "" + (((int)row["total"] - 1) / 5 + 1);
                list.Add(account);
            }
            response.Object = list;
            return response;
        }

        public ResponseObject<Account> GetProductByCode(int getCode)
        {
            ResponseObject<Account> response = new ResponseObject<Account>();
            string query = "SELECT code, is_available FROM " + TableName + " WHERE code = '" + getCode + "'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                query, ConnectionString);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            Account account = new Account();
            if (dataTable.Rows.Count > 0)
            {
                account.Code = (int)dataTable.Rows[0]["code"];
                account.IsAvailable = (bool)dataTable.Rows[0]["is_available"];
                response.Object = account;
            }
            else
            {
                response.Log = "The code is invalid!";
            }
            return response;
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
    }
}
