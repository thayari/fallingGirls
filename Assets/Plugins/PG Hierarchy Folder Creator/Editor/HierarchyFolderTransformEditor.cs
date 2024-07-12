using UnityEditor;
using UnityEngine;

namespace PG.HierarchyFolder
{
    [CustomEditor(typeof(HierarchyFolderTransform))]
    public class HierarchyFolderTransformEditor : Editor
    {
        private Transform targetTransform;

        void OnEnable()
        {
            targetTransform = ((HierarchyFolderTransform)target).transform;
            LockTargetTransform();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Трансформации этого объекта заблокированы.", MessageType.Info);
        }

        private void LockTargetTransform()
        {
            targetTransform.hideFlags = HideFlags.NotEditable; // Делает трансформ не редактируемым в инспекторе
        }

        void OnSceneGUI()
        {
            LockTargetTransform();
            if (targetTransform.localPosition != Vector3.zero)
            {
                targetTransform.localPosition = Vector3.zero;
            }

            if (targetTransform.localScale != Vector3.one)
            {
                targetTransform.localScale = Vector3.one;
            }

            if (targetTransform.localRotation != Quaternion.identity)
            {
                targetTransform.localRotation = Quaternion.identity;
            }
        }
    }
}