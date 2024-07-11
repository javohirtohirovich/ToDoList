using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ToDoList.Data;
using ToDoList.ViewModel;

namespace ToDoList;

public partial class MainPage : ContentPage
{
    private readonly MainContext _context;

    public MainPage(MainViewModel vm, MainContext mainContext)
    {
        InitializeComponent();
        BindingContext= vm;
        _context = mainContext;
    }

    protected override async void OnAppearing()
    {
        var category = await _context.Categories.FirstOrDefaultAsync();
        if (category is not null)
        {
            nameLbl.Text = $"Category Name in Database {category.CategoryId}-{category.Name}";
        }
    }
}
