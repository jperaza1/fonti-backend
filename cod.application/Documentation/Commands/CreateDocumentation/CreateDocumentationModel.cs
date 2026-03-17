namespace cod.application.Documentation.Commands.CreateDocumentation
{
    public class CreateDocumentationModel
    {
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public int ExternalId { get; set; }
    }
}