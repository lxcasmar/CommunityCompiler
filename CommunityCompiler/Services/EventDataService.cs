using System;
using System.Collections.ObjectModel;
using System.Text;
using CommunityCompiler.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace CommunityCompiler.Services
{
    /// <summary>
	/// <inheritdoc/>
	/// <c>EventDataService</c> is used to make requests regarding events, including:
	/// <list type="bullet">
	/// <item>Searching for an event</item>
	/// <item>Requesting information on a specific event</item>
	/// <item>Create a new event</item>
	/// <item>Request the information of all events</item>
	/// </list>
	/// </summary>
	public class EventDataService : DataService
	{
        public enum EventColumns
        {
            Name,
            Location,
            Description,
            StartDate,
            EndDate,
            Capacity
        }

        public static string testEventData;
        public static ObservableCollection<Event> Events;

        public static void PopulateEvents(string data)
        {
            JArray arr = JArray.Parse(data);
            var temp = new ObservableCollection<Event>();
            foreach (JObject obj in arr)
            {
                Event e = JsonConvert.DeserializeObject<Event>(obj.ToString());
                Console.WriteLine(e);
                temp.Add(e);
            }
            Console.WriteLine("Done");
            Events = temp;
        }

        public EventDataService()
		{
		}

        public async Task<string> Init()
        {
            Send("ALLEVT");
            //string res = await stcs.Task;
            testEventData = await stcs.Task;
            return testEventData;
        }

        /// <summary>
        /// Sends a test message to the server. Similar to a Ping
        /// </summary>
        /// <returns>A simple response from the server</returns>
        public async Task<string> Hello()
        {
            Send("HELLO");
            string response = await stcs.Task;
            return response;
        }

        /// <summary>
        /// Get all information on the specified event
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>JSON-formatted string containing the event's information</returns>
        public async Task<string> SelectEvent(string uuid)
        {
            Send("SELEVT;" + uuid);
            string response = await stcs.Task;
            return response;
        }

        /// <summary>
        /// Get a JSON-formatted representation of all the events in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<string> AllEvents()
        {
            Send("ALLEVT");
            return await stcs.Task;
        }

        /// <summary>
        /// Search for an event based on a specific parameter
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns>JSON-formatted information on events matching the provided parameters, if any.</returns>
        public async Task<string> SearchEvent(EventColumns column, string value)
        {
            Send("SRCHEVT;" + column.ToString() + _ParamDelimiter + value);
            string response = await stcs.Task;
            return response;
        }

        /// <summary>
        /// Attempts to create an event
        /// </summary>
        /// <param name="ownerUUID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="start">Date formatted as YYYY-MM-DD HH:MM:SS</param>
        /// <param name="end"></param>
        /// <param name="capacity"></param>
        /// <returns><c>true</c> if success</returns>
        public async Task<bool> CreateEvent(string ownerUUID,
                                            string title,
                                            string description,
                                            string location,
                                            string start,
                                            string end=null,
                                            int capacity=-1)
        {
            Send("CRTEVT;"
                 + ownerUUID + _ParamDelimiter
                 + title + _ParamDelimiter
                 + description + _ParamDelimiter
                 + location + _ParamDelimiter
                 + start + _ParamDelimiter
                 + (end is not null ? end + _ParamDelimiter : "")
                 + (capacity != -1 ? capacity : ""));
            bool response = await btcs.Task;
            return response;
        }

        public override void OnMessage(object sender, MessageEventArgs e)
        {
            base.OnMessage(sender, e);
            string tag = Encoding.UTF8.GetString(e.RawData).Split(Environment.NewLine)[0];

            if (tag == "ALLEVT")
            {
                EventDataService.testEventData = e.Data;
            }

            switch (tag)
            {
                case "HELLO" or "SELEVT" or "ALLEVT" or "SRCHEVT":
                    stcs.SetResult(e.Data);
                    break;
                case "CRTEVT":
                    btcs.SetResult(Boolean.Parse(e.Data));
                    break;
            }
        }
    }
}

