package aec.fragments;


public class MyEventsAdapter_SuggestionsFilter
	extends android.widget.Filter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_performFiltering:(Ljava/lang/CharSequence;)Landroid/widget/Filter$FilterResults;:GetPerformFiltering_Ljava_lang_CharSequence_Handler\n" +
			"n_publishResults:(Ljava/lang/CharSequence;Landroid/widget/Filter$FilterResults;)V:GetPublishResults_Ljava_lang_CharSequence_Landroid_widget_Filter_FilterResults_Handler\n" +
			"";
		mono.android.Runtime.register ("AEC.Fragments.MyEventsAdapter/SuggestionsFilter, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyEventsAdapter_SuggestionsFilter.class, __md_methods);
	}


	public MyEventsAdapter_SuggestionsFilter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyEventsAdapter_SuggestionsFilter.class)
			mono.android.TypeManager.Activate ("AEC.Fragments.MyEventsAdapter/SuggestionsFilter, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MyEventsAdapter_SuggestionsFilter (aec.fragments.MyEventsAdapter p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyEventsAdapter_SuggestionsFilter.class)
			mono.android.TypeManager.Activate ("AEC.Fragments.MyEventsAdapter/SuggestionsFilter, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "AEC.Fragments.MyEventsAdapter, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public android.widget.Filter.FilterResults performFiltering (java.lang.CharSequence p0)
	{
		return n_performFiltering (p0);
	}

	private native android.widget.Filter.FilterResults n_performFiltering (java.lang.CharSequence p0);


	public void publishResults (java.lang.CharSequence p0, android.widget.Filter.FilterResults p1)
	{
		n_publishResults (p0, p1);
	}

	private native void n_publishResults (java.lang.CharSequence p0, android.widget.Filter.FilterResults p1);

	java.util.ArrayList refList;
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
