using System;
using System.Windows.Input;
using CommunityCompiler.Models;
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

            Submit = new Command(async (args) =>
			{
				string username = (args as String[])[0];
				string password = (args as String[])[1];
				// TODO: need to test edge case here. pretty sure it actually returns "null"
				var res = await _UserDataService.Auth(username, password);
                if ((res is not null) && (res.Length > 0 ) )
				{
					Console.WriteLine("Authication successfull");
					UserState._UserSignedIn = true;
					UserState._CurUserName = username;
					UserState._UserUuid = res;
				} else
				{
					Console.WriteLine("Authentication failed");
				}
			});
		}
	}
}

