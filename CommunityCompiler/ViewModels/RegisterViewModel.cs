using System;
using System.Windows.Input;
using CommunityCompiler.Services;

namespace CommunityCompiler.ViewModels
{
	public class RegisterViewModel : BaseViewModel
	{
		UserDataService _UserDataService;

        /// <summary>
        /// Command bound to the "Submit" button when registering a new user
        /// </summary>
        public ICommand Submit { get; set; }

        public RegisterViewModel(UserDataService userDataService)
		{
			_UserDataService = userDataService;

			Submit = new Command((args) =>
            {
                string username = (args as String[])[0];
                string password = (args as String[])[1];
                string email = (args as String[])[2];
                string phoneNumber = (args as String[])[3];
                int isAdmin = Int32.Parse((args as String[])[4]);
                var result = _UserDataService.CreateUser(username, password, email, phoneNumber, isAdmin);
                if (result.Result)
                {
                    Console.WriteLine("Successfully created user: " + username);
                    UserSignedIn = true;
                } else
                {
                    Console.WriteLine("User creation failed");
                }
            });
        }
	}
}