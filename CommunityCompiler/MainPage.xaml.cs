using System;
using CommunityCompiler.Views;

using CommunityCompiler.Services;

namespace CommunityCompiler;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
    }

	private async void loginBtn_clicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new LoginView());
    }

    private async void registerBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
    }
}


