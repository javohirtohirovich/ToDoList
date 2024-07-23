using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;
using System;

namespace ToDoList.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly ITaskItemService _taskItemService;

        public AddTaskViewModel(ITaskItemService taskItemService)
        {
            TaskItemViewModel = new TaskItemViewModel(new TaskItem());
            TodayDate = DateTime.Now.Date;
            IsEnableDueDate = false;
            DueDateTask = null;
            DueTimeTask = null;
            this._taskItemService = taskItemService;
        }

        [ObservableProperty]
        private TaskItemViewModel taskItemViewModel;

        [ObservableProperty]
        private bool isEnableDueDate;

        [ObservableProperty]
        private DateTime? dueDateTask;

        [ObservableProperty]
        private TimeSpan? dueTimeTask;

        [ObservableProperty]
        private DateTime todayDate;

        [RelayCommand]
        public async Task AddTaskItem()
        {
            if (TaskItemViewModel is not null && !String.IsNullOrWhiteSpace(TaskItemViewModel.Title))
            {
                DateTime? dueDateTime = null;
                if (DueDateTask.HasValue && DueTimeTask.HasValue)
                {
                    dueDateTime = CombineDateAndTime(DueDateTask.Value, DueTimeTask.Value);
                    if (dueDateTime < DateTime.Now)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "The specified time has passed!", "OK");
                        return;
                    }
                }

                var taskItem = new TaskItem
                {
                    Title = TaskItemViewModel.Title,
                    Description = TaskItemViewModel.Description,
                    Priority = TaskItemViewModel.Priority,
                    Status = TaskItemViewModel.Status,
                    DueDate = dueDateTime,
                };
                await _taskItemService.AddTaskItemAsync(taskItem);

                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "A task name is required", "OK");
            }
        }

        private DateTime CombineDateAndTime(DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }
    }
}
