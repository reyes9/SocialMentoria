using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Mentorapp;
using System.Linq;
using XLabs.Forms.Controls;

namespace SocialMentorApp
{
	public class TriaLlistaPage : ContentPage
	{
		List<ItemLlista> itemsFinal;
		Button save;
		string tipusLlista;
		Entry entry;

		public TriaLlistaPage (string tipus, string opcions)
		{
			tipusLlista = tipus;
			if (tipus.Equals ("tipusActivitat")) {
				this.Title = "Tipus d'activitat";
				GetTipusActivitats (opcions);
			} else {
				this.Title = "Temes tractats";
				GetTemesTractats (opcions);
			}

			ListView itemsList = new ListView {
				HasUnevenRows = true,
				ItemsSource = itemsFinal,
				ItemTemplate = new DataTemplate (typeof(ItemCell)),
				SeparatorColor = Color.FromHex("#ddd"),
				SeparatorVisibility = SeparatorVisibility.Default


			};

			itemsList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
				var item = ((ItemLlista)e.Item);
				if (item.check) item.check = false;
				else item.check = true;
				itemsList.ItemsSource = null;
				itemsList.ItemsSource = itemsFinal;

			};

			entry = new Entry () {
				Placeholder = "Altres: escriu separant amb comes"
			};

			Button save = new Button () {
				Text = "Guardar",
				Font = Font.SystemFontOfSize(NamedSize.Large) 
			};
			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

			// Build the page.
			this.Content = new StackLayout {
				Children = {
					itemsList,
					entry,
					save
				}
			};

			save.Clicked += async delegate {
				string opcionsCheck = itemsChecked();
				if (opcionsCheck.Equals("")) opcionsCheck = entry.Text;
				else opcionsCheck += ", "+entry.Text;
				MessagingCenter.Send(opcionsCheck,tipus);
				await Navigation.PopAsync();
			};
		}

