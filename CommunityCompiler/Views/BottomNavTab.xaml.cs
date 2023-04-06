//using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.Platform;

//namespace CommunityCompiler.Views;

//public partial class BottomNavTab : TabbedPage
//{
//    public BottomNavTab()
//	{
//        InitializeComponent();

//        nav_home.IconImageSource = "selected_home.png";
//        nav_search.IconImageSource = "unselected_search.png";
//        nav_settings.IconImageSource = "unselected_settings.png";
//        nav_favorites.IconImageSource = "unselected_favorites.png";

//        CurrentPageChanged += CurrentPageHasChanged;
//    }

//    private void CurrentPageHasChanged(object sender, EventArgs e)
//    {
//        var tabbedPage = (TabbedPage)sender;
//        Title = tabbedPage.CurrentPage.Title;

//        if (Title == "Home")
//        {
//            nav_home.IconImageSource = "selected_home.png";
//            nav_search.IconImageSource = "unselected_search.png";
//            nav_settings.IconImageSource = "unselected_settings.png";
//            nav_favorites.IconImageSource = "unselected_favorites.png";
//        }

//        if (Title == "Search")
//        {
//            nav_home.IconImageSource = "unselected_home.png";
//            nav_search.IconImageSource = "selected_search.png";
//            nav_settings.IconImageSource = "unselected_settings.png";
//            nav_favorites.IconImageSource = "unselected_favorites.png";
//        }

//        if (Title == "Settings")
//        {
//            nav_home.IconImageSource = "unselected_home.png";
//            nav_search.IconImageSource = "unselected_search.png";
//            nav_settings.IconImageSource = "selected_settings.png";
//            nav_favorites.IconImageSource = "unselected_favorites.png";
//        }

//        if (Title == "Favorites")
//        {
//            nav_home.IconImageSource = "unselected_home.png";
//            nav_search.IconImageSource = "unselected_search.png";
//            nav_settings.IconImageSource = "unselected_settings.png";
//            nav_favorites.IconImageSource = "selected_favorites.png";
//        }
//    }
//}
