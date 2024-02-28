using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State 
{
    protected BattleController owner;
    protected Driver driver;
    public CameraRig cameraRig { get { return owner.cameraRig; }}
    public Board board { get { return owner.board; }}
    public LevelData levelData { get { return owner.levelData; }}
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
    public Point selectPos { get { return owner.selectPos; } set { owner.selectPos = value; }}
    public Turn turn { get { return owner.turn; }}
    public List<Unit> units { get { return owner.units; }}

    // public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; }}
    public AbilityPanelController abilityPanelController { get { return owner.abilityPanelController; }}
    public PanelController panelController { get { return owner.panelController; }}
    public ForecastPanel forecastPanel { get { return owner.forecastPanel; }}
    public TurnOrderController turnOrderController { get { return owner.turnOrderController; }}
    public GameUIController guiController { get { return owner.guiController; }}

    // public bool currentlyActive = false;                    //used for update functions of the different states

    protected virtual void Awake (){
        owner = GetComponent<BattleController>();
    }
    protected override void AddListeners (){
        if (driver == null || driver.Current == Drivers.Human){
			InputController.moveEvent += OnMove;
			InputController.fireEvent += OnFire;
		}
    }
    
    protected override void RemoveListeners (){
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }

    public override void Enter (){
		driver = (turn.actingUnit != null) ? turn.actingUnit.GetComponent<Driver>() : null;
		base.Enter ();
	}

    

    protected virtual void OnMove (object sender, InfoEventArgs<Point> e){
    }
    
    protected virtual void OnFire (object sender, InfoEventArgs<int> e){  
    }

    //moves tile selection indicator to point
    protected virtual void SelectTile (Point p) {
        // Debug.Log("selecting tile " + p);
        if (selectPos == p || !board.tiles.ContainsKey(p))
            return;
        selectPos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
        
        tileSelectionIndicator.gameObject.GetComponent<SpriteRenderer>().color = board.selectValid;
    }
    //takes in a bool to see whether the selected tile is targetable. will change color of the highlight
    protected virtual void SelectTile (Point p, bool targetable = true) {
        // Debug.Log("selecting tile " + p);
        if (selectPos == p || !board.tiles.ContainsKey(p))
            return;
        selectPos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
        if(targetable)
            tileSelectionIndicator.gameObject.GetComponent<SpriteRenderer>().color = board.selectValid;
        else
            tileSelectionIndicator.gameObject.GetComponent<SpriteRenderer>().color = board.selectInvalid;
    }

    protected virtual Unit GetUnit (Point p) {
        Tile t = board.GetTile(p);
        GameObject content = t != null ? t.content : null;
        return content != null ? content.GetComponent<Unit>() : null;
    }

    protected virtual void RefreshPrimaryPanel (Point p){
        // Debug.Log("refreshing primary panel");
        Unit target = GetUnit(p);
        if (target != null){
            // statPanelController.ShowPrimary(target.gameObject);
            panelController.ShowPrimary(target.gameObject);
        }
        else{
            // statPanelController.HidePrimary();
            panelController.HidePrimary();
        }
    }
    protected virtual void RefreshPrimaryStatusPanel (Point p){
        // Debug.Log("refreshing primary panel");
        Unit target = GetUnit(p);
        if (target != null){
            // Debug.Log("showing status"); 
            panelController.ShowStatus(target.gameObject);
        }
        else{
            panelController.HideStatus();
        }
    }
    protected virtual void RefreshSecondaryPanel (Point p){
        // Debug.Log("refreshing second panel");
        Unit target = GetUnit(p);
        // Debug.Log(target);
        if (target != null){
            // statPanelController.ShowSecondary(target.gameObject);
            panelController.ShowSecondary(target.gameObject);
        }
        else{
            // statPanelController.HideSecondary();
            panelController.HideSecondary();
        }
    }

    protected virtual bool DidPlayerWin (){
        return owner.GetComponentInChildren<BaseVictoryCondition>().Victor == Alliances.Ally;
    }
    protected virtual bool IsBattleOver (){
        // Debug.Log("checking if battle is over " + (owner.GetComponentInChildren<BaseVictoryCondition>().Victor != Alliances.None));
        return owner.GetComponentInChildren<BaseVictoryCondition>().Victor != Alliances.None;
    }
}
