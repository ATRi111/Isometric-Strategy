using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AnimationObject), true)]
public class AnimationObjectEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty audio_activate, audio_recycle, lifeSpan, nomalizedLatency;

    protected override void MyOnInspectorGUI()
    {
        audio_activate.PropertyField("生成时音效");
        audio_recycle.PropertyField("结束时音效");
        lifeSpan.FloatField("生命周期");
        nomalizedLatency.Slider("后续动画衔接时机", 0, 1);
    }
}