
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
	public class CheckItemAdapter : RecyclerView.Adapter
	{
		private List<CheckItemHolder> viewHolderList;
		private List<Tuple<String,Boolean>> items;

		private Context mContext;

		// Load the adapter with the data set (photo album) at construction time:
		public CheckItemAdapter (List<Tuple<String,Boolean>> items, Context context)
		{
			this.items = items;
			mContext = context;
			viewHolderList = new List<CheckItemHolder>();
		}

		public override RecyclerView.ViewHolder 
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.CheckItem, parent, false);
			CheckItemHolder vh = new CheckItemHolder (itemView);
			viewHolderList.Add (vh);
			return vh;
		}

		public override void 
		OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			CheckItemHolder vh = holder as CheckItemHolder;
			//viewHolderList [position] = vh;
			// Set the ImageView and TextView in this ViewHolder's CardView 
			// from this position in the photo album:
			vh.Check.SetOnCheckedChangeListener (new CheckedChangeListener(mContext, vh));
			vh.Title.Text = items.ElementAt(position).Item1;
			vh.Check.Checked = items.ElementAt(position).Item2;
		}

		public override int ItemCount
		{
			get { return items.Count; }
		}

		public String itemsChecked() {
			String items = "";
			for (int i = 0; i < viewHolderList.Count; ++i) {
				if (viewHolderList[i].Check.Checked) {
					if (items.Equals (""))
						items += viewHolderList[i].Title.Text;
					else
						items += ", " + viewHolderList[i].Title.Text;
				}
			}
			return items;
		}


}

	public class CheckItemHolder : RecyclerView.ViewHolder
	{
		public TextView Title { get; private set; }
		public CheckBox Check { get; private set; }

		// Get references to the views defined in the CardView layout.
		public CheckItemHolder (View itemView) 
			: base (itemView)
		{
			// Locate and cache view references:
			Title = itemView.FindViewById<TextView> (Resource.Id.titleAct);
			Check = itemView.FindViewById<CheckBox> (Resource.Id.checkBox);
		}
	}

	public class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
	{
		private Context context;
		private CheckItemHolder checkItemHolder;

		public CheckedChangeListener(Context context1, CheckItemHolder ch)
		{
			this.context = context1;
			checkItemHolder = ch;
		}

		public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			checkItemHolder.Check.Checked = isChecked;
		}
	}


}

