package md5a8b970a38e409b0c9da77f21d37c29d3;


public class CheckedChangeListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.CompoundButton.OnCheckedChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCheckedChanged:(Landroid/widget/CompoundButton;Z)V:GetOnCheckedChanged_Landroid_widget_CompoundButton_ZHandler:Android.Widget.CompoundButton/IOnCheckedChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SocialMentorApp.Droid.CheckedChangeListener, SocialMentorApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CheckedChangeListener.class, __md_methods);
	}


	public CheckedChangeListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CheckedChangeListener.class)
			mono.android.TypeManager.Activate ("SocialMentorApp.Droid.CheckedChangeListener, SocialMentorApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CheckedChangeListener (android.content.Context p0, md5a8b970a38e409b0c9da77f21d37c29d3.CheckItemHolder p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CheckedChangeListener.class)
			mono.android.TypeManager.Activate ("SocialMentorApp.Droid.CheckedChangeListener, SocialMentorApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:SocialMentorApp.Droid.CheckItemHolder, SocialMentorApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1 });
	}


	public void onCheckedChanged (android.widget.CompoundButton p0, boolean p1)
	{
		n_onCheckedChanged (p0, p1);
	}

	private native void n_onCheckedChanged (android.widget.CompoundButton p0, boolean p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
