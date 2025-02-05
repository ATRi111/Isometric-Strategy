using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    private EquipmentIcon[] icons;

    private void Awake()
    {
        icons = GetComponentsInChildren<EquipmentIcon>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].index = i;
        }
    }
}
