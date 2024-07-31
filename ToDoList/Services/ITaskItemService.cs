using ToDoList.Data.Models;

namespace ToDoList.Services;

public interface ITaskItemService
{
    public Task AddTaskItemAsync(TaskItem taskItem);
    public Task<bool> EditTaskItemAsync(int taskId, TaskItem taskItem);
    public IQueryable<TaskItem> GetAllTasks();
    public Task<bool> DeleteTaskItemAsync(int taskId);
    public Task<bool> ChangeTaskToCompletedOrIncompleteAsync(int taskId, bool isCompleted);
    public Task<TaskItem> GetTaskItemAsync(int taskId);
    public Task<bool> ChangeTaskImportantStatus(int taskId, bool isImportant);
}
