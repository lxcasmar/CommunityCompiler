using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityCompiler.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommunityCompiler.ViewModels
{
    public class SearchEventsViewModel : BaseViewModel
    {
        private ObservableCollection<Event> _Events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get { return _Events; }
            set
            {
                _Events = value;
                NotifyPropertyChanged(nameof(Events));
            }
        }


        public override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Events):
                    Console.WriteLine("changed Events");
                    break;
            }
            base.OnViewModelPropertyChanged(sender, e);
        }

        public void PopulateEvents(string data)
        {
            JArray arr = JArray.Parse(data);
            var temp = new List<Event>();
            foreach (JObject obj in arr)
            {
                Event e = JsonConvert.DeserializeObject<Event>(obj.ToString());
                temp.Add(e);
            }
            Console.WriteLine("Done");
            Events = new ObservableCollection<Event>(temp);
        }

        public SearchEventsViewModel()
        {
             Events = new ObservableCollection<Event>();

            //{
            //    new Event(){Name = "Cavs v Warriors", Location = "Cleveland", StartDate = "11/11/23", EndDate = "11/11/23", StartTime = "7:30", EndTime = "11:00", Dist = 50},
            //    new Event(){Name = "Lantern Fest", Location = "Pittsburgh", StartDate = "7/4/23", EndDate = "7/5/23", StartTime = "6:30", EndTime = "10:00", Dist = 4},
            //    new Event(){Name = "Tame Impala Concert", Location = "Pittsburgh", StartDate = "05/11/23", EndDate = "05/11/23", StartTime = "8:00", EndTime = "12:00", Dist = 1},
            //    new Event(){Name = "Steelers v Eagles", Location = "Pittsburgh", StartDate = "12/17/23", EndDate = "12/17/23", StartTime = "7:00", EndTime = "10:00", Dist = 50},
            //    new Event(){Name = "Rib Fest", Location = "Pittsburgh", StartDate = "7/4/23", EndDate = "7/6/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Pickle Week", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00"},
            //    new Event(){Name = "Pitt v UNC Basketball", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Pickle Week", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Hour of Code", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Filler Event", Location = "Pittsburgh", StartDate = "8/1/23", EndDate = "8/1/23", StartTime = "12:00", EndTime = "4:00", Dist = 50},
            //    new Event(){Name = "Filler Event 2", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Filler Event 3", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50},
            //    new Event(){Name = "Filler Event 4", Location = "Pittsburgh", StartDate = "8/10/23", EndDate = "8/17/23", StartTime = "12:00", EndTime = "12:00", Dist = 50}

            //};
        }
    }
}

