using UnityEngine;

public class SelectPlayerPanel : MonoBehaviour
{
    private PlayerIcon[] icons;

    private void Awake()
    {
        icons = GetComponentsInChildren<PlayerIcon>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].index = i;
        }
    }
}
