using cod.domain.Features.Enums;
using System;

namespace cod.application.Inventory.Common
{
    public class FilterInventory
    {
        public int? CafeteriaId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
