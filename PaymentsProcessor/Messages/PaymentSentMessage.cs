using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsProcessor.Messages
{
    class PaymentSentMessage
    {
        public int AccountNumber { get; private set; }
        public PaymentSentMessage(int accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}
