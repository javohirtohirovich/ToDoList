using Microsoft.Extensions.Logging;
using ToDoList.Data;
using ToDoList.Services;
using ToDoList.ViewModels;
using ToDoList.Views;
using CommunityToolkit.Maui;

namespace ToDoList;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddTaskPage>();
        builder.Services.AddTransient<EditTaskPage>();

        builder.Services.AddScoped<MainViewModel>();
        builder.Services.AddTransient<AddTaskViewModel>();
        builder.Services.AddTransient<EditTaskViewModel>();

        builder.Services.AddTransient<ITaskItemService, TaskItemService>();

        builder.Services.AddDbContext<MainContext>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
