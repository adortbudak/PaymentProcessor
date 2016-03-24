using System.Threading.Tasks;

namespace PaymentsProcessor.ExternalSystems
{
    internal interface IPaymentGateway
    {
        Task<PaymentReceipt> Pay(int accountNumber, decimal amount);
    }
}