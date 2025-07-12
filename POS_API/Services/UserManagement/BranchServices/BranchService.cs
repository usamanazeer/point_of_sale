using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.UserManagement;
using Models.Enums;
using POS_API.Repositories.UserManagement.BranchRepos;

namespace POS_API.Services.UserManagement.BranchServices
{
    public class BranchService : IBranchService, IService
    {
        private readonly IBranchRepository _branchRepository;
        public BranchService(IBranchRepository branchRepository) => _branchRepository = branchRepository;

        public async Task<Response> GetAll(BranchDto model)
        {
            var response = new Response();
            var res = await _branchRepository.GetAll(model: model);
            if (res.Any())
            {
                response.Model = res;
                response.ResponseCode =  StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode =  StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Modifier Found.";
            }
            return response;
        }

        public async Task<Response> GetDetails(BranchDto model)
        {
            var response = new Response();
            var res = await _branchRepository.GetDetails(model: model);
            if (res != null)
            {
                response.Model = res;
                response.ResponseCode =  StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode =  StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "Branch Not Found";
            }
            return response;
        }

        public async Task<Response> Create(BranchDto model)
        {
            var response = new Response();
            var isExists = await IsExist(model: model);
            if (!isExists)
            {
                response.Model = await _branchRepository.Create(model: model);
                response.ResponseCode =  StatusCodes.Created.ToInt();
                response.ResponseMessage = "Branch Created Successfully.";
                return response;
            }
            response.Model = model;
            response.ErrorCode =  StatusCodes.Error_Occured.ToInt();
            response.ErrorMessage = "Branch Already Exists.";
            return response;
        }

        public async Task<bool> Delete(BranchDto model) => await _branchRepository.Delete(model: model);

        public async Task<Response> Edit(BranchDto model)
        {
            var response = new Response();
            var isExists = await IsExist(model: model);
            if (!isExists)
            {
                response.Model = await _branchRepository.Edit(model: model);
                if (response.Model != null)
                {
                    response.ResponseCode =  StatusCodes.Updated.ToInt();
                    response.ResponseMessage = "Branch Updated Successfully.";
                }
                else
                {
                    response.ResponseCode =  StatusCodes.Not_Found.ToInt();
                    response.ResponseMessage = "Branch Not Found.";
                }
            }
            else
            {
                response.Model = model;
                response.ErrorCode =  StatusCodes.Error_Occured.ToInt();
                response.ErrorMessage = "Branch Already Exists.";
            }

            return response;
        }

        public async Task<bool> IsExist(BranchDto model) => await _branchRepository.IsExists(model: model);

        public async Task<Response> GetSelectList(BranchDto model)
        {
            var response = new Response();
            var itemsList = await _branchRepository.GetSelectList(model: model);
            if (itemsList != null)
            {
                response.Model = itemsList;
                response.ResponseCode =  StatusCodes.OK.ToInt();
            }
            else
            {
                response.ResponseCode =  StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "Branches Not Found";
            }

            return response;
        }
    }
}