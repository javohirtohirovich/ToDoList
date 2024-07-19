using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Enums;
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

    public IQueryable<TaskItem> GetAllTasks()
    {
        return _context.Tasks;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task UpdateExpiredTasksAsync()
    {
        var now = DateTime.UtcNow.AddHours(5);
        var expiredTasks = await _context.Tasks
            .Where(task => task.DueDate < now && task.Status != TaskStatusEnum.Overdue)
            .ToListAsync();

        foreach (var task in expiredTasks)
        {
            task.Status = TaskStatusEnum.Overdue;
        }
        await _context.SaveChangesAsync();
    }
}
