using System;
using SocialMentorApp;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Mentorapp
{
	public class TipusActivitat
	{
		List<Tuple<String,Boolean>> itemsFinal;
		Button save;

		public TipusActivitat (string title, bool itemChecked)
		{
			this.text = title;
			this.itemChecked = itemChecked;
			Usuari usuari = new Usuari ();
		}

		public string text { private set; get; }

		public bool itemChecked { private set; get; }
	}
}

