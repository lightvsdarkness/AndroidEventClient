package aec.service;


public class DataServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AEC.Service.DataServiceBinder, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DataServiceBinder.class, __md_methods);
	}


	public DataServiceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataServiceBinder.class)
			mono.android.TypeManager.Activate ("AEC.Service.DataServiceBinder, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DataServiceBinder (aec.service.DataService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataServiceBinder.class)
			mono.android.TypeManager.Activate ("AEC.Service.DataServiceBinder, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "AEC.Service.DataService, AndroidEventClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
