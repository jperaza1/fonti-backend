using cod.application.Employee.Commands.RequestVacation;
using cod.domain.Features.Enums;
using cod.domain.Rrhh;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Common
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public DateTime BirthDay { get; set; }
        public DomainEnums.GenderType Gender { get; set; }
        public string GenderName { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthPlace { get; set; }
        public string Profession { get; set; }
        public string Address { get; set; }
        public string HasChronicCondition { get; set; }
        public string ChronicConditionDetails { get; set; }
        public string HealthDisclosureDetails { get; set; }
        public DomainEnums.MaritalStatusType MaritalStatus { get; set; }
        public string SpouseFullName { get; set; }
        public DateTime? SpouseBirthDay { get; set; }
        public bool HasChildren { get; set; }
        public DomainEnums.HousingType HousingType { get; set; }
        public int YearsAtResidence { get; set; }
        public string MotherFullName { get; set; }
        public DateTime? MotherBirthDay { get; set; }
        public string MotherProfession { get; set; }
        public string FatherFullName { get; set; }
        public DateTime? FatherBirthDay { get; set; }
        public string FatherProfession { get; set; }
        public DomainEnums.EducationLevelType EducationLevel { get; set; }
        public string EducationField { get; set; }
        public string EducationInstitution { get; set; }
        public bool IsCurrentlyStudying { get; set; }
        public string CurrentStudyInstitution { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string EmergencyContactAddress { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public bool HasFamilyAtCompany { get; set; }
        public DomainEnums.EmploymentType EmploymentType { get; set; }
        public string EmploymentTypeName { get; set; }
        public DomainEnums.EmploymentStatusType EmploymentStatus { get; set; }
        public string EmploymentStatusName { get; set; }
        public DomainEnums.ShiftType Shift { get; set; }
        public string ShiftName { get; set; }
        public DomainEnums.PayFrequencyType PayFrequency { get; set; }
        public string PayFrequencyName { get; set; }
        public int TotalVacation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PermanentStartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public double Salary { get; set; }
        public int PositionId { get; set; }
        public string Position { get; set; }
        public int CafeteriaId { get; set; }
        public string Cafeteria { get; set; }
        public int UniformSizeId { get; set; }
        public List<VacationPeriodModel> VacationPeriod { get; set; }
        public List<RequestVacationModel> VacationRequested { get; set; }
    }
    
    public class VacationPeriodModel
    {
        public int Id { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int ServiceYear { get; set; }
        public int EntitledDays { get; set; }
    }

}
