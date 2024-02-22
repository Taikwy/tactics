using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour 
{
    public string statusName;
    [TextArea(5,20)]public string statusEffectDescription;
}
