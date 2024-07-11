using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Entites;

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
            SeedDatabaseTest();
            MainPage = new AppShell();
        }

        private void SeedDatabaseTest()
        {
            if (_context.Categories.Any())
            {
                return;
            }

            var category = new Category
            {
                Name = "JavohirTest",
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

        }
    }
}
