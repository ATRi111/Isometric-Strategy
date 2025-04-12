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
        audioNames = EditorExtendUtility.FindAssetsNames($"��Ч t:GameObject");
    }

    protected override void MyOnInspectorGUI()
    {
        audio_activate.TextFieldWithOptionButton("����ʱ��Ч", audioNames);
        audio_recycle.TextFieldWithOptionButton("����ʱ��Ч", audioNames);
        lifeSpan.FloatField("��������");
    }
}