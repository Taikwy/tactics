using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAbilityPower : BaseAbilityPower 
{
  public int level;
  
  protected override int GetBaseAttack ()
  {
    return GetComponentInParent<Stats>()[StatTypes.AT];
  }
  protected override int GetBaseDefense (Unit target)
  {
    return target.GetComponent<Stats>()[StatTypes.DF];
  }
  
  protected override int GetPower ()
  {
    return level;
  }
}