using UnityEngine;
using System;
using System.Collections;
using Handler = System.Action<System.Object, System.Object>;

//extension class for eventcenter.cs, lets u call eventcenter methods without actually referencing the instance
public static class EventExtensions
{
	public static void AddObserver (this object obj, Handler handler, string notificationName){
		EventCenter.instance.AddObserver(handler, notificationName);
	}	
	public static void AddObserver (this object obj, Handler handler, string notificationName, object sender){
		EventCenter.instance.AddObserver(handler, notificationName, sender);
	}
	
	public static void RemoveObserver (this object obj, Handler handler, string notificationName){
		EventCenter.instance.RemoveObserver(handler, notificationName);
	}	
	public static void RemoveObserver (this object obj, Handler handler, string notificationName, System.Object sender){
		EventCenter.instance.RemoveObserver(handler, notificationName, sender);
	}

	public static void PostEvent (this object obj, string notificationName){
		EventCenter.instance.PostEvent(notificationName, obj);
	}	
	public static void PostEvent (this object obj, string notificationName, object e){
		EventCenter.instance.PostEvent(notificationName, obj, e);
	}
}