using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotRangeBetweenObjects))]
    public class PGPlotRangeBetweenObjectsEditor : PGPlotConditionEditor
    {
        private PGPlotRangeBetweenObjects _rangeBetweenObjects;
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Range between objects Condition");
            _rangeBetweenObjects = target as PGPlotRangeBetweenObjects;
            PGPlotControllerEditorWindow.ObjectsPopup(ref _rangeBetweenObjects.objectIndex);
            PGPlotControllerEditorWindow.ObjectsPopup(ref _rangeBetweenObjects.objectTriggerIndex, "Trigger");
            _rangeBetweenObjects.conditionType = (PGPlotRangeBetweenObjects.ConditionType)EditorGUILayout.EnumPopup("Condition Type", _rangeBetweenObjects.conditionType);
            _rangeBetweenObjects.radius = EditorGUILayout.FloatField("Radius", _rangeBetweenObjects.radius);
            base.OnInspectorGUI();
        }
    }
}