
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Android.Content.PM;
using System.Text.RegularExpressions;

namespace SocialMentorApp.Droid
{
	[Activity (Label = "FormActivity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class FormActivity : Activity
	{
	    DateTime _date;
		const int DATE_DIALOG = 0;
		const int TIME_DIALOG = 1;
		const int VALORACIO = 2;
		Formulari formulari;
		RadioButton radioIncidenciesSi;
		RadioButton radioIncidenciesNo;
		CardView cvIncidencies;
		MentorAppDatabase db;
		bool connectionError;
		string errorMessage = "";
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Form);
			int idMentorat = Intent.GetIntExtra ("IdMentorat",-1);
			if (idMentorat == -1)
				Finish ();
			formulari = new Formulari (idMentorat);
			db = MainApplication.AppDatabase;
			db.GuardaFormulari (formulari);
			connectionError = false;
			int idRelacio = db.GetRelacioByIDMentorat (idMentorat).ID;
			var dateButton = FindViewById<CardView> (Resource.Id.cvDia);
			var hourButton = FindViewById<CardView> (Resource.Id.cvHora);
			var triaValoracio = FindViewById<CardView> (Resource.Id.cvValoracio);
			var escriuActivitat = FindViewById<CardView> (Resource.Id.cvActivitat);
			var escriuIncidencies = FindViewById<CardView> (Resource.Id.cvResumIncidencies);
			var triaTipusActivitat = FindViewById<CardView> (Resource.Id.cvTipusActivitat);
			var triaTemesTractats = FindViewById<CardView> (Resource.Id.cvTemesTractats);
			var date = FindViewById<TextView> (Resource.Id.pickDate);
			var hour = FindViewById<TextView> (Resource.Id.pickTime);
			//var save = FindViewById<Button> (Resource.Id.Save);
			var submit = FindViewById<Button> (Resource.Id.Submit);
			ActionBar.SetIcon (null);
			ActionBar.SetTitle (Resource.String.informe);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			dateButton.Click += delegate {
				ShowDialog (DATE_DIALOG);
			};
			hourButton.Click += delegate{
				ShowDialog(TIME_DIALOG);
			};
			triaValoracio.Click += delegate {
				ShowDialog(VALORACIO);
			};
			escriuActivitat.Click += delegate {
				var myIntent = new Intent (this, typeof(EditorActivity));
				myIntent.PutExtra("tipus","activitat");
				if (formulari.TextActivitat != null) myIntent.PutExtra("text",formulari.TextActivitat);
				StartActivityForResult (myIntent, 0);
			};
			escriuIncidencies.Click += delegate {
				var myIntent = new Intent (this, typeof(EditorActivity));
				myIntent.PutExtra("tipus","incidencies");
				if (formulari.Incidencies != null) myIntent.PutExtra("text",formulari.Incidencies);
				StartActivityForResult (myIntent, 1);
			};
			triaTipusActivitat.Click += delegate {
				var myIntent = new Intent (this, typeof(MultiCheckActivity));
				myIntent.PutExtra("tipus","activitat");
				if (formulari.TipusActivitats != null) myIntent.PutExtra("opcions",formulari.TipusActivitats);
				//myIntent.PutExtra("list",);
				//if (formulari.TextActivitat != null) myIntent.PutExtra("text",formulari.TextActivitat);
				StartActivityForResult (myIntent, 2);
			};
			triaTemesTractats.Click += delegate {
				var myIntent = new Intent (this, typeof(MultiCheckActivity));
				myIntent.PutExtra("tipus","temes");
				if (formulari.TemesTractats != null) myIntent.PutExtra("opcions",formulari.TemesTractats);
				//if (formulari.TextActivitat != null) myIntent.PutExtra("text",formulari.TextActivitat);
				StartActivityForResult (myIntent, 3);
			};
			_date = DateTime.Today;
			date.Text = _date.ToString ("d");
			hour.Text = _date.ToString ("t", CultureInfo.CreateSpecificCulture("es-ES"));
			radioIncidenciesSi = FindViewById<RadioButton> (Resource.Id.radioSi);
			radioIncidenciesNo = FindViewById<RadioButton> (Resource.Id.radioNo);
			cvIncidencies = FindViewById<CardView> (Resource.Id.cvResumIncidencies);
			radioIncidenciesSi.Click += RadioButtonClick;
			radioIncidenciesNo.Click += RadioButtonClick;
			submit.Click += async (sender, e) =>  {
				//el text d'activitat i el d'incidències ja s'han posat abans al formulari
				formulari.Dia = date.Text;
				formulari.Hora = hour.Text;
				string valoracio = FindViewById<TextView> (Resource.Id.valoracioText).Text.ToString();
				if (!hiHaErrors(formulari,valoracio)) {
					formulari.Valoracio = getNumeroValoracio(valoracio);
					Usuari u = db.GetUsuari();


					ProgressDialog progress = new ProgressDialog(this);
					progress.Indeterminate = true;
					progress.SetProgressStyle(ProgressDialogStyle.Spinner);
					progress.SetMessage(GetString(Resource.String.enviant));
					progress.SetCancelable(false);
					progress.Show();
					await PostItem(u,formulari,idRelacio);
					progress.Dismiss();
					if (connectionError) {
						Toast.MakeText (this, GetString(Resource.String.error_enviament), ToastLength.Long).Show ();
						connectionError = false;
					}
					else {
						Toast.MakeText (this, GetString(Resource.String.formulari_enviat), ToastLength.Long).Show ();
						Finish();
					}
				};
			};

		}

		private bool hiHaErrors(Formulari f, string val) {
			string missatge = "";
			bool correcte = true;
			if (f.TextActivitat == null) {
				correcte = false;
				missatge = GetString(Resource.String.falta_activitat);
			}
			else if (f.TipusActivitats == null) {
				correcte = false;
				missatge = GetString(Resource.String.falta_tipus);
			}
			else if (f.TemesTractats == null) {
				correcte = false;
				missatge = GetString(Resource.String.falta_temes);
			}
			else if (val.Equals(Resource.String.tria)) {
				correcte = false;
				missatge = GetString(Resource.String.falta_valoracio);
			}
			if (!correcte) {
				Toast.MakeText (this, missatge, ToastLength.Long).Show ();
				return true;
			}

			return false;
			
		}

		private int getNumeroValoracio(string val) {
			if (val.Contains ("5"))
				return 5;
			else if (val.Contains ("4"))
				return 4;
			else if (val.Contains ("3"))
				return 3;
			else if (val.Contains ("2"))
				return 2;
			else
				return 1;
		}
			

		private void RadioButtonClick (object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Equals (radioIncidenciesSi))
				cvIncidencies.Visibility = ViewStates.Visible;
			else
				cvIncidencies.Visibility = ViewStates.Gone;
		}

