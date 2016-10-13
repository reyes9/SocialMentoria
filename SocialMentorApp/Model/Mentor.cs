using System;
using SQLite;

namespace SocialMentorApp
{
	public class Mentor
	{
		public Mentor ()
		{
		}

		public Mentor (int codi, String edat, String dni, String telefon, String dataNaixement, String poblacio, String codiPostal, String direccio,
			String organitzacio, String subcampOrganitzacio) {

			DateTime data;
		}

		[PrimaryKey, AutoIncrement]
		public int ID { private set; get; }
		public String Edat { private set; get; }
		public String Dni { private set; get; }
		public String Telefon { private set; get; }
		public String DataNaixement { private set; get; }
		public String Poblacio { private set; get; }
		public String CodiPostal { private set; get; }
		public String Direccio { private set; get; }
		public String Organitzacio { private set; get; }
		public String SubcampOrganitzacio { private set; get; }











	}
}

