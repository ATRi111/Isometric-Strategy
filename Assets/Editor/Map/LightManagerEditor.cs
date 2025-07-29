using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(LightManager))]
public class LightManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty lightColor, lightDirection, texelSize, shadowMap, pawnPositionMap;
    private bool foldout_lightMatrix, foldout_shadowMatrix;
    private LightManager shadowManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        shadowManager = target as LightManager;
    }

    protected override void MyOnInspectorGUI()
    {
        lightColor.ColorField("光照颜色");
        lightDirection.Vector3Field("光照方向");
        texelSize.IntField("阴影贴图尺寸");
        shadowMap.PropertyField("阴影贴图");
        foldout_lightMatrix = EditorGUILayout.Foldout(foldout_lightMatrix, "逻辑坐标→光照空间坐标矩阵");
        if (foldout_lightMatrix)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < 4; i++)
            {
                EditorGUILayout.Vector4Field(string.Empty, shadowManager.lightMatrix.GetRow(i));
            }
            EditorGUI.indentLevel--;
        }
        foldout_shadowMatrix = EditorGUILayout.Foldout(foldout_shadowMatrix, "逻辑坐标→ShadowMap uv矩阵");
        if (foldout_shadowMatrix)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < 4; i++)
            {
                EditorGUILayout.Vector4Field(string.Empty, shadowManager.shadowMatrix.GetRow(i));
            }
            EditorGUI.indentLevel--;
        }
        pawnPositionMap.PropertyField("单位位置纹理");
    }
}