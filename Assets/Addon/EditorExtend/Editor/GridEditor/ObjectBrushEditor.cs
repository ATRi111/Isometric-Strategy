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
        /// �Ƿ��ڱ༭״̬���༭״̬�£��Ż���Ӧ��������¼�
        /// </summary>
        protected bool isEditting;

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
        }

        protected override void MyOnInspectorGUI()
        {
            string s = isEditting ? "�����༭" : "��ʼ�༭";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                focusMode = isEditting ? EFocusMode.Lock : EFocusMode.Default;
                SceneView.RepaintAll();
            }

            if(isEditting)
            {
                EditorGUI.BeginDisabledGroup(true);
                cellPosition.Vector3IntField("����λ��");
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