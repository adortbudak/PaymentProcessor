using Akka.Actor;
using PaymentsProcessor.ExternalSystems;
using PaymentsProcessor.Messages;
using System;
using System.Threading.Tasks;

namespace PaymentsProcessor.Actors
{
    internal class PaymentWorkerActor : ReceiveActor
    {
        private readonly IPaymentGateway _paymentGateway;
        
              

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;

            Receive<SendPaymentMessage>(message => HandleSendPayment(message));

            Receive<PaymentReceipt>(message => HandlePaymentReceipt(message));              
        }

        private void HandlePaymentReceipt(PaymentReceipt message)
        {
            Sender.Tell(new PaymentSentMessage(message.AccountNumber, message.PaymentConfirmationReceipt));
        }

        private void HandleSendPayment(SendPaymentMessage message)
        {

            Console.WriteLine("Sending payment for {0} {1}", message.FirstName, message.AccountNumber);

            _paymentGateway.Pay(message.AccountNumber, message.Amount).PipeTo(Self,Sender);
                
        }
            
        

        
    }
}
