using Newtonsoft.Json;

namespace Library_Management_System.DTO
{
    public class Book
    {
        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]

        public string UId { get; set; }


        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Isbn { get; set; }
        public Boolean Isissued { get; set; }
    }
}
