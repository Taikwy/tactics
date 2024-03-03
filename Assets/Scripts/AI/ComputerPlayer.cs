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
        print("evaluating _________________________________________");
		PlanOfAttack plan = new PlanOfAttack();
		AttackPattern pattern = actingUnit.GetComponentInChildren<AttackPattern>();
        // print("pattern? " + pattern);
		if (pattern){
			pattern.Pick(plan);
			plan.validTargetsLeft = CheckForTargets(plan.target);
			print("valid targets? " + plan.validTargetsLeft);
			if(!plan.validTargetsLeft){
				plan.ability = null;
				// plan.canPerformAbility = false;
			}
			// print("plan selected " + plan + " | " + plan.ability);
		}
		else{
			Debug.LogError("NO ATTACK PATTERN FOUND FOR " + actingUnit);
		}
		
		//handles firetarget and mvoement target given the plan's selected ability
		if(plan.ability!= null){
			print("ability selected " + plan.ability);
			if (IsPositionIndependent(plan))
				PlanPositionIndependent(plan);
			else if (IsDirectionIndependent(plan))
				PlanDirectionIndependent(plan);
		}
		
		//handle movement in the case of NO ABILITY BEING PERFORMED
		if(plan.ability == null){
			print("plan's ability was null, checking movement");
			switch(plan.subMovement){
				case PlanOfAttack.SubMovement.PASS:
					plan.moveLocation = actingUnit.tile.position;
					break;
				case PlanOfAttack.SubMovement.FOE:
					MoveTowardFoe(plan);
					if(!CheckForTargets(Targets.Foe))
						plan.moveLocation = actingUnit.tile.position;
					break;
				case PlanOfAttack.SubMovement.ALLY:
					MoveTowardAlly(plan);
					if(!CheckForTargets(Targets.Ally))
						plan.moveLocation = actingUnit.tile.position;
					break;
				case PlanOfAttack.SubMovement.RANDOM:
					MoveRandom(plan);
					break;
			}
		}

		pattern.UpdatePicker(plan);
		
        print("_________________________________________ finish evaluating");
		return plan;
	}
    //always defaults to getting the basic attack
    // void BasicAttackPattern (PlanOfAttack plan){
    //     print("getting basic attack");
	// 	plan.ability = actingUnit.GetComponentInChildren<Ability>();
	// 	plan.target = Targets.Foe;
	// }

	bool IsPositionIndependent (PlanOfAttack plan){
        // print("is position independent???");
		AbilityRange range = plan.ability.GetComponent<AbilityRange>();
		return range.positionOriented == false;
	}
	
	bool IsDirectionIndependent (PlanOfAttack plan){
        // print("is direction independent???");
		AbilityRange range = plan.ability.GetComponent<AbilityRange>();
		return !range.directionOriented;
	}
	
	void PlanPositionIndependent (PlanOfAttack plan){
        // print("planning position independent");

		//if the ability doesn't require moving, the unit decides not to move
		plan.moveLocation = actingUnit.tile.position;


		// List<Tile> moveOptions = GetMoveOptions();
		// Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
		// plan.moveLocation = plan.fireLocation = tile.position;
	}
	
	void PlanDirectionIndependent (PlanOfAttack plan){
        // print("planning direction independent");
		Tile startTile = actingUnit.tile;
		Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
		AbilityRange ar = plan.ability.GetComponent<AbilityRange>();
		List<Tile> moveOptions = GetMoveOptions();
		moveOptions.Add(actingUnit.tile);
		
        // print("moveoptions " + moveOptions.Count);
		for (int i = 0; i < moveOptions.Count; ++i){
			Tile moveTile = moveOptions[i];
        	// print("movable tile " + moveTile);
			actingUnit.Place( moveTile );
			List<Tile> fireOptions = ar.GetTilesInRange(bc.board);
			
        // print("fireoptions " + fireOptions.Count);
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
					// option.direction = actingUnit.dir;
					RateFireLocation(plan, option);
				}

				option.AddMoveTarget(moveTile);
				// print(i + ":" + j + " | fire tile " + fireTile + " | move tile " + moveTile);
			}
		}
		actingUnit.Place(startTile);
		List<AttackOption> list = new List<AttackOption>(map.Values);
		// print("list size " + list.Count);
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
    
    
	
	//rates all the possibile firing locations GIVEN the attack option's move location
	void RateFireLocation (PlanOfAttack plan, AttackOption option){
        // print("rating firing locations | fire tile " + option.target + " | movetile " + actingUnit.tile);
		AbilityArea area = plan.ability.GetComponent<AbilityArea>();
		List<Tile> tiles = area.GetTilesInArea(bc.board, option.target.position);
		
		option.areaTargets = tiles;
		option.isCasterMatch = IsAbilityTargetMatch(plan, actingUnit.tile);

        // Debug.Log(plan.ability + " | " + actingUnit.tile + " | RATING FIRE LOCATION AND ADDING TILES " + tiles.Count);
        //iterates and checks whether the tile is a valid target. if so, add a mark
		for (int i = 0; i < tiles.Count; ++i){
			Tile tile = tiles[i];
            // print(tile + " | actingunit " +  actingUnit.tile + " | " + plan.ability.IsTarget(tile));
			if (actingUnit.tile == tiles[i] || !plan.ability.IsTarget(tile))
				continue;
            // print("targeting " + tile + " | actingunit " +  actingUnit.tile + " | " + plan.ability.IsTarget(tile));
			
			bool isMatch = IsAbilityTargetMatch(plan, tile);
			// Debug.Log(plan.ability + " is abiliy target matching? " + isMatch);
			option.AddMark(tile, isMatch);
		}
		
        area.targets.Clear();
	}
	
	
	void PickBestOption (PlanOfAttack plan, List<AttackOption> list){
        // print("picking best option");
		int bestScore = 1;
		List<AttackOption> bestOptions = new List<AttackOption>();


		// print("choosing best from list " + list + " | " + list.Count);
		for (int i = 0; i < list.Count; ++i){
			AttackOption option = list[i];
			int score = option.GetScore(actingUnit, plan.ability);
            // print(i + " getting score | score " + score + " | firing target " + option.target + " | move " + option.bestMoveTile);
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
            // print("all options were garbage");
			plan.ability = null; // Clear ability as a sign not to perform it
			return;
		}
		// print("best options chosen " + bestOptions + " |     " + bestOptions.Count + "     | move " + bestOptions[0].bestMoveTile);
		// foreach(AttackOption option in bestOptions){
		// 	print("best options");
		// 	foreach(Tile tile in option.moveTiles){
		// 		print("tile " + tile + "              | " + actingUnit.tile);
		// 	}
		// }

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
		// print("final picks " + finalPicks.Count);
		// foreach(AttackOption ao in finalPicks){
		// 	print("target " + ao.target.position+  " | attack dir  + ao.direction +  | move " + ao.bestMoveTile.position);
		// }
		
		AttackOption choice = finalPicks[ Random.Range(0, finalPicks.Count)  ];
		plan.fireLocation = choice.target.position;
		// plan.attackDirection = choice.direction;
		plan.moveLocation = choice.bestMoveTile.position;

		// print("FINAL PICK target " + plan.fireLocation+  " | move " + plan.moveLocation);
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

	List<Tile> GetMoveOptions (){
        // print("getting move options");
		return actingUnit.GetComponent<Movement>().GetTilesInRange(bc.board);
	}
	void MoveRandom(PlanOfAttack plan){
		List<Tile> moveOptions = GetMoveOptions();
		plan.moveLocation = moveOptions[ Random.Range(0, moveOptions.Count) ].position;
	}
	void MoveTowardFoe (PlanOfAttack plan){
        // print("moving towards opponent");
		List<Tile> moveOptions = GetMoveOptions();
        // print("move options " + moveOptions.Count);
		FindNearestFoe();
        // print("ffound foe - " + nearestFoe);
		if (nearestFoe != null){
			Tile toCheck = nearestFoe.tile;
            // print("ffound foe's tile - " + toCheck);
			while (toCheck != null){
				if (moveOptions.Contains(toCheck)){
					plan.moveLocation = toCheck.position;
					return;
				}
                // print("riiperoni " + toCheck.prev);
				toCheck = toCheck.prev;
			}
		}
        // print("reached the end??");

		plan.moveLocation = actingUnit.tile.position;
	}
    //copied movetowardopponent to move towards an ally instead
    void MoveTowardAlly (PlanOfAttack plan){
		List<Tile> moveOptions = GetMoveOptions();
		FindNearestAlly();
		if (nearestAlly != null){
			Tile toCheck = nearestAlly.tile;
			while (toCheck != null){
				if (moveOptions.Contains(toCheck)){
					plan.moveLocation = toCheck.position;
					return;
				}
				toCheck = toCheck.prev;
			}
		}
		print("no allies found, moving random instead " + nearestAlly);
		plan.moveLocation = actingUnit.tile.position;
	}

	void FindNearestFoe (){
        // print("finding nearest foe");
		nearestFoe = null;
        bool walker = false;
        //checks if acting unit is a walker
        if(actingUnit.movement.GetType() == typeof(WalkMovement))
            walker = true;
        // print("unit is walker??? " + walker);

		bc.board.Search(actingUnit.tile, delegate(Tile arg1, Tile arg2) {
            //makes sure that the tile being checked is walkable FOR WALKERS
            if(walker && (!arg2.isWalkable || !arg1.isWalkable))
                return false;
            //checks that there isn't currently a nearest foe and that the tile we're checking has content
            if (nearestFoe == null && arg2.content != null){
                Alliance other = arg2.content.GetComponentInChildren<Alliance>();
                //checks that the content in the tile is a unit and is a foe
                if (other != null && alliance.IsMatch(other, Targets.Foe)){
                    Unit unit = other.GetComponent<Unit>();
                    Stats stats = unit.GetComponent<Stats>();
                    if (stats[StatTypes.HP] > 0){
                        nearestFoe = unit;
                        return true;
                    }
                }
            }
                

            return nearestFoe == null;
        }
        );
		// print("nearest foe " + nearestFoe + " | " + nearestFoe.tile.position);
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

	protected bool CheckForTargets(Targets target){
		// print("checking for targets " + target);
		switch(target){
			case Targets.None:
				return true;
				break;
			case Targets.Ally:
				for (int i = 0; i < bc.units.Count; ++i){
					Alliance a = bc.units[i].GetComponent<Alliance>();
					if (a == null)
						continue;
					if(a.IsMatch(alliance, target))
						return true;
				}
				break;
			case Targets.Foe:
				for (int i = 0; i < bc.units.Count; ++i){
					Alliance a = bc.units[i].GetComponent<Alliance>();
					if (a == null)
						continue;
					if(!a.IsMatch(alliance, target))
						return true;
				}
				break;
			case Targets.Tile:
				return true;
				break;
		}
		return false;
		
	}
}