using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
// using UnityEditorInternal;
using Unity.Mathematics;
using System;

public class LevelSetter : MonoBehaviour
{
    [Header("Map layout stuff")]
    public Texture2D map;
    public Color barrierColor, groundColor, pitColor, wallColor;

    [Header("Tile prefabs")]
    [SerializeField] GameObject barrierPrefab;
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject pitPrefab;
    [SerializeField] GameObject wallPrefab;

    [HideInInspector] public List<Vector2> tilePositions;
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    [SerializeField] LevelData levelData;
    [SerializeField] String levelDataName;
    

    public void Clear (){
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    //Creates and sets the leveldata scriptable object from the map layout thingy
    public void SetLevel(){
        Debug.Log("setting level");
        Clear();
        if (map == null)
            return;
        Point p;
        for(int x = 0; x < map.width; x++){
            for(int y = 0; y < map.height; y++){
                Tile t = GenerateTile(x,y);
                //if this shit is null
                if(t == null)
                    continue;
                p = new Point(x,y);
                t.Load(p);
                tiles.Add(t.position, t);
            }
        }
    }

    public Tile GenerateTile(int x, int y){
        Color pixelColor = map.GetPixel(x,y);
        // Debug.Log(pixelColor);
        if(pixelColor.a == 0){
            // Debug.Log("fuck color was transpraent");
            return null;
        }
        if(pixelColor == barrierColor){
            return CreatePrefab(barrierPrefab);
        }
        else if(pixelColor == groundColor){
            return CreatePrefab(groundPrefab);
        }
        else if(pixelColor == pitColor){
            return CreatePrefab(pitPrefab);
        }
        else if(pixelColor == wallColor){
            return CreatePrefab(wallPrefab);
        }
            // Debug.Log("fuck color didnt match " + pixelColor);
        return null;
    }

    Tile CreatePrefab(GameObject prefab){
        GameObject tileInstance = Instantiate(prefab, transform);
        // Debug.Log(tileInstance.GetComponent<Tile>().tileType);
        return tileInstance.GetComponent<Tile>();
    }

    //Actually loads the level data in
    public void LoadLevel (){
        Clear();
        if (levelData == null)
            return;
        Vector2 position;
        for(int i = 0; i < levelData.tilePositions.Count; i++){
            position = levelData.tilePositions[i];
            // Tile t = CreatePrefab();
            Tile t;
            switch(levelData.tileTypes[i]){
                case Tile.TILETYPE.GROUND:
                    t = CreatePrefab(barrierPrefab);
                    break;
                case Tile.TILETYPE.BARRIER:
                    t = CreatePrefab(groundPrefab);
                    break;
                case Tile.TILETYPE.PIT:
                    t = CreatePrefab(pitPrefab);
                    break;
                case Tile.TILETYPE.WALL:
                    t = CreatePrefab(wallPrefab);
                    break;
                default:
                    t = null;
                    Debug.Log("missing tile type");
                    continue;
            }
            t.Load(position);
            tiles.Add(t.position, t);
        }
    }

    //Saves a set level in the resources folder
    public void SaveLevel (){
        if(levelDataName == ""){
            Debug.Log("Level name is empty.");
            return;
        }
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory ();
        
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.tilePositions = new List<Vector2>( tiles.Count );
        levelData.tileTypes = new List<Tile.TILETYPE>( tiles.Count );

        Debug.Log(tiles.Count);
        foreach (Tile t in tiles.Values){
            // Debug.Log(t + " " + t.position + " " + t.tileType);
            levelData.tilePositions.Add( new Vector2(t.position.x, t.position.y) );
            levelData.tileTypes.Add(t.tileType);
        }
        
        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, levelDataName);
        AssetDatabase.CreateAsset(levelData, fileName);
        Debug.Log("Level Saved");
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
