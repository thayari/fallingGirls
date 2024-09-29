using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotWithoutConditions))]
    public class PGPlotWithoutConditionsEditor : PGPlotConditionEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Without Conditions");
            base.OnInspectorGUI();
        }
    }
}