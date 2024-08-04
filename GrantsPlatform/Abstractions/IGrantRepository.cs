using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;

namespace GrantsPlatform.Abstractions
{
    public interface IGrantRepository
    {
        Task<IEnumerable<GrantDto>> GetGrantsAsync(int? page = null, int pageSize = 20);

        Task<GrantDto?> GetGrantByIdAsync(int id);

        Task<FilterMappingDto> GetFilterMappingsAsync();

        Task<Meta> GetMetaAsync(int? page = null, int pageSize = 20);
    }
}
