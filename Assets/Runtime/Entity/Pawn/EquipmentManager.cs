using Character;
using MyTool;
using UnityEngine;

public enum EEquipmentSlot
{
    Weapon,
}

public class EquipmentManager : CharacterComponentBase
{
    [SerializeField]
    private SerializedDictionary<EEquipmentSlot, Equipment> equipments;
    private PawnEntity pawn;

    public void Initialize()
    {
        foreach (Equipment equipment in equipments.Values)
        {
            equipment.Register(pawn);
        }
    }

    public void Equip(Equipment equipment)
    {
        if(equipments.ContainsKey(equipment.slot))
        {
            Unequip(equipment.slot);
        }
        equipment.Register(pawn);
        equipments.Add(equipment.slot, equipment);
    }

    public void Unequip(EEquipmentSlot slot)
    {
        if(equipments.ContainsKey(slot))
        {
            equipments[slot].Unregister(pawn);
            equipments.Remove(slot);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = entity as PawnEntity;
    }
}
