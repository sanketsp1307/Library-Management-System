using Visitor_Security_Clearance_System.DTO;

namespace Visitor_Security_Clearance_System.Interface
{
    public interface IFunctionalityService
    {
        Task<bool> Login(string username, string password, string role);
        Task<PassModel> AddPass(PassModel passModel);
        Task<PassModel> UpdateApprovedStatusOfPass(string PassId, bool IsApproved);
        Task<List<VisitorModel>> GetVisitorsByStatusAndOfficeId(string officeId, bool acceptedOrNot);
    }
}
