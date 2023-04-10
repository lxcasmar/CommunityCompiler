using CommunityCompiler.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

namespace CommunityCompiler.Views
{

    public partial class SearchEventsView : ContentPage
    {

        public SearchEventsView()
        {

            InitializeComponent();
            BindingContext = new SearchEventsViewModel();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as SearchEventsViewModel;
            EventsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                EventsListView.ItemsSource = _container.Events;
            else
                EventsListView.ItemsSource = _container.Events.Where(i => i.Name.Contains(e.NewTextValue));
            EventsListView.EndRefresh();
        }

        private async void OnListViewClick(object sender, SelectedItemChangedEventArgs e)
        {
            var events = e.SelectedItem as Event;
            await Navigation.PushAsync(new EventDetailsView(events.Name, events.Location, events.StartDate, events.EndDate, events.StartTime, events.EndTime, events.Dist));
        }


    }
}
