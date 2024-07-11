using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ToDoList.ViewModel;

public partial class MainViewModel : ObservableObject
{
    IConnectivity connectivity;
    public MainViewModel(IConnectivity connectivity)
    {
        Items = new ObservableCollection<string>();
        this.connectivity = connectivity;
    }

    [ObservableProperty]
    ObservableCollection<string> items;

    [ObservableProperty]
    private string text;

    [RelayCommand]
    async Task Add()
    {
        if(connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "Ok");
            return;
        }
        if (string.IsNullOrWhiteSpace(Text))
        {
            return;
        }
        Items.Add(Text);
        //add our item
        Text = string.Empty;
    }


    [RelayCommand]
    void Delete(string value)
    {
        if (Items.Contains(value))
        {
            Items.Remove(value);
        }
    }

    [RelayCommand]
    async Task Tap(string s)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
    }
}
