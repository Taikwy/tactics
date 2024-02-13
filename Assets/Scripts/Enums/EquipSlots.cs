using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EquipSlots
{
    None = 0,
    Weapon = 1 << 0,   // default weapon
    Armor = 1 << 1,  // equipment
    Trinket = 1 << 2,    // rings, amulets, gauntlets
}