using AndroidX.Lifecycle;
using CommunityCompiler.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityCompiler.Services;

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

    private async void addANewEventBtnClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new CreateEventPopup(ServiceAid.GetService<EventDataService>()));
    }

    private async void findAnEventBtnClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<object, int>(this, "Search", 1);
    }

    private async void goToFavoritesBtnClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<object, int>(this, "Favorites", 2);
    }

    private async void profileSettingsBtnClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<object, int>(this, "Settings", 3);
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
