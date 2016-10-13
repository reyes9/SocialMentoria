using System;

using Xamarin.Forms;
using System.Diagnostics;
using System.IO;
using SocialMentorApp.iOS;

namespace SocialMentorApp
{
	public class App : Xamarin.Forms.Application
	{
		static MentorAppDatabase db;
		public App ()
		{

			db = new MentorAppDatabase ();
			db.EliminaUsuari ();
			if (db.TeUsuari ()) {
				MainPage = new NavigationPage (new LoadPage ()) {
					BarBackgroundColor = Color.FromHex ("#3498DB"),
					BarTextColor = Color.White
				};
			} else {
				MainPage = new NavigationPage(new LoginPage()) {
					BarBackgroundColor = Color.FromHex("#3498DB"),
					BarTextColor = Color.White
				};
			}



			//MainPage = new LoginPage ();
		}

		public static MentorAppDatabase AppDatabase{ get { return db;} }


		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

