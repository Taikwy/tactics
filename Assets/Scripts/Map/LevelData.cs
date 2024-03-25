using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData : ScriptableObject
{
    // public Texture2D mapLayout;
    // public MapData mapData;
    [Header("rule Tile Prefabs")]
    public TileBase pitTile;
    public TileBase barrierTile;
    public TileBase wallTile;
    public TileBase groundTile;
    [Header("Tile Prefabs")]
    public GameObject barrierTilePrefab;
    public GameObject groundTilePrefab;
    public GameObject pitTilePrefab;
    public GameObject wallTilePrefab;
    [Header("tile positions and data")]
    public List<Vector2> tilePositions;
    public List<Tile.TILETYPE> tileTypes;
    
}
