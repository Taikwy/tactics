using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    public Texture2D map;
    public Color barrierColor, groundColor, pitColor, wallColor;

    [SerializeField] GameObject tileBarrierPrefab;
    [SerializeField] GameObject tileGroundPrefab;
    [SerializeField] GameObject tilePitPrefab;
    [SerializeField] GameObject tileWallPrefab;

}
