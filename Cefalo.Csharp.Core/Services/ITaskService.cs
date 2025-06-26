using Cefalo.Csharp.Core.Entities;

namespace Cefalo.Csharp.Core.Services;

public interface ITaskService
{
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<IEnumerable<TaskItem>> GetTasksByUserAsync(int userId);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task<TaskItem> UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(int id);
}