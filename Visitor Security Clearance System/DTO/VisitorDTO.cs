namespace Visitor_Security_Clearance_System.DTO
{
    public class VisitorModel
    {
        public string UserId { get; set; }
        public string OfficeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
    }
}
