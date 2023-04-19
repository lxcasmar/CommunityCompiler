using CommunityCompiler.Models;
using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;

namespace CommunityCompiler.Views
{

    public partial class SearchEventsView : ContentPage
    {
        SearchEventsViewModel _ViewModel;
        EventDataService _EventDataService;

        public SearchEventsView()
        {

            InitializeComponent();
            _EventDataService = ServiceAid.GetService<EventDataService>();
            BindingContext = _ViewModel = ServiceAid.GetService<SearchEventsViewModel>();
            
        }

        protected override void OnAppearing()
        {
            _ViewModel.PropertyChanged += _ViewModel.OnViewModelPropertyChanged;
            EventDataService.PopulateEvents(EventDataService.testEventData);
            _ViewModel.Events = EventDataService.Events;
            //Console.WriteLine("anyREsponse? " + UserState._UserUuid);
            
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _ViewModel.PropertyChanged -= _ViewModel.OnViewModelPropertyChanged;
            base.OnDisappearing();
        }
       

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = _ViewModel;
            EventsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                EventsListView.ItemsSource = _container.Events;
            else
                EventsListView.ItemsSource = _container.Events.Where(i => i.name.Contains(e.NewTextValue));
            EventsListView.EndRefresh();
        }

        private async void OnListViewClick(object sender, SelectedItemChangedEventArgs e)
        {
            var eventItem = e.SelectedItem as Event;
            await Navigation.PushAsync(new EventDetailsView(eventItem, true));
        }


    }
}
