using ToDoList.ViewModels;

namespace ToDoList;

public partial class DetailPage : ContentPage
{
	public DetailPage(DetailViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
}