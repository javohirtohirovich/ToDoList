using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class TaskItemViewModel : ObservableObject
{
    internal readonly TaskItem _taskItem;
    public TaskItemViewModel(TaskItem taskItem)
    {
        _taskItem = taskItem;
    }

    public int TaskId => _taskItem.TaskId;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private DateTime dueDate;

    [ObservableProperty]
    private TaskPriority priority;

    [ObservableProperty]
    private TaskStatusEnum status;

    [ObservableProperty]
    private int? categoryId;

    public DateTime CreatedAt => _taskItem.CreatedAt;
    public DateTime UpdatedAt => _taskItem.UpdatedAt;

    public Category Category => _taskItem.Category;
    public List<TaskTag> TaskTags => _taskItem.TaskTags;

    public void SaveChanges()
    {
        _taskItem.Title = Title;
        _taskItem.Description = Description;
        _taskItem.DueDate = DueDate;
        _taskItem.Priority = Priority;
        _taskItem.Status = Status;
        _taskItem.CategoryId = CategoryId;
    }

}
