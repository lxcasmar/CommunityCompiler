using System;
using CommunityCompiler.Services;
using CommunityCompiler.ViewModels;

namespace CommunityCompiler;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		//BindingContext = _ViewModel = viewModel;
		//Console.WriteLine(new DataService().Connect(2000));
	}

	SearchEventsViewModel _ViewModel;

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}


