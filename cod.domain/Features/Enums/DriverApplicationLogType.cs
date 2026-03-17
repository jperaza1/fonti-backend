using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum DriverApplicationLogType
        {
            CompanyName = 1,
            Name,
            Phone,
            TestSms,
            DriverType,
            AttributeStatus,
            ApplicationStatus,
            Notes,
            EmailProvider,
            DriverApplicationDateTimes
        }
    }
}
