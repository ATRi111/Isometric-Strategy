using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ModifyPowerBuff))]
public class ModifyPowerBuffEditor : BuffSOEditor
{
    [AutoProperty]
    public SerializedProperty power;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        power.PropertyField("¸½¼ÓÍþÁ¦");
    }
}