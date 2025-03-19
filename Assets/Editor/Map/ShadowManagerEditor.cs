using EditorExtend;
using UnityEditor;
using UnityEngine;

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
        if (GUILayout.Button("刷新阴影"))
        {
            shadowManager.UpdateAllShadow();
        }
        else if(GUILayout.Button("隐藏阴影"))
        {
            shadowManager.ResetAllShadow();
        }
    }
}