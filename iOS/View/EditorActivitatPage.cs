using System;
using Xamarin.Forms;

namespace SocialMentorApp
{
	public class EditorActivitatPage : ContentPage
	{
		Editor editor;
		String tipusText;
		public EditorActivitatPage (String titol, String tipus, String text)
		{
			if (tipus.Equals ("activitat"))
				this.Title = "Activitat i lloc";
			else
				this.Title = "Incidències";
			tipusText = tipus;
			Label header = new Label
			{
				FontFamily = "HelveticaNeue-Medium",
				Text = titol,
				FontSize = 24,
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.Center
			};

			editor = new Editor
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			editor.Text = text;	
			Button guardar = new Button {
				Text = "Guardar",
				Font = Font.SystemFontOfSize(NamedSize.Large)
			};

			guardar.Clicked += async(sender, e) => {
				if (editor.Text != null) MessagingCenter.Send(editor.Text,tipus);
				await Navigation.PopAsync();
			};
			// Build the page.
			Content = new StackLayout
			{
				Children = 
				{
					header,
					editor,
					guardar
				},
				Padding = new Thickness(0,0,0,50)
				};
		}

		protected override void OnDisappearing() {
			if (editor.Text != null) MessagingCenter.Send(editor.Text,tipusText);
			base.OnDisappearing();
		}
			


	}
}

