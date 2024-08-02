using System.Collections.ObjectModel;

namespace ToDoList.ViewModels.DataViewModels;

public class TaskGroup : ObservableCollection<TaskItemViewModel>
{
    public string GroupName { get; private set; }

    public TaskGroup(string groupName, IEnumerable<TaskItemViewModel> tasks) : base(tasks)
    {
        GroupName = groupName;
    }
}