namespace CommunityCompiler.Views;

public partial class EventDetailsView : ContentPage
{
	public EventDetailsView(string name, string location, string startDate, string endDate, string startTime, string endTime, int dist)
	{
		InitializeComponent();
		eventName.Text = name;
		Location.Text = "City: " + location;
		Distance.Text = "Distance: " + dist.ToString();
	}
}
