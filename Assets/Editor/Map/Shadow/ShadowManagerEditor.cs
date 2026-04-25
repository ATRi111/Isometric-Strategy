using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ShadowManager))]
public class ShadowManagerEditor : AutoEditor
{
    private bool shadowMatrix;
    private ShadowManager shadowManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        shadowManager = target as ShadowManager;
    }

    protected override void MyOnInspectorGUI()
    {
        shadowMatrix = EditorGUILayout.Foldout(shadowMatrix, "逻辑坐标→ShadowMap uv矩阵");
        if (shadowMatrix)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < 4; i++)
            {
                EditorGUILayout.Vector4Field(string.Empty, shadowManager.shadowMatrix.GetRow(i));
            }
            EditorGUI.indentLevel--;
        }
    }
}