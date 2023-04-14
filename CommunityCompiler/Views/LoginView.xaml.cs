namespace CommunityCompiler.Views;
using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;

public partial class LoginView : ContentPage
{
    LoginViewModel _ViewModel;

	public LoginView()
	{
		InitializeComponent();
        BindingContext = _ViewModel = ServiceAid.GetService<LoginViewModel>();
	}

    private async void homeBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavBar(ServiceAid.GetService<EventDataService>(),
                                              ServiceAid.GetService<UserDataService>()));
    }

    private async void registerBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
    }
}
