using Micro.BackgroundJobs.SqlServer.Persistence.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Micro.BackgroundJobs.SqlServer.Persistence.Configuration;

internal sealed class BackgroundJobEntityTypeConfiguration : IEntityTypeConfiguration<BackgroundJob>
{
    public void Configure(EntityTypeBuilder<BackgroundJob> builder)
    {
        builder.ToTable("BackgroundJobs", "Micro");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.State).HasMaxLength(32).HasConversion<EnumToStringConverter<BackgroundJobState>>();
        builder.Property(x => x.Queue).HasMaxLength(255);
        builder.Property(x => x.Data).IsRequired().HasConversion<ClassToJsonConverter<BackgroundJobData>>();
        builder.Property(x => x.RetryAttempt);
        builder.Property(x => x.RetryMaxCount);
        builder.Property(x => x.ErrorMessage);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ProcessedAt);
        builder.Property(x => x.InvisibleUntil);
        builder.Property(x => x.ProcessingDuration);
        builder.Property(x => x.ServerId).HasMaxLength(255);
        builder.HasIndex(x => new { x.State, x.ProcessedAt });
        builder.HasIndex(x => new { x.State, x.Queue, x.InvisibleUntil, x.CreatedAt });
    }
}