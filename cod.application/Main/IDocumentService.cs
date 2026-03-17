using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main
{
    public interface IDocumentService
    {
        Task<List<AttachmentModel>> GetAttachmentByTypeAndKeys(int? tenantId, string entity, string type, List<int> keys);
    }
}
