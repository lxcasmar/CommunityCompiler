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

		/// <summary>
		/// Gets the favorited events by a particular user
		/// </summary>
		/// <param name="uuid">The user's UUID</param>
		/// <returns>UUIDs of favorited events</returns>
		public async Task<string[]> GetAllFavorites(String uuid)
		{
			Send("ALLFAV;" + uuid);
			string response = await stcs.Task;
			string[] fin = response.Split(',');
			return fin;
		}

		/// <summary>
		/// Adds an event to a user's favorited events
		/// </summary>
		/// <param name="userUuid"></param>
		/// <param name="eventUuid">UUID of event to be favorited</param>
		/// <returns>success/failure</returns>
		public async Task<bool> AddFavorite(string userUuid, string eventUuid)
		{
			Send("ADDFAV;"
				+ userUuid + _ParamDelimiter
				+ eventUuid);
			bool response = await btcs.Task;
			return response;
		}

		/// <summary>
		/// Deletes an event from the user's favorited events
		/// </summary>
		/// <param name="userUuid"></param>
		/// <param name="eventUuid"></param>
		/// <returns></returns>
		public async Task<bool> DelFavorite(string userUuid, string eventUuid)
		{
			Send("DELFAV;"
				+ userUuid + _ParamDelimiter
				+ eventUuid);
			bool response = await btcs.Task;
			return response;
		}

        public override void OnMessage(object sender, MessageEventArgs e)
        {
            base.OnMessage(sender, e);
			string tag = e.Data.Split(Environment.NewLine)[0];
			switch (tag)
			{
				case "HELLO" or "ALLUSR" or "SELUSR" or "AUTH" or "ALLFAV":
					stcs.SetResult(e.Data);
					break;
				case "CRTUSR" or "ADDFAV" or "DELFAV":
					btcs.SetResult(Boolean.Parse(e.Data));
					break;
			}
        }
    }
}