		private void GetTipusActivitats(String opcions) {
			itemsFinal = new List<ItemLlista> ();
			if (opcions == null || opcions.Equals("")) {
				itemsFinal.Add (new ItemLlista("Coneixement de l'entorn", false));
				itemsFinal.Add (new ItemLlista("Activitat cultural", false));
				itemsFinal.Add (new ItemLlista("Activitat esportiva", false));
				itemsFinal.Add (new ItemLlista("Suport/Orientació inserció laboral", false));
				itemsFinal.Add (new ItemLlista("Suport escolar", false));
				itemsFinal.Add (new ItemLlista("Conversa", false));
				itemsFinal.Add (new ItemLlista("Suport autonomia(llar,salut,...)", false));
			} else {
				List<String> tipusActivitats = converteixALlista (opcions);
				foreach (String act in tipusActivitats) {
					itemsFinal.Add (new ItemLlista(act, true));
				}
				if (!itemsFinal.Any(i=>i.titol.Equals("Coneixement de l'entorn")))
					itemsFinal.Add (new ItemLlista("Coneixement de l'entorn", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Activitat cultural")))
					itemsFinal.Add (new ItemLlista("Activitat cultural", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Activitat esportiva")))
					itemsFinal.Add (new ItemLlista("Activitat esportiva", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Suport/Orientació inserció laboral")))
					itemsFinal.Add (new ItemLlista("Suport/Orientació inserció laboral", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Suport escolar")))
					itemsFinal.Add (new ItemLlista("Suport escolar", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Conversa")))
					itemsFinal.Add (new ItemLlista("Conversa", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Suport autonomia(llar,salut,...)")))
					itemsFinal.Add (new ItemLlista("Suport autonomia(llar,salut,...)", false));
			}

		}

		private void GetTemesTractats(String opcions) {
			itemsFinal = new List<ItemLlista> ();
			if (opcions == null || opcions.Equals("")) {
				itemsFinal.Add (new ItemLlista("Lingüístic", false));
				itemsFinal.Add (new ItemLlista("Formatiu", false));
				itemsFinal.Add (new ItemLlista("Laboral", false));
				itemsFinal.Add (new ItemLlista("Emocional", false));
				itemsFinal.Add (new ItemLlista("Econòmic", false));
				itemsFinal.Add (new ItemLlista("Habitatge", false));
				itemsFinal.Add (new ItemLlista("Legal", false));
				itemsFinal.Add (new ItemLlista("Lleure", false));
				itemsFinal.Add (new ItemLlista("Salut", false));
				itemsFinal.Add (new ItemLlista("Relacional", false));
			} else {
				List<String> temesTractats = converteixALlista (opcions);
				foreach (String tema in temesTractats) {
					itemsFinal.Add (new ItemLlista(tema, true));
				}
				if (!itemsFinal.Any(i=>i.titol.Equals("Lingüístic")))
					itemsFinal.Add (new ItemLlista("Lingüístic", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Formatiu")))
					itemsFinal.Add (new ItemLlista("Formatiu", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Laboral")))
					itemsFinal.Add (new ItemLlista("Laboral", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Emocional")))
					itemsFinal.Add (new ItemLlista("Emocional", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Econòmic")))
					itemsFinal.Add (new ItemLlista("Econòmic", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Habitatge")))
					itemsFinal.Add (new ItemLlista("Habitatge", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Legal")))
					itemsFinal.Add (new ItemLlista("Legal", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Lleure")))
					itemsFinal.Add (new ItemLlista("Lleure", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Salut")))
					itemsFinal.Add (new ItemLlista("Salut", false));
				if (!itemsFinal.Any(i=>i.titol.Equals("Relacional")))
					itemsFinal.Add (new ItemLlista("Relacional", false));
			}

		}

		private List<String> converteixALlista(String s) {
			string[] llista = s.Split (new string[] { ", " },StringSplitOptions.RemoveEmptyEntries);

			return llista.ToList ();
		}

		public String itemsChecked() {
			String items = "";
			for (int i = 0; i < itemsFinal.Count; ++i) {
				if (itemsFinal[i].check) {
					if (items.Equals (""))
						items += itemsFinal[i].titol;
					else
						items += ", " + itemsFinal[i].titol;
				}
			}
			return items;
		}

		protected override void OnDisappearing() {
			string opcionsCheck = itemsChecked();
			if (opcionsCheck.Equals("")) opcionsCheck = entry.Text;
			else if (entry.Text != null) opcionsCheck += ", "+entry.Text;
			if (opcionsCheck != null) MessagingCenter.Send(opcionsCheck,tipusLlista);
			base.OnDisappearing();
		}
	}

	public class ItemCell : ViewCell {

		public ItemCell() {
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();
			Label titol = new Label ();
			titol.FontAttributes = FontAttributes.Bold;
			Label check = new Label ();
			titol.SetBinding (Label.TextProperty, "titol");
			check.SetBinding (Label.TextProperty, "checkText");
			this.StyleId = "none";
			horizontalLayout.Orientation = StackOrientation.Horizontal;
			titol.HorizontalOptions = LayoutOptions.StartAndExpand;
			check.HorizontalOptions = LayoutOptions.EndAndExpand;
			titol.TextColor = Color.FromHex ("#000000");
			check.TextColor = Color.FromHex ("#000000");
			horizontalLayout.Children.Add (titol);
			horizontalLayout.Children.Add (check);
			cellWrapper.Padding = new Thickness (0, 10);
			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}
	}

	public class ItemLlista {

		public ItemLlista(string titol, bool check) {
			this.titol = titol;
			this.check = check;
		}
		public string titol { get; set; }
		public string checkText { get { 
				if (check)
					return "Sí";
				else
					return "No";
			}set {
				if (value.Equals ("Sí"))
					check = true;
				else
					check = false;
			}
		}
		public bool check {get; set;}
	}
}

