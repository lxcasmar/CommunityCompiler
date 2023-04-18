using System;
using System.Windows.Input;
using CommunityCompiler.Services;

namespace CommunityCompiler.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		UserDataService _UserDataService;

		/// <summary>
		/// Command bound to the "Submit" button when authenticating a user
		/// </summary>
		public ICommand Submit { get; set; }

		public LoginViewModel()
		{
            _UserDataService = ServiceAid.GetService<UserDataService>();

            Submit = new Command((args) =>
			{
				string username = (args as String[])[0];
				string password = (args as String[])[1];
				// TODO: need to test edge case here. pretty sure it actually returns "null"
				if (_UserDataService.Auth(username, password).Result is not null)
				{
					Console.WriteLine("Authication successfull");
					UserSignedIn = true;
				} else
				{
					Console.WriteLine("Authentication failed");
				}
			});
		}
	}
}

