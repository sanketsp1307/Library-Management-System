using Library_Management_System.DTO;
using Library_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Xml.Linq;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly Container container;

        private string URi = "https://localhost:8081";
        private string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string DatabaseName = "Project1";
        private string ContainerName = "Library Management System";

        public MemberController()
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
        public async Task<IActionResult> AddMember(Member member)
        {
            string uniqueId = Guid.NewGuid().ToString();
            MemberEntity entity = new MemberEntity()
            {
                UId = uniqueId,
                id = uniqueId,
                DocumentType = "member",
                Version = 1,
                CreatedBy = "Sanket",
                CreatedOn = DateTime.Now,
                UpdatedBy = "Sanket",
                UpdatedOn = DateTime.Now,
                Active = true,
                Archived = false,
                Name =member.Name,
                DateofBirth = member.DateofBirth,
                Email = member.Email
                
                

            };
            MemberEntity entity2 = await container.CreateItemAsync(entity);
            MemberEntity Responce = new MemberEntity()
            {
                UId = entity2.UId,
                Name = entity2.Name,
                DateofBirth= entity2.DateofBirth,
                Email = entity2.Email
                

            };
            return Ok(Responce);
            }
        [HttpGet("{UId}")]
        public async Task<IActionResult> GetById(string UId)
        {
            MemberEntity toget = container.GetItemLinqQueryable<MemberEntity>(true).Where(x => x.UId == UId && x.Active && x.Archived == false).FirstOrDefault();
            if (toget == null)
            {
                return BadRequest();
            }
            MemberEntity Response = new MemberEntity()
            {
                UId = toget.UId,

                Name = toget.Name,
                DateofBirth= toget.DateofBirth,
                Email = toget.Email

            };
            return Ok(Response);
        }
        [HttpGet]
        public async Task<List<Member>> GetAllMember()
        {
            List<MemberEntity> toget = container.GetItemLinqQueryable<MemberEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Book").ToList();
            List<Member> members1 = new List<Member>();
            foreach (var member in toget)
            {
                Member member2= new Member();
                member2.UId = member.UId;
                member2.DateofBirth = member.DateofBirth;
                member2.Email = member.Email;
                members1.Add(member2);
            }
            return members1;
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateMember([FromBody] Member member)
        {
            var existingMember = container.GetItemLinqQueryable<MemberEntity>(true).Where(x => x.UId == member.UId && x.Active && !x.Archived && x.DocumentType == "Member").FirstOrDefault();



            existingMember.Archived = true;
            existingMember.Active = false;
            await container.ReplaceItemAsync(existingMember, existingMember.UId);
            existingMember.UId = Guid.NewGuid().ToString();
            existingMember.Archived = true;
            existingMember.Active = false;
            existingMember.DocumentType = "Member";
            existingMember.UpdatedBy = "Sanket";
            existingMember.UpdatedOn = DateTime.Now;
            existingMember.Version = existingMember.Version + 1;
            existingMember = await container.ReplaceItemAsync(existingMember, existingMember.UId);
            MemberEntity updatedEntity = new MemberEntity
            {
                Name = member.Name,
                DateofBirth = member.DateofBirth,
                Email = member.Email,

            };

            await container.CreateItemAsync(updatedEntity);

            return Ok(updatedEntity);
        }


    }


    }

