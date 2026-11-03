using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common
{
    public class ProviderPaymentFileRequest
    {
        public string FileNameWithoutExtension { get; set; } = "Proveedores";
        public string ProfileCode { get; set; } = "";
        public DateTime GeneratedAt { get; set; }
        public string BatchNumber { get; set; } = "";
        public string SourceBankCode { get; set; } = "028";
        public string SourceAccountType { get; set; } = "01";
        public string SourceBranch { get; set; } = "000";
        public string SourceOffice { get; set; } = "000";
        public string SourceAccount { get; set; } = "";
        public List<ProviderPaymentDetail> Details { get; set; } = new();
    }

    public class ProviderPaymentDetail
    {
        public string DestinationBankCode { get; set; } = "";
        public string DestinationAccountTypeCode { get; set; } = "01";
        public string DestinationBranch { get; set; } = "";
        public string DestinationOffice { get; set; } = "";
        public string DestinationAccount { get; set; } = "";
        public string AccountTypeText { get; set; } = "";
        public string BeneficiaryName { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public string Email { get; set; } = "";
    }

    public class ProviderPaymentDetailAtlantida
    {
        public string BankAccountHolder { get; set; } = "";
        public string IdentificationHolder { get; set; } = "01";
        public string DestinationAccount { get; set; } = "";
        public string AccountTypeText { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public string Email { get; set; } = "";
        public string BankAlias { get; set; }
        public string DestinationBranch { get; set; } = "";
        public string DestinationOffice { get; set; } = "";
    }
}
