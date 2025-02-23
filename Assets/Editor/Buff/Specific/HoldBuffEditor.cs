using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(HoldBuff))]
public class HoldBuffEditor : BuffSOEditor
{
    [AutoProperty]
    public SerializedProperty time;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        time.IntField("定身时间");
    }
}