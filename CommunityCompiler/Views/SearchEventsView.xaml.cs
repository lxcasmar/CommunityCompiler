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

        protected override async void OnAppearing()
        {
            _ViewModel = ServiceAid.GetService<SearchEventsViewModel>();
            BindingContext = _ViewModel;
            _ViewModel.PropertyChanged += _ViewModel.OnViewModelPropertyChanged;
            _ViewModel.PopulateEvents(await _EventDataService.AllEvents());
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
            try
            {

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    EventsListView.ItemsSource = _container.Events;
                else
                {
                    List<Event> temp = new List<Event>();
                    foreach(Event t in _container.Events)
                    {
                        _ = t.name;
                        temp.Add(t);
                    }
                    temp.RemoveAt(temp.Count - 1);
                    EventsListView.ItemsSource = temp;
                }
                //    EventsListView.ItemsSource = _container.Events.Where(i => i.name.Contains(e.NewTextValue));
            } catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            EventsListView.EndRefresh();
        }

        private async void OnListViewClick(object sender, SelectedItemChangedEventArgs e)
        {
            var eventItem = e.SelectedItem as Event;
            await Navigation.PushAsync(new EventDetailsView(eventItem, true));
        }


    }
}
