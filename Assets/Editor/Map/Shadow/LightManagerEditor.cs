using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(LightManager))]
public class LightManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty lightColor, lightDirection;
    private bool foldout;
    private LightManager lightManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        lightManager = target as LightManager;
    }

    protected override void MyOnInspectorGUI()
    {
        lightColor.ColorField("光照颜色");
        lightDirection.Vector3Field("光照方向");
        foldout = EditorGUILayout.Foldout(foldout, "逻辑坐标→光照空间坐标矩阵");
        if (foldout)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < 4; i++)
            {
                EditorGUILayout.Vector4Field(string.Empty, lightManager.lightMatrix.GetRow(i));
            }
            EditorGUI.indentLevel--;
        }
    }
}