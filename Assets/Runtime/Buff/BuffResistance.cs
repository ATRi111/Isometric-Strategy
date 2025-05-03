using UnityEngine;

[System.Serializable]
public class BuffResistance
{
    public BuffSO so;
    [Range(0, 100)]
    public int value;
}
