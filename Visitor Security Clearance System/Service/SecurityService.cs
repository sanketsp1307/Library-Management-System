using Visitor_Security_Clearance_System.Common;
using Visitor_Security_Clearance_System.CosmosDB;
using Visitor_Security_Clearance_System.DTO;
using Visitor_Security_Clearance_System.Entity;
using Visitor_Security_Clearance_System.Interface;

namespace Visitor_Security_Clearance_System.Service
{
    public class SecurityService : ISecurityService
    {
        private readonly ICosmoseDBService _iCosmoseDBService;
        public SecurityService(ICosmoseDBService iCosmoseDBService)
        {
            _iCosmoseDBService = iCosmoseDBService;
        }

        public async Task<SecurityModel> AddSecurity(SecurityModel securityModel)
        {
            SecurityEntity toAdd = new SecurityEntity();
            toAdd.Initialize(true, Credentials.SecurityDocumentType, "SanketUser");

            toAdd.Name = securityModel.Name;
            toAdd.Email = securityModel.Email;
            toAdd.Password = securityModel.Password;
            toAdd.PhoneNumber = securityModel.PhoneNumber;
            toAdd.Role = Credentials.SecurityDocumentType;

            SecurityEntity toMap = await _iCosmoseDBService.AddEntity(toAdd);

            SecurityModel toResponse = new SecurityModel()
            {
                UserId = toMap.UserId,
                Name = toMap.Name,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Role = toMap.Role,
            };

            return toResponse;
        }

        public async Task<string> DeleteSecurityByUId(string userId)
        {
            SecurityEntity toDelete = await _iCosmoseDBService.GetSecurityEntityByUId(userId);
            toDelete.Active = false;
            toDelete.Archived = true;

            await _iCosmoseDBService.UpdateEntity(toDelete);

            toDelete.Initialize(false, Credentials.SecurityDocumentType, "SanketUser");
            toDelete.Active = false;

            await _iCosmoseDBService.AddEntity(toDelete);

            return "Data Removed!!";
        }

        public async Task<SecurityModel> GetSecurityByUId(string uId)
        {
            SecurityEntity toMap = await _iCosmoseDBService.GetSecurityEntityByUId(uId);

            if (toMap == null)
            {
                return null;
            }

            SecurityModel toResponse = new SecurityModel()
            {
                UserId = toMap.UserId,
                Name = toMap.Name,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Role = toMap.Role,
            };

            return toResponse;
        }

        public async Task<bool> IsSecurityExist(string email)
        {
            var response = await _iCosmoseDBService.IsSecurityExist(email);
            return response;
        }

        public async Task<SecurityModel> UpdateSecurity(SecurityModel securityDTO)
        {
            SecurityEntity toUpdate = await _iCosmoseDBService.GetSecurityEntityByUId(securityDTO.UserId);
            toUpdate.Active = false;
            toUpdate.Archived = true;
            await _iCosmoseDBService.UpdateEntity(toUpdate);

            toUpdate.Initialize(false, Credentials.SecurityDocumentType, "SanketUser");
            toUpdate.Name = securityDTO.Name;
            toUpdate.Email = securityDTO.Email;
            toUpdate.Password = securityDTO.Password;
            toUpdate.PhoneNumber = securityDTO.PhoneNumber;
            toUpdate.Role = Credentials.SecurityDocumentType;

            SecurityEntity toMap = await _iCosmoseDBService.AddEntity(toUpdate);

            SecurityModel toResponse = new SecurityModel()
            {
                UserId = toMap.UserId,
                Name = toMap.Name,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Role = toMap.Role,
            };

            return toResponse;
        }
    }
}
