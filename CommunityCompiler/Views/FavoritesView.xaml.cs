using CommunityCompiler.ViewModels;
using CommunityCompiler.Services;

namespace CommunityCompiler.Views;

public partial class FavoritesView : ContentPage
{
    private readonly FavoritesViewModel _favoritesViewModel;

    public FavoritesView()
    {
        InitializeComponent();

        _favoritesViewModel = ServiceAid.GetService<FavoritesViewModel>();
        BindingContext = _favoritesViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        FavoriteEventsListView.ItemsSource = _favoritesViewModel.FavoriteEvents;
    }

    private async void OnFavoriteEventTapped(object sender, EventArgs e)
    {
        var tapGestureRecognizer = sender as TapGestureRecognizer;
        var selectedEvent = tapGestureRecognizer?.CommandParameter as Event;
        if (selectedEvent != null)
        {
            await Navigation.PushAsync(new EventDetailsView(selectedEvent, false));
        }
    }
}