using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class TagViewModel : ObservableObject
{
    internal readonly Tag _tag;

    public TagViewModel(Tag tag)
    {
        _tag = tag;
    }
    public int TagId => _tag.TagId;

    [ObservableProperty]
    private string name;

    public List<TaskTagViewModel> TaskTags
    {
        get
        {
            return _tag.TaskTags.Select(taskTag => new TaskTagViewModel(taskTag)).ToList();
        }
    }

    public void SaveChanges()
    {
        _tag.Name = Name;
    }

}
