using Models.DTO.InventoryManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_API.Repositories.InventoryManagement.ColorRepos
{
    public interface IColorRepository
    {
        Task<InvColorDto> Create(InvColorDto model);
        Task<InvColorDto> Edit(InvColorDto model);
        Task<List<InvColorDto>> GetAll(InvColorDto model);
        Task<bool> IsExist(InvColorDto model);
        Task<bool> Delete(InvColorDto model);
        Task<InvColorDto> GetDetails(InvColorDto model);
    }
}
