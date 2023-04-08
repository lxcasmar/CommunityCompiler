namespace CommunityCompiler.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
	}

    private async void homeBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavBar());
    }

    private async void registerBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
    }
}
