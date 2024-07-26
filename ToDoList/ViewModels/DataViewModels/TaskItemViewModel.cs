using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class TaskItemViewModel : ObservableObject
{
    public TaskItemViewModel(TaskItem taskItem)
    {
        taskId = taskItem.TaskId;
        task = taskItem.Task;
        dueDate = taskItem.DueDate;
        isCompleted = taskItem.IsCompleted;
        categoryId = taskItem.CategoryId;
        createdAt = taskItem.CreatedAt;
        updatedAt = taskItem.UpdatedAt;
    }
    [ObservableProperty]
    private int taskId;

    [ObservableProperty]
    private string task;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private bool isImportant;

    [ObservableProperty]
    private int? categoryId;

    [ObservableProperty]
    private DateTime createdAt;

    [ObservableProperty]
    private DateTime updatedAt;
}
