using CommunityCompiler.Models;
using CommunityCompiler.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CommunityCompiler.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        private ObservableCollection<Event> _FavoriteEvents;
        public ObservableCollection<Event> FavoriteEvents
        {
            get
            {
                return _FavoriteEvents;
            }
            set
            {
                _FavoriteEvents = value;
                NotifyPropertyChanged(nameof(FavoriteEvents));
            }
        }


        public ICommand DeleteFavoriteEventCommand { get; set; }

        public FavoritesViewModel()
        {
            FavoriteEvents = new ObservableCollection<Event>();
            DeleteFavoriteEventCommand = new Command<Event>(DeleteFavoriteEvent);
        }

        public void AddToFavorites(Event eventToAdd)
        {
            //FavoriteEvents.Add(eventToAdd);
        }

        private void DeleteFavoriteEvent(Event eventToDelete)
        {
            if (eventToDelete != null)
            {
                //FavoriteEvents.Remove(eventToDelete);
            }
        }

        public void PopulateFavorites(string [] data)
        {
            if (data?.Length == 0)
            {
                return;
            }

            var temp = new ObservableCollection<Event>();
            foreach (string str in data)
            {
                if (str == "[]") continue;
                JObject obj = JObject.Parse(str);
                Event e = JsonConvert.DeserializeObject<Event>(obj.ToString());
                temp.Add(e);
            }

            Console.WriteLine("PopulatedFavorites");
            FavoriteEvents = temp;
        }
    }
}