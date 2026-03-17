using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cod.application.Documentation.Commands.CreateDocumentation;
using cod.application.Documentation.Commands.DeleteDocumentation;
using cod.application.Features.Models;

namespace cod.data.Controllers
{
    [Route("api/v2/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DocumentationController : BaseController
    {
        private readonly ICreateDocumentationCommand _createDocumentationCommand;
        private readonly IDeleteDocumentationCommand _deleteDocumentationCommand;

        public DocumentationController(
            ICreateDocumentationCommand createDocumentationCommand,
            IDeleteDocumentationCommand deleteDocumentationCommand)
        {
            _createDocumentationCommand = createDocumentationCommand;
            _deleteDocumentationCommand = deleteDocumentationCommand;
            this.ControllerName = "Documentation";
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentationModel model)
        {
            try
            {
                await _createDocumentationCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string entity, int entityId, string documentType, string fileName)
        {
            try
            {
                await _deleteDocumentationCommand.Execute(entity, entityId, documentType, fileName);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