		protected override Dialog OnCreateDialog (int id)
		{
			if (id == DATE_DIALOG)
				return new DatePickerDialog (this, HandleDateSet, _date.Year, _date.Month - 1, _date.Day);
			else if (id == TIME_DIALOG)
				return new TimePickerDialog (this, TimePickerCallback, _date.Hour, _date.Minute, false);
			else if (id == VALORACIO) {
				AlertDialog.Builder alert = new AlertDialog.Builder (this);
				alert.SetTitle (Resource.String.valoracio);
				alert.SetItems (Resource.Array.valoracio, ValoracioClicked);
				return alert.Create ();
			}
			else
				return null;
		}

		private void HandleDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			_date = e.Date;
			var button = FindViewById<TextView> (Resource.Id.pickDate);
			String date = _date.ToString ("d");
			button.Text = date;
			formulari.Dia = date;

		}

		private void TimePickerCallback (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			var button = FindViewById<TextView> (Resource.Id.pickTime);
			String hora = string.Format ("{0}:{1}", e.HourOfDay, e.Minute.ToString ().PadLeft (2, '0'));
			button.Text = hora;
			formulari.Hora = hora;
		}

		private void ValoracioClicked (object sender, DialogClickEventArgs e)
		{
			var items = Resources.GetStringArray (Resource.Array.valoracio);
			var valoracio = FindViewById<TextView> (Resource.Id.valoracioText);
			valoracio.Text = items [(int)e.Which];
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				String text = data.GetStringExtra ("text");
				String opcions = data.GetStringExtra ("opcionsCheck");
				if (requestCode == 0)
					formulari.TextActivitat = text;
				else if (requestCode == 1)
					formulari.Incidencies = text;
				else if (requestCode == 2)
					formulari.TipusActivitats = opcions;
				else
					formulari.TemesTractats = opcions;

				Toast.MakeText (this, Resource.String.informacio_guardada, ToastLength.Short).Show ();

			}
		}

		private String parseData(List<KeyValuePair<string, string>> normal, List<KeyValuePair<string, string>> text, 
			List<KeyValuePair<string, string>> check, KeyValuePair<string, string> entity, KeyValuePair<string, KeyValuePair<string,string>> datetime) {

			String jsonString = "{";
			Console.WriteLine (jsonString);
			Boolean first = true;
			foreach (KeyValuePair<string, string> element in normal) {
				if (element.Value != null) {
					if (!first) {
						jsonString += ',';
					}
					jsonString += '"' + element.Key.ToString () + '"' + ':' + '"' + element.Value.ToString () + '"';

					first = false;
					Console.WriteLine (jsonString);
				}
			}

			foreach (KeyValuePair<string, string> element in text) {
				if (element.Value != null) {
					jsonString += ',';
					jsonString += '"' + element.Key.ToString () + "\":{\"und\":[{\"value\":\"" + element.Value.ToString () +
					"\",\"format\": null," + "\"safe_value\":\"" + element.Value.ToString () + "\"}]}";
				}
			}
			foreach (KeyValuePair<string, string> element in check) {
				if (element.Value != null) {
					Regex regex = new Regex (@", ");
					var values = regex.Split(element.Value);
					jsonString += ',';
					jsonString += '"' + element.Key.ToString () + "\":{\"und\":";
					if (values.Length > 1) {
						jsonString +="[";
						first = true;
						foreach (string value in values) {
							if (!first) {
								jsonString += ',';
							}
							jsonString += '"' + value + '"';
							first = false;
						}
						jsonString += "]";
					}
					else {
						jsonString += "{\"value\":\"" + values[0] + "\"}";
					}
					jsonString +="}";
				}
			}

			if (entity.Value != null) {
				jsonString += ',';
				jsonString += '"' + entity.Key.ToString () + '"' + ':' + "{\"und\":[\"" + entity.Value.ToString () + "\"]}";
			}

			if (datetime.Value.Value != null && datetime.Value.Key != null) {
				jsonString += ',';
				KeyValuePair<string, string> value = datetime.Value;
				jsonString += '"' + datetime.Key.ToString () + '"' + ':' + "{\"und\":[{\"value\":{\"date\":\"" + value.Key.ToString () + "\", \"time\":\"" + value.Value.ToString () + "\"}}]}";
			}

			jsonString += '}';

			return jsonString;
		}

		private async Task<JsonValue> getToken(String url, String user, String pass, HttpClient client) {

			var postData = new List<KeyValuePair<string, string>>();
			postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
			postData.Add(new KeyValuePair<string, string>("client_id", "default_id"));
			postData.Add(new KeyValuePair<string, string>("username", user));
			postData.Add(new KeyValuePair<string, string>("password", pass));

			HttpContent content = new FormUrlEncodedContent(postData); 

			try {
				var response = client.PostAsync(url, content).Result;
				response.EnsureSuccessStatusCode();
				Stream stream = await response.Content.ReadAsStreamAsync();
				JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
				return jsonDoc;
			} catch(Exception e){
				connectionError = true;
				Console.WriteLine (e.ToString ());
			}

			return null;
		}

		private async Task PostItem(Usuari u, Formulari f, int idRelacio) {
			var client = new HttpClient();
			string url = "https://mentoria.fib.upc.edu:8443/api/nodes_api";

			var postAsIs = new List<KeyValuePair<string, string>>();
			var postCheckboxes = new List<KeyValuePair<string, string>>();
			var postTextBox = new List<KeyValuePair<string, string>>();
			var postSpecial = new List<KeyValuePair<string, string>>();

			Console.WriteLine (f.TemesTractats);
			postAsIs.Add(new KeyValuePair<string, string>("title", "Trobada"+idRelacio.ToString()+f.Dia.ToString()+f.Hora.ToString()));
			postAsIs.Add(new KeyValuePair<string, string>("type", "trobada"));
			var datetime = new KeyValuePair<string, KeyValuePair<string, string>>("field_data_trobada", new KeyValuePair<string, string>(f.Dia, f.Hora));
			postTextBox.Add(new KeyValuePair<string, string>("field_activitat", f.TextActivitat));
			postCheckboxes.Add(new KeyValuePair<string, string>("field_tipus_activitat", f.TipusActivitats));
			postCheckboxes.Add(new KeyValuePair<string, string>("field_temes_tractats", f.TemesTractats));
			postTextBox.Add(new KeyValuePair<string, string>("field_incidencies", f.Incidencies));
			postCheckboxes.Add(new KeyValuePair<string, string>("field_valoracio", f.Valoracio.ToString()));
			var entity = new KeyValuePair<string, string>("field_parella", idRelacio.ToString());

			String jsonString = parseData (postAsIs, postTextBox, postCheckboxes, entity, datetime);
			Console.WriteLine (jsonString);

			HttpContent content = new StringContent (jsonString, Encoding.UTF8, "application/json");//new FormUrlEncodedContent(postData); 
			db = MainApplication.AppDatabase;
			Usuari user = db.GetUsuari ();
			try {
				JsonValue tokenInfo = await getToken("https://mentoria.fib.upc.edu:8443/oauth2/token", user.Username, user.Password, client);

				if (tokenInfo != null) {
					String access_token = tokenInfo["access_token"];
					String token_type = tokenInfo["token_type"];

					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);
				}

				await client.PostAsync(url, content).ContinueWith(
					(postTask) =>
					{
						postTask.Result.EnsureSuccessStatusCode();
					});
			}
			catch(Exception e){
				connectionError = true;
				Console.WriteLine (e.ToString ());
				errorMessage = e.ToString ();
			}
					
		}

	}
}

