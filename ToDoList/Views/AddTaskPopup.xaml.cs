using CommunityToolkit.Maui.Views;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class AddTaskPopup : Popup
{
    public AddTaskPopup(AddTaskPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
