namespace CommunityCompiler.Views;

public partial class FavoritesView : ContentPage
{
	public FavoritesView()
	{
		InitializeComponent();
	}

    private async void searchBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchEventsView());
    }

    private async void settingBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingView());
    }

    private async void homeBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomeView());
    }
}
