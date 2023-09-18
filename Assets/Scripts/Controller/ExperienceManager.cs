using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

public static class ExperienceManager
{
	const float minLevelBonus = 1.5f;
	const float maxLevelBonus = 0.5f;

	//temporary experience method
	public static void GainExperience(int amount, Unit unit){
		UnitLevel levelScript = unit.GetComponent<UnitLevel>();
		levelScript.XP += amount;
	}

	public static void AwardExperience (int amount, Party party)
	{
		//list of all level components in the party
		List<UnitLevel> unitLevels = new List<UnitLevel>(party.Count);
		for (int i = 0; i < party.Count; ++i){
			UnitLevel r = party[i].GetComponent<UnitLevel>();
			if (r != null)
				unitLevels.Add(r);
		}

		// Step 1: determine the range in actor level stats
		int min = int.MaxValue;
		int max = int.MinValue;
		for (int i = unitLevels.Count - 1; i >= 0; --i)
		{
			min = Mathf.Min(unitLevels[i].LV, min);
			max = Mathf.Max(unitLevels[i].LV, max);
		}

		// Step 2: weight the amount to award per actor based on their level
		float[] weights = new float[unitLevels.Count];
		float summedWeights = 0;
		for (int i = unitLevels.Count - 1; i >= 0; --i)
		{
			float percent = (float)(unitLevels[i].LV - min) / (float)(max - min);
			weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);
			summedWeights += weights[i];
		}

		// Step 3: hand out the weighted award
		for (int i = unitLevels.Count - 1; i >= 0; --i)
		{
			int subAmount = Mathf.FloorToInt((weights[i] / summedWeights) * amount);
			unitLevels[i].XP += subAmount;
		}
	}
}