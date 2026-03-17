using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum TaskStatusEnum
        {
            PendingToReview = 1,
            PendingAssignment,
            InProgress,
            WaitingAnswer,
            Closed,
            Archived,
            Late
        }
    }
    
}
