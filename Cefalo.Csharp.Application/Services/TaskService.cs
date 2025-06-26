using Cefalo.Csharp.Core.Entities;
using Cefalo.Csharp.Core.Services;
using Cefalo.Csharp.Core.Repositories;

namespace Cefalo.Csharp.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(int userId)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        return await _taskRepository.GetByUserIdAsync(userId);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(task.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {task.UserId} not found");
        }

        // Set default values
        task.Status = TaskStatus.Todo;
        task.CreatedAt = DateTime.UtcNow;

        return await _taskRepository.AddAsync(task);
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
    {
        var existingTask = await _taskRepository.GetByIdAsync(task.Id);
        if (existingTask == null)
        {
            throw new ArgumentException($"Task with ID {task.Id} not found");
        }

        // Validate user exists if changed
        if (task.UserId != existingTask.UserId)
        {
            var user = await _userRepository.GetByIdAsync(task.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {task.UserId} not found");
            }
        }

        return await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new ArgumentException($"Task with ID {id} not found");
        }

        await _taskRepository.DeleteAsync(id);
    }
}