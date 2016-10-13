using System;
using SocialMentorApp;
using Xamarin.Forms.Platform.iOS;
using SocialMentorApp.iOS;
using Xamarin.Forms;

[assembly: ExportCell(typeof(MentoratItem), typeof(MentoratItemRenderer))]

namespace SocialMentorApp.iOS
{
	public class MentoratItemRenderer : ViewCellRenderer
	{

		public override MonoTouch.UIKit.UITableViewCell GetCell (Cell item, MonoTouch.UIKit.UITableView tv)
		{
			var cell = base.GetCell (item, tv);
			cell.SelectionStyle = MonoTouch.UIKit.UITableViewCellSelectionStyle.None;
			return cell;
		}
	}
}

