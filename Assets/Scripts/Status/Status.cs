using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status : MonoBehaviour
{
	public const string AddedEvent = "Status.AddedEvent";
	public const string RemovedEvent = "Status.RemovedEvent";
	public const string RemovedAllEvent = "Status.RemovedAllEvent";
	public List<GameObject> statuses = new List<GameObject>();

	//old attempt at tweaking the tutorial version of th emethod to use a prefab
	// public U Add<T, U> () where T : StatusEffect where U : StatusCondition{
	// 	T effect = GetComponentInChildren<T>();

	// 	GameObject child = new GameObject( typeof(T).Name );
    //     child.transform.SetParent(gameObject.transform);
    //     effect = child.AddComponent<T>();


	// 	if (effect == null){
	// 		GameObject statusObj = new GameObject( typeof(T).Name );
	// 		statusObj.transform.SetParent(gameObject.transform);
	// 		effect = child.AddComponent<T>();
	// 		this.PostEvent(AddedNotification, effect);
	// 	}

	// 	return effect.gameObject.AddComponent<U>();
	// }

	//T is effect, U is condition, adds both to an empty object attaches it to the unit 
	public GameObject Add<T, U> () where T : StatusEffect where U : StatusCondition
	{
		GameObject statusObj = new GameObject();
		statusObj.transform.SetParent(gameObject.transform);
		T effect = statusObj.GetComponent<T>();

		if (effect == null){
			statusObj.AddComponent<T>();
			statusObj.AddComponent<U>();
			statuses.Add(statusObj);
			this.PostEvent(AddedEvent, effect);
		}

		return statusObj;
	}

	public void Remove(StatusCondition condition){
		StatusEffect effect = condition.GetComponentInChildren<StatusEffect>();
		statuses.Remove(effect.gameObject);
		
		effect.transform.SetParent(null);
		condition.transform.SetParent(null);
		Destroy(effect.gameObject);
		Destroy(condition.gameObject);

		this.PostEvent(RemovedEvent, effect);
	}

	public void RemoveAll(){
		Debug.Log("removing all");
		StatusEffect effect;
		StatusCondition condition;
		foreach(GameObject status in statuses){
			effect = status.GetComponentInChildren<StatusEffect>();
			condition = status.GetComponentInChildren<StatusCondition>();
			
			Debug.Log("removing " + effect + " | " + condition);
			effect.transform.SetParent(null);
			condition.transform.SetParent(null);
			Destroy(effect.gameObject);
			Destroy(condition.gameObject);
		}
		statuses.Clear();

		this.PostEvent(RemovedAllEvent, null);
	}


	//Logic for how to handle adding another status effect ofthe same type
	public void CheckStatus(){}
}