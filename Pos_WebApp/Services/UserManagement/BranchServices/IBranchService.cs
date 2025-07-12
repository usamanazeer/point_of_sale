using Models;
using Models.DTO.UserManagement;
using Models.DTO.ViewModels.SelectList.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_WebApp.Services.UserManagement.BranchServices
{
    public interface IBranchService
    {
        Task<BranchDto> Get(string tOKEN, BranchDto model);
        Task<BranchDto> Create(string token, BranchDto model);
        Task<Response> Delete(string token, int id);
        Task<BranchDto> Edit(string token, BranchDto model);
        Task<BranchDto> Details(string token, int id);
        Task<IList<Branch_SLM>> GetSelectList(string tOKEN, BranchDto model = null);
        Task<Response> GetSelectListResponse(string tOKEN, BranchDto model = null);
    }
}
