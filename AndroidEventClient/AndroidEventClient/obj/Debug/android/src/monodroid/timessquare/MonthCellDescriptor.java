package monodroid.timessquare;


public class MonthCellDescriptor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_toString:()Ljava/lang/String;:GetToStringHandler\n" +
			"";
		mono.android.Runtime.register ("MonoDroid.TimesSquare.MonthCellDescriptor, MonoDroid.TimesSquare, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", MonthCellDescriptor.class, __md_methods);
	}


	public MonthCellDescriptor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MonthCellDescriptor.class)
			mono.android.TypeManager.Activate ("MonoDroid.TimesSquare.MonthCellDescriptor, MonoDroid.TimesSquare, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String toString ()
	{
		return n_toString ();
	}

	private native java.lang.String n_toString ();

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
