using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityEffect
{
    ATTACK = 0,
    HEAL = 1 << 0,
    BUFF = 1 << 1,
    DEBUFF = 1 << 2
}
