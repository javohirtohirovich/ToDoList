using ToDoList.Data;
using ToDoList.ViewModel;

namespace ToDoList;

public partial class MainPage : ContentPage
{
    private readonly MainContext _context;

    public MainPage(MainViewModel vm, MainContext mainContext)
    {
        InitializeComponent();
        BindingContext = vm;
        _context = mainContext;
    }

    protected override async void OnAppearing()
    {

    }
}
