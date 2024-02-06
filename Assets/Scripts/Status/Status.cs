using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status : MonoBehaviour
{
	public const string AddedNotification = "Status.AddedNotification";
	public const string RemovedNotification = "Status.RemovedNotification";
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

		if (effect == null)
		{
			statusObj.AddComponent<T>();
			statusObj.AddComponent<U>();
			statuses.Add(statusObj);
			this.PostEvent(AddedNotification, effect);
		}

		return statusObj;
	}


	//destroys condition and gameobject, then destroys effect gameobject
	// public void Remove (StatusCondition target){
	// 	StatusEffect effect = target.GetComponentInParent<StatusEffect>();

	// 	target.transform.SetParent(null);
	// 	Destroy(target.gameObject);

	// 	StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
	// 	if (condition == null){
	// 		effect.transform.SetParent(null);
	// 		Destroy(effect.gameObject);
	// 		this.PostEvent(RemovedNotification, effect);
	// 	}
	// }

	//Currently unused
	// public void Add(){
	// 	StatusEffect effect = GetComponentInChildren<StatusEffect>();
	// 	if(effect != null)
	// 		CheckStatus();
	// 	else{
	// 		GameObject statusObj = new GameObject( typeof(StatusEffect).Name );
	// 		statusObj.transform.SetParent(gameObject.transform);
	// 		effect = statusObj.AddComponent<StatusEffect>();
	// 		statusObj.AddComponent<StatusCondition>();
	// 		this.PostEvent(AddedNotification, effect);
	// 	}
	// }

	//old version wher i just add a status ailment prefab
	// public void Add(GameObject statusPrefab){
	// 	// Debug.Log("adding status");
	// 	GameObject statusObj = Instantiate(statusPrefab, gameObject.transform);
	// 	statuses.Add(statusObj);
	// 	this.PostEvent(AddedNotification, statusObj.GetComponent<StatusEffect>());
	// }
	public void Remove(StatusCondition condition){
		StatusEffect effect = condition.GetComponentInChildren<StatusEffect>();
		statuses.Remove(effect.gameObject);
		
		effect.transform.SetParent(null);
		condition.transform.SetParent(null);
		Destroy(effect.gameObject);
		Destroy(condition.gameObject);

		this.PostEvent(RemovedNotification, effect);
	}


	//Logic for how to handle adding another status effect ofthe same type
	public void CheckStatus(){}
}