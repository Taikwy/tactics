using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine 
{
    [Header("")]
    public CameraRig cameraRig;
    
    [Header("Level and Board")]
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point selectPos;
    public Tile selectedTile { get { return board.GetTile(selectPos); }}
    [Header("Unit Stuff")]
    public Unit currentUnit;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    [Header("Controller Scripts")]
    public AbilityMenuPanelController abilityMenuPanelController;
    public AbilityPanelController abilityPanelController;
    public StatPanelController statPanelController;
    public PanelController panelController;
    public TurnOrderController turnController;
    public ForecastPanel forecastPanel;
    public IEnumerator round;
    

    void Start ()
    {
        ChangeState<InitBattleState>();
    }
}
