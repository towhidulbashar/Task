using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core.Domain;

namespace Task.Api.Persistance
{
    public class TaskDbContext: IdentityDbContext<ApplicationUser>
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options): base(options)
        {
            
        }
        public DbSet<WorkItem> WorkItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new WorkItemConfiguration());
            foreach (var foreignKey in builder.Model
                                            .GetEntityTypes()
                                            .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> applicationUser)
        {
            applicationUser.Property(p => p.FirstName).IsRequired();
            applicationUser.Property(p => p.LastName).IsRequired();
            applicationUser.Property(p => p.Address).IsRequired();
            applicationUser.HasMany(x => x.WorkItems)
                .WithOne(x => x.AssignedTo);
        }
    }

    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> workItem)
        {
            workItem.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
