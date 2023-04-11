using System;
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
}
