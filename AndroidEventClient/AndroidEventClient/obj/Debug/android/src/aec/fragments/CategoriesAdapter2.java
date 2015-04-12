package aec.fragments;


public class CategoriesAdapter2
	extends aec.service.DataAdapter_3
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AEC.Fragments.CategoriesAdapter2, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CategoriesAdapter2.class, __md_methods);
	}


	public CategoriesAdapter2 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CategoriesAdapter2.class)
			mono.android.TypeManager.Activate ("AEC.Fragments.CategoriesAdapter2, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
