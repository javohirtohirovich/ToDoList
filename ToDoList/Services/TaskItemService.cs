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
        _context = mainContext ?? throw new ArgumentNullException(nameof(mainContext));
    }

    public async Task AddTaskItemAsync(TaskItem taskItem)
    {
        await _context.AddAsync(taskItem);
        await SaveChangesAsync();
    }

    public async Task<bool> ChangeTaskStatus(int taskId, TaskStatusEnum status)
    {
        var taskItem = await _context.Tasks.FindAsync(taskId);
        if(taskItem is not null)
        {
            taskItem.Status = status;
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
            taskItemDatabase.Title = taskItem.Title;
            taskItemDatabase.Description = taskItem.Description;
            taskItemDatabase.DueDate = taskItem.DueDate;
            taskItemDatabase.Status = taskItem.Status;
            taskItemDatabase.Priority = taskItem.Priority;
            taskItemDatabase.IsCompleted = taskItem.IsCompleted;

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

    public async Task UpdateExpiredTasksAsync()
    {
        var now = DateTime.UtcNow.AddHours(5);

        var expiredTasks = await _context.Tasks
            .Where(task => task.DueDate < now && task.Status != TaskStatusEnum.Overdue)
            .ToListAsync();

        var notExpiredTasks = await _context.Tasks
            .Where(task => task.DueDate > now && task.Status == TaskStatusEnum.Overdue)
            .ToListAsync();

        if (expiredTasks.Any() || notExpiredTasks.Any())
        {
            foreach (var task in expiredTasks)
            {
                task.Status = TaskStatusEnum.Overdue;
            }
            foreach (var task in notExpiredTasks)
            {
                task.Status = TaskStatusEnum.Pending;
            }
            await SaveChangesAsync();
        }
    }
}
