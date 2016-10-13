using System;
using SQLite;

namespace SocialMentorApp
{
	public class TipusActivitat
	{
		public TipusActivitat (string tipus, int idFormulari)
		{
			this.Tipus = tipus;
			this.IDFormulari = idFormulari;
		}

		[PrimaryKey,AutoIncrement]
		public int ID { private set; get;}
		public string Tipus { private set; get;}
		public int IDFormulari { set; get;}

	}
}

