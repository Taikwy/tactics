using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour 
{
    
	public const string UnitDiedEvent = "Unit.UnitDiedEvent";
    public Tile tile { get; protected set; }
    public Directions dir;
    // public string unitName;

    public const int minLevel = 1;
    public const int maxLevel = 20;
    // public UnitStatData statData;
    // public XPCurveData xpData;
    public Sprite portrait;
    public Color portraitColor;

    [HideInInspector] public Stats statsScript;
    [HideInInspector] public UnitLevel levelScript;
    [HideInInspector] public Alliance allianceScript;

    //public getter for level and experience
    public int LV{
        get { return statsScript[StatTypes.LV]; }
    }
    public int XP{
        get { return statsScript[StatTypes.XP]; }
        set { statsScript[StatTypes.XP] = value; }
    }
    public Alliances ALLIANCE{
        get { return allianceScript.type; }
        // set { statsScript[StatTypes.XP] = value; }
    }
    public Movement movement{
        get{ return GetComponent<Movement>();}
    }
    public Canvas canvas{
        get{ return GetComponentInChildren<Canvas>(); }
    }
    public Transform canvasObj{
        get{ return transform.GetChild(1); }
    }
    public void Init(Tile t){
        Place(t);
        Match();

        statsScript = GetComponent<Stats>();
        levelScript = GetComponent<UnitLevel>();
        allianceScript = GetComponent<Alliance>();

		// this.AddObserver(GetComponent<UnitLevel>().OnLvChangeEvent, Stats.DidChangeEvent(StatTypes.LV), statsScript);
		// Feature[] features = GetComponentsInChildren<Feature>();
		// for (int i = 0; i < features.Length; ++i)
		// 	features[i].Activate(gameObject);

    }
    public void Place (Tile target){
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;
        
        // Link unit and tile references
        tile = target;
        
        if (target != null)
        target.content = gameObject;
    }
    public void Match (){
        transform.localPosition = tile.center;
    }

    //will need to change this when i actually deal with unit death
    public void OnDeath(){
        // Debug.Log("ondeath");
        // Feature[] features = GetComponentsInChildren<Feature>();
		// for (int i = 0; i < features.Length; ++i)
		// 	features[i].Deactivate();

		// this.RemoveObserver(GetComponent<UnitLevel>().OnLvChangeEvent, Stats.DidChangeEvent(StatTypes.LV), statsScript);
        // print("ON DEATH");
        AbilityCatalog catalog = GetComponentInChildren<AbilityCatalog>();
        if(catalog){
            if(catalog.basicAbility){
                catalog.basicAbility.SetActive(false);
            }
            if(catalog.primarySkillAbility){
                catalog.primarySkillAbility.SetActive(false);
            }
            if(catalog.secondarySkillAbility){
                catalog.secondarySkillAbility.SetActive(false);
            }
            if(catalog.traitAbility){
                catalog.traitAbility.SetActive(false);
            }
            if(catalog.burstAbility){
                catalog.burstAbility.SetActive(false);
            }
        }
        this.PostEvent(UnitDiedEvent, gameObject);
        Destroy(gameObject);
        GetComponentInParent<BattleController>().audioManager.Play("Unit Death");
    }    
}