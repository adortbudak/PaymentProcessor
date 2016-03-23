using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsProcessor.Messages
{
    class SendPaymentMessage
    {
        public int AccountNumber { get; private set; }

        public decimal Amount { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public SendPaymentMessage(string firstName, string lastName,int accountNumber,decimal amount)
        {
            AccountNumber = accountNumber;
            Amount = amount;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
