using Visitor_Security_Clearance_System.DTO;

namespace Visitor_Security_Clearance_System.Interface
{
    public interface IManagerService
    {
        Task<bool> IsManagerExist(string email);
        Task<ManagerModel> GetManagerByUId(string userId);
        Task<ManagerModel> AddManager(ManagerModel managerDTO);
        Task<ManagerModel> UpdateManager(ManagerModel managerDTO);
        Task<string> DeleteManagerByUId(string userId);
    }
}
