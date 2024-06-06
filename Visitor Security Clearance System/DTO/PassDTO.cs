namespace Visitor_Security_Clearance_System.DTO
{
    public class PassModel
    {
        public string UserId { get; set; }
        public string VisitorId { get; set; }
        public string ApprovedByManagerId { get; set; }
        public bool IsApproved { get; set; }
        public string PdfFileName { get; set; }
    }
}
