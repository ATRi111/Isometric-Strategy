using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "�����ֵ�", menuName = "����/�����ֵ�", order = -100)]
public class PawnPropertyDict : ScriptableObject
{
    [SerializeField]
    private List<FindPawnPropertySO> properties;
    private Dictionary<string, FindPawnPropertySO> propertyDict;

    public FindPawnPropertySO this[string propertyName] => propertyDict[propertyName];

    public void Initialize()
    {
        propertyDict = new();
        for (int i = 0; i < properties.Count; i++)
        {
            propertyDict.Add(properties[i].name, properties[i]);
        }
    }
}
