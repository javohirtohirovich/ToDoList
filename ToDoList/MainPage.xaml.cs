using CommunityToolkit.Maui.Views;
using ToDoList.Data;
using ToDoList.ViewModel;
using ToDoList.Views;

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

    private void ShowPopupButton_Clicked(object sender, EventArgs e)
    {
        var popup = new TaskItemPopup();
        this.ShowPopup(popup);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }
}
