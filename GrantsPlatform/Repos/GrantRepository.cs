using GrantsPlatform.Abstractions;
using GrantsPlatform.Data;
using GrantsPlatform.Helpers;
using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;
using Microsoft.EntityFrameworkCore;

namespace GrantsPlatform.Repos
{
    public class GrantRepository : IGrantRepository
    {
        private readonly ApplicationDbContext _context;

        public GrantRepository(ApplicationDbContext db)
        {
            _context = db;
        }

        public async Task<FilterMappingDto> GetFilterMappingsAsync()
        {
            var filterMappings = await _context.FilterMappings.Include(x => x.Filter)
                .ToListAsync();
            return filterMappings.ToDto();
        }

        public async Task<IEnumerable<GrantDto>> GetGrantsAsync(int? page = null, int pageSize = 20)
        {
            if (page == null)
            {
                var grants = await _context.Grants.ToListAsync();
                return grants.Select(x => x.ToDto());
            }
            var pageGrants = await _context.Grants.OrderBy(x => x.Id)
                .Skip((page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return pageGrants.Select(x => x.ToDto());
        }

        public async Task<GrantDto?> GetGrantByIdAsync(int id)
        {
            var grant = await _context.Grants.FirstOrDefaultAsync(x => x.Id == id);
            return grant?.ToDto();
        }

        public async Task<Meta> GetMetaAsync(int? page = null, int pageSize = 20)
        {
            var grantsCount = await _context.Grants.CountAsync();
            var pagesCount = grantsCount / pageSize;
            if (grantsCount % pageSize != 0)
            {
                pagesCount++;
            }
            var meta = new Meta()
            {
                CurrentPage = page.GetValueOrDefault(),
                TotalPages = pagesCount
            };
            return meta;
        }
    }
}
