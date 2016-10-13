
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace SocialMentorApp.Droid
{
	public class MentoratItemAdapter : RecyclerView.Adapter
	{
		// Event handler for item clicks:
		public event EventHandler<int> ItemClick;

		private List<Mentorat> mentorats;

		private Context mContext;

		public MentoratItemAdapter (List<Mentorat> mentorats, Context context)
		{
			this.mentorats = mentorats;
			mContext = context;
		}

		public override RecyclerView.ViewHolder 
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.MentoratItem, parent, false);

			// Create a ViewHolder to find and hold these view references, and 
			// register OnClick with the view holder:
			MentoratItemHolder vh = new MentoratItemHolder (itemView, OnClick); 
			return vh;
		}

		public override void 
		OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			MentoratItemHolder vh = holder as MentoratItemHolder;
			vh.Name.Text = mentorats.ElementAt(position).NomComplet;
		}

		// Return the number of photos available in the photo album:
		public override int ItemCount
		{
			get { return mentorats.Count; }
		}

		// Raise an event when the item-click takes place:
		void OnClick (int position)
		{
			if (ItemClick != null)
				ItemClick (this, position);
			var myIntent = new Intent (mContext, typeof(FormActivity));
			myIntent.PutExtra("IdMentorat",mentorats.ElementAt(position).ID);
			mContext.StartActivity (myIntent);
		}
	}

	public class MentoratItemHolder : RecyclerView.ViewHolder
	{
		public TextView Name { get; private set; }

		// Get references to the views defined in the CardView layout.
		public MentoratItemHolder (View itemView, Action<int> listener) 
			: base (itemView)
		{
			// Locate and cache view references:
			Name = itemView.FindViewById<TextView> (Resource.Id.person_name);

			// Detect user clicks on the item view and report which item
			// was clicked (by position) to the listener:
			itemView.Click += (sender, e) => listener (base.Position);
		}
	}
}

