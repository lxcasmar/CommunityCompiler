using CommunityToolkit.Maui.Views;

namespace CommunityCompiler.Views;

public partial class AboutPopup : Popup
{
	public AboutPopup()
	{
		InitializeComponent();
	}

    private async void closeBtnClicked(object sender, EventArgs e)
    {
        Close();
    }
}
