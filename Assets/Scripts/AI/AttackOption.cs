using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackOption 
{
	class Mark{
		public Tile tile;
		public bool isMatch;
		
		public Mark (Tile tile, bool isMatch)
		{
			this.tile = tile;
			this.isMatch = isMatch;
		}
	}
	public Tile target;
	// public Directions direction;
	public List<Tile> areaTargets = new List<Tile>();
	public bool isCasterMatch;
	public Tile bestMoveTile { get; private set; }
	public int bestAngleBasedScore { get; private set; }
	List<Mark> marks = new List<Mark>();
	List<Tile> moveTargets = new List<Tile>();
	public List<Tile> moveTiles { get{ return moveTargets; }}
	public void AddMoveTarget (Tile tile){
        // Debug.Log("firetile " + target + " adding new move target " + tile);
		// Dont allow moving to a tile that would negatively affect the caster
		if (!isCasterMatch && areaTargets.Contains(tile))
			return;
		moveTargets.Add(tile);
	}

	public void AddMark (Tile tile, bool isMatch){
            // Debug.Log(tile + " adding mark " + isMatch);
		marks.Add (new Mark(tile, isMatch));
	}

	// Scores the option based on how many of the targets are of the desired type
	public int GetScore (Unit caster, Ability ability){
		GetBestMoveTarget(caster, ability);
		if (bestMoveTile == null)
			return 0;

		int score = 0;
		for (int i = 0; i < marks.Count; ++i){
			if (marks[i].isMatch)
				score++;
			else
				score--;
		}

		//if ability is beneficial for allies (Caster) increment score if the mvoetile falls underneath this
		if (isCasterMatch && areaTargets.Contains(bestMoveTile))
			score++;

        // Debug.Log(bestMoveTile + " getting score " + score + " | marks " + marks.Count);
		return score;
	}

	// Returns the tile which is the most effective point for the caster to attack from
	void GetBestMoveTarget (Unit caster, Ability ability){
		if (moveTargets.Count == 0)
			return;
		
		// Debug.Log("getting bedt move target " + moveTargets.Contains(caster.tile));
		if(moveTargets.Contains(caster.tile))
			bestMoveTile = caster.tile;
		else
			bestMoveTile = moveTargets[ Random.Range(0, moveTargets.Count) ];
	}

    //filters moves in case the caster WANTS to get hit by the ability
	void FilterBestMoves (List<Tile> list){
		if (!isCasterMatch)
			return;

		bool canTargetSelf = false;
		for (int i = 0; i < list.Count; ++i){
			if (areaTargets.Contains(list[i])){
				canTargetSelf = true;
				break;
			}
		}

		if (canTargetSelf){
			for (int i = list.Count - 1; i >= 0; --i){
				if (!areaTargets.Contains(list[i]))
					list.RemoveAt(i);
			}
		}
	}

	// int MultiplierForAngle (Unit caster, Tile tile){
	// 	if (tile.content == null)
	// 		return 0;

	// 	Unit defender = tile.content.GetComponentInChildren<Unit>();
	// 	if (defender == null)
	// 		return 0;

	// 	Facings facing = caster.GetFacing(defender);
	// 	if (facing == Facings.Back)
	// 		return 90;
	// 	if (facing == Facings.Side)
	// 		return 75;
	// 	return 50;
	// }
}