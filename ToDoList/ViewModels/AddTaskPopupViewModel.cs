using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.NullableDateTimePicker;
using System.Globalization;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;

namespace ToDoList.ViewModels;

public partial class AddTaskPopupViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;
    public AddTaskPopupViewModel(ITaskItemService taskItemService)
    {
        this._taskItemService = taskItemService;
        DueDateTasakLbl = "Set due date";
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

    [ObservableProperty]
    private DateTime? dueDateTask;

    [ObservableProperty]
    private string dueDateTasakLbl;

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
