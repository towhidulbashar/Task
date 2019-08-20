using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
