using SQLitePCL;
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

    public IQueryable<TaskItem> GetAllTasksAsync()
    {
        return _context.Tasks;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
