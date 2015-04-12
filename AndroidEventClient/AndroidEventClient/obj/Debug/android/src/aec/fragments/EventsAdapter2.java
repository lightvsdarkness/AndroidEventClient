package aec.fragments;


public class EventsAdapter2
	extends aec.service.DataAdapter_3
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AEC.Fragments.EventsAdapter2, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EventsAdapter2.class, __md_methods);
	}


	public EventsAdapter2 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EventsAdapter2.class)
			mono.android.TypeManager.Activate ("AEC.Fragments.EventsAdapter2, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
