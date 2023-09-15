using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State 
{
    protected BattleController owner;
    public CameraRig cameraRig { get { return owner.cameraRig; }}
    public Board board { get { return owner.board; }}
    public LevelData levelData { get { return owner.levelData; }}
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
    public Point selectPos { get { return owner.selectPos; } set { owner.selectPos = value; }}
    public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; }}
    public AbilityPanelController abilityPanelController { get { return owner.abilityPanelController; }}
    public Turn turn { get { return owner.turn; }}
    public List<Unit> units { get { return owner.units; }}


    public StatPanelController statPanelController { get { return owner.statPanelController; }}
    public PanelController panelController { get { return owner.panelController; }}

    public ForecastPanel forecastPanel { get { return owner.forecastPanel; }}

    protected virtual void Awake (){
        owner = GetComponent<BattleController>();
    }
    protected override void AddListeners (){
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }
    
    protected override void RemoveListeners (){
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }
    protected virtual void OnMove (object sender, InfoEventArgs<Point> e){
    }
    
    protected virtual void OnFire (object sender, InfoEventArgs<int> e){  
    }
    protected virtual void SelectTile (Point p) {
        if (selectPos == p || !board.tiles.ContainsKey(p))
            return;
        selectPos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }

    protected virtual Unit GetUnit (Point p) {
        Tile t = board.GetTile(p);
        GameObject content = t != null ? t.content : null;
        return content != null ? content.GetComponent<Unit>() : null;
    }

    protected virtual void RefreshBasePanel (Point p){
        Unit target = GetUnit(p);
        if (target != null){
            // statPanelController.ShowPrimary(target.gameObject);
            panelController.ShowBase(target.gameObject);
        }
        else{
            // statPanelController.HidePrimary();
            panelController.HideBase();
        }
    }
    protected virtual void RefreshSecondaryBasePanel (Point p){
        Unit target = GetUnit(p);
        Debug.Log(target);
        if (target != null)
            statPanelController.ShowSecondary(target.gameObject);
        else
            statPanelController.HideSecondary();
    }
}
