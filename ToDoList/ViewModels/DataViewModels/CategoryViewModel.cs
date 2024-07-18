using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Data.Models;

namespace ToDoList.ViewModels.DataViewModels;

public partial class CategoryViewModel : ObservableObject
{
    private readonly Category _category;

    public CategoryViewModel(Category category)
    {
        _category = category;
        Name = category.Name;
    }

    public int CategoryIf => _category.CategoryId;

    [ObservableProperty]
    private string name;

    public List<TaskItemViewModel> TaskItems
    {
        get
        {
            return _category.Tasks.Select(task => new TaskItemViewModel(task)).ToList();

        }
    }
    public void SaveChanges()
    {
        _category.Name = Name;
    }
}
