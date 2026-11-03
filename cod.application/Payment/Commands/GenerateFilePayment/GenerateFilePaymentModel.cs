using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.GenerateFilePayment
{
    public class GenerateFilePaymentModel
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
