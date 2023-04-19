using System;
namespace CommunityCompiler.Models
{
	public static class UserState
	{
		public static bool _UserSignedIn { get; set; } = false;

		public static string _UserUuid { get; set; } = null;
	}
}

