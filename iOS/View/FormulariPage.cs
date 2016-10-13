using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SocialMentorApp
{

	class FormulariPage : ContentPage
	{
		ActivityIndicator loadingIndicator;
		bool connectionError;
		MentorAppDatabase db;
		public FormulariPage(int idMentorat)
		{
			Formulari formulari = new Formulari ();
			connectionError = false;
			db = App.AppDatabase;
			int idRelacio = db.GetRelacioByIDMentorat (idMentorat).ID;
			this.Title = "Escriure informe";
			Label diaLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Dia",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center

			};

			DatePicker datePicker = new DatePicker () {
				Format = "dd/MM/yyyy",
				HorizontalOptions = LayoutOptions.End,

			};

			Label horaLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Hora",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			TimePicker timePicker = new TimePicker () {
				Time = new TimeSpan(8,0,0)
			};

			Label activitatLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Activitat i lloc",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Label escriuAct = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Gray,
				Text = "Escriu",
				VerticalOptions = LayoutOptions.Center
			};

			var tgr = new TapGestureRecognizer ();
			tgr.Tapped += async(s, e) => {
				await Navigation.PushAsync(new EditorActivitatPage("Descriu la trobada","activitat", formulari.TextActivitat));
				MessagingCenter.Subscribe<String>(this,"activitat", (activitats) => 
					{
						formulari.TextActivitat = activitats;
					});

			};
			escriuAct.GestureRecognizers.Add(tgr);

			//Aqui va el botó per anar a esciure l'activitat a un editor

			Label tipusActLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Tipus d'activitat",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Label triaTipusActLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Gray,
				Text = "Tria",
				VerticalOptions = LayoutOptions.Center
			};

			Label temesTractatsLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Temes tractats",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Label triaTemesTractatsLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Gray,
				Text = "Tria",
				VerticalOptions = LayoutOptions.Center
			};

			var tgr1 = new TapGestureRecognizer ();
			tgr1.Tapped += (s, e) => {
				Navigation.PushAsync(new TriaLlistaPage("tipusActivitat",formulari.TipusActivitats));
				MessagingCenter.Subscribe<String>(this,"tipusActivitat", (tipusAct) => 
					{
						formulari.TipusActivitats = tipusAct;
					});
			};
			triaTipusActLabel.GestureRecognizers.Add(tgr1);
			var tgr2 = new TapGestureRecognizer ();
			tgr2.Tapped += (s, e) => {
				Navigation.PushAsync(new TriaLlistaPage("temes",formulari.TemesTractats));
				MessagingCenter.Subscribe<String>(this,"temes", (temes) => 
					{
						formulari.TemesTractats = temes;
					});
			};

			triaTemesTractatsLabel.GestureRecognizers.Add (tgr2);

			// En comptes d'un picker, una page a part que deixi triar amb una llista, fem servir la mateixa pàgina per els altres pickers
			// excepte el de valoració

			Label incidenciesLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Incidències",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Switch incidenciesSwitch = new Switch {
				IsToggled = false
			};

			Label resumIncidenciesLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Resum incidències",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center,
				IsEnabled = false
			};

			Label escriuInc = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Gray,
				Text = "Escriu",
				VerticalOptions = LayoutOptions.Center,
				IsEnabled = false
			};

			var tgrIncidencies = new TapGestureRecognizer ();
			tgrIncidencies.Tapped += (s, e) => {
				if (escriuInc.IsEnabled) Navigation.PushAsync(new EditorActivitatPage("Resum d'incidències","incidencies", formulari.Incidencies));
			};
			escriuInc.GestureRecognizers.Add(tgrIncidencies);

			incidenciesSwitch.Toggled += (s, e) => {
				if (e.Value.Equals(true)) Navigation.PushAsync(new EditorActivitatPage("Resum d'incidències","incidencies", formulari.Incidencies));
				MessagingCenter.Subscribe<String>(this,"incidencies", (incidencies) => 
				{
						formulari.Incidencies = incidencies;
				});
			};
            


			Label valoracioLabel = new Label () {
				FontFamily = "HelveticaNeue-Medium",
				FontSize = 16,
				TextColor = Color.Black,
				Text = "Valoració",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Picker valoracioPicker = new Picker () {
				AnchorX = 0,
				Title = "Tria",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.EndAndExpand

			};

			valoracioPicker.Items.Add ("5 - Molt bé");
			valoracioPicker.Items.Add ("4 - Bé");
			valoracioPicker.Items.Add ("3 - Normal");
			valoracioPicker.Items.Add ("2 - Malament");
			valoracioPicker.Items.Add ("1 - Molt malament");

			Button envia = new Button {
				Text = "Envia",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Font = Font.SystemFontOfSize(NamedSize.Large)

			};
			envia.Text = "Envia";
			envia.Clicked += async(s, e) => {
				DateTime dia = datePicker.Date;
				formulari.Dia = dia.ToString("dd/MM/yyyy");
				formulari.Hora = timePicker.Time.ToString();
				if (!hiHaErrors(formulari,valoracioPicker.SelectedIndex)){
					string valoracio = valoracioPicker.Items[valoracioPicker.SelectedIndex];
					formulari.Valoracio = getNumeroValoracio(valoracio);
					Usuari u = new Usuari();
					loadingIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;
					loadingIndicator.IsRunning = true;
					loadingIndicator.IsEnabled = true;
					this.IsBusy = true;
					await PostItem(u,formulari,idRelacio);
					this.IsBusy = false;
					if (connectionError) {
						await DisplayAlert("Error", "Error en l'enviament, sisplau torna a intentar-ho", "Ok");
					}
					else {
						await DisplayAlert("Enviat", "Informe enviat correctament", "Ok");
						await Navigation.PopAsync();
					}
				}

			};
			TableView table = new TableView (new TableRoot {
				new TableSection () {
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { diaLabel, datePicker }
						}
					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { horaLabel, timePicker }
						}
					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { activitatLabel, escriuAct }
						}
					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { tipusActLabel, triaTipusActLabel }
						}
					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { temesTractatsLabel, triaTemesTractatsLabel }
						}
					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { incidenciesLabel, incidenciesSwitch }
						}
					},
					//					new ViewCell {
					//						View = new StackLayout {
					//							Spacing = 0,
					//							Padding = new Thickness (10, 5, 10, 5),
					//							Orientation = StackOrientation.Horizontal,
					//							Children = { resumIncidenciesLabel, escriuInc }
					//						},
					//						IsEnabled = false
					//					},
					new ViewCell {
						StyleId = "none",
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { valoracioLabel, valoracioPicker }
						}
					}

				}

			}) { Intent = TableIntent.Data };
			loadingIndicator = new ActivityIndicator();
			loadingIndicator.BindingContext = this;
			loadingIndicator.SetBinding (ActivityIndicator.IsVisibleProperty, "IsBusy");
			AbsoluteLayout.SetLayoutFlags(table, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(table, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			AbsoluteLayout.SetLayoutFlags(loadingIndicator, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(loadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			Content = new StackLayout {
				Children = {loadingIndicator,table,envia},
				Padding = new Thickness(0,0,0,50)
			};


		}



		private bool hiHaErrors(Formulari f, int val) {
			string missatge = "";
			bool correcte = true;
			if (f.TextActivitat == null) {
				correcte = false;
				missatge = "Falta escriure a Activitat i lloc";
			}
			else if (f.TipusActivitats == null) {
				correcte = false;
				missatge = "Falta definir el tipus d'activitat";
			}
			else if (f.TemesTractats == null) {
				correcte = false;
				missatge = "Falta definir els temes tractats";
			}
			else if (val < 0) {
				correcte = false;
				missatge = "Has de definir la valoració de la trobada";
			}
			if (!correcte) {
				DisplayAlert ("Error", missatge, "Ok");
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


        //*********************************************************************  nuevas funciones

        private String parseData(List<KeyValuePair<string, string>> normal, List<KeyValuePair<string, string>> text,
    List<KeyValuePair<string, string>> check, KeyValuePair<string, string> entity, KeyValuePair<string, KeyValuePair<string, string>> datetime)
        {

            String jsonString = "{";
            Console.WriteLine(jsonString);
            Boolean first = true;
            foreach (KeyValuePair<string, string> element in normal)
            {
                if (element.Value != null)
                {
                    if (!first)
                    {
                        jsonString += ',';
                    }
                    jsonString += '"' + element.Key.ToString() + '"' + ':' + '"' + element.Value.ToString() + '"';

                    first = false;
                    Console.WriteLine(jsonString);
                }
            }

            foreach (KeyValuePair<string, string> element in text)
            {
                if (element.Value != null)
                {
                    jsonString += ',';
                    jsonString += '"' + element.Key.ToString() + "\":{\"und\":[{\"value\":\"" + element.Value.ToString() +
                    "\",\"format\": null," + "\"safe_value\":\"" + element.Value.ToString() + "\"}]}";
                }
            }
            foreach (KeyValuePair<string, string> element in check)
            {
                if (element.Value != null)
                {
                    Regex regex = new Regex(@", ");
                    var values = regex.Split(element.Value);
                    jsonString += ',';
                    jsonString += '"' + element.Key.ToString() + "\":{\"und\":";
                    if (values.Length > 1)
                    {
                        jsonString += "[";
                        first = true;
                        foreach (string value in values)
                        {
                            if (!first)
                            {
                                jsonString += ',';
                            }
                            jsonString += '"' + value + '"';
                            first = false;
                        }
                        jsonString += "]";
                    }
                    else
                    {
                        jsonString += "{\"value\":\"" + values[0] + "\"}";
                    }
                    jsonString += "}";
                }
            }

            if (entity.Value != null)
            {
                jsonString += ',';
                jsonString += '"' + entity.Key.ToString() + '"' + ':' + "{\"und\":[\"" + entity.Value.ToString() + "\"]}";
            }

            if (datetime.Value.Value != null && datetime.Value.Key != null)
            {
                jsonString += ',';
                KeyValuePair<string, string> value = datetime.Value;
                jsonString += '"' + datetime.Key.ToString() + '"' + ':' + "{\"und\":[{\"value\":{\"date\":\"" + value.Key.ToString() + "\", \"time\":\"" + value.Value.ToString() + "\"}}]}";
            }

            jsonString += '}';

            return jsonString;
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
                connectionError = true;
                Console.WriteLine(e.ToString());
            }

            return null;
        }


        //*********************************************************************   fin nuevas



        private async Task PostItem(Usuari u, Formulari f, int idRelacio) {
			var client = new HttpClient();
            //string url = "http://moralesjoan.com/api/MentoriaSocial/TrobadaApi";
            string url = "https://mentoria.fib.upc.edu:8443/api/nodes_api";  //nova url
            /*
            var postData = new List<KeyValuePair<string, string>>();
			postData.Add(new KeyValuePair<string, string>("dataTrobada", f.Dia));
			postData.Add(new KeyValuePair<string, string>("hora", f.Hora));
			postData.Add(new KeyValuePair<string, string>("activitat", f.TextActivitat));
			postData.Add(new KeyValuePair<string, string>("tipusActivitat", f.TipusActivitats));
			postData.Add(new KeyValuePair<string, string>("temesTractats", f.TemesTractats));
			postData.Add(new KeyValuePair<string, string>("incidencies", f.Incidencies));
			postData.Add(new KeyValuePair<string, string>("valoracio", f.Valoracio.ToString()));
			postData.Add(new KeyValuePair<string, string>("idRelacio", idRelacio.ToString()));
//			postData.Add(new KeyValuePair<string, string>("username", u.Username));
//			postData.Add(new KeyValuePair<string, string>("password", u.Password));
			postData.Add(new KeyValuePair<string, string>("username", "joan807"));
			postData.Add(new KeyValuePair<string, string>("password", "b9a9eecc68d0f37ecda138f565be4d8c"));          */

            //nuevos datos ******************************************

            var postAsIs = new List<KeyValuePair<string, string>>();
            var postCheckboxes = new List<KeyValuePair<string, string>>();
            var postTextBox = new List<KeyValuePair<string, string>>();
            var postSpecial = new List<KeyValuePair<string, string>>();

            Console.WriteLine(f.TemesTractats);
            postAsIs.Add(new KeyValuePair<string, string>("title", "Trobada" + idRelacio.ToString() + f.Dia.ToString() + f.Hora.ToString()));
            postAsIs.Add(new KeyValuePair<string, string>("type", "trobada"));
            var datetime = new KeyValuePair<string, KeyValuePair<string, string>>("field_data_trobada", new KeyValuePair<string, string>(f.Dia, f.Hora));
            postTextBox.Add(new KeyValuePair<string, string>("field_activitat", f.TextActivitat));
            postCheckboxes.Add(new KeyValuePair<string, string>("field_tipus_activitat", f.TipusActivitats));
            postCheckboxes.Add(new KeyValuePair<string, string>("field_temes_tractats", f.TemesTractats));
            postTextBox.Add(new KeyValuePair<string, string>("field_incidencies", f.Incidencies));
            postCheckboxes.Add(new KeyValuePair<string, string>("field_valoracio", f.Valoracio.ToString()));
            var entity = new KeyValuePair<string, string>("field_parella", idRelacio.ToString());

            String jsonString = parseData(postAsIs, postTextBox, postCheckboxes, entity, datetime);

            //*******************************************************

            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");//new FormUrlEncodedContent(postData); 
            db = App.AppDatabase;
            Usuari user = db.GetUsuari();
            try
            {
                JsonValue tokenInfo = await getToken("https://mentoria.fib.upc.edu:8443/oauth2/token", user.Username, user.Password, client);

                if (tokenInfo != null)
                {
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
            catch (Exception e)
            {
                connectionError = true;

            }



            /* HttpContent content = new FormUrlEncodedContent(postData); 

             try {
                 await client.PostAsync(url, content).ContinueWith(
                     (postTask) =>
                     {
                         postTask.Result.EnsureSuccessStatusCode();
                     });
             }
                         catch(Exception e){
                             connectionError = true;

                         }*/

        }
	}
}

