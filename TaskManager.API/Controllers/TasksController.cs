using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.DTOs;
using TaskManager.API.Repositories;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            // For simplicity, we're using a hardcoded user ID
            // In a real application, you would get this from the authenticated user
            int userId = GetUserId();

            var tasks = await _taskRepository.GetAllTasksAsync(userId);

            return Ok(tasks.Select(t => new TaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate
            }));
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            int userId = GetUserId();

            var task = await _taskRepository.GetTaskByIdAsync(id, userId);

            if (task == null)
            {
                return NotFound();
            }

            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            };
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> CreateTask(TaskDTO taskDto)
        {
            int userId = GetUserId();

            var task = new Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                CreatedAt = DateTime.Now,
                DueDate = taskDto.DueDate,
                UserId = userId
            };

            await _taskRepository.CreateTaskAsync(task);

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                new TaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt,
                    DueDate = task.DueDate
                });
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDTO taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            int userId = GetUserId();

            var existingTask = await _taskRepository.GetTaskByIdAsync(id, userId);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.IsCompleted = taskDto.IsCompleted;
            existingTask.DueDate = taskDto.DueDate;

            await _taskRepository.UpdateTaskAsync(existingTask);

            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            int userId = GetUserId();

            var result = await _taskRepository.DeleteTaskAsync(id, userId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // For simplicity, we're using a hardcoded user ID
        // In a real application, you would get this from the authenticated user
        private int GetUserId()
        {
            // This would normally come from the authenticated user's claims but assigment doc not need to jwt authentication but not user idetify
            return 1; 
        }
    }
}
