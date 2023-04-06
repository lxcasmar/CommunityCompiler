using CommunityCompiler.Views;

namespace CommunityCompiler;

public partial class AppShell : Shell
{
	public AppShell()
	{
        //Routing.RegisterRoute("Views/HomeView", typeof(HomeView));
        //Routing.RegisterRoute("Views/LoginView", typeof(LoginView));
        //Routing.RegisterRoute("Views/RegisterView", typeof(RegisterView));

        Routing.RegisterRoute($"{nameof(SearchEventsView)}", typeof(SearchEventsView));
        Routing.RegisterRoute($"{nameof(EventDetailsView)}", typeof(EventDetailsView));
        InitializeComponent();
    }
}

