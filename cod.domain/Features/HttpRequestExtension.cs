using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace cod.domain.Features
{
    public static class HttpRequestExtension
    {
        public static int? GetCarrier(this HttpRequest httpRequest)
        {

            string carrierId = httpRequest.Headers["App.CarrierId"].ToString() ?? "";

            if (string.IsNullOrEmpty(carrierId) || carrierId.Contains("null"))
            {
                return null;
            }

            return Convert.ToInt32(carrierId);
        }
        public static int? GetTenant(this HttpRequest httpRequest)
        {

            string sosTenantId = httpRequest.Headers["App.TenantId"].ToString() ?? "";

            if (string.IsNullOrEmpty(sosTenantId) || sosTenantId.Contains("null"))
            {
                sosTenantId = httpRequest.Query["token"].ToString();
            }

            if (string.IsNullOrEmpty(sosTenantId) || sosTenantId.Contains("null"))
            {
                return null;
            }

            return Convert.ToInt32(sosTenantId);
        }

        public static int? GetCompany(this HttpRequest httpRequest)
        {

            string companyId = httpRequest.Headers["App.CompanyId"].ToString() ?? "";

            if (string.IsNullOrEmpty(companyId) || companyId.Contains("null"))
            {
                return null;
            }

            return Convert.ToInt32(companyId);
        }

        public static string GetTimeZone(this HttpRequest httpRequest)
        {

            string timeZone = httpRequest.Headers["Timezone"].ToString() ?? "America/Tegucigalpa";

            return timeZone;
        }
    }
}
