using CommunityCompiler.Views;
using CommunityCompiler.Services;

namespace CommunityCompiler;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavBar(ServiceAid.GetService<EventDataService>(),
							  ServiceAid.GetService<UserDataService>());
    }
}
