using ToDoList.Data.Models;

namespace ToDoList.Services;

public interface ITaskItemService
{
    public Task AddTaskItemAsync(TaskItem taskItem);
    public IQueryable<TaskItem> GetAllTasksAsync();

}
