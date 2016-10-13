
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
using Android.Support.V7.Widget;
using Android.Content.PM;

namespace SocialMentorApp.Droid
{
	[Activity (Label = "MultiCheckActivity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class MultiCheckActivity : Activity
	{
		List<Tuple<String,Boolean>> itemsFinal;
		Button save;
		EditText input;
		CheckItemAdapter adapter;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MultiCheck);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			string who = Intent.GetStringExtra ("tipus") ?? "";
			string opcions = Intent.GetStringExtra ("opcions") ?? "";
			save = FindViewById<Button> (Resource.Id.SaveCheck);
			if (who.Equals ("activitat")) {
				ActionBar.SetTitle (Resource.String.tipus_activitat);
				GetTipusActivitats(opcions);
			} else {
				ActionBar.SetTitle (Resource.String.temes_tractats);
				GetTemesTractats (opcions);
			}
			RecyclerView recyclerView = FindViewById<RecyclerView> (Resource.Id.RecyclerViewMultiCheck);
			input = FindViewById<EditText> (Resource.Id.introText);
			LinearLayoutManager llm = new LinearLayoutManager(this);
			recyclerView.SetLayoutManager (llm);
			adapter = new CheckItemAdapter (itemsFinal,this);
			recyclerView.SetAdapter (adapter);
			save.Click += delegate {
				Intent myIntent = new Intent (this, typeof(FormActivity));
				string opcionsCheck = adapter.itemsChecked();
				if (opcionsCheck.Equals("")) opcionsCheck = input.Text;
				else if (!input.Text.Equals("")) opcionsCheck += ", "+input.Text;
				myIntent.PutExtra ("opcionsCheck", opcionsCheck);
				SetResult (Result.Ok, myIntent);
				Finish();
			};
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Intent myIntent = new Intent (this, typeof(FormActivity));
				string opcionsCheck = adapter.itemsChecked();
				if (opcionsCheck.Equals("")) opcionsCheck = input.Text;
				else if (!input.Text.Equals("")) opcionsCheck += ", "+input.Text;
				myIntent.PutExtra ("opcionsCheck", opcionsCheck);
				SetResult (Result.Ok, myIntent);
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		private void GetTipusActivitats(String opcions) {
			itemsFinal = new List<Tuple<String,Boolean>> ();
			if (opcions.Equals("")) {
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.coneixement), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.cultural), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.esportiva), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.laboral), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.escolar), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.conversa), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.autonomia), false));
			} else {
				List<String> tipusActivitats = converteixALlista (opcions);
				foreach (String act in tipusActivitats) {
					itemsFinal.Add (new Tuple<string, bool> (act, true));
				}
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.coneixement))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.coneixement), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.cultural))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.cultural), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.esportiva))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.esportiva), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.laboral))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.laboral), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.escolar))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.escolar), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.conversa))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.conversa), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.autonomia))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.autonomia), false));
			}

		}

		private void GetTemesTractats(String opcions) {
			itemsFinal = new List<Tuple<String,Boolean>> ();
			if (opcions.Equals("")) {
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.linguistic), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.formatiu), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.laboral), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.emocional), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.economic), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.habitatge), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.legal), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.lleure), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.salut), false));
				itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.relacional), false));
			} else {
				List<String> temesTractats = converteixALlista (opcions);
				foreach (String tema in temesTractats) {
					itemsFinal.Add (new Tuple<string, bool> (tema, true));
				}
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.linguistic))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.linguistic), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.formatiu))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.formatiu), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.laboral))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.laboral), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.emocional))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.emocional), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.economic))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.economic), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.habitatge))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.habitatge), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.legal))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.legal), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.lleure))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.lleure), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.salut))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.salut), false));
				if (!itemsFinal.Any(i=>i.Item1.Equals(GetString(Resource.String.relacional))))
					itemsFinal.Add (new Tuple<string, bool> (GetString(Resource.String.relacional), false));
			}

		}

		private List<String> converteixALlista(String s) {
			string[] llista = s.Split (new string[] { ", " },StringSplitOptions.RemoveEmptyEntries);

			return llista.ToList ();
		}


	}
}

