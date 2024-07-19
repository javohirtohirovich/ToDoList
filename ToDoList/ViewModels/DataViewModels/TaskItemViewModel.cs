using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class TaskItemViewModel : ObservableObject
{
    public TaskItemViewModel(TaskItem taskItem)
    {
        taskId = taskItem.TaskId;
        title = taskItem.Title;
        description = taskItem.Description;
        dueDate = taskItem.DueDate;
        priority = taskItem.Priority;
        status = taskItem.Status;
        isCompleted = taskItem.IsCompleted;
        categoryId = taskItem.CategoryId;
        taskTags = taskItem.TaskTags?.ToObservableCollection() ?? new ObservableCollection<TaskTag>();
        createdAt = taskItem.CreatedAt;
        updatedAt = taskItem.UpdatedAt;
    }

    [ObservableProperty]
    private int taskId;

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
    public bool isCompleted;

    [ObservableProperty]
    private int? categoryId;

    [ObservableProperty]
    private DateTime createdAt;

    [ObservableProperty]
    private DateTime updatedAt;

    [ObservableProperty]
    private ObservableCollection<TaskTag> taskTags;

}
