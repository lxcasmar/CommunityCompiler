using System;
using System.Collections.Generic;
using CommunityCompiler.ViewModels;

namespace CommunityCompiler.Services
{
    public static class ServiceAid
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        static ServiceAid()
        {
            Services.Add(typeof(SearchEventsViewModel), new SearchEventsViewModel());
            Services.Add(typeof(FavoritesViewModel), new FavoritesViewModel());
            Services.Add(typeof(EventDataService), new EventDataService());
            Services.Add(typeof(UserDataService), new UserDataService());
            Services.Add(typeof(HomeViewModel), new HomeViewModel());
            Services.Add(typeof(LoginViewModel), new LoginViewModel());
            Services.Add(typeof(RegisterViewModel), new RegisterViewModel());
            Services.Add(typeof(BaseViewModel), new BaseViewModel());
        }

        public static TService GetService<TService>()
        {
            return (TService)Services[typeof(TService)];
        }

        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
            MauiWinUIApplication.Current.Services;
#elif ANDROID
            MauiApplication.Current.Services;
#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
            null;
#endif
    }
}
