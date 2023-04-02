﻿namespace CommunityCompiler.Views;

public partial class SearchEventsView : ContentPage
{
	public SearchEventsView()
	{
		InitializeComponent();
	}

    private async void favoriteBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritesView());
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
