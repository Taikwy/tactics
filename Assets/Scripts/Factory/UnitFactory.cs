using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

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
						Debug.LogError("No Unit Recipe for at path: " + recipe);
						return null;
					}
				}
			}
		}
		// Debug.Log("not empty");
		return Create(recipe, level);
	}

	public static GameObject Create (UnitRecipe recipe, int level){
		GameObject unit = InstantiatePrefab("Units/" + recipe.unitBase);
		unit.name = recipe.name;
		unit.GetComponent<SpriteRenderer>().sprite = recipe.sprite;

		InitUnitScript(unit, recipe);
		AddStats(unit, recipe.statData);
		AddLevel(unit, level, recipe.xpData);
		unit.AddComponent<Status>();
		unit.AddComponent<Equipment>();

		AddHealth(unit, recipe.statData.minHP);
		AddBurst(unit);
		AddSkill(unit);
		AddAlliance(unit, recipe.alliance);
		AddMovement(unit, recipe.movementType, recipe.moveSpeed, recipe.moveDelay);
		// AddJob(unit, recipe.job);

		// AddAttack(unit, recipe.attack);
		AddEquipment(unit, recipe.equipment);
		AddAbilityCatalog(unit, recipe.abilityCatalog);
		AddCanvas(unit);

		AddAttackPattern(unit,recipe.cpuDriver, recipe.attackPattern);
		return unit;
	}

	static GameObject InstantiatePrefab (string filePath){
		GameObject prefab = Resources.Load<GameObject>(filePath);
		if (prefab == null){
			// Debug.LogError("No Prefab for at path: " + filePath);
			// return new GameObject(filePath);
			return null;
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
				// Debug.Log("setting ally color as "+ unitScript.portraitColor);
				break;
			case Alliances.Enemy:
				unitScript.portraitColor = new Color32 (255,0,124,255);
				// Debug.Log("setting enemy color as "+ unitScript.portraitColor);
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
	static void AddLevel (GameObject obj, int level, XPCurveData data){
		UnitLevel unitLevel = obj.AddComponent<UnitLevel>();
		unitLevel.Init(level, data);
	}
	//=================================================================================================================================
	static Health AddHealth(GameObject unit, int minHP){
		Health health = unit.AddComponent<Health>();
		health.MinHP = minHP;
		return health;
	}	
	static Burst AddBurst(GameObject unit, int BP = 0){
		Burst burst = unit.AddComponent<Burst>();
		burst.BP = BP;
		return burst;
	}
	static SkillPoints AddSkill(GameObject unit, int SK = 1){
		SkillPoints skill = unit.AddComponent<SkillPoints>();
		skill.SK = SK;
		return skill;
	}
	static Alliances AddAlliance (GameObject obj, Alliances type){
		Alliance alliance = obj.AddComponent<Alliance>();
		alliance.type = type;
		return type;
	}
	static void AddMovement(GameObject obj, MovementTypes type, float moveSpeed, float moveDelay){
		Movement movement;
		switch (type){
            case MovementTypes.Walk:
                movement = obj.AddComponent<WalkMovement>();
                break;
            case MovementTypes.Fly:
                movement = obj.AddComponent<FlyMovement>();
                break;
			case MovementTypes.Teleport:
				movement = obj.AddComponent<TeleportMovement>();
				break;
			default:
				movement = null;
				break;
		}
		// movement.moveSpeed = moveSpeed;
		// movement.moveDelaySpeed = moveDelay;
		movement.moveSpeed = 7f;
		movement.moveDelayAmount = 8f;
	}

	static void AddEquipment(GameObject unit, List<GameObject> equipment){
        Equipment equipmentScript = unit.GetComponent<Equipment>();
		foreach(GameObject equipmentPiece in equipment){
			Equippable toEquip = GameObject.Instantiate(equipmentPiece).GetComponent<Equippable>();
			toEquip.name = equipmentPiece.name;
			equipmentScript.Equip (toEquip, toEquip.defaultSlots);
		}
	}
	static void AddAbilityCatalog (GameObject obj, string name){
		GameObject catalog = new GameObject("Ability Catalog");
		catalog.transform.SetParent(obj.transform);
		AbilityCatalog catalogScript = catalog.AddComponent<AbilityCatalog>();

		AbilityCatalogRecipe recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/" + name);
		if (recipe == null){
			recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/Basic Attacks/" + name);
			if (recipe == null){
				recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/Skills/" + name);
				if (recipe == null){
					recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/Bursts/" + name);
					if (recipe == null){
						Debug.LogError("No Ability Catalog Recipe Found: " + recipe);
						return;
					}
				}
			}
		}
		// Debug.Log("catalog found! " + recipe);

		for (int i = 0; i < recipe.entries.Length; ++i){
			string abilityName = string.Format("Abilities/{0}", recipe.entries[i]);
			// Debug.Log("ability creation " + abilityName);
			GameObject ability = InstantiatePrefab(abilityName);
			if(!ability){
				abilityName = string.Format("Abilities/Basic Attacks/{0}", recipe.entries[i]);
				ability = InstantiatePrefab(abilityName);
				if(!ability){
					abilityName = string.Format("Abilities/Skills/{0}", recipe.entries[i]);
					ability = InstantiatePrefab(abilityName);
					if(!ability){
						abilityName = string.Format("Abilities/Bursts/{0}", recipe.entries[i]);
						ability = InstantiatePrefab(abilityName);
						if(!ability){
							Debug.LogError("No ability found in any path");
							continue;
						}
					}
				}
			}
			
			ability.transform.SetParent(catalog.transform);
			switch(ability.GetComponent<Ability>().type){
				case AbilityTypes.BASIC:
					catalogScript.basicAbility = ability;
					break;
				case AbilityTypes.TRAIT:
					catalogScript.traitAbility = ability;
					break;
				case AbilityTypes.SKILL:
					if(catalogScript.primarySkillAbility == null){
						catalogScript.primarySkillAbility = ability;
					}else if(catalogScript.secondarySkillAbility == null){
						catalogScript.secondarySkillAbility = ability;
					}else{
						Debug.LogError("Primary and Secondary Skill Ability slots are already filled");
					}
					// catalogScript.skillAbility = ability;
					break;
				case AbilityTypes.BURST:
					catalogScript.burstAbility = ability;
					break;
			}
			ability.GetComponent<Ability>().SetOwner();
		}
	}

	static void AddCanvas(GameObject obj){
		GameObject canvasObj = new GameObject();
		// canvasObj
		canvasObj.transform.parent = obj.transform;
		Canvas canvas = canvasObj.AddComponent<Canvas>();
	}

	static void AddAttackPattern (GameObject obj, bool cpuDriver, string name){
		Driver driver = obj.AddComponent<Driver>();
		if(!cpuDriver){
			driver.normal = Drivers.Human;
		}
		else{
			GameObject pattern = InstantiatePrefab("Attack Patterns/" + name);
			if(pattern != null){
				pattern.transform.SetParent(obj.transform);
				driver.normal = Drivers.Computer;
			}
			else{
				Debug.LogError(obj.name + " set to CPU Driver but no Attack Pattern found");
				driver.normal = Drivers.Human;
			}
		}
	}
}