using GrantsPlatform.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using GrantsPlatform.Helpers.Converters;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using GrantsPlatform.Models.Viewmodels.Grant;

namespace GrantsPlatform.Data.Configurations
{
    public class FilterMappingConfiguration : IEntityTypeConfiguration<FilterMapping>
    {
        public void Configure(EntityTypeBuilder<FilterMapping> builder)
        {
            builder.ToTable("filtermapping");

            builder.HasKey(fm => fm.Id);

            builder.Property(fm => fm.FilterName)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(fm => fm.Filter)
                .WithMany()
                .HasForeignKey(fm => fm.FilterId);
        }
    }
}
