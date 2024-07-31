using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;
using ToDoList.Views;

namespace ToDoList.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAudioManager _audioManager;

    public MainViewModel(ITaskItemService taskItemService, IServiceProvider serviceProvider, IAudioManager audioManager)
    {
        TaskItems = new ObservableCollection<TaskItemViewModel>();
        this._taskItemService = taskItemService;
        this._serviceProvider = serviceProvider;
        this._audioManager = audioManager;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadTasks();
    }

    public async Task LoadTasks()
    {
        var tasks = await _taskItemService.GetAllTasks()
                                       .OrderBy(x => x.IsCompleted)
                                       .ThenByDescending(x => x.CreatedAt)
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
        addTaskPopupViewModel.TaskAdded += OnTaskAdded;

        var popup = new AddTaskPopup(addTaskPopupViewModel);
        await Application.Current.MainPage.ShowPopupAsync(popup);

        addTaskPopupViewModel.TaskAdded -= OnTaskAdded;
    }

    [RelayCommand]
    private async Task ShowEditTaskItemPopup(TaskItemViewModel taskItemViewModel)
    {

        var editTaskPopupViewModel = _serviceProvider.GetService<EditTaskPopupViewModel>();
        if (editTaskPopupViewModel != null && taskItemViewModel != null)
        {
            editTaskPopupViewModel.TaskEdited += OnTaskEdited;
            await editTaskPopupViewModel.LoadTaskItem(taskItemViewModel.TaskId);
            var popup = new EditTaskPopup(editTaskPopupViewModel);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            editTaskPopupViewModel.TaskEdited -= OnTaskEdited;
        }
    }

    private void OnTaskAdded(object sender, TaskItemViewModel taskItemViewModel)
    {
        if (taskItemViewModel.IsCompleted)
        {
            var existingTaskItem = TaskItems.FirstOrDefault(x => x.IsCompleted);
            if(existingTaskItem != null)
            {
                var index = TaskItems.IndexOf(existingTaskItem);
                TaskItems.Insert(index, taskItemViewModel);
            }
            else
            {
                TaskItems.Add(taskItemViewModel);
            }
            
        }
        else
        {
            TaskItems.Insert(0, taskItemViewModel);
        }
    }

    private void OnTaskEdited(object sender, TaskItemViewModel taskItemViewModel)
    {
        var existingTaskItem = TaskItems.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);

        if (existingTaskItem is not null)
        {
            var index = TaskItems.IndexOf(existingTaskItem);
            TaskItems[index] = taskItemViewModel;

            SortTaskItems();
        }
    }

    [RelayCommand]
    public async Task CheckTask(TaskItemViewModel taskItemViewModel)
    {
        var existingTaskItem = TaskItems.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
        if (existingTaskItem is not null)
        {
            var result = await _taskItemService.ChangeTaskToCompletedOrIncompleteAsync(taskItemViewModel.TaskId, taskItemViewModel.IsCompleted);
            if (result)
            {
                var index = TaskItems.IndexOf(existingTaskItem);
                TaskItems[index].IsCompleted = !taskItemViewModel.IsCompleted;
                SortTaskItems();
                await PlayCompletionSound();
                var toast = Toast.Make("Task completed!", ToastDuration.Short, 12);
                await toast.Show();
            }
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
    private async Task ChangeTaskImportantStatus(TaskItemViewModel taskItemViewModel)
    {
        var existingTaskItem = TaskItems.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
        if (existingTaskItem is not null)
        {
            var result = await _taskItemService.ChangeTaskImportantStatus(taskItemViewModel.TaskId, taskItemViewModel.IsImportant);
            if (result)
            {
                var index = TaskItems.IndexOf(existingTaskItem);
                TaskItems[index].IsImportant = !taskItemViewModel.IsImportant;
                SortTaskItems();
            }
        }

    }

    private async Task PlayCompletionSound()
    {
        var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("complete_task.wav"));
        player.Play();
    }

    private void SortTaskItems()
    {
        var sortedTasks = TaskItems.OrderBy(x => x.IsCompleted)
                                   .ThenByDescending(x => x.CreatedAt)
                                   .ToList();

        TaskItems.Clear();

        foreach (var task in sortedTasks)
        {
            TaskItems.Add(task);
        }
    }

}
