using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuffManager))]
public class BuffManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty buffs, buffResistances;

    protected override void MyOnInspectorGUI()
    {
        if(Application.isPlaying)
        {
            buffs.ListField("ËùÓÐ×´Ì¬");
        }
        buffResistances.ListField("×´Ì¬¿¹ÐÔ");
    }
}