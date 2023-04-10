using CommunityToolkit.Maui.Views;
using CommunityCompiler.Services;
using Microsoft.Maui.Controls;

namespace CommunityCompiler.Views;

public partial class CreateEventPopup : Popup
{
    EventDataService eventDataService;

	public CreateEventPopup(EventDataService eventDataService)
	{
		InitializeComponent();
        this.eventDataService = eventDataService;

    }

    private async void closeBtnClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.DisplayAlert("Success", "Event created successfully!", "OK");
        Close();
    }

    private async void submitBtnClicked(object sender, EventArgs e)
    {
        string ownerUUID = "your_owner_uuid"; 
        string title = eventName.Text;
        string description = eventDescription.Text;
        string location = eventAddress.Text;
        string start = eventDate.Date.ToString("yyyy-MM-dd") + " " + startTime.Time.ToString(@"hh\:mm\:ss");
        string end = eventDate.Date.ToString("yyyy-MM-dd") + " " + endTime.Time.ToString(@"hh\:mm\:ss");
        int capacity = 1; 

        bool success = await eventDataService.CreateEvent(ownerUUID, title, description, location, start, end, capacity);

        if (success)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Event created successfully!", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to create event. Please try again.", "OK");
        }

        Close();
    }
}
