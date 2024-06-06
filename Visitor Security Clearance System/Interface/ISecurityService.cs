using Visitor_Security_Clearance_System.DTO;

namespace Visitor_Security_Clearance_System.Interface
{
    public interface ISecurityService
    {
        Task<bool> IsSecurityExist(string email);
        Task<SecurityModel> GetSecurityByUId(string userId);
        Task<SecurityModel> AddSecurity(SecurityModel securityDTO);
        Task<SecurityModel> UpdateSecurity(SecurityModel securityDTO);
        Task<string> DeleteSecurityByUId(string uerId);
    }
}
