using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core.Domain;

namespace Task.Api.Persistance
{
    public class TaskContext: IdentityDbContext<ApplicationUser>
    {

    }
}
