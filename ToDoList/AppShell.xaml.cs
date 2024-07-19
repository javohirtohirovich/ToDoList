using ToDoList.Views;

namespace ToDoList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));   
            Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
        }
    }
}
