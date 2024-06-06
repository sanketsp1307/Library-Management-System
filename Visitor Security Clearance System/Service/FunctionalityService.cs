using SendGrid.Helpers.Mail;
using SendGrid;
using Visitor_Security_Clearance_System.Common;
using Visitor_Security_Clearance_System.CosmosDB;
using Visitor_Security_Clearance_System.DTO;
using Visitor_Security_Clearance_System.Entity;
using Visitor_Security_Clearance_System.Interface;
using System;
using System.IO;
using iTextSharp.text.pdf;

namespace Visitor_Security_Clearance_System.Service
{
    public class FunctionalityService : IFunctionalityService
    {
        private readonly ICosmoseDBService _iCosmoseDBService;

        public FunctionalityService(ICosmoseDBService iCosmoseDBService)
        {
            _iCosmoseDBService = iCosmoseDBService;
        }

        public async Task<PassModel> AddPass(PassModel passModel)
        {
            PassEntity toAdd = new PassEntity();
            toAdd.Initialize(true, Credentials.PassDocumentType, "SanketUser");

            toAdd.VisitorId = passModel.VisitorId;
            toAdd.ApprovedByManagerId = passModel.ApprovedByManagerId;
            toAdd.IsApproved = passModel.IsApproved;

            PassEntity toMap = new PassEntity();

            if (passModel.IsApproved)
            {
                toAdd.PdfFileName = "Pass_"+ passModel.VisitorId + ".pdf";
                toMap = await _iCosmoseDBService.AddEntity(toAdd);

                var Visitor = await _iCosmoseDBService.GetVisitorEntityByUId(toMap.VisitorId);

                var pdfFilePath = await CreatePassPDF(Visitor, toMap);

                await SendMailToVisitor(Visitor, pdfFilePath);
            }
            else
            {
                toAdd.PdfFileName = null;
                toMap = await _iCosmoseDBService.AddEntity(toAdd);
            }

            PassModel response = new PassModel()
            {
                UserId = toMap.UserId,
                VisitorId =toMap.VisitorId,
                ApprovedByManagerId = toMap.ApprovedByManagerId,
                PdfFileName = toMap.PdfFileName,
                IsApproved = toMap.IsApproved,   
            };

            return response;
        }

        public async Task SendMailToVisitor(VisitorEntity visitorEntity, string pdfFilePath)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("./EmailTemplate/VisitApprovedToVisitor.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{VisitorName}", visitorEntity.Name).Replace("{Purpose}", visitorEntity.Purpose);

            var apiKey = Credentials.EmailApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Credentials.SenderMail, "Visitor Security Clearance System");
            var to = new EmailAddress(visitorEntity.Email);
            var msg = MailHelper.CreateSingleEmail(from, to, "Visit Request Approved", body, body);

            var attachment = new Attachment
            {
                Content = Convert.ToBase64String(File.ReadAllBytes(pdfFilePath)),
                Type = "application/pdf",
                Filename = Path.GetFileName(pdfFilePath),
                Disposition = "attachment"
            };
            msg.AddAttachment(attachment);

            var response = await client.SendEmailAsync(msg);

        }

        public async Task<string> CreatePassPDF(VisitorEntity visitorEntity, PassEntity passEntity)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Passes");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, passEntity.PdfFileName);
            


            return filePath;
        }

        public async Task<List<VisitorModel>> GetVisitorsByStatusAndOfficeId(string officeId, bool isApproved)
        {
            var PassEntities = await _iCosmoseDBService.GetPassEntitiesByApprovalStatus(officeId, isApproved);
            List<VisitorEntity> visitorEntities = await _iCosmoseDBService.GetVisitorEntities();

            List<VisitorModel> response = new List<VisitorModel>();

            foreach (var pass in PassEntities)
            {
                VisitorEntity entity = visitorEntities.Where(x => x.UserId == pass.VisitorId).FirstOrDefault();

                VisitorModel visitor = new VisitorModel()
                {
                    UserId = entity.UserId,
                    OfficeId = entity.OfficeId,
                    Name = entity.Name,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    Address = entity.Address,
                    Purpose = entity.Purpose,
                    CompanyName = entity.CompanyName,
                    EntryTime = entity.EntryTime,
                    ExitTime = entity.ExitTime
                };
                response.Add(visitor);
            }

            return response;
        }

        public async Task<bool> Login(string username, string password, string role)
        {
            if (role == Credentials.SecurityDocumentType)
            {
                return await _iCosmoseDBService.IsSecurityExist(username, password);
            }
            else if(role == Credentials.ManagerDocumentType)
            {
                return await _iCosmoseDBService.IsManagerExist(username, password);
            }
            else if (role == Credentials.OfficeDocumentType)
            {
                return await _iCosmoseDBService.IsOfficeExist(username, password);
            }

            return false;
        }

        public async Task<PassModel> UpdateApprovedStatusOfPass(string PassId, bool IsApproved)
        {
            PassEntity toUpdate = await _iCosmoseDBService.GetPassEntityByUId(PassId);
            toUpdate.Active = false;
            toUpdate.Archived = true;
            await _iCosmoseDBService.UpdateEntity(toUpdate);

            toUpdate.Initialize(false, Credentials.PassDocumentType, "SanketUser");
            toUpdate.IsApproved = IsApproved;

            PassEntity toMap = await _iCosmoseDBService.AddEntity(toUpdate);

            PassModel response = new PassModel()
            {
                UserId = toMap.UserId,
                VisitorId = toMap.VisitorId,
                ApprovedByManagerId = toMap.ApprovedByManagerId,
                PdfFileName = toMap.PdfFileName,
                IsApproved = toMap.IsApproved,
            };

            return response;

        }
    }
}
