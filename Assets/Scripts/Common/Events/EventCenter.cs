using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//just an event handler, first param is sender, second param is args/info
using Handler = System.Action<System.Object, System.Object>;

//key of sender and list of handler methods 
using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Action<System.Object, System.Object>>>;

public class EventCenter{

    //holds a sendertable for every event of that name
	private Dictionary<string, SenderTable> _table = new Dictionary<string, SenderTable>();
    //list of handler methods that are currently being invoked, used to avoid changing list when iterating
	private HashSet<List<Handler>> currentlyInvoking = new HashSet<List<Handler>>();

	//singleton stuff
	public readonly static EventCenter instance = new EventCenter();
	private EventCenter() {}

    //shorthand method to add an observer of event
	public void AddObserver (Handler handler, string eventName){
		AddObserver(handler, eventName, null);
	}
	
	public void AddObserver (Handler handler, string eventName, System.Object sender){
		if (handler == null){
			Debug.LogError("Null handler error for event: " + eventName);
			return;
		}
		if (string.IsNullOrEmpty(eventName)){
			Debug.LogError("Unnamed event error, can't observe");
			return;
		}
		//Checks if event is already being observed
		if (!_table.ContainsKey(eventName))
			_table.Add(eventName, new SenderTable());
		
		SenderTable subTable = _table[eventName];
		
		System.Object key = (sender != null) ? sender : this;
		
        //checks if the sendertable already contains handler methods for this sender
		if (!subTable.ContainsKey(key))
			subTable.Add(key, new List<Handler>());
		
        //adds the handler method to subtable. creates new list if the current one is being iterated upon
		List<Handler> list = subTable[key];
		if (!list.Contains(handler)){
			if (currentlyInvoking.Contains(list))
				subTable[key] = list = new List<Handler>(list);
			
			list.Add( handler );
		}
		if(eventName == "BaseAbilityEffect.GetAttackEvent"){
			Debug.Log("event being added by " + sender + " for " + eventName);
			Debug.Log("handlers: " + handler);
		}
	}
	
    //shorthand method to remove an observer of event
	public void RemoveObserver (Handler handler, string eventName){
		RemoveObserver(handler, eventName, null);
	}
	
	public void RemoveObserver (Handler handler, string eventName, System.Object sender){
		if (handler == null){
			Debug.LogError("Null handler error for event: " + eventName);
			return;
		}
		
		if (string.IsNullOrEmpty(eventName)){
			Debug.LogError("Unnamed event, can't stop observing");
			return;
		}
		
		//checks if event is being monitored
		if (!_table.ContainsKey(eventName))
			return;
		
		SenderTable subTable = _table[eventName];
		System.Object key = (sender != null) ? sender : this;
		//checks if sendertable has any handler methods for this sender
		if (!subTable.ContainsKey(key))
			return;
		//removes handler method from subtable if it exists. creates new list if current one is being iterated upon
		List<Handler> list = subTable[key];
		int index = list.IndexOf(handler);
		if (index != -1){
			if (currentlyInvoking.Contains(list))
				subTable[key] = list = new List<Handler>(list);
			list.RemoveAt(index);
		}
	}
	
    //tbh i have no clue what this is rn
	public void Clean (){
		string[] notKeys = new string[_table.Keys.Count];
		_table.Keys.CopyTo(notKeys, 0);
		
		for (int i = notKeys.Length - 1; i >= 0; --i){
			string eventName = notKeys[i];
			SenderTable senderTable = _table[eventName];
			
			object[] senKeys = new object[ senderTable.Keys.Count ];
			senderTable.Keys.CopyTo(senKeys, 0);
			
			for (int j = senKeys.Length - 1; j >= 0; --j){
				object sender = senKeys[j];
				List<Handler> handlers = senderTable[sender];
				if (handlers.Count == 0)
					senderTable.Remove(sender);
			}
			
			if (senderTable.Count == 0)
				_table.Remove(eventName);
		}
	}
	
    //shorthand for invoking event without sender or argument for handler
	public void PostEvent (string eventName){
		PostEvent(eventName, null);
	}
	//shorthand for invoking event without argument for handler
	public void PostEvent (string eventName, System.Object sender){
		PostEvent(eventName, sender, null);
	}
	
	public void PostEvent (string eventName, System.Object sender, System.Object args){
		// if(eventName == "BaseAbilityEffect.GetAttackEvent"){
		// 	Debug.Log("event being posted by " + sender + " for " + eventName);
		// 	// Debug.Log("ARGS: " + args);
		// }
		// if(eventName == "TurnOrderController.turnBegan"){
		// 	Debug.Log("turn beginning");
		// 	Debug.Log("event being posted by " + sender + " for " + eventName);
		// 	// Debug.Log("ARGS: " + args);
		// }
		// if(eventName == "BaseAbilityEffect.HitEvent"){
		// 	Debug.Log("event being posted by " + sender + " for " + eventName);
		// 	Debug.Log("ARGS: " + args);
		// }

		if (string.IsNullOrEmpty(eventName)){
			Debug.LogError("Cannot invoke unnamed event");
			return;
		}
		// checks if event is monitored
		if (!_table.ContainsKey(eventName))
			return;
		
		//gets table holding all senders and respective handlers
		SenderTable subTable = _table[eventName];
		//posts to subscribers that gave a sender
		if (sender != null && subTable.ContainsKey(sender)){
			//gets handler methods given the sender
			List<Handler> handlers = subTable[sender];
			currentlyInvoking.Add(handlers);
			for (int i = 0; i < handlers.Count; ++i)
				handlers[i]( sender, args );
			currentlyInvoking.Remove(handlers);
		}
		
		//posts to subscribers that did not give a sender
		if (subTable.ContainsKey(this)){
			// Debug.Log("IF YOU'RE SEEING THIS THAT'S WEIRD CUZ IM USING THE EVENTEXTENSION TO CALL ALL EVENTCENTER FUNCTIONS SO THERE SHOULD ALWYAS BE A SENDER");
			List<Handler> handlers = subTable[this];
			currentlyInvoking.Add(handlers);
			for (int i = 0; i < handlers.Count; ++i)
				handlers[i]( sender, args );
			currentlyInvoking.Remove(handlers);
		}
	}
}