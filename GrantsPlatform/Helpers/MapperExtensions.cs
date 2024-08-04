using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;
using System.Text.Json;

namespace GrantsPlatform.Helpers
{
    public static class MapperExtensions
    {
        public static GrantDto ToDto(this Grant grant)
        {
            var grantDto = new GrantDto()
            {
                Id = grant.Id.GetValueOrDefault(),
                Title = grant.Title,
                SourceUrl = grant.SourceUrl,
            };
            var cuttingOffCriteria = JsonSerializer.SerializeToElement(grant.CuttingOffCriteria);
            var projectDirections = JsonSerializer.SerializeToElement(grant.ProjectDirections);
            var legalForm = JsonSerializer.SerializeToElement(grant.LegalForms);
            var amount = JsonSerializer.SerializeToElement(grant.Amount);
            var age = JsonSerializer.SerializeToElement(grant.Age);
            grantDto.FilterValues = new GrantFilters()
            {
                Properties = new Dictionary<string, JsonElement>()
                {
                    { "cutting_off_criteria", cuttingOffCriteria },
                    { "project_direction", projectDirections },
                    { "legal_form", legalForm },
                    { "amount", amount },
                    { "age", age }
                }
            };
            return grantDto;
        }

        public static Grant FromDto(this GrantDto grantDto)
        {
            var grant = new Grant()
            {
                Id = grantDto.Id,
                Title = grantDto.Title,
                SourceUrl = grantDto.SourceUrl,
            };
            if (grantDto.FilterValues.Properties.TryGetValue("cutting_off_criteria", out var cuttingOffCriteria))
            {
                grant.CuttingOffCriteria = JsonSerializer.Deserialize<IList<int>>(cuttingOffCriteria);
            }
            if (grantDto.FilterValues.Properties.TryGetValue("project_direction", out var projectDirections))
            {
                grant.ProjectDirections = JsonSerializer.Deserialize<IList<int>>(projectDirections);
            }
            if (grantDto.FilterValues.Properties.TryGetValue("legal_form", out var legalForm))
            {
                grant.LegalForms = JsonSerializer.Deserialize<IList<int>>(legalForm);
            }
            if (grantDto.FilterValues.Properties.TryGetValue("amount", out var amount))
            {
                grant.Amount = JsonSerializer.Deserialize<int>(amount);
            }
            if (grantDto.FilterValues.Properties.TryGetValue("age", out var age))
            {
                grant.Age = JsonSerializer.Deserialize<int>(age);
            }
            return grant;
        }

        public static FilterMappingDto ToDto(this IEnumerable<FilterMapping> filterMappings)
        {
            var dto = new FilterMappingDto
            {
                Filters = filterMappings.ToDictionary(
                    fm => fm.FilterName,
                    fm => fm.Filter)
            };

            return dto;
        }

        public static IEnumerable<FilterMapping> FromDto(this FilterMappingDto dto)
        {
            return dto.Filters.Select(kvp => new FilterMapping
            {
                FilterName = kvp.Key,
                Filter = kvp.Value
            });
        }
    }
}
