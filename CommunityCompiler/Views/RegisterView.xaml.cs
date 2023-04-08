using System;

namespace CommunityCompiler.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView()
	{
		InitializeComponent();
	}

    private async void loginBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
    }
}
