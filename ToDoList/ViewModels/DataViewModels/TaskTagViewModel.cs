using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class TaskTagViewModel : ObservableObject
{
    private readonly TaskTag _taskTag;

    public TaskTagViewModel(TaskTag taskTag)
    {
        _taskTag = taskTag;
        Task = new TaskItemViewModel(taskTag.Task);
        Tag = new TagViewModel(taskTag.Tag);
    }

    public int TaskId => _taskTag.TaskId;
    public int TagId => _taskTag.TagId;

    public TaskItemViewModel Task { get; }
    public TagViewModel Tag { get; }

    //public void SaveChanges()
    //{
    //    Task.SaveChanges();
    //    Tag.SaveChanges();
    //    _taskTag.Task = Task._taskItem;
    //    _taskTag.Tag = Tag._tag;
    //}
}
