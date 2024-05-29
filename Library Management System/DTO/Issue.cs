namespace Library_Management_System.DTO
{
    public class Issue
    {
        public string UId { get; set; }
        public string BookId { get; set; }
        public string MemberId { get; set; }
       
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public Boolean isReturned { get; set; }
    }
}
