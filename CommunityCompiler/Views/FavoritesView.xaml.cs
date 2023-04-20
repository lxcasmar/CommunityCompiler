using CommunityCompiler.ViewModels;
using CommunityCompiler.Services;
using CommunityCompiler.Models;
using System.Linq;

namespace CommunityCompiler.Views;

public partial class FavoritesView : ContentPage
{
    private readonly FavoritesViewModel _favoritesViewModel;
    private UserDataService _UserDataService;
    private EventDataService _EventDataService;

    public FavoritesView()
    {
        InitializeComponent();

        _favoritesViewModel = ServiceAid.GetService<FavoritesViewModel>();
        _UserDataService = ServiceAid.GetService<UserDataService>();
        _EventDataService = ServiceAid.GetService<EventDataService>();
        BindingContext = _favoritesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (UserState._UserSignedIn)
        {
            var uuids = await _UserDataService.GetAllFavorites(UserState._UserUuid);
            List<string> events = new List<string>();
            foreach(var uuid in uuids)
            {
                events.Add(await _EventDataService.SelectEvent(uuid));
            }
            _favoritesViewModel.PopulateFavorites(events.ToArray<string>());
        }
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