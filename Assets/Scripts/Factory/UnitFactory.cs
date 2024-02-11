using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class UnitFactory
{
	public static GameObject Create (string name, int level){
		UnitRecipe recipe = Resources.Load<UnitRecipe>("Unit Recipes/" + name);
		if (recipe == null){
			Debug.LogError("No Unit Recipe for name: " + name);
			return null;
		}
		return Create(recipe, level);
	}

	public static GameObject Create (UnitRecipe recipe, int level)
	{
		GameObject unit = InstantiatePrefab("Units/" + recipe.unitBase);
		unit.name = recipe.name;
		unit.GetComponent<SpriteRenderer>().sprite = recipe.sprite;

		// Unit unitScript = unit.AddComponent<Unit>();
		// unitScript.statData = recipe.statData;
		// unitScript.xpData = recipe.xpData;
		// unitScript.portrait = recipe.portrait;
		InitUnit(unit, recipe);
		AddStats(unit, recipe.statData);
		
		AddMovement(unit, recipe.movementType);
		unit.AddComponent<Status>();
		unit.AddComponent<Equipment>();
		// AddJob(unit, recipe.job);

		AddLevel(unit, level, recipe.xpData);
		// unit.AddComponent<Health>();
		// unit.AddComponent<Mana>();

		// AddAttack(unit, recipe.attack);
		AddAbilityCatalog(unit, recipe.abilityCatalog);
		AddAlliance(unit, recipe.alliance);

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

	static void InitUnit(GameObject unit, UnitRecipe recipe){
		Unit unitScript = unit.AddComponent<Unit>();
		// unitScript.statData = recipe.statData;
		// unitScript.xpData = recipe.xpData;
		unitScript.portrait = recipe.portrait;
	}

	static Stats AddStats (GameObject unit, UnitStatData data){
		Stats s = unit.AddComponent<Stats>();
		s.statData = data;
		s.InitBaseStats();

		Health health = unit.AddComponent<Health>();
		health.MinHP = data.minHP;

		// TurnOrderController.CalculateAV(unit.GetComponent<Unit>());
		return s;
	}

	static void AddJob (GameObject obj, string name){
		GameObject instance = InstantiatePrefab("Jobs/" + name);
		instance.transform.SetParent(obj.transform);
		Job job = instance.GetComponent<Job>();
		job.Employ();
		job.LoadDefaultStats();
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

	static void AddAlliance (GameObject obj, Alliances type){
		Alliance alliance = obj.AddComponent<Alliance>();
		alliance.type = type;
	}

	static void AddLevel (GameObject obj, int level, XPCurveData data){
		UnitLevel unitLevel = obj.AddComponent<UnitLevel>();
		unitLevel.Init(level, data);
	}

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