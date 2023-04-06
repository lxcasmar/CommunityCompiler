namespace CommunityCompiler.Views;

public partial class EventDetailsView : ContentPage
{
	public EventDetailsView()
	{
		InitializeComponent();
	}

	public async void OnBackButtonClick(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
    }
}
