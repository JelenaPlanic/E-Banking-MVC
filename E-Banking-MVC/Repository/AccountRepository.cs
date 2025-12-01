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

        public void UpdateStatus(int AccountId, bool status)
        {
            string query = "update Account set IsActive = @status where AccountId = @id";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", AccountId);
            cmd.Parameters.AddWithValue("@status", status);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

        public decimal GetTotal(int AccountId)
        {
            string query = "select coalesce(sum(Amount),0) as total from Payment where AccountId = @id";
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
            adapter.Fill(ds, "Payment");
            dt = ds.Tables["Payment"];

            cmd.Dispose();
            conn.Close();

            decimal total = 0;
            foreach(DataRow row in dt.Rows)
            {
                total = decimal.Parse(row["total"].ToString());
            }
            return total;

        }

        public List<Account> SearchAccountsBy(string accountHolder, string accountNum, decimal? amountFrom, decimal? amountTo, bool? active)
        {
            string query = "select * from Account as a " +
                           "left join (select Payment.AccountId, coalesce(sum(Amount),0) as total from Payment " +
                           "group by Payment.AccountId) as p " +
                           "on p.AccountId = a.AccountId " +
                           "where AccountHolder like @holder " +
                           "and AccountNumber like @number";

            if(amountFrom != null)
            {
                query += " and total >= @amountFrom";
            }
            if(amountTo != null)
            {
                query += " and total <= @amountTo";
            }
            if(active != null)
            {
                query += " and IsActive = @active";
            }

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@holder", $"%{accountHolder ?? ""}%"); // if null AddWithValue: exception
            cmd.Parameters.AddWithValue("@number", $"%{accountNum ?? ""}%"); // sql like%%
            cmd.Parameters.AddWithValue("amountFrom", amountFrom ?? 0);
            cmd.Parameters.AddWithValue("amountTo", amountTo ?? 0);
            cmd.Parameters.AddWithValue("active", active ?? false);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds, "Account");
            dt = ds.Tables["Account"];

            cmd.Dispose();
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

    }
}
