using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;
using ToDoList.Views;

namespace ToDoList.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;

    public MainViewModel(ITaskItemService taskItemService)
    {
        TaskItems = new ObservableCollection<TaskItemViewModel>();
        this._taskItemService = taskItemService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadTasks();
    }

    [ObservableProperty]
    ObservableCollection<TaskItemViewModel> taskItems;

    [ObservableProperty]
    private string taskTitle;

    [RelayCommand]
    public async Task AddQuickTask()
    {
        if (!string.IsNullOrWhiteSpace(TaskTitle))
        {
            var taskItemViewModel = new TaskItemViewModel(new TaskItem
            {
                Title = TaskTitle,
                Status = TaskStatusEnum.InProgress,
                Priority = TaskPriority.Critical,
            });

            await _taskItemService.AddTaskItemAsync(new TaskItem
            {
                Title = TaskTitle,
                Status = TaskStatusEnum.InProgress,
                Priority = TaskPriority.Critical,
            });

            TaskItems.Add(taskItemViewModel);
            TaskTitle = string.Empty;
        }
    }

    [RelayCommand]
    public async Task CheckTask(TaskItemViewModel taskItemViewModel)
    {
        if (taskItemViewModel is not null)
        {
            var result = await _taskItemService.ChangeTaskToCompletedOrIncompleteAsync(taskItemViewModel.TaskId, taskItemViewModel.IsCompleted);
            if (result) { TaskItems.Remove(taskItemViewModel); }
        }
    }

    [RelayCommand]
    public async Task DeleteTaskItem(TaskItemViewModel taskItemViewModel)
    {
        if (taskItemViewModel is not null)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Warning!", "Are you sure you want to delete this task?", "Yes", "No");
            if (answer)
            {
                var result = await _taskItemService.DeleteTaskItemAsync(taskItemViewModel.TaskId);
                if (result) { TaskItems.Remove(taskItemViewModel); }
            }
        }
    }

    [RelayCommand]
    async Task GoAddTaskPage()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

    public async Task LoadTasks()
    {
        await _taskItemService.UpdateExpiredTasksAsync();
        var table = _taskItemService.GetAllTasks().Where(x => x.IsCompleted == false);
        var tasks = await table.ToListAsync();
        TaskItems.Clear();
        foreach (var task in tasks)
        {
            TaskItems.Add(new TaskItemViewModel(task));
        }
    }
}
