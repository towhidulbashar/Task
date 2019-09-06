using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Api.Controllers.Resources;
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
        private readonly IMapper mapper;

        public WorkItemController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(WorkItem workItem)
        {
            try
            {
                if (string.IsNullOrEmpty(workItem.ApplicationUserId))
                    workItem.ApplicationUserId = null;
                unitOfWork.WorkItemRepository.AddAsync(workItem);
                unitOfWork.Complete();
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("update")]
        public IActionResult Update(WorkItem workItem)
        {
            try
            {
                unitOfWork.WorkItemRepository.Update(workItem);
                unitOfWork.Complete();
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var workItem = await unitOfWork.WorkItemRepository.GetByIdAsync(id);
                if (workItem != null)
                {
                    unitOfWork.WorkItemRepository.Remove(workItem);
                    unitOfWork.Complete();
                }
                else
                {
                    unitOfWork.Complete();
                    return NotFound("Task not found.");
                }                
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var includes = new List<string>() { "AssignedTo" };
                var workItems = await unitOfWork.WorkItemRepository.GetAllAsync(includes);
                unitOfWork.Complete();
                var response = mapper.Map<IEnumerable<WorkItem>, IEnumerable<GetWorkItemResource>>(workItems);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get-current-user-tasks")]
        public IActionResult GetCurrentUserTasks()
        {
            try
            {
                string currentUserId = this.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                var workItems = unitOfWork.WorkItemRepository.GetCurrentUserTasks(currentUserId);
                unitOfWork.Complete();
                var response = mapper.Map<IEnumerable<WorkItem>, IEnumerable<GetWorkItemResource>>(workItems);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}