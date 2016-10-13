using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;

namespace SocialMentorApp
{
	public class MentorAppDatabase
	{
		static object locker = new object ();

		SQLiteConnection database;

		string DatabasePath {
			get { 
				var sqliteFilename = "SocialMentorAppSQLite.db3";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				var path = Path.Combine(documentsPath, sqliteFilename);
				#else
				// WinPhone
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
				#endif
				#endif
				return path;
			}
		}

		public MentorAppDatabase()
		{
			database = new SQLiteConnection (DatabasePath);
			database.CreateTable<Formulari> ();
			database.CreateTable<Mentor> ();
			database.CreateTable<Mentorat> ();
			database.CreateTable<Usuari> ();
			database.CreateTable<RelacioDeMentoria> ();
		}

		


		public int GuardaRelacio (RelacioDeMentoria relacio) 
		{
			lock (locker) {
				int existeix =  database.ExecuteScalar<int> (("SELECT COUNT(*) FROM [RelacioDeMentoria] WHERE ID = ?"),relacio.ID);
				if (existeix == 0) {
					database.Insert (relacio);
				} 
			}
			return relacio.ID;
		}

		public Boolean TeMentorats () {
			lock (locker) {
				int hasMentorats =  database.ExecuteScalar<int> ("SELECT COUNT(*) FROM [Mentorat]");
				if (hasMentorats > 0)
					return true;
				return false;
			}
		}

		public Boolean TeUsuari() {
			lock (locker) {
				int hasUser =  database.ExecuteScalar<int> ("SELECT COUNT(*) FROM [Usuari]");
				if (hasUser > 0)
					return true;
				return false;
			}
		}

		public int GuardaUsuari (Usuari user) 
		{
			lock (locker) {
				int existeix =  database.ExecuteScalar<int> (("SELECT COUNT(*) FROM [Usuari] WHERE username = ?"),user.Username);
				if (existeix > 0) {
					return database.Update(user);
				} else {
					return database.Insert(user);
				}
			}
		}

		public int EliminaUsuari () {
			lock (locker) {
				return database.Table<Usuari> ().Delete (m => m.ID > -1);
			}
		}

		public int EliminaMentorats () {
			lock (locker) {
				return database.Table<Mentorat> ().Delete (m => m.ID > -1);
			}
		}

		public int GuardaMentorat (Mentorat mentorat) 
		{
			lock (locker) {
				int existeix =  database.ExecuteScalar<int> (("SELECT COUNT(*) FROM [Mentorat] WHERE ID = ?"),mentorat.ID);
				if (existeix > 0) {
					return database.Update(mentorat);
				} else {
					return database.Insert(mentorat);
				}
			}
		}

		public Usuari GetUsuari ()
		{
			lock (locker) {

				return database.Table<Usuari> ().FirstOrDefault ();
				
			}
		}

		public IEnumerable<Mentorat> GetMentorats ()
		{
			lock (locker) {
				return (from i in database.Table<Mentorat>() select i).ToList();
			}
		}

		public Mentorat getMentorat(int IdMentorat)
		{
			lock (locker) {
				return database.ExecuteScalar<Mentorat> ("SELECT * FROM [Mentorat] WHERE [ID] = ?",IdMentorat);
			}
		}

		public Formulari getFormulari(int idFormulari)
		{
			lock (locker) {
				return database.ExecuteScalar<Formulari> ("SELECT * FROM [Formulari] WHERE [ID] = ?",idFormulari);
			}
		}

		public Boolean TeFormulariSenseEnviar () {
			lock (locker) {
				int hasFormularis =  database.ExecuteScalar<int> ("SELECT COUNT(*) FROM [Formulari] WHERE [Enviat] = false");
				if (hasFormularis > 0)
					return true;
				return false;
			}
		}



		public Formulari getFormNoEnviatByIDMentorat(int id) {
			lock (locker) {
				Formulari f = database.Get<Formulari> (id);
				return f;
			}
		}

		public int GuardaFormulari(Formulari form) 
		{
			lock (locker) {
				if (form.ID != 0) {
					database.Update(form);
					return form.ID;
				} else {
					return database.Insert(form);
				}
			}
		}

		public IEnumerable<Formulari> GetAllFormularis ()
		{
			lock (locker) {
				return database.Query<Formulari>("SELECT * FROM [Formulari] WHERE [Enviat] = true");
			}
		}

		public RelacioDeMentoria GetRelacioByIDMentorat(int idMentorat)
		{
			lock (locker) {
				return database.Table<RelacioDeMentoria>().FirstOrDefault(r => r.IDMentorat == idMentorat);
			}
		}
	}
}

