using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using CommunityCompiler.Services;

namespace CommunityCompiler.Views;

public partial class NavBar : TabbedPage
{
    UserDataService _UserDataService;

    public NavBar(EventDataService eventDataService, UserDataService userDataService)
    {
        InitializeComponent();
#if ANDROID || WINDOWS10_0_17763_0_OR_GREATER
        nav_home.IconImageSource = "selected_home.png";
        nav_search.IconImageSource = "unselected_search.png";
        nav_settings.IconImageSource = "unselected_settings.png";
        nav_favorites.IconImageSource = "unselected_favorites.png";
#else
        // for some reason, not automatic resizing for icon images on iOS
        nav_home.IconImageSource = "selected_home_s.png";
        nav_search.IconImageSource = "unselected_search_s.png";
        nav_settings.IconImageSource = "unselected_settings_s.png";
        nav_favorites.IconImageSource = "unselected_favorites_s.png";
#endif

        CurrentPageChanged += CurrentPageHasChanged;

        MessagingCenter.Subscribe<Object, int>(this, "Search", ((arg, idx) => {

            CurrentPage = this.Children[1];

        }));

        MessagingCenter.Subscribe<Object, int>(this, "Favorites", ((arg, idx) => {

            CurrentPage = this.Children[2];

        }));

        MessagingCenter.Subscribe<Object, int>(this, "Settings", ((arg, idx) => {

            CurrentPage = this.Children[3];

        }));

        eventDataService.Connect("ec2-3-16-160-137.us-east-2.compute.amazonaws.com");
        _UserDataService = userDataService;
        _UserDataService.Connect("ec2-3-14-68-89.us-east-2.compute.amazonaws.com");
    }

    private async void CurrentPageHasChanged(object sender, EventArgs e)
    {
        var tabbedPage = (TabbedPage)sender;
        Title = tabbedPage.CurrentPage.Title;

#if ANDROID || WINDOWS10_0_17763_0_OR_GREATER
        if (Title == "Home")
        {
            nav_home.IconImageSource = "selected_home.png";
            nav_search.IconImageSource = "unselected_search.png";
            nav_settings.IconImageSource = "unselected_settings.png";
            nav_favorites.IconImageSource = "unselected_favorites.png";
        }

        if (Title == "Search")
        {
            nav_home.IconImageSource = "unselected_home.png";
            nav_search.IconImageSource = "selected_search.png";
            nav_settings.IconImageSource = "unselected_settings.png";
            nav_favorites.IconImageSource = "unselected_favorites.png";
        }

        if (Title == "Settings")
        {
            nav_home.IconImageSource = "unselected_home.png";
            nav_search.IconImageSource = "unselected_search.png";
            nav_settings.IconImageSource = "selected_settings.png";
            nav_favorites.IconImageSource = "unselected_favorites.png";
        }

        if (Title == "Favorites")
        {
            nav_home.IconImageSource = "unselected_home.png";
            nav_search.IconImageSource = "unselected_search.png";
            nav_settings.IconImageSource = "unselected_settings.png";
            nav_favorites.IconImageSource = "selected_favorites.png";
        }
#else
        if (Title == "Home")
        {
            nav_home.IconImageSource = "selected_home_s.png";
            nav_search.IconImageSource = "unselected_search_s.png";
            nav_settings.IconImageSource = "unselected_settings_s.png";
            nav_favorites.IconImageSource = "unselected_favorites_s.png";
        }

        if (Title == "Search")
        {
            nav_home.IconImageSource = "unselected_home_s.png";
            nav_search.IconImageSource = "selected_search_s.png";
            nav_settings.IconImageSource = "unselected_settings_s.png";
            nav_favorites.IconImageSource = "unselected_favorites_s.png";
        }

        if (Title == "Settings")
        {
            nav_home.IconImageSource = "unselected_home_s.png";
            nav_search.IconImageSource = "unselected_search_s.png";
            nav_settings.IconImageSource = "selected_settings_s.png";
            nav_favorites.IconImageSource = "unselected_favorites_s.png";
        }

        if (Title == "Favorites")
        {
            nav_home.IconImageSource = "unselected_home_s.png";
            nav_search.IconImageSource = "unselected_search_s.png";
            nav_settings.IconImageSource = "unselected_settings_s.png";
            nav_favorites.IconImageSource = "selected_favorites_s.png";
        }
#endif
    }
}
