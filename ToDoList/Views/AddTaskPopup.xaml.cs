using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class AddTaskPopup : Popup
{
    public AddTaskPopup(AddTaskPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        if (BindingContext is AddTaskPopupViewModel viewModel)
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
