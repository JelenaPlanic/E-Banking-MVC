using E_Banking_MVC.Models;
using E_Banking_MVC.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_Banking_MVC.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private IConfiguration Configuration { get; }
        public PaymentRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public List<Payment> Search(int accountId, string transant, decimal? amountFrom, decimal? amountTo, DateTime? dateFrom, DateTime? dateTo)
        {
            string query = "select * from Payment where PayerName like @transant and AccountId = @AccountId";

            if(amountFrom != null)
            {
                query += " and Amount >= @amountFrom";
            }
            if(amountTo != null)
            {
                query += " and Amount <= @amountFrom";
            }
            if(dateFrom != null)
            {
                query += " and TransactionDate >= @dateFrom";
            }
            if(dateTo != null)
            {
                query += " and TransactionDate <= @dateTo";
            }

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@transant",$"%{(transant?? "")}%");
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            cmd.Parameters.AddWithValue("@AmountFrom", amountFrom?? 0);
            cmd.Parameters.AddWithValue("@AmountTo", amountTo?? 0);
            cmd.Parameters.AddWithValue("@DateFrom", dateFrom?? DateTime.Now);
            cmd.Parameters.AddWithValue("@DateTo", dateTo?? DateTime.Now);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.Fill(ds, "Payment");
            dt = ds.Tables["Payment"];

            cmd.Dispose();
            conn.Close();

            List<Payment> payments = new List<Payment>();
            foreach(DataRow row in dt.Rows)
            {
                Payment payment = new Payment();
                payment.Id = int.Parse(row["PaymentId"].ToString());
                payment.Amount = decimal.Parse(row["Amount"].ToString());
                payment.TransactionDate = DateTime.Parse(row["TransactionDate"].ToString());
                payment.Purpose = row["Purpose"].ToString();
                payment.PayerName = row["PayerName"].ToString();
                payment.IsUrgent = bool.Parse(row["IsUrgent"].ToString());
                payments.Add(payment);
            }
            return payments;

        }
        public List<Payment> GetPositiveOrNegative(int accountId, bool positive)
        {
            string query = "";
            if (positive)
            {
                query = "select * from Payment where AccountId = @accountId and Amount >= 0";
            }
            else
            {
                query = "select * from Payment where AccountId = @accountId and Amount < 0";
            }
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@accountId", accountId);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.Fill(ds, "Payment");
            dt = ds.Tables["Payment"];

            cmd.Dispose();
            conn.Close();

            List<Payment> payments = new List<Payment>();
            foreach (DataRow row in dt.Rows)
            {
                Payment payment = new Payment();
                payment.Id = int.Parse(row["PaymentId"].ToString());
                payment.Amount = decimal.Parse(row["Amount"].ToString());
                payment.TransactionDate = DateTime.Parse(row["TransactionDate"].ToString());
                payment.Purpose = row["Purpose"].ToString();
                payment.PayerName = row["PayerName"].ToString();
                payment.IsUrgent = bool.Parse(row["IsUrgent"].ToString());
                payments.Add(payment);
            }
            return payments;

        }

        public List<Payment> GetAll(int AccountId)
        {
            string query = "select * from Payment where AccountId = @accountId";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@accountId", AccountId);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.Fill(ds, "Payment");
            dt = ds.Tables["Payment"];

            cmd.Dispose();
            conn.Close();

            List<Payment> payments = new List<Payment>();
            foreach (DataRow row in dt.Rows)
            {
                Payment payment = new Payment();
                payment.Id = int.Parse(row["PaymentId"].ToString());
                payment.Amount = decimal.Parse(row["Amount"].ToString());
                payment.TransactionDate = DateTime.Parse(row["TransactionDate"].ToString());
                payment.Purpose = row["Purpose"].ToString();
                payment.PayerName = row["PayerName"].ToString();
                payment.IsUrgent = bool.Parse(row["IsUrgent"].ToString());
                payments.Add(payment);
            }
            return payments;
        } // get all payment by AccountID(FK)

        public Payment GetOne(int PaymentId)
        {
            string query = "select * from Payment where PaymentId =@id";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", PaymentId);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.Fill(ds, "Payment");
            dt = ds.Tables["Payment"];

            cmd.Dispose();
            conn.Close();

            Payment payment = new Payment();
            foreach (DataRow row in dt.Rows)
            {
                payment.Id = int.Parse(row["PaymentId"].ToString());
                payment.AccountId = int.Parse(row["AccountId"].ToString());
                payment.Amount = decimal.Parse(row["Amount"].ToString());
                payment.TransactionDate = DateTime.Parse(row["TransactionDate"].ToString());
                payment.Purpose = row["Purpose"].ToString();
                payment.PayerName = row["PayerName"].ToString();
                payment.IsUrgent = bool.Parse(row["IsUrgent"].ToString());
            }
            return payment;
        }

        public void Create(Payment payment)
        {
            string query = "insert into Payment (AccountId,Amount,TransactionDate,Purpose,PayerName,IsUrgent) " +
                            "set values (@accountId,@amount, @date,@purpose,@payerName,@isUrgent)";

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@accountId", payment.AccountId);
            cmd.Parameters.AddWithValue("@amount", payment.Amount);
            cmd.Parameters.AddWithValue("@date", payment.TransactionDate);
            cmd.Parameters.AddWithValue("@purpose", payment.Purpose);
            cmd.Parameters.AddWithValue("@payerName", payment.PayerName);
            cmd.Parameters.AddWithValue("@isUrgent", payment.IsUrgent);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        } // add new relation

        public void Update(Payment payment) // update relation
        {
            string query = "update Payment set AccountId=@accountId, Amount=@amount, TransactionDate= @date, Purpose =@purpose, PayerName=@payerName, IsUrgent=@isUrgent " +
                            "where PaymentId = @paymentId";

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@accountId", payment.AccountId);
            cmd.Parameters.AddWithValue("@amount", payment.Amount);
            cmd.Parameters.AddWithValue("@date", payment.TransactionDate);
            cmd.Parameters.AddWithValue("@purpose", payment.Purpose);
            cmd.Parameters.AddWithValue("@payerName", payment.PayerName);
            cmd.Parameters.AddWithValue("@isUrgent", payment.IsUrgent);
            cmd.Parameters.AddWithValue("@paymentId", payment.Id);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

        public void Delete(int PaymentId)
        {
            string query = "delete from Payment where PaymentId = @paymentId";
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();

            conn.Open();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@paymentId", PaymentId);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }
    }
}
