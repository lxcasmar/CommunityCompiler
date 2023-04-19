using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
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
            _ViewModel.Events = new ObservableCollection<Event>();
            string res = _EventDataService.AllEvents().Result;
            Console.WriteLine(res);
            JArray arr = JArray.Parse(res);
            foreach (JObject obj in arr)
            {
                Event e = JsonConvert.DeserializeObject<Event>(obj.ToString());
                Console.WriteLine(e);
            }
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
