using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetAndobaStatus;
using cod.application.EnumDefinition.Queries.GetApplicationStatus;
using cod.application.EnumDefinition.Queries.GetCarrierType;
using cod.application.EnumDefinition.Queries.GetCheckListStatus;
using cod.application.EnumDefinition.Queries.GetDigitalDocumentType;
using cod.application.EnumDefinition.Queries.GetDocumentType;
using cod.application.EnumDefinition.Queries.GetDriverType;
using cod.application.EnumDefinition.Queries.GetDriverTypeOperator;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Queries.GetEscrowBy;
using cod.application.EnumDefinition.Queries.GetFilterDriverApplicationReport;
using cod.application.EnumDefinition.Queries.GetFuelType;
using cod.application.EnumDefinition.Queries.GetIdentificationType;
using cod.application.EnumDefinition.Queries.GetLanguageType;
using cod.application.EnumDefinition.Queries.GetPayBy;
using cod.application.EnumDefinition.Queries.GetPaymentFilterDate;
using cod.application.EnumDefinition.Queries.GetPaymentStatus;
using cod.application.EnumDefinition.Queries.GetPersonType;
using cod.application.EnumDefinition.Queries.GetPurchaseOrderStatus;
using cod.application.EnumDefinition.Queries.GetTaskFilterCriteria;
using cod.application.EnumDefinition.Queries.GetTaskStatus;
using cod.application.EnumDefinition.Queries.GetTransportZone;
using cod.application.EnumDefinition.Queries.GetTruckType;
using cod.application.EnumDefinition.Queries.GetTypeApplicationTruck;
using cod.application.EnumDefinition.Queries.GetUserAlertOption;
using cod.application.EnumDefinition.Queries.GetUserType;
using cod.application.EnumDefinition.Queries.GetWeightType;
using cod.application.EnumDefinition.Queries.PhoneCallType;
using cod.domain.Features.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.data.Controllers
{
    [ApiController]
    public class EnumDefinitionController : BaseController
    {
        private readonly IGetUserTypeQuery _getUserTypeQuery;
        private readonly IGetPersonTypeQuery _getPersonTypeQuery;
        private readonly IGetIdentificationTypeQuery _getIdentificationTypeQuery;
        private readonly IGetDriverType _getDriverType;
        private readonly IGetCheckListStatus _getCheckListStatus;
        private readonly IGetApplicationStatus _getApplicationStatus;
        private readonly IGetFilterDriverApplicationReport _getFilterDriverApplicationReport;
        private readonly IGetPayByQuery _getPayByQuery;
        private readonly IGetEscrowByQuery _getEscrowByQuery;
        private readonly IGetWeightTypeQuery _getWeightType;
        private readonly IGetDriverTypeOperatorQuery _getDriverTypeOperatorQuery;
        private readonly IGetDocumentTypeQuery _getDocumentTypeQuery;
        private readonly IGetUserAlertOptionQuery _getUserAlertOptionQuery;
        private readonly ILeadCustomerType _leadCustomerType;
        private readonly IPhoneCallTypeQuery _phoneCallTypeQuery;
        private readonly IGetCarrierType _getCarrierType;
        private readonly IGetTaskFilterCriteriaQuery _getTaskFilterCriteriaQuery;
        private readonly IGetDigitalDocumentTypeQuery _getDigitalDocumentTypeQuery;
        private readonly IGetFuelTypeQuery _getFuelTypeQuery;
        private readonly IGetPaymentStatusQuery _getPaymentStatusQuery;
        private readonly IGetLanguageTypeQuery _getLanguageTypeQuery;
        private readonly IGetAndobaStatusQuery _getAndobaStatusQuery;
        private readonly IGetTaskStatusQuery _getTaskStatusQuery;
        private readonly IGetTruckTypeQuery _getTruckTypeQuery;
        private readonly IGetTypeApplicationTruckQuery _getTypeApplicationTruckQuery;
        private readonly IGetTransportZoneQuery _getTransportZoneQuery;
        private readonly IGetPaymentFilterDateQuery _getPaymentFilterDateQuery;
        private readonly IGetEnumDefinitionValuesQuery<PurchaseOrderStatus> _getPurchaseOrderStatusQuery;
        private readonly IGetEnumDefinitionValuesQuery<DetailStatus> _getDetailStatusQuery;
        private readonly IGetEnumDefinitionValuesQuery<PaymentType> _getPaymentTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<InvoiceType> _getInvoiceTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<PaymentMethod> _getPaymentMethodQuery;
        private readonly IGetEnumDefinitionValuesQuery<SaleType> _getSaleTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<SpecialServiceStatus> _getSpecialServiceStatusQuery;
        private readonly IGetEnumDefinitionValuesQuery<GenderType> _getGenderTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<EmploymentStatusType> _getEmploymentStatusQuery;
        private readonly IGetEnumDefinitionValuesQuery<EmploymentType> _getEmploymentTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<ShiftType> _getShiftTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<PayFrequencyType> _getPayFrequencyQuery;
        private readonly IGetEnumDefinitionValuesQuery<DishType> _getDishTypeQuery;
        private readonly IGetEnumDefinitionValuesQuery<SaleTypeDish> _getSaleTypeDishQuery;
        private readonly IGetEnumDefinitionValuesQuery<VacationRequestType> _getRequestTypeQuery;
        public EnumDefinitionController(
            IGetUserTypeQuery getUserTypeQuery,
            IGetPersonTypeQuery getPersonTypeQuery,
            IGetIdentificationTypeQuery getIdentificationTypeQuery,
            IGetDriverType getDriverType,
            IGetCheckListStatus getCheckListStatus,
            IGetApplicationStatus getApplicationStatus,
            IGetFilterDriverApplicationReport getFilterDriverApplicationReport,
            IGetPayByQuery getPayByQuery,
            IGetEscrowByQuery escrowByQuery,
            IGetWeightTypeQuery getWeightType,
            IGetDriverTypeOperatorQuery getDriverTypeOperatorQuery,
            IGetUserAlertOptionQuery getUserAlertOptionQuery,
            IGetDocumentTypeQuery getDocumentTypeQuery,
            ILeadCustomerType leadCustomerType,
            IPhoneCallTypeQuery phoneCallTypeQuery,
            IGetCarrierType getCarrierType,
            IGetTaskFilterCriteriaQuery getTaskFilterCriteriaQuery,
            IGetDigitalDocumentTypeQuery getDigitalDocumentTypeQuery,
            IGetFuelTypeQuery getFuelTypeQuery,
            IGetPaymentStatusQuery getPaymentStatusQuery,
            IGetLanguageTypeQuery getLanguageTypeQuery,
            IGetAndobaStatusQuery getAndobaStatusQuery,
            IGetTaskStatusQuery getTaskStatusQuery,
            IGetTruckTypeQuery getTruckTypeQuery,
            IGetTypeApplicationTruckQuery getTypeApplicationTruckQuery,
            IGetTransportZoneQuery getTransportZoneQuery,
            IGetPaymentFilterDateQuery getPaymentFilterDateQuery,
            IGetEnumDefinitionValuesQuery<PurchaseOrderStatus> getPurchaseOrderStatusQuery,
            IGetEnumDefinitionValuesQuery<DetailStatus> getDetailStatusQuery,
            IGetEnumDefinitionValuesQuery<PaymentType> getPaymentTypeQuery,
            IGetEnumDefinitionValuesQuery<InvoiceType> getInvoiceTypeQuery,
            IGetEnumDefinitionValuesQuery<PaymentMethod> getPaymentMethodQuery,
            IGetEnumDefinitionValuesQuery<SaleType> getSaleTypeQuery,
            IGetEnumDefinitionValuesQuery<SpecialServiceStatus> getSpecialServiceStatusQuery,
            IGetEnumDefinitionValuesQuery<GenderType> getGenderTypeQuery,
            IGetEnumDefinitionValuesQuery<EmploymentStatusType> getEmploymentStatusQuery,
            IGetEnumDefinitionValuesQuery<EmploymentType> getEmploymentTypeQuery,
            IGetEnumDefinitionValuesQuery<ShiftType> getShiftTypeQuery,
            IGetEnumDefinitionValuesQuery<PayFrequencyType> getPayFrequencyQuery,
            IGetEnumDefinitionValuesQuery<DishType> getDishTypeQuery,
            IGetEnumDefinitionValuesQuery<SaleTypeDish> getSaleTypeDishQuery,
            IGetEnumDefinitionValuesQuery<VacationRequestType> getRequestTypeQuery)
        {
            _getUserTypeQuery = getUserTypeQuery;
            _getPersonTypeQuery = getPersonTypeQuery;
            _getIdentificationTypeQuery = getIdentificationTypeQuery;
            _getDriverType = getDriverType;
            _getCheckListStatus = getCheckListStatus;
            _getApplicationStatus = getApplicationStatus;
            _getFilterDriverApplicationReport = getFilterDriverApplicationReport;
            this._getPayByQuery = getPayByQuery;
            this._getEscrowByQuery = escrowByQuery;
            this._getWeightType = getWeightType;
            this._getDriverTypeOperatorQuery = getDriverTypeOperatorQuery;
            this._getDocumentTypeQuery = getDocumentTypeQuery;
            _getUserAlertOptionQuery = getUserAlertOptionQuery;
            _getCarrierType = getCarrierType;
            _phoneCallTypeQuery = phoneCallTypeQuery;
            _leadCustomerType = leadCustomerType;
            _getTaskFilterCriteriaQuery = getTaskFilterCriteriaQuery;
            _getDigitalDocumentTypeQuery = getDigitalDocumentTypeQuery;
            _getFuelTypeQuery = getFuelTypeQuery;
            _getPaymentStatusQuery = getPaymentStatusQuery;
            _getLanguageTypeQuery = getLanguageTypeQuery;
            _getAndobaStatusQuery = getAndobaStatusQuery;
            _getTaskStatusQuery = getTaskStatusQuery;
            _getTruckTypeQuery = getTruckTypeQuery;
            _getTypeApplicationTruckQuery = getTypeApplicationTruckQuery;
            _getTransportZoneQuery = getTransportZoneQuery;
            _getPaymentFilterDateQuery = getPaymentFilterDateQuery;
            _getPurchaseOrderStatusQuery = getPurchaseOrderStatusQuery;
            _getDetailStatusQuery = getDetailStatusQuery;
            _getPaymentTypeQuery = getPaymentTypeQuery;
            _getInvoiceTypeQuery = getInvoiceTypeQuery;
            _getPaymentMethodQuery = getPaymentMethodQuery;
            _getSaleTypeQuery = getSaleTypeQuery;
            _getSpecialServiceStatusQuery = getSpecialServiceStatusQuery;
            _getGenderTypeQuery = getGenderTypeQuery;
            _getEmploymentStatusQuery = getEmploymentStatusQuery;
            _getEmploymentTypeQuery = getEmploymentTypeQuery;
            _getShiftTypeQuery = getShiftTypeQuery;
            _getPayFrequencyQuery = getPayFrequencyQuery;
            _getDishTypeQuery = getDishTypeQuery;
            _getSaleTypeDishQuery = getSaleTypeDishQuery;
            _getRequestTypeQuery = getRequestTypeQuery;
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetSaleType()
        {
            return _getSaleTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPaymentMethod()
        {
            return _getPaymentMethodQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPaymentType()
        {
            return _getPaymentTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetInvoiceType()
        {
            return _getInvoiceTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPurchaseOrderStatus()
        {
            return _getPurchaseOrderStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetSpecialServiceStatus()
        {
            return _getSpecialServiceStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDetailStatus()
        {
            return _getDetailStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetEscrowBy()
        {
            return _getEscrowByQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPayBy()
        {
            return _getPayByQuery.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetWeightType()
        {
            return _getWeightType.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDriveTypeOperator()
        {
            return _getDriverTypeOperatorQuery.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDocumentType()
        {
            return _getDocumentTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetUserType()
        {
            return _getUserTypeQuery.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPersonType()
        {
            return _getPersonTypeQuery.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetIdentificationType()
        {
            return _getIdentificationTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDriverType()
        {
            return _getDriverType.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetCheckListStatus()
        {
            return _getCheckListStatus.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetApplicationStatus()
        {
            return _getApplicationStatus.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetFilterDriverApplicationReport()
        {
            return _getFilterDriverApplicationReport.Execute<int>();
        }
        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetAlerts()
        {
            return _getUserAlertOptionQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetCarrierType()
        {
            return _getCarrierType.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPhoneCallType()
        {
            return _phoneCallTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetLeadCustomerType()
        {
            return _leadCustomerType.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetTaskFilterCriteria()
        {
            return _getTaskFilterCriteriaQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDigitalDocumentType()
        {
            return _getDigitalDocumentTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetFuelType()
        {
            return _getFuelTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPaymentStatus()
        {
            return _getPaymentStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetLanguageType()
        {
            return _getLanguageTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetAndobaStatus()
        {
            return _getAndobaStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetTaskStatus()
        {
            return _getTaskStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetTruckType()
        {
            return _getTruckTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetTypeApplicationTruck()
        {
            return _getTypeApplicationTruckQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetTransportZone()
        {
            return _getTransportZoneQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPaymentFilterDate()
        {
            return _getPaymentFilterDateQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetGenderType()
        {
            return _getGenderTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetEmploymentStatus()
        {
            return _getEmploymentStatusQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetEmploymentType()
        {
            return _getEmploymentTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetShiftType()
        {
            return _getShiftTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetPayFrequency()
        {
            return _getPayFrequencyQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetDishType()
        {
            return _getDishTypeQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetSaleTypeDish()
        {
            return _getSaleTypeDishQuery.Execute<int>();
        }

        [HttpGet]
        public List<GetEnumDefinitionValuesModel<int>> GetRequestType()
        {
            return _getRequestTypeQuery.Execute<int>();
        }
    }
}

