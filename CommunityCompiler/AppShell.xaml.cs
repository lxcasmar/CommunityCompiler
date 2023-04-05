using CommunityCompiler.Views;

namespace CommunityCompiler;

public partial class AppShell : Shell
{
	public AppShell()
	{
		Routing.RegisterRoute("Views/HomeView", typeof(HomeView));
		Routing.RegisterRoute("Views/LoginView", typeof(LoginView));
		Routing.RegisterRoute("Views/RegisterView", typeof(RegisterView));
		Routing.RegisterRoute("Views/EventDetailsView", typeof(EventDetailsView));
		Routing.RegisterRoute("Views/FavoritesView", typeof(FavoritesView));
		Routing.RegisterRoute("Views/FeaturedEventsView", typeof(FeaturedEventsView));
		Routing.RegisterRoute("Views/SearchEventsView", typeof(SearchEventsView));
		Routing.RegisterRoute("Views/SettingsView", typeof(SettingView));
		InitializeComponent();

		Shell.SetNavBarIsVisible(this, false);
	}
}