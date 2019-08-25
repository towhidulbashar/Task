using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpDelete("delete/{id}")]
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
                List<WorkItemResponse> response = MapWorkItem(workItems);
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
                List<WorkItemResponse> response = MapWorkItem(workItems);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [NonAction]
        private static List<WorkItemResponse> MapWorkItem(IEnumerable<WorkItem> workItems)
        {
            List<WorkItemResponse> response = new List<WorkItemResponse>();
            foreach (var item in workItems)
            {
                response.Add(new WorkItemResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StartDate = item.StartDate.HasValue ? item.StartDate.Value.Date : item.StartDate,
                    EndDate = item.EndDate,
                    ApplicationUserId = item.ApplicationUserId,
                    ApplicationUserName = item.AssignedTo == null ? string.Empty : $"{item.AssignedTo.FirstName} {item.AssignedTo.LastName}"
                });
            }
            return response;
        }
    }
}