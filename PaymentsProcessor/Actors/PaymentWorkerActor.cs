using Akka.Actor;
using PaymentsProcessor.ExternalSystems;
using PaymentsProcessor.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsProcessor.Actors
{
    internal class PaymentWorkerActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IPaymentGateway _paymentGateway;
        public IStash Stash
        {
            get; set;
        }

        private ICancelable _unstashSchedule;

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;

            Receive<SendPaymentMessage>(message => HandleSendPayment(message));

            Receive<ProcessStashedPaymentsMessage>(message => HandleUnstashed());

            
        }

        private void HandleUnstashed()
        {
            if (!PeakTimeDemoSimulator.IsPeakHours)
            {
                Console.WriteLine("Not in peak hours so unstashing");
                Stash.UnstashAll();
            }
        }       

        

        private void HandleSendPayment(SendPaymentMessage message)
        {
           

            if (message.Amount > 100 && PeakTimeDemoSimulator.IsPeakHours)
            {
                Console.WriteLine("Stashing payment for {0} {1}", message.FirstName, message.AccountNumber);
                Stash.Stash();
            }
            else
            {
                Console.WriteLine("Sending payment for {0} {1}", message.FirstName, message.AccountNumber);
                _paymentGateway.Pay(message.AccountNumber, message.Amount);

                Sender.Tell(new PaymentSentMessage(message.AccountNumber));
            }
            
        }

        protected override void PreStart()
        {
            _unstashSchedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1),
                Self,
                new ProcessStashedPaymentsMessage(),
                Self);
        }

        protected override void PostStop()
        {
            _unstashSchedule.Cancel();
        }
    }
}
