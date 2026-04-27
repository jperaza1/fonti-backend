using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Common
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CafeteriaId { get; set; }
        public string Cafeteria { get; set; }
        public bool IsActive { get; set; }
        public bool UseInInventory { get; set; }
    }
}
