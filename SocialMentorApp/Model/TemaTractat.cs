using System;
using SQLite;

namespace SocialMentorApp
{
	public class TemaTractat
	{
		public TemaTractat (string tema, int idFormulari)
		{
			this.Tema = tema;
			this.IDFormulari = idFormulari;
		}


		[PrimaryKey,AutoIncrement]
		public int ID { private set; get;}
		public string Tema { private set; get;}
		public int IDFormulari { set; get;}
	}
}

