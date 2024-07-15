using Microsoft.Extensions.Logging;
using Telerik.Maui.Controls.Compatibility;
using ToDoList.Data;
using ToDoList.Services;
using ToDoList.ViewModel;
using ToDoList.Views;

namespace ToDoList;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseTelerik()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<AddTaskPage>();
        builder.Services.AddTransient<ITaskItemService, TaskItemService>();

        builder.Services.AddTransient<DetailViewModel>();

        builder.Services.AddDbContext<MainContext>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
