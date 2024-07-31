using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class EditTaskPopup : Popup
{
    public EditTaskPopup(EditTaskPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        if (BindingContext is EditTaskPopupViewModel viewModel)
        {
            viewModel.OnClose += ClosePopup;
            viewModel.OnReFocusEditor = ReFocusEditorAsync;
        }
    }

    private async void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        await Task.Delay(100);
        TaskEditor.Focus();
        await TaskEditor.ShowKeyboardAsync(CancellationToken.None);
    }

    private async Task ClosePopup()
    {
        await CloseAsync();
    }

    private async Task ReFocusEditorAsync()
    {
        await Task.Delay(100);
        TaskEditor.Focus();
        await TaskEditor.ShowKeyboardAsync(CancellationToken.None);
    }
}