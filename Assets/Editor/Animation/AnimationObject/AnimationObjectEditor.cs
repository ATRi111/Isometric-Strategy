using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AnimationObject), true)]
public class AnimationObjectEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty audio_activate, audio_recycle, lifeSpan, nomalizedLatency;

    protected override void MyOnInspectorGUI()
    {
        audio_activate.PropertyField("����ʱ��Ч");
        audio_recycle.PropertyField("����ʱ��Ч");
        lifeSpan.FloatField("��������");
        nomalizedLatency.Slider("���������ν�ʱ��", 0, 1);
    }
}