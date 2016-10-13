using System;
using Xamarin.Forms;

namespace SocialMentorApp
{
	// Although this page is actually a ContentPage, it can 
	//  function as a NavigationPage because the HomePage
	//  is launched as an ApplicationPage in App. 
	class NavigationPageDemoPage : ContentPage
	{
		public NavigationPageDemoPage()
		{
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
				Time = new TimeSpan(17,0,0)
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
			tgr.Tapped += (s, e) => {
				Navigation.PushAsync(new EditorActivitatPage("Resum d'activitats i lloc"),Formulari);
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
				Navigation.PushAsync(new TriaLlistaPage());
			};
			triaTipusActLabel.GestureRecognizers.Add(tgr1);

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
				if (escriuInc.IsEnabled) Navigation.PushAsync(new EditorActivitatPage("Resum d'incidències"));
			};
			escriuInc.GestureRecognizers.Add(tgrIncidencies);

			incidenciesSwitch.Toggled += (s, e) => {
				if (e.Value.Equals(true)) Navigation.PushAsync(new EditorActivitatPage("Resum d'incidències"));
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

			Content = new TableView (new TableRoot {
				new TableSection ("Informe de trobada") {
					new ViewCell {
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { diaLabel, datePicker }
						}
					},
					new ViewCell {
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { horaLabel, timePicker }
						}
					},
					new ViewCell {
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { activitatLabel, escriuAct }
						}
					},
					new ViewCell {
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { tipusActLabel, triaTipusActLabel }
						}
					},
					new ViewCell {
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { temesTractatsLabel, triaTemesTractatsLabel }
						}
					},
					new ViewCell {
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
						View = new StackLayout {
							Spacing = 0,
							Padding = new Thickness (10, 5, 10, 5),
							Orientation = StackOrientation.Horizontal,
							Children = { valoracioLabel, valoracioPicker }
						}
					},
					new ViewCell { View = new Button { Text = "Enviar" } }


				},

			}) { Intent = TableIntent.Data };

		}
	}
}

