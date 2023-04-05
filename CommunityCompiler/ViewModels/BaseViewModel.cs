using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityCompiler.Interfaces;

namespace CommunityCompiler.ViewModels
{
	public class BaseViewModel : IViewModel
	{
		public BaseViewModel()
		{
		}

		//TODO: consider moving navigation to a serparate service
		public ICommand NavigateToCommand { get; set; } = new Command((route) =>
		{
            var navigation = Shell.Current.Navigation;

			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await Shell.Current.GoToAsync(route as String, false);
            });

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

