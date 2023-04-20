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
            //EventsListView.ItemTemplate = new DataTemplate(() =>
            //{
            //    var g = new Grid();
            //    g.Padding = 10;
            //    var f = new Frame()
            //    {
            //        CornerRadius = 20
            //    };
            //    var sl = new StackLayout()
            //    {
            //        Padding = 10
            //    };
            //    Label name = new Label();
            //    //name.SetBinding(Label.TextProperty, nameof(Event.name));
            //    Label loc = new Label();
            //    //loc.SetBinding(Label.TextProperty, nameof(Event.location));
            //    Label sd = new Label();
            //    //sd.SetBinding(Label.TextProperty, nameof(Event.startDate));
            //    sl.Add(name);
            //    sl.Add(loc);
            //    sl.Add(sd);
            //    f.Content = sl;
            //    g.Add(f);
            //    var vc = new ViewCell();
            //    vc.View = sl;
            //    return vc;
            //});
        }

        protected override async void OnAppearing()
        {
            _ViewModel = ServiceAid.GetService<SearchEventsViewModel>();
            BindingContext = _ViewModel;
            _ViewModel.PropertyChanged += _ViewModel.OnViewModelPropertyChanged;
            _ViewModel.PopulateEvents(await _EventDataService.AllEvents());
            if (this.BindingContext == null)
            {
                Console.WriteLine("UHHH AWKWARDNSDOAHSODH (OIUAHDOAHSOD");
            }

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
