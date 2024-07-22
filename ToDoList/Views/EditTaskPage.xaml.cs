using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class EditTaskPage : ContentPage
{
    public EditTaskPage(EditTaskViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}