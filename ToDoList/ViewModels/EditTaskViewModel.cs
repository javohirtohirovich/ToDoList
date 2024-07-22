using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Data.Models;
using ToDoList.Services;
using ToDoList.ViewModels.DataViewModels;

namespace ToDoList.ViewModels;

public partial class EditTaskViewModel : ObservableObject, IQueryAttributable
{
    private readonly ITaskItemService _taskItemService;

    public EditTaskViewModel(ITaskItemService taskItemService)
    {
        TaskItemViewModel = new TaskItemViewModel(new TaskItem());
        this._taskItemService = taskItemService;
    }

    [ObservableProperty]
    private string taskItemId;

    [ObservableProperty]
    private TaskItemViewModel taskItemViewModel;

    [ObservableProperty]
    private DateTime todayDate;

    [ObservableProperty]
    private DateTime? dueDateTask;

    [ObservableProperty]
    private TimeSpan? dueTimeTask;

    [RelayCommand]
    public async Task EditTaskItem()
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
            await _taskItemService.EditTaskItemAsync(int.Parse(TaskItemId), taskItem);

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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("TaskItemId"))
        {
            TaskItemId = query["TaskItemId"] as string;
        }
    }

    partial void OnTaskItemIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            InitializeAsync();
        }
    }

    private async void InitializeAsync()
    {
        await GetTaskItemAsync();
    }

    private async Task GetTaskItemAsync()
    {
        var taskItem = await _taskItemService.GetTaskItemAsync(int.Parse(TaskItemId));
        TaskItemViewModel.Title = taskItem.Title;
        TaskItemViewModel.Description = taskItem.Description;
        TaskItemViewModel.DueDate = taskItem.DueDate;
        TaskItemViewModel.Status = taskItem.Status;
        TaskItemViewModel.Priority = taskItem.Priority;

        if(taskItem.DueDate  is not null)
        {
            DueTimeTask = taskItem.DueDate?.TimeOfDay;
            DueDateTask = DateTime.Parse(taskItem.DueDate?.Date.ToString("MM/dd/yyyy"));
        }
    }


}
