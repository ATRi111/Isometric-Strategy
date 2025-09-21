using Services.Save;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuidanceSaveData : SaveData
{
    private GuidancePanel panel;
    public List<int> checkedGuidance;

    public override void Load()
    {
        if (checkedGuidance != null)
        {
            for (int i = 0; i < checkedGuidance.Count; i++)
            {
                panel.checkedGuidance.Add((EGuidance)checkedGuidance[i]);
            }
        }
    }

    public override void Save()
    {
        checkedGuidance = new();
        foreach (EGuidance guidance in panel.checkedGuidance)
        {
            checkedGuidance.Add((int)guidance);
        }
    }

    public override void Initialize(string identifier, Object obj)
    {
        base.Initialize(identifier, obj);
        panel = (obj as GameObject).GetComponent<GuidancePanel>();
    }
}
