using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine 
{
    [Header("")]
    public CameraRig cameraRig;
    
    [Header("Level and Board")]
    public Board board;
    public TileSelectionIndicator tileSelectionIndicator;
    [HideInInspector]public Point selectPos;
    
    public LevelData levelData;
    public LevelRecipe levelRecipe;
    public Tile selectedTile { get { return board.GetTile(selectPos); }}
    // [Header("Unit Stuff")]
    [HideInInspector]public Unit currentUnit;
    public Turn turn = new Turn();
    [HideInInspector]public List<Unit> units = new List<Unit>();

    [Header("Controller Scripts")]
    // public AbilityMenuPanelController abilityMenuPanelController;
    public AbilityPanelController abilityPanelController;
    public PanelController panelController;
    public ForecastPanel forecastPanel;
    public TurnOrderController turnOrderController;
    public GameUIController guiController;
    [Header("turn stuff")]
    public IEnumerator round;
    public Timeline timeline;
    public ComputerPlayer cpu;

    

    void Start (){
        tileSelectionIndicator.ChangeSelect();
        guiController.gameObject.SetActive(true);
        ChangeState<InitBattleState>();
    }
}
