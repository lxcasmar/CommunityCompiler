using CommunityToolkit.Maui.Views;

namespace CommunityCompiler.Views;

public partial class PrivacyPopup : Popup
{
	public PrivacyPopup()
	{
		InitializeComponent();
	}

    private async void closeBtnClicked(object sender, EventArgs e)
    {
        Close();
    }
}
