using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Api.Core;
using Task.Api.Core.Domain;

namespace Task.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> Add(WorkItem workItem)
        {
            try
            {
                unitOfWork.WorkItemRepository.AddAsync(workItem);
                unitOfWork.Complete();
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}