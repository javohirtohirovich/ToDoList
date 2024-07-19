using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

namespace ToDoList;

public partial class App : Application
{
    public App(MainContext mainContext)
    {
        InitializeComponent();
        mainContext.Database.Migrate();
        MainPage = new AppShell();
    }
}
