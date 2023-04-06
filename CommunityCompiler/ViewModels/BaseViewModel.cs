using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityCompiler.Interfaces;
using CommunityCompiler.Services;
using CommunityCompiler.Views;

namespace CommunityCompiler.ViewModels
{
	public class BaseViewModel : IViewModel
	{
		public BaseViewModel()
		{
		}

		//TODO: consider moving navigation to a serparate service
		// Remember it's possible to pass parameters via route
		public ICommand NavigateToCommand { get; set; } = new Command((route) =>
		{
            MainThread.BeginInvokeOnMainThread(async () =>
            {
				await Shell.Current.Navigation.PopToRootAsync(false);
                await Shell.Current.GoToAsync(route as String, false);
            });
            var navigation = Shell.Current.Navigation;
			var stack = navigation.NavigationStack;
			var test = Shell.Current.CurrentState;
			
			Console.WriteLine($"Pushing Navigation to {route}");
		});

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		public virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}
	}
}

