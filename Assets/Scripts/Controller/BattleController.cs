using System.Collections;
using System.Collections.Generic;
// using UnityEditor.EditorTools;
using UnityEngine;

public class BattleController : StateMachine 
{
    [Header("Camera")]
    public CameraRig cameraRig;
    
    [Header("Level and Board")]
    public Board board;
    public TileSelectionIndicator tileSelectionIndicator;
    [HideInInspector]public Point selectPos;
    public LevelData levelData;
    public LevelRecipe levelRecipe;
    
    [Header("Movement")]
    [Tooltip("The delay for PLAYER units to move during the movesequence state")] public float movementDelay;
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
    public CPUActionDelays actionDelays;

    [System.Serializable]public struct CPUActionDelays {
        public float startSelectCommandDelay;
        public float moveSelectDelay;
        public float moveFinishDelay;
        public float actionSelectDelay;
        public float actionFinishDelay;
        public float displayActionDelay;
    }


    

    void Start (){
        tileSelectionIndicator.ChangeSelect();
        guiController.gameObject.SetActive(true);
        ChangeState<InitBattleState>();
    }
}
