using System;
namespace CommunityCompiler
{
    public class Event
    {
        public Event()
        {
        }

        public string uuid { get; set; }

        public string description { get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public string startDate { get; set; }

        public string endDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int Dist { get; set; } = 0;
    }
}
