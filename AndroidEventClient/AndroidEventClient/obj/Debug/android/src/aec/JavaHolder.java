package aec;


public class JavaHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AEC.JavaHolder, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", JavaHolder.class, __md_methods);
	}


	public JavaHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == JavaHolder.class)
			mono.android.TypeManager.Activate ("AEC.JavaHolder, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
