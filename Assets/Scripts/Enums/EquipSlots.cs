using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EquipSlots
{
    None = 0,
    Primary = 1 << 0,   // primary default weapon
    Secondary = 1 << 1,  // shield or another weapon, has a higher speed penalty
    Armor = 1 << 2,    // armor, robe, anything
    Accessory = 1 << 3  // ring, crown, whatever
}