using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine 
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public GameObject playerPrefab;
    public Unit currentUnit;
    public Tile selectedTile { get { return board.GetTile(pos); }}

    public AbilityMenuPanelController abilityMenuPanelController;
    public AbilityPanelController abilityPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    public StatPanelController statPanelController;
    public PanelController panelController;
    public IEnumerator round;
    
    public HitSuccessIndicator hitSuccessIndicator;

    void Start ()
    {
        ChangeState<InitBattleState>();
    }
}
