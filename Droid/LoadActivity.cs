using System;
using System.Linq;

using Android.App;
using Android.OS;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Android.Content.PM;
using Android.Content;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;

namespace SocialMentorApp.Droid
{
    [Activity(Label = "SocialMentorApp", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoadActivity : Activity
    {
        MentorAppDatabase db;
        Boolean error;
        protected override async void OnCreate(Bundle bundle)
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            base.OnCreate(bundle);
            db = MainApplication.AppDatabase;
            //db.EliminaUsuari ();
            error = false;

            if (db.TeUsuari())
            {
                Usuari user = db.GetUsuari();
                ProgressDialog progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage(GetString(Resource.String.connectant));
                progress.SetCancelable(false);

                JsonValue mentorsJson = null;
                JsonValue parellesJson = null;
                JsonValue mentoratsJson = null;

                progress.Show();
                var client = new HttpClient();
                Console.WriteLine(user.Username);
                Console.WriteLine(user.Password);
                JsonValue tokenInfo = await getToken("https://mentoria.fib.upc.edu:8443/oauth2/token", user.Username, user.Password, client);
                if (tokenInfo != null)
                {
                    String access_token = tokenInfo["access_token"];
                    String token_type = tokenInfo["token_type"];

                    Console.WriteLine(access_token);
                    Console.WriteLine(token_type);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);


                    Console.WriteLine("MentoresOK");   //********************************
                    Console.WriteLine("Mentors11OK");


                    mentorsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentors_api", client);
                    parellesJson = await getJson("https://mentoria.fib.upc.edu:8443/api/parelles_api", client);
                    mentoratsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentorats_api", client);
                }
                progress.Dismiss();
                if (error)
                    createDialog();
                else
                {
                    
                    Console.WriteLine("USer+ Pass: " + user.Username +" " + user.Password);

                    String mentorId = seleccionaMentor(user.Username, user.Password, mentorsJson);
                    Console.WriteLine("mentorID: " + mentorId);

                    guardaResultats(mentorId, parellesJson, mentoratsJson);
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                }
            }
            else
            {
                StartActivity(typeof(LoginActivity));
                this.Finish();
            }
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

        /*
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
				error = true;
				Console.WriteLine (e.ToString ());
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

        private String seleccionaMentor(String user, String pass, JsonValue json)
        {

            Console.WriteLine("Entra");
            foreach (JsonValue mentor in json)
            {
                String mentor_user = mentor ["e-mail"];

                user = user.ToLower();
                mentor_user = mentor_user.ToLower();
                //String mentor_user = mentor["nid"];

                Console.WriteLine("mentor_user: " + mentor_user);
                Console.WriteLine("User: " + user);
                Console.WriteLine("MentorJson: " + mentor["nid"]);

                if (mentor_user == user)
                {
                    //return mentor["nid"];
                    return mentor["nid"];  //**************************************************************//
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

        private async Task<JsonValue> getJson(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            // Send the request to the server and wait for the response:
            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Use this stream to build a JSON document object:
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                        // Return the JSON document:
                        return jsonDoc;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                error = true;
            }
            return null;
        }

        private void createDialog()
        {
            String title;
            title = GetString(Resource.String.error_connexio);
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetPositiveButton("Ok", (sender, e) =>
            {
                return;
            });
            alert.Show();
        }

        private void guardaResultats(String mentorId, JsonValue parellesJson, JsonValue mentoratsJson)
        {

            JsonArray relations = new JsonArray();

            foreach (JsonValue relacio in parellesJson)
            {


                Console.WriteLine(mentorId);  //**************************************
                Console.WriteLine("Mentores");
                Console.WriteLine("Mentors11");


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
                Console.WriteLine("Ok");
            }
        }
    }
}