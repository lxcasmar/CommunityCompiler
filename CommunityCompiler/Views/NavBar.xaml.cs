using System.Windows.Input;
using CommunityCompiler.ViewModels;
using CommunityCompiler.Services;

namespace CommunityCompiler.Views;

public partial class NavBar : ContentView
{
	public NavBar()
	{
		InitializeComponent();
		BindingContext = ServiceAid.GetService<NavBarViewModel>();
	}

}