using System.Collections.ObjectModel;

namespace CommunityCompiler.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ObservableCollection<string> ImageSources { get; set; }

    public HomeViewModel()
	{
        ImageSources = new ObservableCollection<string>
            {
                "pittsburgh_home.jpg",
                "cathedral_home.jpg",
                "ppg_home.jpg",
                "downtown_home.jpg"
            };

    }
}
