using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotDelayTimeCondition))]
    public class PGPlotDelayTimeConditionEditor : PGPlotConditionEditor
    {
        private PGPlotDelayTimeCondition _delayTimeCondition;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label("Delay Time");
            _delayTimeCondition = (PGPlotDelayTimeCondition)target;
            _delayTimeCondition.duration = EditorGUILayout.FloatField("Duration", _delayTimeCondition.duration);
        }
    }
}