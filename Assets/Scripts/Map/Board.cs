using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour 
{
    public enum OverlayColor{
        ATTACK,
        HEAL,
        BUFF,
        DEBUFF,
        MOVE,
        PASS,
    }
    [SerializeField] GameObject barrierTilePrefab;
    [SerializeField] GameObject groundTilePrefab;
    [SerializeField] GameObject pitTilePrefab;
    [SerializeField] GameObject wallTilePrefab;
    public enum SelectColor{
        VALID,
        INVALID,
        EMPTY,
        ALLY,
        ENEMY,
    }
    [Header("Select Indicator Colors")]
    public Color selectValid;
    public Color selectInvalid;
    public Color selectEmpty;
    public Color selectAlly;
    public Color selectEnemy;
    [Header("Highlight Colors")]
    [SerializeField] Color moveColor;
    [SerializeField] Color passColor, attackColor, healColor, buffColor, debuffColor;
    [SerializeField] float highlightAlpha, targetAlpha, selectAlpha;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    [HideInInspector]public bool humanDriver = true;

    public Point selectedPoint;
    public Tile selectedTile{
        get{
            return GetTile(selectedPoint);
        }
    }
    Point[] dirs = new Point[4]{
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    // [SerializeField] Color selectedTileColor = new Color(0, 1, 1, 1);
    // [SerializeField] Color defaultTileColor = new Color(1, 1, 1, 1);

    public Point min { get { return _min; }}
    public Point max { get { return _max; }}
    Point _min;
    Point _max;

    public void Load (LevelData data)
    {
        _min = new Point(int.MaxValue, int.MaxValue);
        _max = new Point(int.MinValue, int.MinValue);
        for (int i = 0; i < data.tileTypes.Count; ++i){
            GameObject tileInstance;
            switch(data.tileTypes[i]){
                case Tile.TILETYPE.GROUND:
                    tileInstance = Instantiate(groundTilePrefab, gameObject.transform);
                    break;
                case Tile.TILETYPE.BARRIER:
                    tileInstance = Instantiate(barrierTilePrefab, gameObject.transform);
                    break;
                case Tile.TILETYPE.PIT:
                    tileInstance = Instantiate(pitTilePrefab, gameObject.transform);
                    break;
                case Tile.TILETYPE.WALL:
                    tileInstance = Instantiate(wallTilePrefab, gameObject.transform);
                    break;
                default:
                    tileInstance = new GameObject();
                    break;
            }

            Tile tileScript = tileInstance.GetComponent<Tile>();
            tileScript.Load(data.tilePositions[i]);
            tiles.Add(tileScript.position, tileScript);

            _min.x = Mathf.Min(_min.x, tileScript.position.x);
            _min.y = Mathf.Min(_min.y, tileScript.position.y);
            _max.x = Mathf.Max(_max.x, tileScript.position.x);
            _max.y = Mathf.Max(_max.y, tileScript.position.y);
        }
    }

    public Tile GetTile (Point p){
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }


    public void Update(){
        if(humanDriver)
            UpdateSelectedPoint();
    }
    void UpdateSelectedPoint(){
        foreach(KeyValuePair<Point, Tile> tile in tiles){
            if(tile.Value.hovered){
                selectedPoint = tile.Value.position;
            }
        }
    }

    public void HighlightTiles (List<Tile> tiles, OverlayColor type){
        Color temp = Color.white;
        switch(type){
            case OverlayColor.MOVE:
                temp = moveColor;
                break;
            case OverlayColor.PASS:
                temp = passColor;
                break;
            case OverlayColor.ATTACK:
                temp = attackColor;
                break;
            case OverlayColor.BUFF:
                temp = buffColor;
                break;
            case OverlayColor.HEAL:
                temp = healColor;
                break;
        }
        temp.a = highlightAlpha;
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].Highlight(temp);
        }
    }
    public void UnhighlightTiles (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].Unhighlight();
        }
    }
    
    public void TargetTiles (List<Tile> tiles, OverlayColor type){
        Color temp = Color.white;
        switch(type){
            case OverlayColor.MOVE:
                temp = moveColor;
                break;
            case OverlayColor.PASS:
                temp = passColor;
                break;
            case OverlayColor.ATTACK:
                temp = attackColor;
                break;
            case OverlayColor.BUFF:
                temp = buffColor;
                break;
            case OverlayColor.HEAL:
                temp = healColor;
                break;
        }
        temp.a = targetAlpha;
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].Target(temp);
        }
    }
    public void UntargetTiles (List<Tile> tiles){
        if(tiles != null)
            for (int i = tiles.Count - 1; i >= 0; --i){
                tiles[i].Untarget();
            }
    }

    public void SelectTiles (List<Tile> tiles, OverlayColor type){
        Color temp = Color.white;
        switch(type){
            case OverlayColor.MOVE:
                temp = moveColor;
                break;
            case OverlayColor.PASS:
                temp = passColor;
                break;
            case OverlayColor.ATTACK:
                temp = attackColor;
                break;
            case OverlayColor.BUFF:
                temp = buffColor;
                break;
            case OverlayColor.HEAL:
                temp = healColor;
                break;
        }
        temp.a = selectAlpha;
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].Select(temp);
        }
    }
    public void UnselectTiles (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].Unselect();
        }
    }

    //Finds tiles within range, no filter
    public List<Tile> Search (Tile startTile, Func<Tile, Tile, bool> addTile){
        List<Tile> tiles = new List<Tile>();
        tiles.Add(startTile);
        
        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        startTile.distance = 0;
        checkNow.Enqueue(startTile);

        while (checkNow.Count > 0){
            Tile currentTile = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i){
                Tile nextTile = GetTile(currentTile.position + dirs[i]);
                if (nextTile == null || nextTile.distance <= currentTile.distance + 1)
                    continue;

                if (addTile(currentTile, nextTile)){
                    nextTile.distance = currentTile.distance + 1;
                    nextTile.prev = currentTile;
                    checkNext.Enqueue(nextTile);
                    tiles.Add(nextTile);
                }
            }
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return tiles;
    }

    void ClearSearch (){
        foreach (Tile t in tiles.Values){
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    void SwapReference (ref Queue<Tile> a, ref Queue<Tile> b){
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

}
