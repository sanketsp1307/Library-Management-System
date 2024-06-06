using Newtonsoft.Json;
using Visitor_Security_Clearance_System.Common;

namespace Visitor_Security_Clearance_System.Entity
{
    public class PassEntity : BaseEntity
    { 
        [JsonProperty(PropertyName = "visitorId", NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorId { get; set; }

        [JsonProperty(PropertyName = "ApprovalManager", NullValueHandling = NullValueHandling.Ignore)]
        public string ApprovedByManagerId { get; set; }

        [JsonProperty(PropertyName = "isApproved", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsApproved { get; set; }

        [JsonProperty(PropertyName = "FileName", NullValueHandling = NullValueHandling.Ignore)]
        public string PdfFileName { get; set; }
    }
}
