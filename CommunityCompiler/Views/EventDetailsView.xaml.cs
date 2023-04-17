using CommunityCompiler.ViewModels;
using CommunityCompiler.Services;

namespace CommunityCompiler.Views;

public partial class EventDetailsView : ContentPage
{
    private readonly Event _event;
    private readonly FavoritesViewModel _favoritesViewModel;

    public EventDetailsView(Event eventDetails, Boolean buttonVisibility)
    {
        InitializeComponent();

        _event = eventDetails;
        _favoritesViewModel = ServiceAid.GetService<FavoritesViewModel>();

        favoriteBtn.IsVisible = buttonVisibility;
        eventName.Text = _event.Name;
        Location.Text = "City: " + _event.Location;
        Distance.Text = "Distance: " + _event.Dist.ToString();
    }

    private async void AddToFavoritesButton_Clicked(object sender, EventArgs e)
    {
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
