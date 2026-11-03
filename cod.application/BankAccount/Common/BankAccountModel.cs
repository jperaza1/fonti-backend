using System.ComponentModel.DataAnnotations.Schema;

namespace cod.application.BankAccount.Common
{
    public class BankAccountModel
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
    }
}
