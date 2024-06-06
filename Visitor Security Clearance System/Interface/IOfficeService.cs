using Visitor_Security_Clearance_System.DTO;

namespace Visitor_Security_Clearance_System.Interface
{
    public interface IOfficeService
    {
        Task<bool> IsOfficeExist(string email);
        Task<OfficeModel> GetOfficeByUId(string userId);
        Task<OfficeModel> AddOffice(OfficeModel OfficeDTO);
        Task<OfficeModel> UpdateOffice(OfficeModel OfficeDTO);
        Task<string> DeleteOfficeByUId(string userId);
    }
}
