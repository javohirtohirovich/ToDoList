using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoList.ViewModel;

[QueryProperty("Text","Text")]
public partial class DetailViewModel : ObservableObject
{
    [ObservableProperty]
    string text;
}
