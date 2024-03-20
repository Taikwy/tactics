using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject highlight, target, select;
    public SpriteRenderer highlightRenderer, targetRenderer, selectRenderer;
    public Sprite highlightSprite, targetSprite, selectSprite;
     [Space(10)]
    public Animation targetAnim;

    public bool hovered = false;
    
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
    public bool isWalkable, isFlyable = false;                      //things like pits are flyabale but not walkable
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
        gameObject.name = position.x + " - " + position.y + "     | " + tileType;
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

    void OnMouseEnter(){
        // Debug.Log("entered " + gameObject.name);
        hovered = true;
        // Hover();
    }
    void OnMouseExit(){
        // Debug.Log("exited " + gameObject.name);
        hovered = false;
        // Unhover();
    }

    //curerenlty unused
    // public void Hover(){
    //     highlightRenderer.enabled = true;
    //     highlightRenderer.sprite = highlightSprite;
    //     highlightRenderer.color = new Color(1,1,1, 0.5f);
    // }    public void Unhover(){
    //     highlightRenderer.enabled = false;
    //     highlightRenderer.color = Color.clear;
    // }

    //standard flat color highlight for showing ranges
    public void Highlight(Color color){
        highlightRenderer.enabled = true;
        highlightRenderer.sprite = highlightSprite;
        highlightRenderer.color = color;
    }    public void Unhighlight(){
        highlightRenderer.enabled = false;
        highlightRenderer.color = Color.clear;
    }
    //indicating movement path and hovering targets
    public void Target(Color color){
        targetRenderer.enabled = true;
        targetRenderer.sprite = targetSprite;
        targetRenderer.color = color;
    }    public void Untarget(){
        targetRenderer.enabled = false;
        targetRenderer.color = Color.clear;
    }
    //when the tile gets selected
    public void Select(Color color){
        selectRenderer.enabled = true;
        selectRenderer.sprite = selectSprite;
        selectRenderer.color = color;
    }    public void Unselect(){
        selectRenderer.enabled = false;
        selectRenderer.color = Color.clear;
    }

}
