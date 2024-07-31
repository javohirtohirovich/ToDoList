using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList.Services;

public class TaskItemService : ITaskItemService
{
    private readonly MainContext _context;

    public TaskItemService(MainContext mainContext)
    {
        _context = mainContext ?? throw new ArgumentNullException(nameof(mainContext));
    }

    public async Task AddTaskItemAsync(TaskItem taskItem)
    {
        await _context.AddAsync(taskItem);
        await SaveChangesAsync();
    }

    public async Task<bool> ChangeTaskImportantStatus(int taskId, bool isImportant)
    {
        var taskItem = await _context.Tasks.FindAsync(taskId);
        if (taskItem is not null)
        {
            taskItem.IsImportant = !isImportant;
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }


    public async Task<bool> ChangeTaskToCompletedOrIncompleteAsync(int taskId, bool isCompleted)
    {
        var taskItem = await _context.Tasks.FindAsync(taskId);
        if (taskItem is not null)
        {
            taskItem.IsCompleted = !isCompleted;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }

    public async Task<bool> DeleteTaskItemAsync(int taskId)
    {
        var taskItem = await _context.Tasks.FindAsync(taskId);
        if (taskItem is not null)
        {
            _context.Tasks.Remove(taskItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }

    public async Task<bool> EditTaskItemAsync(int taskId, TaskItem taskItem)
    {
        var taskItemDatabase = await _context.Tasks.FindAsync(taskId);
        if (taskItemDatabase is not null)
        {
            taskItemDatabase.Task = taskItem.Task;
            taskItemDatabase.DueDate = taskItem.DueDate;
            taskItemDatabase.IsImportant = taskItem.IsImportant;
            taskItemDatabase.IsCompleted = taskItem.IsCompleted;
            taskItemDatabase.UpdatedAt = DateTime.UtcNow.AddHours(5);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }

    public IQueryable<TaskItem> GetAllTasks()
    {
        return _context.Tasks.AsNoTracking();
    }

    public async Task<TaskItem> GetTaskItemAsync(int taskId)
    {
        return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == taskId);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
