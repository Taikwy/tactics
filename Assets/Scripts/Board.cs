using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour 
{
    [SerializeField] GameObject barrierTilePrefab;
    [SerializeField] GameObject groundTilePrefab;
    [SerializeField] GameObject pitTilePrefab;
    [SerializeField] GameObject wallTilePrefab;
    [SerializeField] Color moveHighlightColor = new Color(0, 1, 1, 1);
    [SerializeField] Color attackHighlightColor = new Color(0, 1, 1, 1);
    [SerializeField] Color allyHighlightColor = new Color(0, 1, 1, 1);
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
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
            GameObject tileInstance = new GameObject();
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
            }
            // GameObject instance = Instantiate(tilePrefab) as GameObject;
            // tileInstance.transform.parent = gameObject.transform;

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

    // public void SelectTiles (List<Tile> tiles){
    //     for (int i = tiles.Count - 1; i >= 0; --i){
    //         tiles[i].highlightSprite.color = moveHighlightColor;
    //     }
    // }

    public void HighlightMoveTiles (List<Tile> tiles){
        // Debug.Log("highlight " + tiles.Count );
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].highlightSprite.color = moveHighlightColor;
        }
    }
    public void HighlightAttackTiles (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].highlightSprite.color = attackHighlightColor;
        }
    }
    public void HighlightAllyTiles (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i){
            tiles[i].highlightSprite.color = allyHighlightColor;
        }
    }

    public void UnhighlightTiles (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i){
            // tiles[i].GetComponent<SpriteRenderer>().sprite = tiles[i].defaultSprite;
            tiles[i].highlightSprite.color = new Color(1,1,1, 0f);
        }
            //tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
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
