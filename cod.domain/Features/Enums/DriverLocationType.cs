using System;
using System.Collections.Generic;
using System.Text;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum DriverLocationType
        {
            General = 1,
            UpdateDriver = 2,
            UpdateContainer = 3,
            UpdateStatus = 4,
            UploadHazardous = 5,
            UploadBonded = 6,
            UpdateChassis = 7,
            UpdateBillOfLading = 8,
            OnDuty = 9,
            OffDuty = 10,
            UpdateGpsApp = 11
        }
    }
}
