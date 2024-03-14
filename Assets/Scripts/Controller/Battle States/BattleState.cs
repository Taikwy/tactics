using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;

// using UnityEditor.U2D.Aseprite;
using UnityEngine;

public abstract class BattleState : State 
{
    protected BattleController owner;
    protected Driver driver;
    public CameraRig cameraRig { get { return owner.cameraRig; }}
    public Board board { get { return owner.board; }}
    public LevelData levelData { get { return owner.levelData; }}
    public TileSelectionIndicator tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
    public ActorIndicator actorIndicator { get { return owner.actorIndicator; }}
    
    public GameObject tileSelectionIndicatorPrefab { get { return owner.tileSelectionIndicatorPrefab; }}
    public Point selectPos { get { return owner.selectPos; } set { owner.selectPos = value; }}
    public Turn turn { get { return owner.turn; }}
    public List<Unit> units { get { return owner.units; }}

    // public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; }}
    public AbilityPanelController abilityPanelController { get { return owner.abilityPanelController; }}
    public PanelController panelController { get { return owner.panelController; }}
    public ForecastPanel forecastPanel { get { return owner.forecastPanel; }}
    public TurnOrderController turnOrderController { get { return owner.turnOrderController; }}
    public GameUIController guiController { get { return owner.guiController; }}
    public PerformStateUI performStateUI { get { return owner.performStateUI; }}

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
        // if(!driver)
        //     Debug.LogError("DRIVER IS NULL");
		base.Enter ();
        
        this.AddObserver(OnStatusEffectApplied, StatusEffect.EffectAppliedEvent);
        this.AddObserver(OnUnitDeath, Unit.UnitDiedEvent);
	}

    public override void Exit (){
		base.Exit ();
        
        this.RemoveObserver(OnStatusEffectApplied, StatusEffect.EffectAppliedEvent);
        this.RemoveObserver(OnUnitDeath, Unit.UnitDiedEvent);
	}

    void OnStatusEffectApplied(object sender, object args){
        // Debug.LogError("STATUS EFFECT APPLIED " + (sender as StatusEffect) + " | " + args);
        Tile target = (sender as StatusEffect).GetComponentInParent<Unit>().tile;
        // print("ON HIT SENDER " + target + " | " + sender.GetType());
        string effect = "-" + args.ToString();
        if(target){
            DisplayStatusEffect(target, effect);
        }
        // print(info.arg0 + " | " + targetIndex + " ON HIT " + effects[targetIndex].Count);
    }
    void DisplayStatusEffect (Tile target, string effect){
        // Debug.LogError("DISPLATYINY STATUS EFFECT " + target + " | " + effect);
        Vector2 labelOffset = new Vector2(0, .6f);
        Vector2 targetPos = (Vector2)target.transform.position + labelOffset;
        Unit unit = target.content.GetComponent<Unit>();
        GameObject effectLabel = Instantiate(owner.performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);

        effectLabel.GetComponent<EffectLabel>().Initialize(effect, .75f, 2);
        // print(effects[targetIndex][effectIndex] + " | pos " + targetPos);
        // yield return null;
    }

    void OnUnitDeath(object sender, object args){
        // print("ON UNIT DEATH");
        // Unit unit = args as Unit;
        // // units.Remove(unit);
        // bool change = turn.actingUnit == unit;
        // Destroy(unit);
        // if(change){
        //     owner.ChangeState<SelectUnitState>();
        // }
    }

    protected virtual void OnMove (object sender, InfoEventArgs<Point> e){
    }
    
    protected virtual void OnFire (object sender, InfoEventArgs<int> e){  
    }

    protected virtual void HideSelect () {
        Color temp = tileSelectionIndicator.sr.color;
        temp.a = 0f;
        tileSelectionIndicator.sr.color = temp;
    }
    protected virtual void ShowSelect () {
        Color temp = tileSelectionIndicator.sr.color;
        temp.a = 1f;
        tileSelectionIndicator.sr.color = temp;
    }

    protected virtual void IndicateActor(Unit unit){
        actorIndicator.transform.parent = unit.transform;
        actorIndicator.Reset(new Vector2(0,.9f));
        actorIndicator.ChangeColor(unit);

    }


    //moves tile selection indicator to point
    protected virtual void SelectTile (Point p) {
        // Debug.Log("selecting tile " + p);
        if (selectPos == p || !board.tiles.ContainsKey(p))
            return;
        selectPos = p;
        tileSelectionIndicator.transform.localPosition = board.tiles[p].center;
        tileSelectionIndicator.sr.color = board.selectValid;
    }
    //takes in a bool to see whether the selected tile is targetable. will change color of the highlight
    protected virtual void SelectTile (Point p, bool targetable = true) {
        // Debug.Log("selecting tile " + p);
        SelectTile(p);
        if(targetable)
            tileSelectionIndicator.sr.color = board.selectValid;
        else
            tileSelectionIndicator.sr.color = board.selectInvalid;
    }
    protected virtual void SelectTile (Point p, Board.SelectColor color) {
        SelectTile(p);
        Color temp = Color.white;
        switch(color){
            case Board.SelectColor.VALID:
                temp = board.selectValid;
                break;
            case Board.SelectColor.INVALID:
                temp = board.selectInvalid;
                break;
            case Board.SelectColor.EMPTY:
                temp = board.selectEmpty;
                break;
            case Board.SelectColor.ALLY:
                temp = board.selectAlly;
                break;
            case Board.SelectColor.ENEMY:
                temp = board.selectEnemy;
                break;
        }
        tileSelectionIndicator.sr.color = temp;
    }
    protected virtual List<GameObject> IndicateTiles (List<Tile> tiles, Board.SelectColor color) {
        Color temp = Color.white;
        switch(color){
            case Board.SelectColor.VALID:
                temp = board.selectValid;
                break;
            case Board.SelectColor.INVALID:
                temp = board.selectInvalid;
                break;
            case Board.SelectColor.EMPTY:
                temp = board.selectEmpty;
                break;
            case Board.SelectColor.ALLY:
                temp = board.selectAlly;
                break;
            case Board.SelectColor.ENEMY:
                temp = board.selectEnemy;
                break;
        }
        List<GameObject> indicators = new List<GameObject>();
        foreach(Tile t in tiles){
            GameObject indicator = IndicateTile(t.position, color);
            indicator.GetComponent<SpriteRenderer>().color = temp;
            indicators.Add(indicator);
        }
        return indicators;
    }
    protected virtual GameObject IndicateTile (Point p, Board.SelectColor color) {
        if (!board.tiles.ContainsKey(p))
            return null;
        GameObject indicator = Instantiate(tileSelectionIndicatorPrefab, board.tiles[p].center,  Quaternion.identity, board.transform);
        
        Color temp = Color.white;
        switch(color){
            case Board.SelectColor.VALID:
                temp = board.selectValid;
                break;
            case Board.SelectColor.INVALID:
                temp = board.selectInvalid;
                break;
            case Board.SelectColor.EMPTY:
                temp = board.selectEmpty;
                break;
            case Board.SelectColor.ALLY:
                temp = board.selectAlly;
                break;
            case Board.SelectColor.ENEMY:
                temp = board.selectEnemy;
                break;
        }
        indicator.GetComponent<SpriteRenderer>().color = temp;
        indicator.GetComponent<TileSelectionIndicator>().ChangeTarget();
        return indicator;
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
