namespace PaymentsProcessor.ExternalSystems
{
    internal interface IPaymentGateway
    {
        void Pay(int accountNumber, decimal amount);
    }
}