using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoList.ViewModels;

[QueryProperty("Text","Text")]
public partial class DetailViewModel : ObservableObject
{
    [ObservableProperty]
    string text;
}
