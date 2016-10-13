using System;
using SQLite;

namespace SocialMentorApp
{
	public class RelacioDeMentoria
	{

		public RelacioDeMentoria() {}
		public RelacioDeMentoria (int id, int idM)
		{
			this.ID = id;
			this.IDMentorat = idM;
		}

		[PrimaryKey]
		public int ID { private set; get; }

		public int IDMentorat { private set; get;}

	}
}

