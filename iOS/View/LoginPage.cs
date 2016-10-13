using System;
using Xamarin.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;

namespace SocialMentorApp
{
    public class LoginPage : ContentPage
    {
        bool error;
        MentorAppDatabase db;
        ActivityIndicator loadingIndicator;
        public LoginPage()
        {
            db = App.AppDatabase;
            this.Title = "Inciar sessió";
            loadingIndicator = new ActivityIndicator();
            loadingIndicator.BindingContext = this;
            loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            error = false;
            var layout = new StackLayout { Padding = 10, VerticalOptions = LayoutOptions.CenterAndExpand };

            var loginImage = new Image { Aspect = Aspect.AspectFit };
            loginImage.MinimumWidthRequest = 200;
            loginImage.MinimumHeightRequest = 200;
            loginImage.Source = ImageSource.FromFile("Resources/loginimage.png");
            var label = new Label
            {
                Text = "SocialMentorApp",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.Black,
                XAlign = TextAlignment.Center, // Center the text in the blue box.
                YAlign = TextAlignment.Center, // Center the text in the blue box.
            };

            layout.Children.Add(loginImage);

            var username = new Entry { Placeholder = "Username", VerticalOptions = LayoutOptions.Center };
            //username.SetBinding(Entry.TextProperty, LoginViewModel.UsernamePropertyName);
            layout.Children.Add(username);

            var password = new Entry { Placeholder = "Password", IsPassword = true };
            //password.SetBinding(Entry.TextProperty, LoginViewModel.PasswordPropertyName);
            layout.Children.Add(password);

            //****************************************************************
            JsonValue mentorsJson = null;
            JsonValue parellesJson = null;
            JsonValue mentoratsJson = null;
            //****************************************************************


            var button = new Button { Text = "Entrar", Font = Font.SystemFontOfSize(NamedSize.Large) };
            //button.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);

            layout.Children.Add(loadingIndicator);

            layout.Children.Add(button);

            button.Clicked += async (sender, e) =>
            {
                if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text))
                {
                    DisplayAlert("Error", "El nom d'usuari i contrasenya són obligatoris", "Ok");
                }
                else
                {
                    String user = username.Text;
                    String passEncrypted = MD5Hash(password.Text);
                    string url = "https://mentoria.fib.upc.edu:8443/oauth2/token";
                    loadingIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    loadingIndicator.IsRunning = true;
                    loadingIndicator.IsEnabled = true;
                    this.IsBusy = true;

                    var client = new HttpClient();
                    JsonValue json = await getJson(url, user, passEncrypted, client);
                    this.IsBusy = false;
                    if (error)
                    {
                        error = false;
                        DisplayAlert("Error", "Alguna de les dades proporcionades és incorrecte", "Ok");
                    }
                    else
                    {
                        /*guardaResultats (json);
                        Usuari u = new Usuari (user, passEncrypted);
                        db.GuardaUsuari (u);
                        await this.Navigation.PushAsync(new MentoratListPage());*/

                        String access_token = json["access_token"];
                        String token_type = json["token_type"];

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);

                        mentorsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentors_api", client);
                        parellesJson = await getJson("https://mentoria.fib.upc.edu:8443/api/parelles_api", client);
                        mentoratsJson = await getJson("https://mentoria.fib.upc.edu:8443/api/mentorats_api", client);

                        //*********************************************

                        String mentorId = seleccionaMentor(user, mentoratsJson);
                        guardaResultats(mentorId, parellesJson, mentoratsJson);

                        Usuari u = new Usuari(user, password.Text);
                        db.GuardaUsuari(u);
                        await this.Navigation.PushAsync(new MentoratListPage()); 
                        //new MentoratListPage();  provar


                        //*********************************************                        
                    }

                }
            };

            Content = new ScrollView { Content = layout };
        }


        /*
        
        GuardaResultats antic

        private void guardaResultats(JsonValue json)
        {
            db.EliminaMentorats();
            JsonArray relacions = (JsonArray)json["Relacions"];
            foreach (JsonValue relacio in relacions)
            {
                int idRelacio = relacio["id"];
                JsonValue mentorat = relacio["Mentorat"];
                int idMentorat = mentorat["id"];
                String nom = mentorat["nom"];
                String cognom1 = mentorat["cognom1"];
                String cognom2 = mentorat["cognom2"];
                Mentorat m = new Mentorat(idMentorat, nom, cognom1, cognom2);
                RelacioDeMentoria r = new RelacioDeMentoria(idRelacio, idMentorat);
                db.GuardaMentorat(m);
                db.GuardaRelacio(r);
            }
        }

        */


        //GuardaResultats prova         

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


        private String seleccionaMentor(String user, JsonValue json)
        {
            foreach (JsonValue mentor in json)
            {
                String mentor_user = mentor["e-mail"];
                //String mentor_user = mentor["nid"];
                if (mentor_user == user)
                {
                    //return mentor["nid"];
                    return mentor["nid"];    //**************************************************************
                }
            }
            return null;
        }


        private async Task<JsonValue> getJson(string url, string username, string password, HttpClient client)
        {
            /*// Create an HTTP web request using the URL:
			url = url+"?username="+username+"&password="+password;
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
		}*/

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
    }

}