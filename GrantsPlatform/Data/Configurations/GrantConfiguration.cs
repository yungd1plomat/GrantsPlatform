using GrantsPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrantsPlatform.Data.Configurations
{
    public class GrantConfiguration : IEntityTypeConfiguration<Grant>
    {
        public void Configure(EntityTypeBuilder<Grant> builder)
        {
            builder.ToTable("grants");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Title)
                   .HasColumnName("title")
                   .HasColumnType("text");

            builder.Property(e => e.SourceUrl)
                  .HasColumnName("source_url")
                  .HasColumnType("text");

            builder.Property(e => e.ProjectDirections)
                   .HasColumnName("project_directions")
                   .HasColumnType("jsonb");

            builder.Property(e => e.Amount)
                   .HasColumnName("amount")
                   .HasColumnType("integer")
                   .IsRequired();

            builder.Property(e => e.LegalForms)
                   .HasColumnName("legal_forms")
                   .HasColumnType("jsonb");

            builder.Property(e => e.Age)
                   .HasColumnName("age")
                   .HasColumnType("integer");

            builder.Property(e => e.CuttingOffCriteria)
                   .HasColumnName("cutting_off_criterea")
                   .HasColumnType("jsonb");
        }
    }
}
