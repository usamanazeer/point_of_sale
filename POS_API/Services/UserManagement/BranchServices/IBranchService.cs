using Models;
using Models.DTO.UserManagement;
using System.Threading.Tasks;

namespace POS_API.Services.UserManagement.BranchServices
{
    public interface IBranchService
    {
        Task<Response> GetAll(BranchDto model);
        Task<Response> Create(BranchDto model);
        Task<bool> IsExist(BranchDto model);
        Task<Response> GetDetails(BranchDto model);
        Task<Response> Edit(BranchDto model);
        Task<bool> Delete(BranchDto model);
        Task<Response> GetSelectList(BranchDto model);
    }
}
