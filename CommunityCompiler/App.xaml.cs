using CommunityCompiler.Views;

namespace CommunityCompiler;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
    }
}
