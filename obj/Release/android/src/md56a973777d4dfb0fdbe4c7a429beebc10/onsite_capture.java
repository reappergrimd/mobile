package md56a973777d4dfb0fdbe4c7a429beebc10;


public class onsite_capture
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PandC_Mobile.sla.onsite_capture, PandC Mobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", onsite_capture.class, __md_methods);
	}


	public onsite_capture ()
	{
		super ();
		if (getClass () == onsite_capture.class)
			mono.android.TypeManager.Activate ("PandC_Mobile.sla.onsite_capture, PandC Mobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
