using E_Banking_MVC.Models;
using E_Banking_MVC.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_Banking_MVC.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private IConfiguration Configuration { get; }
        public AccountRepository(IConfiguration configuration) //singleton, immutable
        {
            this.Configuration = configuration;
        }

        public List<Account> GetAll()
        {
            string query = "select * from Account";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connectionString); //stateful
            SqlCommand cmd = conn.CreateCommand(); //stateful

            conn.Open();
            cmd.CommandText = query;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(); // brigde between database and memory
            adapter.SelectCommand = cmd;
            adapter.Fill(ds, "Account");
            dt = ds.Tables["Account"];

            cmd.Dispose();  // OPEN -> WORK-> CLOSE-> DISPOSE 
            conn.Close();

            List<Account> accounts = new List<Account>();
            foreach(DataRow row in dt.Rows)
            {
                Account account = new Account();
                account.Id = int.Parse(row["AccountId"].ToString());
                account.Holder = row["AccountHolder"].ToString();
                account.Number = row["AccountNumber"].ToString();
                account.IsActive = bool.Parse(row["IsActive"].ToString());
                account.HasOnlineBanking = bool.Parse(row["HasOnlineBanking"].ToString());
                accounts.Add(account);

            }
            return accounts;

        }

        public Account GetOne(int AccountId)
        {
            string query = "select * from Account where AccountId =@id";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", AccountId);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds, "Account");
            dt = ds.Tables["Account"];

            cmd.Dispose();
            conn.Close();

            Account account = new Account();
            foreach (DataRow row in dt.Rows)
            {
                account.Id = int.Parse(row["AccountId"].ToString());
                account.Holder = row["AccountHolder"].ToString();
                account.Number = row["AccountNumber"].ToString();
                account.IsActive = bool.Parse(row["IsActive"].ToString());
                account.HasOnlineBanking = bool.Parse(row["HasOnlineBanking"].ToString());
            }
            return account;
        }

        public void Create(Account account)
        {
            string query = "insert into Account (AccountHolder,AccountNumber, IsActive,HasOnlineBanking) " +
                            "values(@holder,@number,@isActive, @hasOnlineBanking)";

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@holder", account.Holder);
            cmd.Parameters.AddWithValue("@number", account.Number);
            cmd.Parameters.AddWithValue("@isActive", account.IsActive);
            cmd.Parameters.AddWithValue("@hasOnlineBanking", account.HasOnlineBanking);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

        public void Update(Account account)
        {
            string query = "update Account set AccountHolder=@holder, AccountNumber=@number, IsActive=@isActive, HasOnlineBanking= @hasOnlineBanking " +
                            "where AccountId =@id";

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@holder", account.Holder);
            cmd.Parameters.AddWithValue("@number", account.Number);
            cmd.Parameters.AddWithValue("@isActive", account.IsActive);
            cmd.Parameters.AddWithValue("@hasOnlineBanking", account.HasOnlineBanking);
            cmd.Parameters.AddWithValue("@id", account.Id);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

        public void Delete(int AccountId)
        {
            string query = "delete from Account where AccountId =@id";

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", AccountId);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

    }
}
