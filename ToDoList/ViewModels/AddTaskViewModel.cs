using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;

namespace ToDoList.ViewModels;

public partial class AddTaskViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;

    public AddTaskViewModel(ITaskItemService taskItemService)
    {
        TaskItemViewModel = new TaskItemViewModel(new TaskItem());
        this._taskItemService = taskItemService;
        DueDateTask = DateTime.Now.Date;
        DueTimeTask = DateTime.Now.TimeOfDay;
    }

    [ObservableProperty]
    private TaskItemViewModel taskItemViewModel;
    [ObservableProperty]
    private DateTime dueDateTask;
    [ObservableProperty]
    private TimeSpan dueTimeTask;

    [RelayCommand]
    public async Task AddTaskItem()
    {
        if (TaskItemViewModel is not null)
        {
            var dueDateTime = CombineDateAndTime(DueDateTask, DueTimeTask);
            if (dueDateTime < DateTime.Now)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Belgilangan vaqt o'tib ketgan.", "OK");
                return;
            }
            await _taskItemService.AddTaskItemAsync(new TaskItem
            {
                Title = TaskItemViewModel.Title,
                Description = TaskItemViewModel.Description,
                Priority = TaskItemViewModel.Priority,
                Status = TaskItemViewModel.Status,
                DueDate = dueDateTime,
            });

            await Shell.Current.GoToAsync("//MainPage");

        }
    }

    private DateTime CombineDateAndTime(DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }
}
