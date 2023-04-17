using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

namespace CommunityCompiler.Views
{

    public partial class SearchEventsView : ContentPage
    {
        SearchEventsViewModel _ViewModel;

        public SearchEventsView()
        {

            InitializeComponent();
            BindingContext = _ViewModel = ServiceAid.GetService<SearchEventsViewModel>();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = _ViewModel;
            EventsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                EventsListView.ItemsSource = _container.Events;
            else
                EventsListView.ItemsSource = _container.Events.Where(i => i.Name.Contains(e.NewTextValue));
            EventsListView.EndRefresh();
        }

        private async void OnListViewClick(object sender, SelectedItemChangedEventArgs e)
        {
            var eventItem = e.SelectedItem as Event;
            await Navigation.PushAsync(new EventDetailsView(eventItem, true));
        }


    }
}
