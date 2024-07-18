using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Data.Models;
using ToDoList.Services;

namespace ToDoList.ViewModel;

public partial class TaskItemPopupViewModel : ObservableObject
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemPopupViewModel(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [RelayCommand]
    public async Task DeleteTaskItem(TaskItem taskItem)
    {
        if (taskItem is not null)
        {
            var result = await _taskItemService.DeleteTaskItemAsync(taskItem.TaskId);
            if (result)
            {
                // Task item o'chirildi
            }
        }
    }
}
