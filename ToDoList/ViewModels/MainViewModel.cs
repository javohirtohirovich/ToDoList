using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;
using ToDoList.Views;

namespace ToDoList.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;
    private readonly IServiceProvider _serviceProvider;

    public MainViewModel(ITaskItemService taskItemService, IServiceProvider serviceProvider)
    {
        TaskItems = new ObservableCollection<TaskItemViewModel>();
        this._taskItemService = taskItemService;
        this._serviceProvider = serviceProvider;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadTasks();
    }

    public async Task LoadTasks()
    {
        var tasks = await _taskItemService.GetAllTasks()
                                          .Where(x => !x.IsCompleted)
                                          .ToListAsync();
        TaskItems.Clear();
        foreach (var task in tasks)
        {
            TaskItems.Add(new TaskItemViewModel(task));
        }
    }

    [ObservableProperty]
    ObservableCollection<TaskItemViewModel> taskItems;

    [ObservableProperty]
    private string taskTitle;

    [ObservableProperty]
    private bool isRefreshing;


    [RelayCommand]
    private async Task OnRefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            await LoadTasks();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task ShowAddTaskItemPopup()
    {
        var addTaskPopupViewModel = _serviceProvider.GetService<AddTaskPopupViewModel>();

        // Subscribe to the TaskAdded event
        addTaskPopupViewModel.TaskAdded += OnTaskAdded;

        var popup = new AddTaskPopup(addTaskPopupViewModel);
        await Application.Current.MainPage.ShowPopupAsync(popup);

        // Unsubscribe from the event after the popup is closed to avoid memory leaks
        addTaskPopupViewModel.TaskAdded -= OnTaskAdded;
    }
    private void OnTaskAdded(object sender, TaskItemViewModel taskItemViewModel)
    {
        TaskItems.Add(taskItemViewModel);
    }

    [RelayCommand]
    public async Task AddQuickTask()
    {
        if (!string.IsNullOrWhiteSpace(TaskTitle))
        {
            var taskItem = new TaskItem
            {
                Task = TaskTitle
            };
            var taskItemViewModel = new TaskItemViewModel(taskItem);

            await _taskItemService.AddTaskItemAsync(taskItem);
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

    [RelayCommand]
    private async Task ChangeTaskImportantStatus(TaskItemViewModel taskItemViewModel)
    {
        var result = await _taskItemService.ChangeTaskImportantStatus(taskItemViewModel.TaskId, taskItemViewModel.IsImportant);
        if (result)
        {
            taskItemViewModel.IsImportant = !taskItemViewModel.IsImportant;
        }
    }

    [RelayCommand]
    public async Task GoEditTaskPage(TaskItemViewModel taskItemViewModel)
    {
        await Shell.Current.GoToAsync($"{nameof(EditTaskPage)}?TaskItemId={taskItemViewModel.TaskId}");
    }
}
