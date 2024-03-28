using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData : ScriptableObject
{

public BoardData boardData;

    [HideInInspector] public TileBase pitTile;
    [HideInInspector] public TileBase barrierTile;
    [HideInInspector] public TileBase wallTile;
    [HideInInspector] public TileBase groundTile;
    [HideInInspector] public GameObject barrierTilePrefab;
    [HideInInspector] public GameObject groundTilePrefab;
    [HideInInspector] public GameObject pitTilePrefab;
    [HideInInspector] public GameObject wallTilePrefab;
    [Header("tile positions and data")]
    public List<Vector2> tilePositions;
    public List<Tile.TILETYPE> tileTypes;
    
}
