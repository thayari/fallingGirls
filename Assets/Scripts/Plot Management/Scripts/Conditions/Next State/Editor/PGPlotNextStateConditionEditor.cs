using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotNextStateCondition))]
    public class PGPlotNextStateConditionEditor : PGPlotConditionEditor
    {
        private PGPlotNextStateCondition _nextStateCondition;
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Next State");
            _nextStateCondition = (PGPlotNextStateCondition)target;
            base.OnInspectorGUI();
        }
    }
}