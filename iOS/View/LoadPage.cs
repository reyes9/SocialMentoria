using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;

namespace SocialMentorApp.iOS
{
    public class LoadPage : ContentPage 
    {
        MentorAppDatabase db;
        bool error;
        JsonValue json;

        JsonValue mentorsJson = null;
        JsonValue parellesJson = null;
        JsonValue mentoratsJson = null;
        JsonValue tokenInfo = null;

        public LoadPage()
        {
            db = App.AppDatabase;
            Usuari user = db.GetUsuari();
            ActivityIndicator loadingIndicator;
                error = false;
                var layout = new StackLayout { Padding = 10, VerticalOptions = LayoutOptions.CenterAndExpand };
                var label = new Label {
                    Text = "Connectant...",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    TextColor = Color.Black,
                    XAlign = TextAlignment.Center, // Center the text in the blue box.
                    YAlign = TextAlignment.Center, // Center the text in the blue box.
                };
                string url = "https://mentoria.fib.upc.edu:8443/oauth2/token";
                loadingIndicator = new ActivityIndicator();
                loadingIndicator.BindingContext = this;
                loadingIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsEnabled = true;
                layout.Children.Add(loadingIndicator);
                layout.Children.Add(label);
                Content = layout;
                var client = new HttpClient();
                //*****************************************************************************************

                Task.Run(() => this.getJson(url, user.Username, user.Password, client)).Wait();

                if (error) {
                    loadingIndicator.IsRunning = false;
                    loadingIndicator.IsEnabled = false;
                    Navigation.PushAsync(new MentoratListPage());
                } else {
                    String mentorId = seleccionaMentor(user.Username, user.Password, mentorsJson);
                    guardaResultats(mentorId, parellesJson, mentoratsJson);
                    new MentoratListPage();
                this.OnDisappearing();
                }
                new LoginPage();
        
    }

//		async void carregaDades(string url, string username, string password) {
//			url = url+"?username="+username+"&password="+password;
//			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
//			request.ContentType = "application/json";
//			request.Method = "GET";
//			// Send the request to the server and wait for the response:
//			try {
//				using (WebResponse response = await request.GetResponseAsync ())
//				{
//					// Get a stream representation of the HTTP web response:
//					using (Stream stream = response.GetResponseStream ())
//					{
//						// Use this stream to build a JSON document object:
//						json = await Task.Run (() => JsonObject.Load (stream));
//					}
//				}
//			}catch(Exception e) {
//				error = true;
//			}
//		}


		private async Task<JsonValue> getJson (string url,string username, string password, HttpClient client)
		{ 
            /*url = url+"?username="+username+"&password="+password;                                 **funcion vieja**
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
						json = await Task.Run (() => JsonObject.Load (stream));
						// Return the JSON document:
						return null;
					}
				}
			}catch(Exception e) {
				error = true;
			}
			return null;*/

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            postData.Add(new KeyValuePair<string, string>("client_id", "default_id"));
            postData.Add(new KeyValuePair<string, string>("username", username));
            postData.Add(new KeyValuePair<string, string>("password", password));

            HttpContent content = new FormUrlEncodedContent(postData);

            try
            {
                var response = client.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                tokenInfo = await Task.Run(() => JsonObject.Load(stream));
                //return jsonDoc;
                if (tokenInfo != null)
                {
                    String access_token = tokenInfo["access_token"];
                    String token_type = tokenInfo["token_type"];

                    Console.WriteLine(access_token);
                    Console.WriteLine(token_type);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);

                    mentorsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentors_api", client);
                    parellesJson = await getJson("https://mentoria.fib.upc.edu:8443/api/parelles_api", client);
                    mentoratsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentorats_api", client);
                }
                return null;
            }
            

            catch (Exception e)
            {
                error = true;
                Console.WriteLine(e.ToString());
            }



            return null;

        }



        private String seleccionaMentor(String user, String pass, JsonValue json)
        {
            foreach (JsonValue mentor in json)
            {
                String mentor_user = mentor ["e-mail"];
                //String mentor_user = mentor["nid"];
                if (mentor_user == user)
                {
                    return mentor["nid"];
                    //return mentor;
                }
            }
            return null;
        }




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

        /*  private void guardaResultats()
          {
              db.EliminaMentorats ();
              JsonArray relacions = (JsonArray) json["Relacions"];
              foreach (JsonValue relacio in relacions) {
                  int idRelacio = relacio ["id"];
                  JsonValue mentorat = relacio ["Mentorat"];
                  int idMentorat = mentorat["id"];
                  String nom = mentorat["nom"];
                  String cognom1 = mentorat["cognom1"];
                  String cognom2 = mentorat["cognom2"];
                  Mentorat m = new Mentorat (idMentorat, nom, cognom1, cognom2);
                  RelacioDeMentoria r = new RelacioDeMentoria (idRelacio, idMentorat);
                  db.GuardaMentorat (m);
                  db.GuardaRelacio (r);
              }

          }*/


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


    }
}




