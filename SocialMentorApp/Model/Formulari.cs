using System;
using System.Collections.Generic;
using SQLite;

namespace SocialMentorApp
{
	public class Formulari
	{

		public Formulari ()
		{
			this.Date = DateTime.Now;
		}

		public Formulari(int IdMentorat) {
			this.Date = DateTime.Now;
			this.MentoratFormulari = IdMentorat;
		}

		public Formulari (String dia, String hora, String textActivitat, String incidencies, int valoracio ) {
			this.Date = DateTime.Now;
			this.Dia = dia;
			this.Hora = hora;
			this.TextActivitat = textActivitat;
			this.Incidencies = incidencies;
			this.Valoracio = valoracio;
		}


			
		[PrimaryKey, AutoIncrement]
		public int ID { private set; get; }
		public DateTime Date { private set; get; }
		public String Dia { set; get; }
		public String Hora { set; get; }
		public String TextActivitat { set; get; }
		public String TipusActivitats {set;get;}
		public int MentoratFormulari { set; get; }
		public String TemesTractats{set;get;}
		public String Incidencies { set; get; }
		public int Valoracio { set; get; }
		public Boolean Enviat { set; get;}
		public Boolean HasAllData {
			get {
				if (Dia != null && Hora != null && (TextActivitat != null && !TextActivitat.Equals ("")) &&
				    Valoracio != null)
					return true;
				else
					return false;
				//falta posar a l'if tots els que fallen
			}
		}

		public void SaveActivityList(List<Tuple<String,Boolean>> list) {
			//tipusActivitats = list;
		
		}

		public void SaveTreatedSubjects(List<Tuple<String,Boolean>> list) {
			//temesTractats = list;
		}

//		public string GetTipusActivitatPerEnviar() {
//			String tipusAct = "";
//			for (int i = 0; i < tipusActivitats.Count; ++i) {
//				if (tipusActivitats [i].Item2 == true)
//					tipusAct += tipusActivitats [i].Item1 + ", ";
//				if ((i == tipusActivitats.Count - 1) && !tipusAct.Equals (""))
//					tipusAct = tipusAct.Remove (tipusAct.Length - 2);
//			}
//			return tipusAct;
//		}
//
//		public string GetTemesTractatsPerEnviar() {
//			String temes = "";
//			for (int i = 0; i < temesTractats.Count; ++i) {
//				if (temesTractats [i].Item2 == true)
//					temes += temesTractats [i].Item1 + ", ";
//				if ((i == tipusActivitats.Count - 1) && !temes.Equals (""))
//					temes = temes.Remove (temes.Length - 2);
//			}
//			return temes;
//		}


	}
}
	