using AndroidX.Lifecycle;
using CommunityCompiler.ViewModels;

namespace CommunityCompiler.Views;

public partial class HomeView : ContentPage
{
    HomeViewModel _ViewModel;
	public HomeView()
	{
		InitializeComponent();

        _ViewModel = new HomeViewModel();
        BindingContext = _ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();


        //if (!_ViewModel.UserSignedIn)
        //{
        //    //_ViewModel.NavigateToCommand.Execute("Views/RegisterView");
        //}
    }
}
