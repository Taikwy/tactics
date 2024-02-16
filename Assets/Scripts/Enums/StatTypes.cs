using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatTypes
{   
    LV,                 //Level - indicates unit's current level
    XP,                 //Experience - indicates unit's current experience
    HP,                 //Health Points - indicates unit's current health
        MHP,                //Max Health Points - indicates unit's max health
    BP,                 //Energy Points - indicates unit's current energy
        MBP,                //Max Energy Points - indicates unit's max energy
    SK,                //Skill Points - indicates unit's current skill points
        MSK,               //Max Skill Points
    MV,                 //Move - indicates unit's max move range

    AT,                 //Attack
    DF,                 //Defense
    SP,                 //Speed

    CR,                 //Crit Percent - only from weapons
    CD,                 //Crit DMG - only from weapons

    // EN,
    // MEN,
    // SK,
    // LU,
    // AC,

    AV,                 //Action Value - indicates unit's current action value
    TurnCounter,
    Count
}
