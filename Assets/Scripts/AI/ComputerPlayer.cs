using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerPlayer : MonoBehaviour 
{
	BattleController bc;
	Unit actingUnit { get { return bc.turn.actingUnit; }}
	Alliance alliance { get { return actingUnit.GetComponent<Alliance>(); }}
	Unit nearestFoe, nearestAlly;
	void Awake ()
	{
		bc = GetComponent<BattleController>();
	}

	public PlanOfAttack Evaluate (){
		PlanOfAttack plan = new PlanOfAttack();
		AttackPattern pattern = actingUnit.GetComponentInChildren<AttackPattern>();
		if (pattern)
			pattern.Pick(plan);
		else
			DefaultAttackPattern(plan);
		
		if (IsPositionIndependent(plan))
			PlanPositionIndependent(plan);
		else if (IsDirectionIndependent(plan))
			PlanDirectionIndependent(plan);
		// else
		// 	PlanDirectionDependent(plan);

		if (plan.ability == null)
			MoveTowardOpponent(plan);
		
		return plan;
	}
	void DefaultAttackPattern (PlanOfAttack plan){
		// Just get the first "Attack" ability   THIS NEEDS TO CHANGE TO BASIC ATTACK
		plan.ability = actingUnit.GetComponentInChildren<Ability>();
		plan.target = Targets.Foe;
	}

	bool IsPositionIndependent (PlanOfAttack plan){
		AbilityRange range = plan.ability.GetComponent<AbilityRange>();
		return range.positionOriented == false;
	}
	
	bool IsDirectionIndependent (PlanOfAttack plan){
		AbilityRange range = plan.ability.GetComponent<AbilityRange>();
		return !range.directionOriented;
	}
	
	void PlanPositionIndependent (PlanOfAttack plan){
		List<Tile> moveOptions = GetMoveOptions();
		Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
		plan.moveLocation = plan.fireLocation = tile.position;
	}
	
	void PlanDirectionIndependent (PlanOfAttack plan){
		Tile startTile = actingUnit.tile;
		Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
		AbilityRange ar = plan.ability.GetComponent<AbilityRange>();
		List<Tile> moveOptions = GetMoveOptions();
		
		for (int i = 0; i < moveOptions.Count; ++i){
			Tile moveTile = moveOptions[i];
			actingUnit.Place( moveTile );
			List<Tile> fireOptions = ar.GetTilesInRange(bc.board);
			
			for (int j = 0; j < fireOptions.Count; ++j){
				Tile fireTile = fireOptions[j];
				AttackOption option = null;
				if (map.ContainsKey(fireTile)){
					option = map[fireTile];
				}
				else{
					option = new AttackOption();
					map[fireTile] = option;
					option.target = fireTile;
					option.direction = actingUnit.dir;
					RateFireLocation(plan, option);
				}

				option.AddMoveTarget(moveTile);
			}
		}
		
		actingUnit.Place(startTile);
		List<AttackOption> list = new List<AttackOption>(map.Values);
		PickBestOption(plan, list);
	}
	
	// void PlanDirectionDependent (PlanOfAttack plan){
	// 	Tile startTile = actingUnit.tile;
	// 	Directions startDirection = actingUnit.dir;
	// 	List<AttackOption> list = new List<AttackOption>();
	// 	List<Tile> moveOptions = GetMoveOptions();
		
	// 	for (int i = 0; i < moveOptions.Count; ++i){
	// 		Tile moveTile = moveOptions[i];
	// 		actingUnit.Place( moveTile );
			
	// 		for (int j = 0; j < 4; ++j){
	// 			actingUnit.dir = (Directions)j;
	// 			AttackOption ao = new AttackOption();
	// 			ao.target = moveTile;
	// 			ao.direction = actingUnit.dir;
	// 			RateFireLocation(plan, ao);
	// 			ao.AddMoveTarget(moveTile);
	// 			list.Add(ao);
	// 		}
	// 	}
		
	// 	actingUnit.Place(startTile);
	// 	actingUnit.dir = startDirection;
	// 	PickBestOption(plan, list);
	// }

	
	List<Tile> GetMoveOptions (){
		return actingUnit.GetComponent<Movement>().GetTilesInRange(bc.board);
	}
    
    //simply checks whether the tile being targeted is a match
	bool IsAbilityTargetMatch (PlanOfAttack plan, Tile tile){
		bool isMatch = false;
		if (plan.target == Targets.Tile)
			isMatch = true;
		else if (plan.target != Targets.None){
			Alliance other = tile.content.GetComponentInChildren<Alliance>();
			if (other != null && alliance.IsMatch(other, plan.target))
				isMatch = true;
		}

		return isMatch;
	}
	
	void RateFireLocation (PlanOfAttack plan, AttackOption option){
		AbilityArea area = plan.ability.GetComponent<AbilityArea>();
		List<Tile> tiles = area.GetTilesInArea(bc.board, option.target.pos);
		option.areaTargets = tiles;
		option.isCasterMatch = IsAbilityTargetMatch(plan, actingUnit.tile);

        //iterates and checks whether the tile is a valid target. if so, add a mark
		for (int i = 0; i < tiles.Count; ++i){
			Tile tile = tiles[i];
			if (actingUnit.tile == tiles[i] || !plan.ability.IsTarget(tile))
				continue;
			
			bool isMatch = IsAbilityTargetMatch(plan, tile);
			option.AddMark(tile, isMatch);
		}
	}
	
	void PickBestOption (PlanOfAttack plan, List<AttackOption> list){
		int bestScore = 1;
		List<AttackOption> bestOptions = new List<AttackOption>();


		for (int i = 0; i < list.Count; ++i){
			AttackOption option = list[i];
			int score = option.GetScore(actingUnit, plan.ability);
			if (score > bestScore){
				bestScore = score;
				bestOptions.Clear();
				bestOptions.Add(option);
			}
			else if (score == bestScore){
				bestOptions.Add(option);
			}
		}

        //if all of the options were ass and detrimental, don't perform any ability
		if (bestOptions.Count == 0){
			plan.ability = null; // Clear ability as a sign not to perform it
			return;
		}

        //THIS SECTION I WILL HAVE TO UPDATE THE MOST
		List<AttackOption> finalPicks = new List<AttackOption>();
		bestScore = 0;
		for (int i = 0; i < bestOptions.Count; ++i){
			AttackOption option = bestOptions[i];
			int score = option.bestAngleBasedScore;
			if (score > bestScore){
				bestScore = score;
				finalPicks.Clear();
				finalPicks.Add(option);
			}
			else if (score == bestScore){
				finalPicks.Add(option);
			}
		}
		
		AttackOption choice = finalPicks[ UnityEngine.Random.Range(0, finalPicks.Count)  ];
		plan.fireLocation = choice.target.pos;
		plan.attackDirection = choice.direction;
		plan.moveLocation = choice.bestMoveTile.pos;
	}

	void FindNearestFoe (){
		nearestFoe = null;
		bc.board.Search(actingUnit.tile, delegate(Tile arg1, Tile arg2) {
			if (nearestFoe == null && arg2.content != null){
				Alliance other = arg2.content.GetComponentInChildren<Alliance>();
				if (other != null && alliance.IsMatch(other, Targets.Foe))
				{
					Unit unit = other.GetComponent<Unit>();
					Stats stats = unit.GetComponent<Stats>();
					if (stats[StatTypes.HP] > 0){
						nearestFoe = unit;
						return true;
					}
				}
			}
			return nearestFoe == null;
		});
	}
    //just copied the findnearestfoe code to find the nearest ally instead
    void FindNearestAlly (){
		nearestAlly = null;
		bc.board.Search(actingUnit.tile, delegate(Tile arg1, Tile arg2) {
			if (nearestAlly == null && arg2.content != null){
				Alliance other = arg2.content.GetComponentInChildren<Alliance>();
				if (other != null && alliance.IsMatch(other, Targets.Ally)){
					Unit unit = other.GetComponent<Unit>();
					Stats stats = unit.GetComponent<Stats>();
					if (stats[StatTypes.HP] > 0){
						nearestAlly = unit;
						return true;
					}
				}
			}
			return nearestAlly == null;
		});
	}

	void MoveTowardOpponent (PlanOfAttack poa){
		List<Tile> moveOptions = GetMoveOptions();
		FindNearestFoe();
		if (nearestFoe != null){
			Tile toCheck = nearestFoe.tile;
			while (toCheck != null){
				if (moveOptions.Contains(toCheck)){
					poa.moveLocation = toCheck.position;
					return;
				}
				toCheck = toCheck.prev;
			}
		}

		poa.moveLocation = actingUnit.tile.position;
	}
    //copied movetowardopponent to move towards an ally instead
    void MoveTowardAlly (PlanOfAttack poa){
		List<Tile> moveOptions = GetMoveOptions();
		FindNearestFoe();
		if (nearestAlly != null){
			Tile toCheck = nearestAlly.tile;
			while (toCheck != null){
				if (moveOptions.Contains(toCheck)){
					poa.moveLocation = toCheck.position;
					return;
				}
				toCheck = toCheck.prev;
			}
		}

		poa.moveLocation = actingUnit.tile.position;
	}

	// public Directions DetermineEndFacingDirection (){
	// 	Directions dir = (Directions)UnityEngine.Random.Range(0, 4);
	// 	FindNearestFoe();
	// 	if (nearestFoe != null){
	// 		Directions start = actor.dir;
	// 		for (int i = 0; i < 4; ++i){
	// 			actor.dir = (Directions)i;
	// 			if (nearestFoe.GetFacing(actor) == Facings.Front){
	// 				dir = actor.dir;
	// 				break;
	// 			}
	// 		}
	// 		actor.dir = start;
	// 	}
	// 	return dir;
	// }
}