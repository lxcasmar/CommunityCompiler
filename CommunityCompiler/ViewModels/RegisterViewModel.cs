using System;
using System.Windows.Input;
using CommunityCompiler.Models;
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

        public RegisterViewModel()
		{
            _UserDataService = ServiceAid.GetService<UserDataService>();

            Submit = new Command(async (args) =>
            {
                string username = (args as String[])[0];
                string password = (args as String[])[1];
                string email = (args as String[])[2];
                string phoneNumber = (args as String[])[3];
                int isAdmin = Int32.Parse((args as String[])[4]);
                var result = await _UserDataService.CreateUser(username, password, email, phoneNumber, isAdmin);
                if (result)
                {
                    Console.WriteLine("Successfully created user: " + username);
                    UserState._UserSignedIn = true;
                    UserState._CurUserName = username;
                } else
                {
                    Console.WriteLine("User creation failed");
                }
            });
        }
	}
}