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
        TaskGroups = new ObservableCollection<TaskGroup>();
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
        var tasks = await _taskItemService.GetAllTasks().ToListAsync();

        var groupedTasks = new List<TaskGroup>
        {
            new TaskGroup("Tasks", tasks.Where(t => !t.IsCompleted).OrderByDescending(x=>x.CreatedAt).Select(t => new TaskItemViewModel(t))),
            new TaskGroup("Completed", tasks.Where(t => t.IsCompleted).OrderByDescending(x=>x.UpdatedAt).Select(t => new TaskItemViewModel(t)))
        };

        TaskGroups.Clear();
        foreach (var group in groupedTasks)
        {
            TaskGroups.Add(group);
        }
    }

    [ObservableProperty]
    ObservableCollection<TaskGroup> taskGroups;

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
            var completedGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Completed");
            completedGroup?.Insert(0, taskItemViewModel);
        }
        else
        {
            var pendingGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Tasks");
            pendingGroup?.Insert(0, taskItemViewModel);
        }
    }


    private void OnTaskEdited(object sender, TaskItemViewModel taskItemViewModel)
    {
        foreach (var group in TaskGroups)
        {
            var existingTaskItem = group.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
            if (existingTaskItem != null)
            {
                var index = group.IndexOf(existingTaskItem);
                group.Remove(existingTaskItem);

                existingTaskItem.Task = taskItemViewModel.Task;
                existingTaskItem.DueDate = taskItemViewModel.DueDate;
                existingTaskItem.IsCompleted = taskItemViewModel.IsCompleted;
                existingTaskItem.UpdatedAt = taskItemViewModel.UpdatedAt;

                if (taskItemViewModel.IsCompleted)
                {
                    var completedGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Completed");
                    completedGroup?.Insert(0, existingTaskItem);
                }
                else
                {
                    var pendingGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Tasks");
                    pendingGroup?.Add(existingTaskItem);
                    SortTaskItems(pendingGroup);
                }
                break;
            }
        }
    }



    [RelayCommand]
    public async Task CheckTask(TaskItemViewModel taskItemViewModel)
    {
        foreach (var group in TaskGroups)
        {
            var existingTaskItem = group.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
            if (existingTaskItem != null)
            {
                var result = await _taskItemService.ChangeTaskToCompletedOrIncompleteAsync(taskItemViewModel.TaskId, taskItemViewModel.IsCompleted);
                if (result)
                {
                    group.Remove(existingTaskItem);
                    existingTaskItem.IsCompleted = !taskItemViewModel.IsCompleted;
                    existingTaskItem.UpdatedAt = DateTime.Now.AddHours(5);

                    if (existingTaskItem.IsCompleted)
                    {
                        var completedGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Completed");
                        completedGroup?.Insert(0, existingTaskItem);
                        SortTaskItems(completedGroup);
                    }
                    else
                    {
                        var pendingGroup = TaskGroups.FirstOrDefault(g => g.GroupName == "Tasks");
                        pendingGroup?.Add(existingTaskItem);
                        SortTaskItems(pendingGroup);
                    }

                    await PlayCompletionSound();
                    var toast = Toast.Make("Task completed!", ToastDuration.Short, 12);
                    await toast.Show();
                    break;
                }
            }
        }
    }


    [RelayCommand]
    public async Task DeleteTaskItem(TaskItemViewModel taskItemViewModel)
    {
        foreach (var group in TaskGroups)
        {
            var existingTaskItem = group.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
            if (existingTaskItem != null)
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Warning!", "Are you sure you want to delete this task?", "Yes", "No");
                if (answer)
                {
                    var result = await _taskItemService.DeleteTaskItemAsync(taskItemViewModel.TaskId);
                    if (result)
                    {
                        group.Remove(existingTaskItem);
                    }
                }
                break;
            }
        }
    }


    [RelayCommand]
    private async Task ChangeTaskImportantStatus(TaskItemViewModel taskItemViewModel)
    {
        foreach (var group in TaskGroups)
        {
            var existingTaskItem = group.FirstOrDefault(x => x.TaskId == taskItemViewModel.TaskId);
            if (existingTaskItem != null)
            {
                var result = await _taskItemService.ChangeTaskImportantStatus(taskItemViewModel.TaskId, taskItemViewModel.IsImportant);
                if (result)
                {
                    existingTaskItem.IsImportant = !taskItemViewModel.IsImportant;
                    SortTaskItems(group);
                }
                break;
            }
        }
    }


    private async Task PlayCompletionSound()
    {
        var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("complete_task.wav"));
        player.Play();
    }

    private void SortTaskItems(TaskGroup group)
    {
        var sortedTasks = group.OrderBy(x => x.IsCompleted)
                               .ThenByDescending(x => x.IsCompleted ? x.UpdatedAt : x.CreatedAt)
                               .ToList();

        group.Clear();

        foreach (var task in sortedTasks)
        {
            group.Add(task);
        }
    }


}
