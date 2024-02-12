using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class UnitFactory
{
	public static GameObject Create (string name, int level){
		UnitRecipe recipe = Resources.Load<UnitRecipe>("Unit Recipes/" + name);
		if (recipe == null){
			recipe = Resources.Load<UnitRecipe>("Unit Recipes/Allies/" + name);
			if(recipe == null){
				recipe = Resources.Load<UnitRecipe>("Unit Recipes/Enemies/" + name);
				if(recipe == null){
					recipe = Resources.Load<UnitRecipe>("Unit Recipes/Neutrals/" + name);
					if(recipe == null){
						Debug.LogError("No Unit Recipe for name: " + name);
						return null;
					}
				}
			}
		}
		Debug.Log("not empty");
		return Create(recipe, level);
	}

	public static GameObject Create (UnitRecipe recipe, int level)
	{
		GameObject unit = InstantiatePrefab("Units/" + recipe.unitBase);
		unit.name = recipe.name;
		unit.GetComponent<SpriteRenderer>().sprite = recipe.sprite;

		InitUnitScript(unit, recipe);
		AddStats(unit, recipe.statData);
		AddHealth(unit, recipe.statData.minHP);
		AddBurst(unit, recipe.statData.minBP);
		AddLevel(unit, level, recipe.xpData);

		unit.AddComponent<Status>();
		unit.AddComponent<Equipment>();
		AddAlliance(unit, recipe.alliance);

		AddMovement(unit, recipe.movementType);
		// AddJob(unit, recipe.job);

		// AddAttack(unit, recipe.attack);
		AddAbilityCatalog(unit, recipe.abilityCatalog);

		// AddAttackPattern(unit, recipe.strategy);
		return unit;
	}

	static GameObject InstantiatePrefab (string filePath){
		GameObject prefab = Resources.Load<GameObject>(filePath);
		if (prefab == null){
			Debug.LogError("No Prefab for at path: " + filePath);
			return new GameObject(filePath);
		}
		GameObject instance = GameObject.Instantiate(prefab);

        //CREATE A BETTER NAMING SCHEMA HERE
		instance.name = instance.name.Replace("(Clone)", "");
		return instance;
	}

	static Unit InitUnitScript(GameObject unit, UnitRecipe recipe){
		Unit unitScript = unit.AddComponent<Unit>();
		unitScript.portrait = recipe.portrait;
		switch(recipe.alliance){
			default:
				unitScript.portraitColor = Color.white;
				break;
			case Alliances.Ally:
				unitScript.portraitColor = new Color32 (0,255,248,255);
				Debug.Log("setting ally color as "+ unitScript.portraitColor);
				break;
			case Alliances.Enemy:
				unitScript.portraitColor = new Color32 (255,0,124,255);
				Debug.Log("setting enemy color as "+ unitScript.portraitColor);
				break;
			case Alliances.Neutral:
				unitScript.portraitColor = Color.green;
				break;
		}
		return unitScript;
	}

	static Stats AddStats (GameObject unit, UnitStatData data){
		Stats s = unit.AddComponent<Stats>();
		s.statData = data;
		s.InitBaseStats();
		return s;
	}
	static Health AddHealth(GameObject unit, int minHP){
		Health health = unit.AddComponent<Health>();
		health.MinHP = minHP;
		return health;
	}	
	static Burst AddBurst(GameObject unit, int minBP){
		Burst burst = unit.AddComponent<Burst>();
		burst.MinBP = minBP;
		return burst;
	}
	static void AddLevel (GameObject obj, int level, XPCurveData data){
		UnitLevel unitLevel = obj.AddComponent<UnitLevel>();
		unitLevel.Init(level, data);
	}
	static Alliances AddAlliance (GameObject obj, Alliances type){
		Alliance alliance = obj.AddComponent<Alliance>();
		alliance.type = type;
		return type;
	}
	static void AddMovement(GameObject obj, MovementTypes type){
		switch (type){
            case MovementTypes.Walk:
                obj.AddComponent<WalkMovement>();
                break;
            case MovementTypes.Fly:
                obj.AddComponent<FlyMovement>();
                break;
		// case MovementTypes.Teleport:
		// 	obj.AddComponent<TeleportMovement>();
		// 	break;
		}
	}

	//currently unused, idk if im gonna have jobs
	static void AddJob (GameObject obj, string name){
		GameObject instance = InstantiatePrefab("Jobs/" + name);
		instance.transform.SetParent(obj.transform);
		Job job = instance.GetComponent<Job>();
		job.Employ();
		job.LoadDefaultStats();
	}
	//unused, i think this is for when i used to have ability menus in the ability menu
	static void AddAttack (GameObject obj, string name){
		GameObject instance = InstantiatePrefab("Abilities/" + name);
		instance.transform.SetParent(obj.transform);
	}
	static void AddAbilityCatalog (GameObject obj, string name){
		GameObject catalog = new GameObject("Ability Catalog");
		catalog.transform.SetParent(obj.transform);
		AbilityCatalog catalogScript = catalog.AddComponent<AbilityCatalog>();

		AbilityCatalogRecipe recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/" + name);
		if (recipe == null){
			Debug.LogError("No Ability Catalog Recipe Found: " + name);
			return;
		}

		for (int i = 0; i < recipe.entries.Length; ++i){
			string abilityName = string.Format("Abilities/{0}", recipe.entries[i]);
			GameObject ability = InstantiatePrefab(abilityName);
			ability.transform.SetParent(catalog.transform);
			switch(ability.GetComponent<Ability>().type){
				case AbilityTypes.BASIC:
					catalogScript.basicAbility = ability;
					break;
				case AbilityTypes.TRAIT:
					catalogScript.traitAbility = ability;
					break;
				case AbilityTypes.SKILL:
					catalogScript.skillAbility = ability;
					break;
				case AbilityTypes.BURST:
					catalogScript.burstAbility = ability;
					break;
			}
		}
	}

	// static void AddAttackPattern (GameObject obj, string name)
	// {
	// 	Driver driver = obj.AddComponent<Driver>();
	// 	if (string.IsNullOrEmpty(name))
	// 	{
	// 		driver.normal = Drivers.Human;
	// 	}
	// 	else
	// 	{
	// 		driver.normal = Drivers.Computer;
	// 		GameObject instance = InstantiatePrefab("Attack Pattern/" + name);
	// 		instance.transform.SetParent(obj.transform);
	// 	}
	// }
}