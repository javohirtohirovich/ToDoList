using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList.Services;

public class TaskItemService : ITaskItemService
{
    private readonly MainContext _context;

    public TaskItemService(MainContext mainContext)
    {
        _context = mainContext;
    }
    public async Task AddTaskItemAsync(TaskItem taskItem)
    {
        await _context.AddAsync(taskItem);
        await SaveChangesAsync();
    }

    public async Task<bool> DeleteTaskItemAsync(int taskId)
    {
        var taskItem = await _context.Tasks.FindAsync(taskId);
        if (taskItem is not null)
        {
            _context.Tasks.Remove(taskItem);
            var result = await _context.SaveChangesAsync();
            if (result > 0) { return true; }
            else { return false; }
        }
        else
        {
            return false;
        }
    }

    public IQueryable<TaskItem> GetAllTasksAsync()
    {
        return _context.Tasks;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
