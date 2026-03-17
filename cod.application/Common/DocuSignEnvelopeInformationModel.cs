using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cod.application.Common
{
    [XmlRoot("DocuSignEnvelopeInformation", Namespace = "http://www.docusign.net/API/3.0")]
    public class DocuSignEnvelopeInformationModel
    {
        [XmlElement("EnvelopeStatus")]
        public EnvelopeStatus EnvelopeStatus { get; set; }
    }

    public class EnvelopeStatus
    {
        [XmlElement("EnvelopeID")]
        public string EnvelopeID { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("RecipientStatuses")]
        public RecipientStatusList RecipientStatuses { get; set; }
    }

    public class RecipientStatusList
    {
        [XmlElement("RecipientStatus")]
        public RecipientStatus[] Recipients { get; set; }
    }

    public class RecipientStatus
    {
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("RecipientId")]
        public string RecipientId { get; set; }
    }
}
