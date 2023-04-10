namespace CommunityCompiler.Views;
using CommunityCompiler.Services;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
	}

    private async void homeBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavBar(ServiceAid.GetService<EventDataService>()));
    }

    private async void registerBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
    }
}
