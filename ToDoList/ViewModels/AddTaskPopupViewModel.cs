﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.NullableDateTimePicker;
using System.Globalization;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;

namespace ToDoList.ViewModels;

public delegate Task CloseHandlerAddPopup();
public partial class AddTaskPopupViewModel : ObservableObject
{
    public event EventHandler<TaskItemViewModel> TaskAdded;
    public event CloseHandlerAddPopup OnClose;

    private readonly ITaskItemService _taskItemService;
    public AddTaskPopupViewModel(ITaskItemService taskItemService)
    {
        this._taskItemService = taskItemService;
        DueDateTaskLbl = "Set due date";
    }

    [RelayCommand]
    public async Task AddTask()
    {
        if (!string.IsNullOrWhiteSpace(Task))
        {
            var taskItem = new TaskItem()
            {
                Task = Task,
                DueDate = DueDateTask
            };

            await _taskItemService.AddTaskItemAsync(taskItem);
            var taskItemViewModel = new TaskItemViewModel(taskItem);

            TaskAdded?.Invoke(this, taskItemViewModel);

            var toast = Toast.Make("New task successful add!", ToastDuration.Short, 12);
            await toast.Show();
            DueDateTask = null;
            DueDateTaskLbl = "Set due date";
            Task = string.Empty;
            if (OnClose != null)
            {
                await OnClose.Invoke();
            }
        }
    }

    [RelayCommand]
    private async Task ShowDatePickerPopup()
    {
        INullableDateTimePickerOptions nullableDateTimePickerOptions = new NullableDateTimePickerOptions
        {
            NullableDateTime = DueDateTask,
            Mode = PickerModes.Date,
            ShowWeekNumbers = true,
            HeaderBackgroundColor = Color.Parse("#347980"),
            ActivityIndicatorColor = Color.Parse("#347980"),
            ForeColor = Color.Parse("#347980"),
            //MinDate = DateTime.Today,
        };

        var result = await NullableDateTimePicker.OpenCalendarAsync(nullableDateTimePickerOptions);
        if (result is PopupResult popupResult && popupResult.ButtonResult != PopupButtons.Cancel)
        {
            DueDateTask = popupResult.NullableDateTime;
            DueDateTaskLbl = FormatDueDateLabel(DueDateTask);
        }
    }

    [RelayCommand]
    private void CancelSelectDate()
    {
        DueDateTaskLbl = "Set due date";
        DueDateTask = null;
    }

    [ObservableProperty]
    private DateTime? dueDateTask;

    [ObservableProperty]
    private string dueDateTaskLbl;

    [ObservableProperty]
    private string task;

    private string FormatDueDateLabel(DateTime? dueDate)
    {
        if (dueDate == null)
        {
            return "Set due date";
        }

        DateTime now = DateTime.Now;
        string dateFormat = "ddd, dd MMM";
        if (dueDate.Value.Year != now.Year)
        {
            dateFormat += " yyyy";
        }

        return $"Due {dueDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture)}";
    }

}
