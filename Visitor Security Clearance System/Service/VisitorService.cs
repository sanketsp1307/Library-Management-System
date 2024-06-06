using SendGrid;
using SendGrid.Helpers.Mail;
using Visitor_Security_Clearance_System.Common;
using Visitor_Security_Clearance_System.CosmosDB;
using Visitor_Security_Clearance_System.DTO;
using Visitor_Security_Clearance_System.Entity;
using Visitor_Security_Clearance_System.Interface;

namespace Visitor_Security_Clearance_System.Service
{
    public class VisitorService : IVisitorService
    {
        private readonly ICosmoseDBService _iCosmoseDBService;
        public VisitorService(ICosmoseDBService iCosmoseDBService)
        {
            _iCosmoseDBService = iCosmoseDBService;
        }

        public async Task<VisitorModel> AddVisitorAndSendMailToManager(VisitorModel visitorModel)
        {
            VisitorEntity toAdd = new VisitorEntity();
            toAdd.Initialize(true, Credentials.VisitorDocumentType, "SanketUser");

            toAdd.OfficeId = visitorModel.OfficeId;
            toAdd.Name = visitorModel.Name;
            toAdd.Email = visitorModel.Email;
            toAdd.PhoneNumber = visitorModel.PhoneNumber;
            toAdd.Address = visitorModel.Address;
            toAdd.Purpose = visitorModel.Purpose;
            toAdd.CompanyName = visitorModel.CompanyName;
            toAdd.EntryTime = visitorModel.EntryTime;
            toAdd.ExitTime = visitorModel.ExitTime;

            VisitorEntity toMap = await _iCosmoseDBService.AddEntity(toAdd);

            await SendMailToManagerForApproval(toMap);

            VisitorModel toResponse = new VisitorModel()
            {
                UserId = toMap.UserId,
                OfficeId = toMap.OfficeId,
                Name = toMap.Name,
                Email = toMap.Email,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address,
                Purpose = toMap.Purpose,
                CompanyName = toMap.CompanyName,
                EntryTime = toMap.EntryTime,
                ExitTime = toMap.ExitTime
            };

            return toResponse;
        }

        public async Task SendMailToManagerForApproval(VisitorEntity visitorEntity)
        {
            ManagerEntity manager = await _iCosmoseDBService.GetManagerEntityByOfficeId(visitorEntity.OfficeId);

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("./EmailTemplate/ApprovalRequestToManager.html"))
            { 
                body = reader.ReadToEnd(); 
            }
            body = body.Replace("{VisitorName}", visitorEntity.Name).Replace("{CompanyName}", visitorEntity.CompanyName).Replace("{Purpose}",visitorEntity.Purpose);

            var apiKey = Credentials.EmailApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Credentials.SenderMail, "Visitor Security Clearance System");
            var to = new EmailAddress(manager.Email);
            var msg = MailHelper.CreateSingleEmail(from, to, "Request For Approval", body, body);

            var response = await client.SendEmailAsync(msg);

        }

        public async Task<string> DeleteVisitorByUId(string userId)
        {
            VisitorEntity toDelete = await _iCosmoseDBService.GetVisitorEntityByUId(userId);
            toDelete.Active = false;
            toDelete.Archived = true;

            await _iCosmoseDBService.UpdateEntity(toDelete);

            toDelete.Initialize(false, Credentials.VisitorDocumentType, "SanketUser");
            toDelete.Active = false;

            await _iCosmoseDBService.AddEntity(toDelete);

            return "Data Removed!!";
        }

        public async Task<VisitorModel> GetVisitorByUId(string userId)
        {
            VisitorEntity toMap = await _iCosmoseDBService.GetVisitorEntityByUId(userId);

            if (toMap == null)
            {
                return null;
            }

            VisitorModel toResponse = new VisitorModel()
            {
                UserId = toMap.UserId,
                OfficeId = toMap.OfficeId,
                Name = toMap.Name,
                Email = toMap.Email,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address,
                Purpose = toMap.Purpose,
                CompanyName = toMap.CompanyName,
                EntryTime = toMap.EntryTime,
                ExitTime = toMap.ExitTime
            };

            return toResponse;
        }

        public async Task<bool> IsVisitorExist(string email)
        {
            var response = await _iCosmoseDBService.IsVisitorExist(email);
            return response;
        }

        public async Task<VisitorModel> UpdateVisitor(VisitorModel visitorModel)
        {
            VisitorEntity toUpdate = await _iCosmoseDBService.GetVisitorEntityByUId(visitorModel.UserId);
            toUpdate.Active = false;
            toUpdate.Archived = true;
            await _iCosmoseDBService.UpdateEntity(toUpdate);

            toUpdate.Initialize(false, Credentials.VisitorDocumentType, "SanketUser");
            toUpdate.OfficeId = visitorModel.OfficeId;
            toUpdate.Name = visitorModel.Name;
            toUpdate.Email = visitorModel.Email;
            toUpdate.PhoneNumber = visitorModel.PhoneNumber;
            toUpdate.Address = visitorModel.Address;
            toUpdate.Purpose = visitorModel.Purpose;
            toUpdate.CompanyName = visitorModel.CompanyName;
            toUpdate.EntryTime = visitorModel.EntryTime;
            toUpdate.ExitTime = visitorModel.ExitTime;

            VisitorEntity toMap = await _iCosmoseDBService.AddEntity(toUpdate);

            VisitorModel toResponse = new VisitorModel()
            {
                UserId = toMap.UserId,
                OfficeId = toMap.OfficeId,
                Name = toMap.Name,
                Email = toMap.Email,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address,
                Purpose = toMap.Purpose,
                CompanyName = toMap.CompanyName,
                EntryTime = toMap.EntryTime,
                ExitTime = toMap.ExitTime
            };

            return toResponse;
        }
    }
}
