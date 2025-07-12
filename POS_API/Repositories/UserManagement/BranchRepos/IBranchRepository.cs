using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.UserManagement.BranchRepos
{
    public interface IBranchRepository
    {
        Task<IList<BranchDto>> GetAll(BranchDto model);
        Task<BranchDto> GetDetails(BranchDto model);
        Task<BranchDto> Create(BranchDto model);
        Task<BranchDto> Edit(BranchDto model);
        Task<bool> Delete(BranchDto model);
        Task<bool> IsExists(BranchDto model);
        Task<IList<Branch_SLM>> GetSelectList(BranchDto model);
    }
}
