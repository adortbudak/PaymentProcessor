using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using PaymentsProcessor.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsProcessor.Actors
{
    internal class JobCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _paymentWorker;
        private int _numberOfRemainingPayments;
        public JobCoordinatorActor()
        {
            _paymentWorker = Context.ActorOf(Context.DI().Props<PaymentWorkerActor>(),"PaymentWorkers");              
                

            Receive<ProcessFileMessage>(
                message =>
                {
                    StartNewJob(message.FileName);
                });

            Receive<PaymentSentMessage>(
                message =>
                {
                    _numberOfRemainingPayments--;
                    var jobIsComplete = _numberOfRemainingPayments == 0;
                    if (jobIsComplete)
                    {
                        Context.System.Shutdown();
                    }
                });
        }

        private void StartNewJob(string fileName)
        {
            List<SendPaymentMessage> requests = ParseCvsFile(fileName);

            _numberOfRemainingPayments = requests.Count();

            foreach (var sendPaymentMessage in requests)
            {
                _paymentWorker.Tell(sendPaymentMessage);
                
            }
        }

        private List<SendPaymentMessage> ParseCvsFile(string fileName)
        {
            var messageToSend = new List<SendPaymentMessage>();

            var fileLines = File.ReadAllLines(fileName);

            foreach (var line in fileLines)
            {
                var values = line.Split(',');

                var message = new SendPaymentMessage(
                    values[0],
                    values[1],
                    int.Parse(values[3]),
                    decimal.Parse(values[2]));

                messageToSend.Add(message);
            }

            return messageToSend;
        }
    }
}
