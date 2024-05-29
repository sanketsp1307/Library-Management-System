using Library_Management_System.DTO;
using Library_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly Container container;

        private string URi = "https://localhost:8081";
        private string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string DatabaseName = "Project1";
        private string ContainerName = "Library Management System";

        public BookController()
        {
            container = GetContainer();
        }

        private Container GetContainer()
        {
            CosmosClient cosmosclient = new CosmosClient(URi, PrimaryKey);
            Database database = cosmosclient.GetDatabase(DatabaseName);
            Container cotainer = database.GetContainer(ContainerName);
            return cotainer;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            string uniqueId = Guid.NewGuid().ToString();
            BookEntity entity = new BookEntity()
            {
                UId = uniqueId,
                id = uniqueId,
                DocumentType = "Book",
                Version = 1,
                CreatedBy = "Sanket",
                CreatedOn = DateTime.Now,
                UpdatedBy = "Sanket",
                UpdatedOn = DateTime.Now,
                Active = true,
                Archived = false,
                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                Isbn = book.Isbn,
                Isissued = book.Isissued

            };
            BookEntity entity2 = await container.CreateItemAsync(entity);
            BookEntity Responce = new BookEntity()
            {
                UId = entity2.UId,
                Title = entity2.Title,
                Author = entity2.Author,
                PublishedDate = DateTime.Now,
                Isbn = entity2.Isbn,
                Isissued = entity2.Isissued,

            };
            return Ok(Responce);

        }

        [HttpGet("{UId}")]
        public async Task<IActionResult> GetById(string UId)
        {
            BookEntity toget = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.UId == UId && x.Active && x.Archived == false).FirstOrDefault();
            if (toget == null)
            {
                return BadRequest();
            }
            BookEntity Response = new BookEntity()
            {
                UId = toget.UId,
                Title = toget.Title,
                Author = toget.Author,
                PublishedDate = DateTime.Now,
                Isbn = toget.Isbn,
                Isissued = toget.Isissued,

            };
            return Ok(Response);
        }
        [HttpGet("{Name}")]
        public async Task<IActionResult> FindbookByName(string Name)
        {
            BookEntity FindbookByName = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Title == Name && x.Active && x.Archived == false).FirstOrDefault();
            if (FindbookByName == null)
            {
                return BadRequest();
            }
            BookEntity Response = new BookEntity()
            {
                UId = FindbookByName.UId,
                Title = FindbookByName.Title,
                Author = FindbookByName.Author,
                PublishedDate = DateTime.Now,
                Isbn = FindbookByName.Isbn,
                Isissued = FindbookByName.Isissued,

            };
            return Ok(Response);
        }

        [HttpGet]
        public async Task<List<Book>> GetAllBook()
        {
            List<BookEntity> toget = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Book").ToList();
            List<Book> book1 = new List<Book>();
            foreach (var book in toget)
            {
                Book book2 = new Book();
                book2.UId = book.UId;
                book2.Author = book.Author;
                book2.Title = book.Title;
                book2.PublishedDate = book.PublishedDate;
                book2.Isbn = book.Isbn;
                book2.Isissued = book.Isissued;
                book1.Add(book2);
            }
            return book1;
        }
        [HttpPut]
        public async Task<Book> Updatebook(Book book)
        {

            var existinhbook = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.UId == book.UId && x.Active && x.Archived == false && x.DocumentType == "Book").FirstOrDefault();
            existinhbook.Archived = true;
            existinhbook.Active = false;
            await container.ReplaceItemAsync(existinhbook, existinhbook.UId);

            existinhbook.UId = Guid.NewGuid().ToString();
            existinhbook.UpdatedBy = "Sanket";
            existinhbook.UpdatedOn = DateTime.Now;
            existinhbook.Version = existinhbook.Version + 1;
            existinhbook.Active = true;
            existinhbook.Archived = false;

            existinhbook.Title = book.Title;
            existinhbook.Author = book.Author;
            existinhbook.PublishedDate = book.PublishedDate;
            existinhbook.Isbn = book.Isbn;
            existinhbook.Isissued = book.Isissued;

            existinhbook = await container.ReplaceItemAsync(existinhbook, existinhbook.UId);

            Book response = new Book();

            response.Title = book.Title;
            response.Author = book.Author;
            response.PublishedDate = book.PublishedDate;
            response.Isbn = book.Isbn;
            response.Isissued = book.Isissued;
            return response;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllNotIssuedBook()
        {
            List<BookEntity> toget = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Member").ToList();

            List<Book> books = new List<Book>();

            foreach (var book in toget)
            {
                Book toAdd = new Book()
                {
                    UId = book.UId,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    Isbn = book.Isbn,
                    Isissued = book.Isissued
                };

                books.Add(toAdd);
            }

            return Ok(books);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllIssuedBook()
        {
            var Getbooks = container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Member").ToList();

            List<Book> books = new List<Book>();

            foreach (var book in Getbooks)
            {
                Book toAdd = new Book()
                {
                    UId = book.UId,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    Isbn = book.Isbn,
                    Isissued = book.Isissued
                };

                books.Add(toAdd);
            }

            return Ok(books);
        }

    }
}
        
    
        





