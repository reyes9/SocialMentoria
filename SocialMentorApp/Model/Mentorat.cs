using System;
using SQLite;

namespace SocialMentorApp
{
	public class Mentorat
	{
		public Mentorat ()
		{
		}

		public Mentorat (int id, String nom, String cognom1, String cognom2){

			this.ID = id;
			this.Nom = nom;
			this.Cognom1 = cognom1;
			this.Cognom2 = cognom2;
		}

		[PrimaryKey]
		public int ID { private set; get; }
		public String Nom { set; get; }
		public String Cognom1 { set; get; }
		public String Cognom2 { set; get; }
		public String NomComplet {
			get {
				return Nom + " " + Cognom1 + " " + Cognom2;
			}
		}
		public DateTime UltimFormulari{ set; get;}

	}
}

