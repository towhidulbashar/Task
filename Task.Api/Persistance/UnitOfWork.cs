using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core;
using Task.Api.Core.Domain;
using Task.Api.Core.Repositories;

namespace Task.Api.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;

        public UnitOfWork(TaskDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IWorkItemRepository workItemRepository)
        {
            _context = context;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.WorkItemRepository = workItemRepository;
        }

        public IWorkItemRepository WorkItemRepository { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
