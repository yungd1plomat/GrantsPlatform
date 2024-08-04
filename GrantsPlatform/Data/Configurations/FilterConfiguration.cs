using GrantsPlatform.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using GrantsPlatform.Helpers.Converters;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GrantsPlatform.Data.Configurations
{
    public class FilterConfiguration : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> builder)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Converters = 
                { 
                    new FilterConverter() 
                },
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            builder.ToTable("filters");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.Mapping)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<Dictionary<string, Mapping>>(v, jsonOptions))
                .Metadata
                .SetValueComparer(new ValueComparer<Dictionary<string, Mapping>>(
                    (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => JsonSerializer.Deserialize<Dictionary<string, Mapping>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
                ));
        }
    }
}
