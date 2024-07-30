using CommunityToolkit.Maui.Views;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class EditTaskPopup : Popup
{
    public EditTaskPopup(EditTaskPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext=vm;
        if (BindingContext is EditTaskPopupViewModel viewModel)
        {
            viewModel.OnClose += ClosePopup;
        }
    }

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        TaskEditor.Focus();
    }

    private async Task ClosePopup()
    {
        await CloseAsync();
    }
}