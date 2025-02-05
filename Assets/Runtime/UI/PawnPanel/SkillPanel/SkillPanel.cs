using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    private SkillIconToDisplay[] icons;

    private void Awake()
    {
        icons = GetComponentsInChildren<SkillIconToDisplay>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].index = i;
        }
    }
}
