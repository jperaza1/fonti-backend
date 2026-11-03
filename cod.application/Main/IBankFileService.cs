using cod.application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main
{
    public interface IBankFileService
    {
        byte[] GenerateTxt(ProviderPaymentFileRequest request);
    }
}
