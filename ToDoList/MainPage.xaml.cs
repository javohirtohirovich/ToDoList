    using ToDoList.Data;
using ToDoList.ViewModels;

namespace ToDoList;

public partial class MainPage : ContentPage
{
    private readonly MainContext _context;
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel vm, MainContext mainContext)
    {
        InitializeComponent();
        BindingContext = vm;
        _context = mainContext;
        _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTasks();
    }
}
