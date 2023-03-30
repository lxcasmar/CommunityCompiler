using System;
using WebSocketSharp;

namespace CommunityCompiler.Services
{
	/// <summary>
	/// <inheritdoc/>
	/// <c>UserDataService</c> is used to make requests regarding users, including:
	/// <list type="bullet">
	/// <item>Authenticating a user</item>
	/// <item>Requesting information on a specific user</item>
	/// <item>Create a new user</item>
	/// <item>Request the information of all users</item>
	/// </list>
	/// </summary>
	public class UserDataService : DataService
	{
		public UserDataService()
		{
		}

		/// <summary>
		/// Sends a test message to the server. Similar to a Ping
		/// </summary>
		/// <returns>A simple response from the server</returns>
		public async Task<string> Hello()
		{
			Send("HELLO");
			string response = await stcs.Task;
			return response;
		}

		/// <summary>
		/// Attempts to authenticate a user
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns><c>null</c> if failed, UUID of user otherwise</returns>
		public async Task<string> Auth(string username, string password)
		{
			Send("AUTH;" + username + _ParamDelimiter + password);
			string response = await stcs.Task;
			return response;
		}

		/// <summary>
		/// Get all information on the specified user
		/// </summary>
		/// <param name="uuid"></param>
		/// <returns>JSON-formatted string containing all information on the user</returns>
		public async Task<string> SelectUser(string uuid)
		{
			Send("SELUSR;" + uuid);
			string response = await stcs.Task;
			return response;
		}

		/// <summary>
		/// Attempts to create a user
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="email"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="isAdmin"><c>1</c> for admin users, any other number for non-admins</param>
		/// <returns>success/failure</returns>
		public async Task<bool> CreateUser(string username, string password, string email, string phoneNumber, int isAdmin)
		{
			Send("CRTUSR;"
				+ username + _ParamDelimiter
				+ password + _ParamDelimiter
				+ email + _ParamDelimiter
				+ phoneNumber + _ParamDelimiter
				+ isAdmin.ToString());
			bool response = await btcs.Task;
			return response;
		}

		/// <summary>
		/// Gets the JSON-formatted data on all users
		/// </summary>
		/// <returns></returns>
		public async Task<string> AllUsers()
		{
			Send("ALLUSR");
			string response = await stcs.Task;
			return response;
		}

        public override void OnMessage(object sender, MessageEventArgs e)
        {
            base.OnMessage(sender, e);
			string tag = e.Data.Split(Environment.NewLine)[0];
			switch (tag)
			{
				case "HELLO" or "ALLUSR" or "SELUSR" or "AUTH":
					stcs.SetResult(e.Data);
					break;
				case "CRTUSR":
					btcs.SetResult(Boolean.Parse(e.Data));
					break;
			}
        }
    }
}

