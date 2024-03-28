using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Recipe", menuName = "ScriptableObjects/LevelRecipe", order = 1)]
public class LevelRecipe : ScriptableObject 
{
    public string levelName;
    public int levelIndex;
    public List<string> allyUnitRecipes = new List<string>();
    public List<Vector2> allySpawnLocations = new List<Vector2>();
    public List<string> enemyUnitRecipes = new List<string>();
    public List<Vector2> enemySpawnLocations = new List<Vector2>();

}