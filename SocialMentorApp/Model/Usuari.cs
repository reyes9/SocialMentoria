using System;
using SQLite;

namespace SocialMentorApp
{
	public class Usuari
	{
		public Usuari ()
		{
		}

		public Usuari (String username, String password) {
			this.Username = username;
			this.Password = password;
		}

		[PrimaryKey, AutoIncrement]
		public int ID { private set; get; }

		public String Username {set; get; }

		public String Password{ private set; get;}

	}
}

