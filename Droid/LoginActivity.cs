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
using Android.Util;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Android.Content.PM;

namespace SocialMentorApp.Droid
{
    [Activity(Label = "LoginActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        Button submitButton;
        Boolean error = false;
        MentorAppDatabase db;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            db = MainApplication.AppDatabase;
            submitButton = FindViewById<Button>(Resource.Id.Submit);
            EditText username = FindViewById<EditText>(Resource.Id.Username);
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            ActionBar.SetIcon(null);
            ActionBar.SetTitle(Resource.String.login);
            submitButton.Click += async (sender, e) => {

                ProgressDialog progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage(GetString(Resource.String.connectant));
                progress.SetCancelable(false);
                String user = username.Text;
                String pass = password.Text;
                //String passEncrypted = MD5Hash(pass);

                JsonValue mentorsJson = null;
                JsonValue parellesJson = null;
                JsonValue mentoratsJson = null;

                progress.Show();

                var client = new HttpClient();
                JsonValue tokenInfo = await getToken("https://mentoria.fib.upc.edu:8443/oauth2/token", user, pass, client);
                if (tokenInfo != null)
                {
                    String access_token = tokenInfo["access_token"];
                    String token_type = tokenInfo["token_type"];

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);

                    mentorsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentors_api", client);
                    parellesJson = await getJson("https://mentoria.fib.upc.edu:8443/api/parelles_api", client);
                    mentoratsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentorats_api", client);
                }

                progress.Dismiss();
                if (error)
                {
                    error = false;
                    createDialog(1);
                }
                else
                {
                    String mentorId = seleccionaMentor(user, mentorsJson);
                    guardaResultats(mentorId, parellesJson, mentoratsJson);

                    Usuari u = new Usuari(user, pass);
                    db.GuardaUsuari(u);
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                }
            };
        }

        private async Task<JsonValue> getToken(String url, String user, String pass, HttpClient client)
        {

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            postData.Add(new KeyValuePair<string, string>("client_id", "default_id"));
            postData.Add(new KeyValuePair<string, string>("username", user));
            postData.Add(new KeyValuePair<string, string>("password", pass));

            HttpContent content = new FormUrlEncodedContent(postData);

            try
            {
                var response = client.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                return jsonDoc;
            }
            catch (Exception e)
            {
                error = true;
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        private String seleccionaMentor(String user, JsonValue json)
        {
            foreach (JsonValue mentor in json)
            {
                String mentor_user = mentor ["e-mail"];

                user = user.ToLower();
                mentor_user = mentor_user.ToLower();


                //String mentor_user = mentor["nid"];
                if (mentor_user == user)
                {
                    //return mentor["nid"];
                    return mentor["nid"];    //**************************************************************
                }
            }
            return null;
        }

        private JsonValue seleccionaMentorat(String mentoratId, JsonValue json)
        {
            foreach (JsonValue mentorat in json)
            {
                String mentorat_id = mentorat["nid"];
                if (mentorat_id == mentoratId)
                    return mentorat;
            }

            return null;
        }

        private void guardaResultats(String mentorId, JsonValue parellesJson, JsonValue mentoratsJson)
        {

            JsonArray relations = new JsonArray();

            Console.WriteLine(parellesJson.ToString());

            foreach (JsonValue relacio in parellesJson)
            {
                if (mentorId == relacio["mentor"])
                {
                    relations.Add(relacio);
                }
            }

            relations.ToArray();

            db.EliminaMentorats();

            foreach (JsonValue relacio in relations)
            {
                int idRelacio = Int32.Parse(relacio["nid"]);

                JsonValue mentorat = seleccionaMentorat(relacio["mentorat"], mentoratsJson);

                int idMentorat = Int32.Parse(mentorat["nid"]);
                String nom = mentorat["nom"];
                String cognom1 = mentorat["cognom1"];

                String cognom2;
                if (mentorat["cognom2"].JsonType.Equals(JsonType.Array))
                    cognom2 = "";
                else
                {
                    cognom2 = mentorat["cognom2"];
                }

                Mentorat m = new Mentorat(idMentorat, nom, cognom1, cognom2);
                RelacioDeMentoria r = new RelacioDeMentoria(idRelacio, idMentorat);
                db.GuardaMentorat(m);
                db.GuardaRelacio(r);
            }
        }

        /*
		public static string MD5Hash(string text)
		{
			MD5 md5 = new MD5CryptoServiceProvider();

			//compute hash from the bytes of text
			md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

			//get hash result after compute it
			byte[] result = md5.Hash;

			StringBuilder strBuilder = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				//change it into 2 hexadecimal digits
				//for each byte
				strBuilder.Append(result[i].ToString("x2"));
			}

			return strBuilder.ToString();
		}
		*/
        /*
		private async Task<JsonValue> getJson (string url)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";
			// Send the request to the server and wait for the response:
			try {
				using (WebResponse response = await request.GetResponseAsync ())
				{
					// Get a stream representation of the HTTP web response:
					using (Stream stream = response.GetResponseStream ())
					{
						// Use this stream to build a JSON document object:
						JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));

						// Return the JSON document:
						return jsonDoc;
					}

				}
			}catch(Exception e) {
				error = true;
			}
			return null;
		}
		*/

        private async Task<JsonValue> getJson(string url, HttpClient client)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            try
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                return jsonDoc;
            }
            catch (Exception e)
            {
                error = true;
            }
            return null;
        }

        private void createDialog(int tipus)
        {
            String title;
            if (tipus == 0) title = GetString(Resource.String.error_connexio);
            else title = GetString(Resource.String.error_dades);
            var alert = new AlertDialog.Builder(submitButton.Context);
            alert.SetTitle(title);
            alert.SetPositiveButton("Ok", (sender, e) =>
            {
                return;
            });
            alert.Show();
        }

        private void displayAlert(String message)
        {
            var alert = new AlertDialog.Builder(submitButton.Context);
            alert.SetTitle("notification");
            alert.SetMessage(message);
            alert.SetPositiveButton("Ok", (sender, e) => {
                return;
            });
            alert.Show();
        }
    }
}