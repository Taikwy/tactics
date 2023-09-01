using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject tileGroundPrefab;
    [SerializeField] GameObject tileWallPrefab;
    [SerializeField] GameObject tilePitPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    
    [SerializeField] public List<Vector2> tilePositions;

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] Point position;
    [SerializeField] LevelData levelData;

    Transform _selectionMarker;

    //Tile indicaton marker stuff
    Transform selectionMarker{
        get{
            if (_selectionMarker == null){
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                instance.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("UI");
                // item.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("NameOfTheSortingLayer");
                _selectionMarker = instance.transform;
            }
            return _selectionMarker;
        }
    }

    public void UpdateSelectionMarker (){
        Tile t = tiles.ContainsKey(position) ? tiles[position] : null;
        selectionMarker.localPosition = t != null ? t.center : new Vector2(position.x, position.y);
    }

    public void GenerateArea(){
        for(int i = 0; i < width; i++){
            for (int j = 0; j < depth; j++){
                Tile t = CreateTile(new Point(i, j));
            }
        }
        //Tile t = GetOrCreate(position);
    }

    public void Generate(){
        Tile t = CreateTile(position);
        // if (t.height < height)
        //     t.Grow();
    }

    public void GenerateWall(){
        Tile tile = CreateTile(position);
        GameObject wall = Instantiate(wallPrefab) as GameObject;
        wall.transform.parent = tile.transform;
        tile.content = wall;
    }

    //If tile already exists return the tile, otherwise creates and adds the tile
    Tile CreateTile (Point p){
        if (tiles.ContainsKey(p))
            return tiles[p];
        
        Tile tileScript = CreatePrefab();
        tileScript.Load(p);
        tiles.Add(p, tileScript);
        
        return tileScript;
    }

    Tile CreatePrefab (){
        GameObject tileInstance = Instantiate(tilePrefab, transform);
        return tileInstance.GetComponent<Tile>();
    }
    Tile Create (GameObject prefab){
        GameObject instance = Instantiate(prefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }


    public void Clear (){
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }


    //For saving a craeted level. this part will be unused, but still useful as ref
    public void Save (){
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory ();
        
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.tilePositions = new List<Vector2>( tiles.Count );
        foreach (Tile t in tiles.Values)
            levelData.tilePositions.Add( new Vector2(t.position.x, t.position.y) );
        
        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(levelData, fileName);
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

    //Will be more useful, can store levels as text files that are loaded in later
    public void Load (){
        Clear();
        if (levelData == null)
            return;
        foreach (Vector2 position in levelData.tilePositions){
            Tile t = CreatePrefab();
            t.Load(position);
            tiles.Add(t.position, t);
        }
    }

}
