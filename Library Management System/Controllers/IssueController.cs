using Library_Management_System.DTO;
using Library_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Library_Management_System.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IssueController : Controller
    {
        private readonly Container container;

        private string URi = "https://localhost:8081";
        private string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string DatabaseName = "Project1";
        private string ContainerName = "Library Management System";

        public IssueController()
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
        public async Task<IActionResult> AddIssue(Issue issue)
        {
            string uniqueId = Guid.NewGuid().ToString();
            IssueEntity entity = new IssueEntity()
            {
                UId = uniqueId,
                id = uniqueId,
                BookId = issue.BookId,
                MemberId = issue.MemberId,
                IssueDate = issue.IssueDate,
                ReturnDate = issue.ReturnDate,
                isReturned = issue.isReturned,
                Active = true,
                Archived = false,
                DocumentType = "Issue"
            };
            IssueEntity entity2 = await container.CreateItemAsync(entity);
            IssueEntity Responce = new IssueEntity()
            {
                UId = entity2.UId,
                BookId = entity2.BookId,

                MemberId = entity2.MemberId,
                IssueDate = entity2.IssueDate,
                ReturnDate = entity2.ReturnDate,
                isReturned = entity2.isReturned

            };

            return Ok(Responce);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateIssue(Issue issue)
        {

            var existingIssue = container.GetItemLinqQueryable<IssueEntity>(true).Where(x => x.UId == issue.UId && x.Active && !x.Archived && x.DocumentType == "Issue").FirstOrDefault();

            existingIssue.Archived = true;
            existingIssue.Active = false;
            await container.ReplaceItemAsync(existingIssue, existingIssue.UId);
            existingIssue.UId = Guid.NewGuid().ToString();

            existingIssue.Active = true;
            existingIssue.Archived = false;
            existingIssue.DocumentType = "Issue";
            existingIssue.UpdatedBy = "Sanket";
            existingIssue.UpdatedOn = DateTime.Now;
            existingIssue.Version = existingIssue.Version + 1;
            existingIssue = await container.ReplaceItemAsync(existingIssue, existingIssue.UId);

            IssueEntity updatedEntity = new IssueEntity();
            {
                updatedEntity.BookId = issue.BookId;
                updatedEntity.MemberId = issue.MemberId;
                updatedEntity.IssueDate = issue.IssueDate;
                updatedEntity.ReturnDate = issue.ReturnDate;
                updatedEntity.isReturned = issue.isReturned;

            };

            return Ok(updatedEntity);
        }
        [HttpGet("{UId}")]
        public IActionResult GetById(string UId)
        {
            var issueEntity = container.GetItemLinqQueryable<IssueEntity>(true)
                .Where(x => x.UId == UId && x.Active && !x.Archived && x.DocumentType == "Issue").FirstOrDefault();


            if (issueEntity == null)
            {
                return NotFound("Issue not found.");
            }

            return Ok(issueEntity);
        }

    }



}

        
    

    

       









