using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityCompiler.Interfaces;
using CommunityCompiler.Models;
using CommunityCompiler.Services;
using CommunityCompiler.Views;

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
				await Shell.Current.Navigation.PopToRootAsync(false);
				//TODO: above line should probably replaced with removing pages
				// from the navigation stack instead of actually navigating.
                await Shell.Current.GoToAsync(route as String, false);
            });
			
			Console.WriteLine($"Pushing Navigation to {route}");
		});

		public bool UserSignedIn
		{
			get { return UserState._UserSignedIn; }
			set
			{
                UserState._UserSignedIn = value;
				NotifyPropertyChanged();
			}
		}

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

