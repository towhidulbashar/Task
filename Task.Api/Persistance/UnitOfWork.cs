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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UnitOfWork(TaskDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.WorkItemRepository = new WorkItemRepository(_context);
        }

        public IWorkItemRepository WorkItemRepository { get; }
        public UserManager<ApplicationUser> UserManager { get{ return userManager; } }
        public SignInManager<ApplicationUser> SignInManager { get { return signInManager; } }

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
