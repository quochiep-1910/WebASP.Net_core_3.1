using eShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Data.Configurations
{
    public class WorkingScheduleConfiguration : IEntityTypeConfiguration<WorkingSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkingSchedule> builder)
        {
            builder.ToTable("WorkingSchedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.UserName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.LyDo).IsRequired().HasMaxLength(200);
        }
    }
}