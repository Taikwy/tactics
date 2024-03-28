using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetCreator
{
	[MenuItem("Assets/Create/Unit Recipe")]
	public static void CreateUnitRecipe ()
	{
		ScriptableObjectCreator.CreateAsset<UnitRecipe> ();
	}
	
	[MenuItem("Assets/Create/Ability Catalog Recipe")]
	public static void CreateAbilityCatalogRecipe ()
	{
		ScriptableObjectCreator.CreateAsset<AbilityCatalogRecipe> ();
	}
}
