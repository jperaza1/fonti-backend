using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum TypeApplicationTruck
        {
            Original = 1,
            Renewal,
            AddFleet,
            AddVehicle,
            Transfer,
            FleetToFleetTransfer,
            IncreaseWeight,
            Delete
        }
    } 
    
}
