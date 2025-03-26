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
        radiance_up.Slider("上表面亮度", 0f, 1f);
        radiance_left.Slider("左表面亮度", 0f, 1f);
        radiance_right.Slider("右表面亮度", 0f, 1f);
        projectShadowIntensity.Slider("人物阴影强度", 0f, 1f);
    }
}