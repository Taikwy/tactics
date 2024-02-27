using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerPlayer : MonoBehaviour 
{
    BattleController bc;
    Unit actor { get { return bc.turn.actingUnit; }}
    void Awake ()
    {
        bc = GetComponent<BattleController>();
    }
    public PlanOfAttack Evaluate (){
        // Create and fill out a plan of attack
        PlanOfAttack plan = new PlanOfAttack();
        // Step 1: Decide what ability to use
        AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
        if (pattern)
            pattern.Pick(plan);
        else
        DefaultAttackPattern(plan);
        // Step 2: Determine where to move and aim to best use the ability
        PlaceholderCode(plan);
        // Return the completed plan
        return plan;
    }

    //returns the basic ability
    void DefaultAttackPattern (PlanOfAttack poa){
        poa.ability = actor.GetComponentInChildren<AbilityCatalog>().basicAbility.GetComponent<Ability>();
        // poa.ability = actor.GetComponentInChildren<Ability>();
        poa.target = Targets.Foe;
    }
    void PlaceholderCode (PlanOfAttack poa){
        // Move to a random location within the unit's move range
        List<Tile> tiles = actor.GetComponent<Movement>().GetTilesInRange(bc.board);
        Tile randomTile = (tiles.Count > 0) ? tiles[ Random.Range(0, tiles.Count) ] : null;
        poa.moveLocation = (randomTile != null) ? randomTile.position : actor.tile.position;
        // Pick a random attack direction (for direction based abilities)
        poa.attackDirection = (Directions)Random.Range(0, 4);
        // Pick a random fire location based on having moved to the random tile
        Tile start = actor.tile;
        actor.Place(randomTile);
        tiles = poa.ability.GetComponent<AbilityRange>().GetTilesInRange(bc.board);
        if (tiles.Count == 0)
        {
        poa.ability = null;
        poa.fireLocation = poa.moveLocation;
        }
        else
        {
        randomTile = tiles[ Random.Range(0, tiles.Count) ];
        poa.fireLocation = randomTile.position;
        }
        actor.Place(start);
    }
    //   public Directions DetermineEndFacingDirection ()
    //   {
    //         return (Directions)Random.Range(0, 4);
    //   }
}