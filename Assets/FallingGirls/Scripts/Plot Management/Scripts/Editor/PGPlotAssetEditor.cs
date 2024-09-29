using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotAsset))]
    public class PGPlotAssetEditor : Editor
    {
        private PGPlotAsset plotGraphAsset;
        private void OnEnable()
        {
            plotGraphAsset = (PGPlotAsset)target;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Asset"))
            {
                PGPlotControllerEditorWindow.OpenWindow(plotGraphAsset);
            }
        }
        [OnOpenAsset]
        //Handles opening the editor window when double-clicking project files
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is PGPlotAsset)
            {
                PGPlotAsset asset = (PGPlotAsset)obj;
                PGPlotControllerEditorWindow.OpenWindow(asset);
                return true;
            }
            return false;
        }
    }
}