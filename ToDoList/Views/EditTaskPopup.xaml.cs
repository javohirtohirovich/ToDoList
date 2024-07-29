using CommunityToolkit.Maui.Views;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class EditTaskPopup : Popup
{
    public EditTaskPopup(EditTaskPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}