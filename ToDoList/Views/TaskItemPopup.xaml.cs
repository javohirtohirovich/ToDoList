using CommunityToolkit.Maui.Views;

namespace ToDoList.Views;

public partial class TaskItemPopup : Popup
{
    public TaskItemPopup()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}