using Models;
using System;
using System.Threading.Tasks;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using POS_API.Repositories.UserManagement.RightsRepos;

namespace POS_API.Services.UserManagement.RightsServices
{
    public class RightsService : IRightsService, IService
    {
        private readonly IRightsRepository _rightsRepository;
        public RightsService(IRightsRepository rightsRepository) => _rightsRepository = rightsRepository;

        public async Task<bool> ChangeStatus(RightsDto model) => await _rightsRepository.ChangeStatus(model);

        [Obsolete]
        public async Task<Response> ClaimRight(RoleRightsDto model)
        {
            var response = new Response();
            var res = await _rightsRepository.ClaimRight(model);
            response.Model = res;
            return response;
        }
        public async Task<Response> ClaimRight1(RoleRightsDto model)
        {
            var response = new Response();
            var res = await _rightsRepository.ClaimRight1(model);
            response.Model = res;
            return response;
        }
        public async Task<RightsDto> Create(RightsDto model) => await _rightsRepository.Create(model);

        public async Task<RightsDto> Edit(RightsDto model) => await _rightsRepository.Edit(model);

        public async Task<IEnumerable<CompanyModulesDto>> GetAll(CompanyModulesDto model) => await _rightsRepository.GetAll(model);

        public async Task<IEnumerable<RightModel>> GetRightsByRole(RoleDto role) => await _rightsRepository.GetByRole(role);
    }
}
