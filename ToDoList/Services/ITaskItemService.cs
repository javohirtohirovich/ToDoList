using ToDoList.Data.Enums;
using ToDoList.Data.Models;

namespace ToDoList.Services;

public interface ITaskItemService
{
    public Task AddTaskItemAsync(TaskItem taskItem);
    public Task<bool> EditTaskItemAsync(int taskId, TaskItem taskItem);
    public IQueryable<TaskItem> GetAllTasks();
    public Task<bool> DeleteTaskItemAsync(int taskId);
    public Task UpdateExpiredTasksAsync();
    public Task<bool> ChangeTaskToCompletedOrIncompleteAsync(int taskId, bool isCompleted);
    public Task<bool> ChangeTaskStatus(int taskId, TaskStatusEnum status);
    public Task<TaskItem> GetTaskItemAsync(int taskId);
}
