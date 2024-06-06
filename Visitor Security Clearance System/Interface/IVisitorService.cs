using Visitor_Security_Clearance_System.DTO;

namespace Visitor_Security_Clearance_System.Interface
{
    public interface IVisitorService
    {
        Task<bool> IsVisitorExist(string email);
        Task<VisitorModel> GetVisitorByUId(string userId);
        Task<VisitorModel> AddVisitorAndSendMailToManager(VisitorModel visitorDTO);
        Task<VisitorModel> UpdateVisitor(VisitorModel visitorDTO);
        Task<string> DeleteVisitorByUId(string userId);
    }
}
