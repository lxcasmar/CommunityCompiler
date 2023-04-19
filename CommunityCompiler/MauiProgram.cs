using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;
using CommunityCompiler.Views;
using CommunityToolkit.Maui;

namespace CommunityCompiler;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var _Builder = MauiApp.CreateBuilder();
		_Builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services.AddSingleton<HomeViewModel>()
			//.AddSingleton<HomeView>()
			.AddSingleton<LoginViewModel>()
			.AddSingleton<LoginView>()
			.AddSingleton<RegisterViewModel>()
			.AddSingleton<RegisterView>()
			.AddSingleton<SettingsViewModel>()
			//.AddSingleton<SettingView>()
			.AddSingleton<FeaturedEventsViewModel>()
			.AddSingleton<FeaturedEventsView>()
			.AddSingleton<SearchEventsViewModel>()
			//.AddSingleton<SearchEventsView>()
			.AddSingleton<NavBarViewModel>()
			.AddSingleton<NavBar>()
			//.AddSingleton<FavoritesView>()
			.AddSingleton<FavoritesViewModel>()
			.AddSingleton<EventDataService>()
			.AddSingleton<UserDataService>();

		return _Builder.Build();
	}
}

