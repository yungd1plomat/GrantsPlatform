using GrantsPlatform.Abstractions;
using GrantsPlatform.Data;
using GrantsPlatform.Helpers;
using GrantsPlatform.Models.Viewmodels;
using GrantsPlatform.Models.Viewmodels.Grant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GrantsPlatform.Controllers
{
    [Route("/admin/api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GrantsController : ControllerBase
    {
        const int grants_per_page = 10;

        private readonly IGrantRepository _grantRepository;
        
        private readonly ApplicationDbContext _context;

        public GrantsController(IGrantRepository grantRepository, ApplicationDbContext context) 
        {
            _grantRepository = grantRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Grants([FromQuery] int? page = null)
        {
            var grants = await _grantRepository.GetGrantsAsync(page, grants_per_page);
            var filtersMapping = await _grantRepository.GetFilterMappingsAsync();
            var meta = await _grantRepository.GetMetaAsync(page, grants_per_page);

            filtersMapping.Filters = filtersMapping.Filters.OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var filters = filtersMapping.Filters.Select(x => x.Key)
                .Order()
                .ToList();

            var grantsResponse = new GrantsResponse()
            {
                Grants = grants,
                FiltersMapping = filtersMapping,
                FiltersOrder = filters,
                Meta = meta
            };
            return Ok(grantsResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Grant(int id)
        {
            var grant = await _grantRepository.GetGrantByIdAsync(id);
            if (grant == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Грант не найден"
                });
            }
            var filtersMapping = await _grantRepository.GetFilterMappingsAsync();

            filtersMapping.Filters = filtersMapping.Filters.OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var filters = filtersMapping.Filters.Select(x => x.Value.Title)
                .Order()
                .ToList();
            var grantByIdResponse = new GrantByIdResponse()
            {
                Grant = grant,
                FiltersMapping = filtersMapping,
                FiltersOrder = filters,
            };
            return Ok(grantByIdResponse);
        }

        [HttpPut("{id}/filters")]
        public async Task<IActionResult> Filters(int id, [FromBody] GrantFiltersRequest? filters)
        {
            if (filters == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Фильтры не указаны"
                });
            }
            var grantDto = await _grantRepository.GetGrantByIdAsync(id);
            if (grantDto == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Грант не найден"
                });
            }
            grantDto.FilterValues = filters.Filters;
            var grant = grantDto.FromDto();

            // Возникает конфликт ошибки сущностей, поэтому сначала отсоединим отслеживаемую сущность
            // Ошибка возникает из-за конверсии FromDto, т.к в этом методе создается новая сущность
            // с тем же Id, что и отслеживается
            // Как решение можно использовать AsNoTracking, но тогда придется вручную управлять
            // всеми изменениями сущности
            var existingGrant = _context.Grants.Local.FirstOrDefault(e => e.Id == grantDto.Id);
            if (existingGrant != null)
            {
                _context.Entry(existingGrant).State = EntityState.Detached;
            }

            // Присоединяем ту же сущность и меняем ее
            _context.Grants.Attach(grant);
            _context.Entry(grant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
