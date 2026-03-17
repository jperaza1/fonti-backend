using cod.application.Interfaces;
using cod.application.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.infraestructure.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHttpClientService _httpClientService;

        public DocumentService(IConfigurationService configurationService,
            IHttpClientService httpClientService)
        {
            _configurationService = configurationService;
            _httpClientService = httpClientService;
        }

        public async Task<List<AttachmentModel>> GetAttachmentByTypeAndKeys(int? tenantId, string entity, string type, List<int> keys)
        {
            try
            {
                string urlDocument = await _configurationService.Get<string>("URL_API_DOCUMENT");

                var documentResponse = await _httpClientService.PostResult<List<AttachmentModel>>($"{urlDocument}Attachment/GetAttachmentByTypeAndKeyList", new
                {
                    TenantId = tenantId,
                    Type = type,
                    Keys = keys,
                    Entity = entity
                });

                return documentResponse;

            }
            catch (Exception)
            {
                return new List<AttachmentModel>();
            }
        }
    }
}
