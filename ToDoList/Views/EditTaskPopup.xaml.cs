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

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        TaskEditor.Focus();
    }
}