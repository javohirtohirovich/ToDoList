using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDoList.Data.Models;
using ToDoList.Data.Enums;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModel;

public partial class MainViewModel : ObservableObject
{
    IConnectivity connectivity;
    private readonly ITaskItemService _taskItemService;

    public MainViewModel(IConnectivity connectivity,  ITaskItemService taskItemService)
    {
        TaskItems = new ObservableCollection<TaskItem>();
        this.connectivity = connectivity;
        this._taskItemService = taskItemService;
        LoadTasks();
    }

    [ObservableProperty]
    ObservableCollection<TaskItem> taskItems;

    [ObservableProperty]
    private string taskTitle;

    [RelayCommand]
    public async Task AddQuickTask()
    { 
        if (string.IsNullOrWhiteSpace(TaskTitle))
        {
            return;
        }
        var taskItem = new TaskItem()
        {
            Title = TaskTitle,
            Status = TaskStatusEnum.InProgress,
            Priority = TaskPriority.Critical,
            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
            DueDate = DateTime.Now,
        };

        await _taskItemService.AddTaskItemAsync(taskItem);
        TaskItems.Add(taskItem);
        TaskTitle = string.Empty;
    }

    [RelayCommand]
    public async Task DeleteTaskItem(TaskItem taskItem)
    {
        if (taskItem is not null)
        {
            var result = await _taskItemService.DeleteTaskItemAsync(taskItem.TaskId);
            if (result) { TaskItems.Remove(taskItem); }
        }
    }
    [RelayCommand]
    public async Task Tap(string s)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
    }

    [RelayCommand]
    async Task GoAddTaskPage()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

    private void LoadTasks()
    {
        var tasks =  _taskItemService.GetAllTasksAsync().ToList();
        foreach (var task in tasks)
        {
            TaskItems.Add(task);
        }
    }
}
