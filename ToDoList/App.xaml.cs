using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList
{
    public partial class App : Application
    {
        private readonly MainContext _context;

        public App(MainContext mainContext)
        {
            InitializeComponent();
            _context = mainContext;
            mainContext.Database.Migrate();
            MainPage = new AppShell();
        }
    }
}
