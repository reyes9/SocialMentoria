using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace SocialMentorApp
{
	public class MentoratListPage : ContentPage
	{
		List<Mentorat> mentorats;
		MentorAppDatabase db;
		bool login;
		public MentoratListPage ()
		{
			NavigationPage.SetHasBackButton (this, false);
			db = App.AppDatabase;
			this.Title = "Mentorats/des";
			mentorats = new List<Mentorat> ();
			mentorats = db.GetMentorats ().ToList();
			ListView mentoratList = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(MentoratCell)),
				ItemsSource = mentorats,
				SeparatorColor = Color.FromHex("#ddd"),
				SeparatorVisibility = SeparatorVisibility.Default

			};

			mentoratList.ItemTapped += async(object sender, ItemTappedEventArgs e) => {
				((ListView)sender).SelectedItem = null;
				var item = ((Mentorat)e.Item);
				await Navigation.PushAsync(new FormulariPage(item.ID));
			};

			Padding = new Thickness (5,20,5,0);
			Content = mentoratList;

		}
	}

	public class MentoratCell : ViewCell {

		public MentoratCell() {
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();
			Label titol = new Label ();
			titol.FontAttributes = FontAttributes.Bold;
			//Label check = new Label ();
			titol.SetBinding (Label.TextProperty, "NomComplet");
			//check.SetBinding (Label.TextProperty, "checkText");
			this.StyleId = "disclosure-button";
			horizontalLayout.Orientation = StackOrientation.Horizontal;
			titol.HorizontalOptions = LayoutOptions.StartAndExpand;
			//check.HorizontalOptions = LayoutOptions.EndAndExpand;
			titol.TextColor = Color.FromHex ("#000000");
			//check.TextColor = Color.FromHex ("#000000");
			horizontalLayout.Children.Add (titol);
			//horizontalLayout.Children.Add (check);
			cellWrapper.Padding = new Thickness (5, 20);
			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}
	}
}

