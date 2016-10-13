using System;
using Android.App;
using Android.Runtime;

namespace SocialMentorApp.Droid
{
	[Application (Label = "SocialMentorApp", Theme = "@style/AppTheme", Icon="@drawable/icon")]
	public class MainApplication : Android.App.Application
	{
		private static MentorAppDatabase db;
		public MainApplication (IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			db = new MentorAppDatabase ();
		}

		public static MentorAppDatabase AppDatabase{ get { return db;} }

	}
}

