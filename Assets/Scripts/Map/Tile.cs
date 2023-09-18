using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // public Sprite defaultSprite;
    // public Sprite highlightedSprite;

    // public SpriteRenderer tileRenderer;
    public SpriteRenderer highlightRenderer, targetRenderer, selectRenderer;
    // public SpriteRenderer overlayRenderer;
    // public Sprite tileSprite;
    public Sprite highlightSprite, targetSprite, selectSprite;
     [Space(10)]
    public Animation targetAnim;
    // public Color moveHighlight = new Color(1,1,1);
    // public Color attackHighlight = new Color(1,1,1);
    // public Color allyHighlight = new Color(1,1,1);
    
    public Point position;
    //public int height;
    public Vector2 center{
        get {return new Vector2(position.x, position.y);}
    }

    public enum TILETYPE{
        GROUND,
        BARRIER,
        PIT,
        WALL
    }

    public TILETYPE tileType;
    public bool isWalkable, isFlyable = false;
    public GameObject content;
    // public bool isTraversible;
    [HideInInspector] public Tile prev;
    [HideInInspector] public int distance;
    // public SpriteRenderer overlayRenderer;
    // public Sprite tileSprite;
    public void Load (Point p){
        highlightRenderer.enabled = false;
        targetRenderer.enabled = false;
        selectRenderer.enabled = false;
        // Debug.Log(tileType);
        position = p;
        Match();
    }

    public void Load (Vector2 pos){
        Load(new Point((int)pos.x, (int)pos.y));
    }

    //Updates tile position and stuff to match Point
    void Match(){
        transform.localPosition = new Vector2(position.x, position.y);
        transform.localScale = new Vector2(1,1);
    }

}
