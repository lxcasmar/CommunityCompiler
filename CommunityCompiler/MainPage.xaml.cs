using System;
using CommunityCompiler.Services;

namespace CommunityCompiler;

public partial class MainPage : ContentPage
{
	int count = 0;
	DataService test;
	public MainPage()
	{
		InitializeComponent();
        test = new DataService();
        string response = test.Connect("10.4.69.165", 2000);
        Console.WriteLine(response);
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);

		test.send("HELLO");
    }
}


