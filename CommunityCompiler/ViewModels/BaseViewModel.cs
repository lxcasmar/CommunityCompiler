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

		public ICommand NavigateToCommand { get; set; } = new Command((route) =>
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await Shell.Current.GoToAsync(route as String);
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

