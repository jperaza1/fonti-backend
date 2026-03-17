using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum AndobaStatus
        {
            NotApplicable= 1,
            WorkWithAndoba,
            ExAndoba,
            NoRecommendation,
            LastRecomendation,
            Transfer
        }
    }
}
