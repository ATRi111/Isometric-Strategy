using EditorExtend;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AnimationObject), true)]
public class AnimationObjectEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty audio_activate, audio_recycle, lifeSpan;

    protected List<string> audioNames;
    protected override void OnEnable()
    {
        base.OnEnable();
        audioNames = EditorExtendUtility.FindAssetsNames($"音效 t:GameObject");
    }

    protected override void MyOnInspectorGUI()
    {
        audio_activate.TextFieldWithOptionButton("生成时音效", audioNames);
        audio_recycle.TextFieldWithOptionButton("结束时音效", audioNames);
        lifeSpan.FloatField("生命周期");
    }
}