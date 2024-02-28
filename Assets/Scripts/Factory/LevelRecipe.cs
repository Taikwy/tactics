using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Recipe", menuName = "ScriptableObjects/LevelRecipe", order = 1)]
public class LevelRecipe : ScriptableObject 
{
    public string levelName;
    public int levelIndex;
    public string[] allyUnitRecipes = new string[]{};
    public Point[] allySpawnLocations = new Point[]{};
    public string[] enemyUnitRecipes = new string[]{};
    public Point[] enemySpawnLocations = new Point[]{};

}