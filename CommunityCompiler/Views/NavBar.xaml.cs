using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;

namespace CommunityCompiler.Views;

public partial class NavBar : TabbedPage
{
    public NavBar()
    {
        InitializeComponent();

        nav_home.IconImageSource = "selected_home.png";
        nav_search.IconImageSource = "unselected_search.png";
        nav_settings.IconImageSource = "unselected_settings.png";
        nav_favorites.IconImageSource = "unselected_favorites.png";

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
    }

    private void CurrentPageHasChanged(object sender, EventArgs e)
    {
        var tabbedPage = (TabbedPage)sender;
        Title = tabbedPage.CurrentPage.Title;

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
    }
}
