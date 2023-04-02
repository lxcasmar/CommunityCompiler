namespace CommunityCompiler.Views;

public partial class HomeView : ContentPage
{
	public HomeView()
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

    private async void profileSettingBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingView());
    }
}
