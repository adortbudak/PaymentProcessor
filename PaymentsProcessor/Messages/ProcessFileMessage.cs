using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsProcessor.Messages
{
    class ProcessFileMessage
    {
        public string FileName { get; private set; }

        public ProcessFileMessage(string fileName)
        {
            FileName = fileName;
        }
    }
}
