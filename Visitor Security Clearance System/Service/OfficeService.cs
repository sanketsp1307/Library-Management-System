using Visitor_Security_Clearance_System.Common;
using Visitor_Security_Clearance_System.CosmosDB;
using Visitor_Security_Clearance_System.DTO;
using Visitor_Security_Clearance_System.Entity;
using Visitor_Security_Clearance_System.Interface;

namespace Visitor_Security_Clearance_System.Service
{
    public class OfficeService : IOfficeService
    {
        private readonly ICosmoseDBService _iCosmoseDBService;
        public OfficeService(ICosmoseDBService iCosmoseDBService)
        {
            _iCosmoseDBService = iCosmoseDBService;
        }

        public async Task<OfficeModel> AddOffice(OfficeModel officeModel)
        {
            OfficeEntity toAdd = new OfficeEntity();
            toAdd.Initialize(true, Credentials.OfficeDocumentType, "SanketUser");

            toAdd.Organization = officeModel.Organization;
            toAdd.Role = Credentials.OfficeDocumentType;
            toAdd.Email = officeModel.Email;
            toAdd.Password = officeModel.Password;
            toAdd.PhoneNumber = officeModel.PhoneNumber;
            toAdd.Address = officeModel.Address;
            
            OfficeEntity toMap = await _iCosmoseDBService.AddEntity(toAdd);

            OfficeModel toResponse = new OfficeModel()
            {
                UserId = toMap.UserId,
                Organization = toMap.Organization,
                Role = toMap.Role,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address
            };

            return toResponse;
        }

        public async Task<string> DeleteOfficeByUId(string uId)
        {
            OfficeEntity toDelete = await _iCosmoseDBService.GetOfficeEntityByUId(uId);
            toDelete.Active = false;
            toDelete.Archived = true;

            await _iCosmoseDBService.UpdateEntity(toDelete);

            toDelete.Initialize(false, Credentials.OfficeDocumentType, "SanketUser");
            toDelete.Active = false;

            await _iCosmoseDBService.AddEntity(toDelete);

            return "Data Removed!!";
        }

        public async Task<OfficeModel> GetOfficeByUId(string uId)
        {
            OfficeEntity toMap = await _iCosmoseDBService.GetOfficeEntityByUId(uId);

            if (toMap == null)
            {
                return null;
            }

            OfficeModel toResponse = new OfficeModel()
            {
                UserId = toMap.UserId,
                Organization = toMap.Organization,
                Role = toMap.Role,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address
            };

            return toResponse;
        }

        public async Task<bool> IsOfficeExist(string email)
        {
            var response = await _iCosmoseDBService.IsOfficeExist(email);
            return response;
        }

        public async Task<OfficeModel> UpdateOffice(OfficeModel officeModel)
        {
            OfficeEntity toUpdate = await _iCosmoseDBService.GetOfficeEntityByUId(officeModel.UserId);
            toUpdate.Active = false;
            toUpdate.Archived = true;
            await _iCosmoseDBService.UpdateEntity(toUpdate);

            toUpdate.Initialize(false, Credentials.OfficeDocumentType, "SanketUser");

            toUpdate.Organization = officeModel.Organization;
            toUpdate.Role = officeModel.Role;
            toUpdate.Email = officeModel.Email;
            toUpdate.Password = officeModel.Password;
            toUpdate.PhoneNumber = officeModel.PhoneNumber;
            toUpdate.Address = officeModel.Address;

            OfficeEntity toMap = await _iCosmoseDBService.AddEntity(toUpdate);

            OfficeModel toResponse = new OfficeModel()
            {
                UserId = toMap.UserId,
                Organization = toMap.Organization,
                Role = toMap.Role,
                Email = toMap.Email,
                Password = toMap.Password,
                PhoneNumber = toMap.PhoneNumber,
                Address = toMap.Address
            };

            return toResponse;
        }
    }
}
