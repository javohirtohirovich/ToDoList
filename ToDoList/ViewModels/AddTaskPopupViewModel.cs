using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.NullableDateTimePicker;
using System.Globalization;

namespace ToDoList.ViewModels;

public partial class AddTaskPopupViewModel : ObservableObject
{
    public AddTaskPopupViewModel()
    {
        DueDateTasakLbl = "Set due date";
    }
    [RelayCommand]
    private async Task ShowDatePickerPopup()
    {
        //var popup = new SelectDatePopup(new SelectDatePopupViewModel());
        //await Application.Current.MainPage.ShowPopupAsync(popup);
        INullableDateTimePickerOptions nullableDateTimePickerOptions = new NullableDateTimePickerOptions
        {
            NullableDateTime = DueDateTask,
            Mode = PickerModes.Date,
            ShowWeekNumbers = true
            // .. other calendar options
        };

        var result = await NullableDateTimePicker.OpenCalendarAsync(nullableDateTimePickerOptions);
        if (result is PopupResult popupResult && popupResult.ButtonResult != PopupButtons.Cancel)
        {
            DueDateTask = popupResult.NullableDateTime;
            DueDateTasakLbl = FormatDueDateLabel(DueDateTask);
            // DateTimeEntry.Text = popupResult.NullableDateTime?.ToString("g"); //If you are not using ViewModel
        }
    }

    [ObservableProperty]
    private DateTime? dueDateTask;

    [ObservableProperty]
    private string dueDateTasakLbl;

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
