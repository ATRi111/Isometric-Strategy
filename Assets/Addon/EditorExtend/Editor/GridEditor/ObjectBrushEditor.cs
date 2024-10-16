using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class ObjectBrushEditor : AutoEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;

        [AutoProperty]
        public SerializedProperty cellPosition;

        /// <summary>
        /// 是否处于编辑状态；编辑状态下，才会响应各种鼠标事件
        /// </summary>
        protected bool isEditting;

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
        }

        protected override void MyOnInspectorGUI()
        {
            string s = isEditting ? "结束编辑" : "开始编辑";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                focusMode = isEditting ? EFocusMode.Lock : EFocusMode.Default;
                SceneView.RepaintAll();
            }

            if(isEditting)
            {
                EditorGUI.BeginDisabledGroup(true);
                cellPosition.Vector3IntField("网格位置");
                EditorGUI.EndDisabledGroup();
            }
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            if(isEditting)
            {
                Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
                cellPosition.vector3IntValue = ObjectBrush.CalculateCellPosition(world);
            }
        }
    }
}