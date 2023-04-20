using CommunityToolkit.Maui.Views;

namespace CommunityCompiler.Views;

public partial class SettingView : ContentPage
{
	public SettingView()
	{
		InitializeComponent();
	}

    private async void accountBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
    }

    private async void aboutBtnClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new AboutPopup());
    }

    private async void privacyBtnClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new PrivacyPopup());
    }
}
