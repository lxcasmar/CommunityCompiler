using CommunityCompiler.ViewModels;

namespace CommunityCompiler.Views;

public partial class HomeView : ContentPage
{
    HomeViewModel _ViewModel;
	public HomeView(HomeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _ViewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();


        if (!_ViewModel.UserSignedIn)
        {
            //_ViewModel.NavigateToCommand.Execute("Views/RegisterView");
        }
    }
}
