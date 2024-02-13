using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public const string EquippedEvent = "Equipment.EquippedEvent";
    public const string UnEquippedEvent = "Equipment.UnEquippedEvent";
    public Equippable equippedWeapon, equippedArmor, equippedTrinket;
    public IList<Equippable> equippedItems { get { return _equippedItems.AsReadOnly(); }}
    List<Equippable> _equippedItems = new List<Equippable>();
    public void Equip (Equippable item, EquipSlots slots){
        UnEquip(slots);
        switch(slots){
            case EquipSlots.Weapon:
                equippedWeapon = item;
                break;
            case EquipSlots.Armor:
                equippedArmor = item;
                break;
            case EquipSlots.Trinket:
                equippedTrinket = item;
                break;
            default:
                break;
        }
        _equippedItems.Add(item);
        item.transform.SetParent(transform);
        item.equippedSlot = slots;
        item.OnEquip();
        this.PostEvent(EquippedEvent, item);
    }
    public void UnEquip (Equippable item){
        item.OnUnEquip();
        switch(item.equippedSlot){
            case EquipSlots.Weapon:
                equippedWeapon = null;
                break;
            case EquipSlots.Armor:
                equippedArmor = null;
                break;
            case EquipSlots.Trinket:
                equippedTrinket = null;
                break;
            default:
                break;
        }
        item.equippedSlot = EquipSlots.None;
        item.transform.SetParent(transform);
        _equippedItems.Remove(item);
        this.PostEvent(UnEquippedEvent, item);
    }
    
    public void UnEquip (EquipSlots slots){
        for (int i = _equippedItems.Count - 1; i >= 0; --i){
            Equippable item = _equippedItems[i];
            if ( (item.equippedSlot & slots) != EquipSlots.None )
                UnEquip(item);
        }
    }
}