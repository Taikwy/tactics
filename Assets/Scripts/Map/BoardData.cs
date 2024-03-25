using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Board Data", menuName = "ScriptableObjects/BoardData", order = 1)]
public class BoardData : ScriptableObject{
    [Header("rule Tile Prefabs")]
    public TileBase barrierTile;
    public TileBase groundTile;
    public TileBase pitTile;
    public TileBase wallTile;
    [Header("Tile Prefabs")]
    public GameObject barrierTilePrefab;
    public GameObject groundTilePrefab;
    public GameObject pitTilePrefab;
    public GameObject wallTilePrefab;
    
}
