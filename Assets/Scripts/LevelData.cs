using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public Texture2D mapLayout;
    public MapData mapData;
    public List<Vector2> tilePositions;
    public List<Tile.TILETYPE> tileTypes;
    
}
