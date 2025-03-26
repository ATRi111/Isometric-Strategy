using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ShadowManager))]
public class ShadowManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty radiance_up, radiance_left, radiance_right, projectShadowIntensity;
    private ShadowManager shadowManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        shadowManager = target as ShadowManager;
    }

    protected override void MyOnInspectorGUI()
    {
        radiance_up.Slider("�ϱ�������", 0f, 1f);
        radiance_left.Slider("���������", 0f, 1f);
        radiance_right.Slider("�ұ�������", 0f, 1f);
        projectShadowIntensity.Slider("������Ӱǿ��", 0f, 1f);
    }
}