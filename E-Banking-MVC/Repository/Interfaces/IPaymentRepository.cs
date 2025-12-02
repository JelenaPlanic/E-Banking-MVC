using E_Banking_MVC.Models;

namespace E_Banking_MVC.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        List<Payment> Search(int accountId, string transant, decimal? amountFrom, decimal? amountTo,DateTime? dateFrom, DateTime? dateTo );

        List<Payment> GetPositiveOrNegative(int accountId, bool positive);

        List<Payment> GetAll(int AccountId);
        Payment GetOne(int PaymentId);
        void Create(Payment payment);
        void Update(Payment payment);
        void Delete(int PaymentId);
    }
}
