using CommunityToolkit.Maui.Views;

namespace CommunityCompiler.Views;

public partial class CreateEventPopup : Popup
{
	public CreateEventPopup()
	{
		InitializeComponent();
	}

    private async void closeBtnClicked(object sender, EventArgs e)
    {
        Close();
    }
}
