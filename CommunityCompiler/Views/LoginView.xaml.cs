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

    private void loginBtn_clicked(object sender, EventArgs e)
    {
        string[] args = new string[]
        {
            userNameEntry.Text,
            passwordEntry.Text
        };

        _ViewModel.Submit?.Execute(args);
    }
}
