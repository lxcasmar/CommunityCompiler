using CommunityCompiler.Models;
using CommunityCompiler.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CommunityCompiler.ViewModels
{
    public class FavoritesViewModel
    {
        public ObservableCollection<Event> FavoriteEvents { get; set; }
        public ICommand DeleteFavoriteEventCommand { get; set; }

        public FavoritesViewModel()
        {
            FavoriteEvents = new ObservableCollection<Event>();
            DeleteFavoriteEventCommand = new Command<Event>(DeleteFavoriteEvent);
        }

        public void AddToFavorites(Event eventToAdd)
        {
            FavoriteEvents.Add(eventToAdd);
        }

        private void DeleteFavoriteEvent(Event eventToDelete)
        {
            if (eventToDelete != null)
            {
                FavoriteEvents.Remove(eventToDelete);
            }
        }
    }
}