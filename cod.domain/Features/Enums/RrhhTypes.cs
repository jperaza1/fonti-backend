using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum GenderType
        {
            [Description("Masculino")]
            Male = 1,
            [Description("Femenino")]
            Female
        }

        public enum MaritalStatusType
        {
            [Description("Soltero")]
            Single = 1,
            [Description("Casado")]
            Married,
            [Description("Unión Libre")]
            Cohabitation
        }

        public enum HousingType
        {
            [Description("Propia")]
            Owned = 1,
            [Description("Alquilada")]
            Rented,
            [Description("Familiar")]
            Family
        }

        public enum EducationLevelType
        {
            [Description("Primaria")]
            Primary = 1,
            [Description("Secundaria")]
            Secondary,
            [Description("Universidad")]
            University
        }
        public enum EmploymentType
        {
            [Description("Permanente")]
            FullTime = 1,
            [Description("Temporal")]
            Temporary
        }

        public enum EmploymentStatusType
        {
            [Description("Activo")]
            Active = 1,
            [Description("Despedido")]
            Terminated,
            [Description("Renunció")]
            Resigned
        }

        public enum ShiftType
        {
            [Description("Turno A")]
            TurnoA = 1,
            [Description("Turno B")]
            TurnoB,
            [Description("Turno C")]
            TurnoC
        }

        public enum PayFrequencyType
        {
            [Description("Semanal")]
            Weekly = 1,
            [Description("Quincenal")]
            BiWeekly,
        }
    }
}
