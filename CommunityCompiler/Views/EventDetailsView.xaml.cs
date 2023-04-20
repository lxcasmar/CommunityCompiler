using CommunityCompiler.ViewModels;
using CommunityCompiler.Services;
using CommunityCompiler.Models;

namespace CommunityCompiler.Views;

public partial class EventDetailsView : ContentPage
{
    private readonly Event _event;
    private readonly FavoritesViewModel _favoritesViewModel;
    private UserDataService _UserDataService;

    public EventDetailsView(Event eventDetails, Boolean buttonVisibility)
    {
        InitializeComponent();

        _event = eventDetails;
        _favoritesViewModel = ServiceAid.GetService<FavoritesViewModel>();
        _UserDataService = ServiceAid.GetService<UserDataService>();

        favoriteBtn.IsVisible = buttonVisibility;
        eventName.Text = _event.name;
        Location.Text = "City: " + _event.location;
    }

    private async void AddToFavoritesButton_Clicked(object sender, EventArgs e)
    {
        if (!UserState._UserSignedIn)
        {
            await ShowToastAsync("Not Signed In!", 3000);
            return;
        }
        bool res = await _UserDataService.AddFavorite(UserState._UserUuid, _event.uuid);

        _favoritesViewModel.AddToFavorites(_event);
        await ShowToastAsync("Event added to favorites!", 3000); // display toast
    }

    private async Task ShowToastAsync(string message, int duration)
    {
        toastLabel.Text = message;
        toastLabel.IsVisible = true;

        await toastLabel.FadeTo(1, 250);
        await Task.Delay(duration);
        await toastLabel.FadeTo(0, 250);

        toastLabel.IsVisible = false;
    }
}
