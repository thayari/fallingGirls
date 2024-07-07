using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace PG.LanguageManagement
{
    [CustomEditor(typeof(PGLanguageProfile))]
    public class PGLanguageProfileEditor : Editor
    {
        private PGLanguageProfile _profile;
        private void OnEnable()
        {
            _profile = (PGLanguageProfile)target;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open profile"))
            {
                PGLanguageProfileEditorWindow.OpenWindow(_profile);
            }
        }
        [OnOpenAsset]
        //Handles opening the editor window when double-clicking project files
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is PGLanguageProfile)
            {
                PGLanguageProfile asset = (PGLanguageProfile)obj;
                PGLanguageProfileEditorWindow.OpenWindow(asset);
                return true;
            }
            return false;
        }
    }
}
