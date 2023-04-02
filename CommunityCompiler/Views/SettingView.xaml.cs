namespace CommunityCompiler.Views;

public partial class SettingView : ContentPage
{
	public SettingView()
	{
		InitializeComponent();
	}

    private async void favoriteBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritesView());
    }

    private async void searchEventBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchEventsView());
    }

    private async void homeBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomeView());
    }
}
