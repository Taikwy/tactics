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
		GameObject unit = InstantiatePrefab("Units/" + recipe.model);
		unit.name = recipe.name;
		unit.AddComponent<Unit>();
		AddStats(unit);
		AddMovement(unit, recipe.movementType);
		// unit.AddComponent<Status>();
		unit.AddComponent<Equipment>();
		AddJob(unit, recipe.job);
		// AddRank(unit, level);
		// unit.AddComponent<Health>();
		// unit.AddComponent<Mana>();
		AddAttack(unit, recipe.attack);
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

	static void AddStats (GameObject obj){
		Stats s = obj.AddComponent<Stats>();
		s.SetValue(StatTypes.LV, 1, false);
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

	// static void AddRank (GameObject obj, int level){
	// 	Rank rank = obj.AddComponent<Rank>();
	// 	rank.Init(level);
	// }

	static void AddAttack (GameObject obj, string name){
		GameObject instance = InstantiatePrefab("Abilities/" + name);
		instance.transform.SetParent(obj.transform);
	}

	static void AddAbilityCatalog (GameObject obj, string name){
		GameObject main = new GameObject("Ability Catalog");
		main.transform.SetParent(obj.transform);
		main.AddComponent<AbilityCatalog>();

		AbilityCatalogRecipe recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/" + name);
		if (recipe == null){
			Debug.LogError("No Ability Catalog Recipe Found: " + name);
			return;
		}

		for (int i = 0; i < recipe.categories.Length; ++i){
			GameObject category = new GameObject( recipe.categories[i].name );
			category.transform.SetParent(main.transform);

			for (int j = 0; j < recipe.categories[i].entries.Length; ++j){
				string abilityName = string.Format("Abilities/{0}/{1}", recipe.categories[i].name, recipe.categories[i].entries[j]);
				GameObject ability = InstantiatePrefab(abilityName);
				ability.transform.SetParent(category.transform);
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