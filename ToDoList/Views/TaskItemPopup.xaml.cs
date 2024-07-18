using CommunityToolkit.Maui.Views;
using ToDoList.Services;
using ToDoList.ViewModel;

namespace ToDoList.Views;

public partial class TaskItemPopup : Popup
{
    public TaskItemPopup(ITaskItemService taskItemService)
    {
        InitializeComponent();
        BindingContext = new TaskItemPopupViewModel(taskItemService);

    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}