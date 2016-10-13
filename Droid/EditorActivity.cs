
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace SocialMentorApp.Droid
{
	[Activity (Label = "EditorActivity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class EditorActivity : Activity
	{
		String  text;
		EditText textEdit;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Editor);
			var saveButton = FindViewById<Button> (Resource.Id.Submit);
			var eraseButton = FindViewById<Button> (Resource.Id.Erase);
			textEdit = FindViewById<EditText> (Resource.Id.textContent);
			var textTitle = FindViewById<TextView> (Resource.Id.textTitle);
			string who = Intent.GetStringExtra ("tipus") ?? "";
			if (who.Equals ("activitat")) {
				ActionBar.SetTitle (Resource.String.activitat_lloc);
				textTitle.SetText(Resource.String.titol_activitat);
			} else {
				ActionBar.SetTitle (Resource.String.resum_incidencies);
				textTitle.SetText(Resource.String.titol_resum_incidencies);
			}

			ActionBar.SetDisplayHomeAsUpEnabled (true);
			if (Intent.HasExtra("text")) textEdit.Text = Intent.GetStringExtra ("text") ?? "";
			saveButton.Click += delegate {
				Intent myIntent = new Intent (this, typeof(FormActivity));
				text = textEdit.Text;
				myIntent.PutExtra ("text", text);
				SetResult (Result.Ok, myIntent);
				Finish();
			};
			eraseButton.Click += delegate {
				eraseAll();
			};

		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Intent myIntent = new Intent (this, typeof(FormActivity));
				text = textEdit.Text;
				myIntent.PutExtra ("text", text);
				SetResult (Result.Ok, myIntent);
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		public override void OnBackPressed ()
		{
			Intent myIntent = new Intent (this, typeof(FormActivity));
			myIntent.PutExtra ("text", text);
			SetResult (Result.Ok, myIntent);
			Finish();
			base.OnBackPressed ();
		}

		private void eraseAll() {
			var alert = new AlertDialog.Builder(this);
			alert.SetTitle(GetString(Resource.String.alerta));
			alert.SetMessage (GetString(Resource.String.esborrar_tot_missatge));
			alert.SetPositiveButton(GetString(Resource.String.si), (sender, e) =>
				{
					textEdit.Text = "";
					return;
				});
			alert.SetNegativeButton(GetString(Resource.String.no), (sender, e) =>
				{
					return;
				});
			alert.Show();
		}
	}
}

