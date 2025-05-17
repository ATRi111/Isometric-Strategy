using EditorExtend;
using UnityEditor;

//[CustomEditor(typeof(LightManager))]
public class LightManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty radiance_up, radiance_left, radiance_right, projectShadowIntensity;
    private LightManager shadowManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        shadowManager = target as LightManager;
    }

    protected override void MyOnInspectorGUI()
    {
        radiance_up.ColorField("上表面光照颜色");
        radiance_left.ColorField("左表面光照颜色");
        radiance_right.ColorField("右表面光照颜色");
        projectShadowIntensity.Slider("人物阴影强度", 0f, 1f);
    }
}