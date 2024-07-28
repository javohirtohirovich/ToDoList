using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.NullableDateTimePicker;
using System.Globalization;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;

namespace ToDoList.ViewModels;

public partial class EditTaskPopupViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;
    public event EventHandler<TaskItemViewModel> TaskEdited;

    public EditTaskPopupViewModel(ITaskItemService taskItemService)
    {
        this._taskItemService = taskItemService;
    }

    [ObservableProperty]
    private int taskId;

    [ObservableProperty]
    private string task;

    [ObservableProperty]
    private DateTime? dueDateTask;

    [ObservableProperty]
    private string dueDateTasakLbl;

    public async Task LoadTaskItem(int taskId)
    {
        var taskItem = await _taskItemService.GetTaskItemAsync(taskId);
        if (taskItem is not null)
        {
            TaskId = taskItem.TaskId;
            Task = taskItem.Task;
            DueDateTask = taskItem.DueDate;
            DueDateTasakLbl = FormatDueDateLabel(taskItem.DueDate);
        }
    }

    [RelayCommand]
    public async Task EditTaskItem()
    {
        if (!String.IsNullOrWhiteSpace(Task))
        {
            var taskItem = new TaskItem
            {
                TaskId = this.TaskId,
                Task = this.Task,
                DueDate = DueDateTask,
                UpdatedAt = DateTime.Now,

            };
            await _taskItemService.EditTaskItemAsync(TaskId, taskItem);
            var taskItemViewModel = new TaskItemViewModel(taskItem);

            TaskEdited?.Invoke(this, taskItemViewModel);

            var toast = Toast.Make("Task successful edit!", ToastDuration.Short, 12);
            await toast.Show();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "A task is required", "OK");
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
            HeaderBackgroundColor = Color.Parse("#5D70BD"),
            ActivityIndicatorColor = Color.Parse("#5D70BD"),
            ForeColor = Color.Parse("#5D70BD"),
            MinDate = DateTime.Today,

            // .. other calendar options
        };

        var result = await NullableDateTimePicker.OpenCalendarAsync(nullableDateTimePickerOptions);
        if (result is PopupResult popupResult && popupResult.ButtonResult != PopupButtons.Cancel)
        {
            DueDateTask = popupResult.NullableDateTime;
            DueDateTasakLbl = FormatDueDateLabel(DueDateTask);
        }
    }

    [RelayCommand]
    private void CancelSelectDate()
    {
        DueDateTasakLbl = "Set due date";
        DueDateTask = null;
    }

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
