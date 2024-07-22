using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class AddTaskPage : ContentPage
{
    public AddTaskPage(AddTaskViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}