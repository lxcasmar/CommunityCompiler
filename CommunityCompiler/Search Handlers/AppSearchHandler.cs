using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityCompiler.Search_Handlers
{
    internal class AppSearchHandler : SearchHandler
    {
        public IList<Event> Event { get; set; }
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = Event.Where(Event => Event.Name.ToLower().Contains(newValue.ToLower())).ToList();
            }
        }
        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
        }
    }
}
