﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDoList.Data.Models;
using ToDoList.Data.Enums;
using ToDoList.Services;
using ToDoList.Views;
using ToDoList.ViewModels.DataViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ToDoList.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;

    public MainViewModel(ITaskItemService taskItemService)
    {
        TaskItems = new ObservableCollection<TaskItemViewModel>();
        this._taskItemService = taskItemService;
        LoadTasks();
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
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                DueDate = DateTime.Now,
            });

            await _taskItemService.AddTaskItemAsync(new TaskItem
            {
                Title = TaskTitle,
                Status = TaskStatusEnum.InProgress,
                Priority = TaskPriority.Critical,
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                DueDate = DateTime.Now,
            });

            TaskItems.Add(taskItemViewModel);
            TaskTitle = string.Empty;
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
    public async Task Tap(string s)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
    }

    [RelayCommand]
    async Task GoAddTaskPage()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

    private async Task LoadTasks()
    {
        var table = _taskItemService.GetAllTasks();
        var tasks = await table.ToListAsync();
        TaskItems.Clear();
        foreach (var task in tasks)
        {
            TaskItems.Add(new TaskItemViewModel(task));
        }
    }
}
