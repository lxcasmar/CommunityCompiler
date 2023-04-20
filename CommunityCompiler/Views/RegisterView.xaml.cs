using System;
using System.Windows.Input;
using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;

namespace CommunityCompiler.Views;

public partial class RegisterView : ContentPage
{
    RegisterViewModel _ViewModel;

	public RegisterView()
	{
		InitializeComponent();
        BindingContext = _ViewModel = ServiceAid.GetService<RegisterViewModel>();
	}

    private async void loginBtn_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
    }

    private void registerBtn_clicked(object sender, EventArgs e)
    {
        // check existing username or email
        // check passwords match

        string[] args = new string[]
        {
            userNameEntry.Text,
            passwordEntry.Text,
            emailEntry.Text,
            phoneNumberEntry.Text,
            "0"
        };
        _ViewModel.Submit?.Execute(args);
    }
}
