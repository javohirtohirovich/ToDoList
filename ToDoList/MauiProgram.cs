using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Maui.NullableDateTimePicker;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using ToDoList.Data;
using ToDoList.Services;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureNullableDateTimePicker()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddTaskPopup>();
        builder.Services.AddTransient<EditTaskPopup>();

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<AddTaskPopupViewModel>();
        builder.Services.AddTransient<EditTaskPopupViewModel>();

        builder.Services.AddTransient<ITaskItemService, TaskItemService>();
        builder.Services.AddTransient<IPopupService, PopupService>();

        builder.Services.AddDbContext<MainContext>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
