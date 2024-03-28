using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

public class UnitSetter : MonoBehaviour
{
    [Header("Unit layout stuff")]
    [Tooltip("image the units will be generated from")]public Texture2D unitImage;
    public Color allyColor, enemyColor;

    [HideInInspector] public List<string> allies, enemies;
    [HideInInspector] public List<Vector2> allyPositions, enemyPositions;
    // Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    [Header("Output and Loading stuff")]
    // [SerializeField] LevelRecipe levelRecipe;
    [SerializeField] String levelRecipeName;

    //  public void Clear (){
    //     for (int i = transform.childCount - 1; i >= 0; --i)
    //         DestroyImmediate(transform.GetChild(i).gameObject);
    //     tiles.Clear();
    // }

    //Creates and sets the leveldata scriptable object from the map layout thingy
    public void SetUnits(){
        Debug.Log("setting units");
        Clear();
        if (unitImage == null)
            return;
        for(int x = 0; x < unitImage.width; x++){
            for(int y = 0; y < unitImage.height; y++){
                SetUnit(x, y);
            }
        }
    }

    public void SetUnit(int x, int y){
        Color pixelColor = unitImage.GetPixel(x,y);
        // Debug.Log(pixelColor);
        if(pixelColor.a == 0){
            Debug.Log("fuck color was transpraent");
            return;
        }
        if(pixelColor == allyColor){
            allyPositions.Add(new Vector2(x, y));
            allies.Add("ALLY");
        }
        else if(pixelColor == enemyColor){
            enemyPositions.Add(new Vector2(x, y));
            enemies.Add("ENEMY");
        }
            // Debug.Log("fuck color didnt match " + pixelColor);
    }

    public void Clear(){
        allyPositions = new List<Vector2>();
        enemyPositions = new List<Vector2>();
        allies = new List<string>();
        enemies = new List<string>();
    }

    // Tile CreatePrefab(GameObject prefab){
    //     GameObject tileInstance = Instantiate(prefab, transform);
    //     // Debug.Log(tileInstance.GetComponent<Tile>().tileType);
    //     return tileInstance.GetComponent<Tile>();
    // }

    //Actually loads the level data in
    // public void LoadLevel (){
    //     Clear();
    //     if (levelData == null)
    //         return;
    //     Vector2 position;
    //     for(int i = 0; i < levelData.tilePositions.Count; i++){
    //         position = levelData.tilePositions[i];
    //         // Tile t = CreatePrefab();
    //         Tile t;
    //         switch(levelData.tileTypes[i]){
    //             case Tile.TILETYPE.GROUND:
    //                 t = CreatePrefab(barrierPrefab);
    //                 break;
    //             case Tile.TILETYPE.BARRIER:
    //                 t = CreatePrefab(groundPrefab);
    //                 break;
    //             case Tile.TILETYPE.PIT:
    //                 t = CreatePrefab(pitPrefab);
    //                 break;
    //             case Tile.TILETYPE.WALL:
    //                 t = CreatePrefab(wallPrefab);
    //                 break;
    //             default:
    //                 t = null;
    //                 Debug.Log("missing tile type");
    //                 continue;
    //         }
    //         t.Load(position);
    //         tiles.Add(t.position, t);
    //     }
    // }

    //Saves a set level in the resources folder
    public void SaveRecipe (){
        if(levelRecipeName == ""){
            Debug.LogError("Level name is empty.");
            return;
        }
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory ();

        LevelRecipe levelRecipe = ScriptableObject.CreateInstance<LevelRecipe>();
        levelRecipe.allySpawnLocations =  new List<Vector2>( allyPositions.Count );
        levelRecipe.enemySpawnLocations = new List<Vector2>( enemyPositions.Count );
        
        // LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        // levelData.tilePositions = new List<Vector2>( tiles.Count );
        // levelData.tileTypes = new List<Tile.TILETYPE>( tiles.Count );

        Debug.Log(allyPositions.Count);
        Debug.Log(enemyPositions.Count);
        for(int i = 0; i < allyPositions.Count; i++){
            levelRecipe.allySpawnLocations.Add( new Vector2(allyPositions[i].x, allyPositions[i].y) );
            levelRecipe.allyUnitRecipes.Add(allies[i]);
            // levelRecipe.allyUnitRecipes[i] = allies[i];
        }
        for(int i = 0; i < enemyPositions.Count; i++){
            levelRecipe.enemySpawnLocations.Add( new Vector2(enemyPositions[i].x, enemyPositions[i].y) );
            levelRecipe.enemyUnitRecipes.Add(enemies[i]);
            // levelRecipe.enemyUnitRecipes[i] = enemies[i];
        }
        // foreach (Vector2 pos in allyPositions){
        //     levelRecipe.allySpawnLocations.Add( new Vector2(pos.x, pos.y) );
        //     levelRecipe.allyUnitRecipes.Add( allies);
        // }
        // foreach (Vector2 pos in enemyPositions){
        //     levelRecipe.enemySpawnLocations.Add( new Vector2(pos.x, pos.y) );
        // }
        
        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, levelRecipeName);
        AssetDatabase.CreateAsset(levelRecipe, fileName);
        Debug.Log("Unit Recipe Saved");
        // Debug.Log(name);
        // Debug.Log(fileName);
        // Debug.Log(filePath);
    }

    void CreateSaveDirectory (){
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }
}
