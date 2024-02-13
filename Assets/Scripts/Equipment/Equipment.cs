using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public const string EquippedEvent = "Equipment.EquippedEvent";
    public const string UnEquippedEvent = "Equipment.UnEquippedEvent";
    public IList<Equippable> equippedItems { get { return _equippedItems.AsReadOnly(); }}
    List<Equippable> _equippedItems = new List<Equippable>();
    public void Equip (Equippable item, EquipSlots slots){
        UnEquip(slots);
        _equippedItems.Add(item);
        item.transform.SetParent(transform);
        item.slots = slots;
        item.OnEquip();
        this.PostEvent(EquippedEvent, item);
    }
    public void UnEquip (Equippable item){
        item.OnUnEquip();
        item.slots = EquipSlots.None;
        item.transform.SetParent(transform);
        _equippedItems.Remove(item);
        this.PostEvent(UnEquippedEvent, item);
    }
    
    public void UnEquip (EquipSlots slots){
        for (int i = _equippedItems.Count - 1; i >= 0; --i){
            Equippable item = _equippedItems[i];
            if ( (item.slots & slots) != EquipSlots.None )
                UnEquip(item);
        }
    }
}