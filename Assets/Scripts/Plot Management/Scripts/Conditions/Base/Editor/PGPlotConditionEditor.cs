using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotCondition))]
    public class PGPlotConditionEditor : Editor
    {
        private PGPlotCondition _plotCondition;
        public override void OnInspectorGUI()
        {
            _plotCondition = (PGPlotCondition)target;
            _plotCondition.transitionType = (PGPlotCondition.TransitionType)EditorGUILayout.EnumPopup("Transition Type", _plotCondition.transitionType);
            switch (_plotCondition.transitionType)
            {
                case PGPlotCondition.TransitionType.NextState:
                    break;
                case PGPlotCondition.TransitionType.PreviousState:
                    break;
                case PGPlotCondition.TransitionType.SetState:
                    _plotCondition.stateValue = EditorGUILayout.IntField("Target State", _plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.NextChapter:
                    break;
                case PGPlotCondition.TransitionType.PreviousChapter:
                    break;
                case PGPlotCondition.TransitionType.SetChapter:
                    _plotCondition.chapterValue = EditorGUILayout.IntField("Target Chapter", _plotCondition.chapterValue);
                    break;
                case PGPlotCondition.TransitionType.SetAll:
                    _plotCondition.chapterValue = EditorGUILayout.IntField("Target Chapter", _plotCondition.chapterValue);
                    _plotCondition.stateValue = EditorGUILayout.IntField("Target State", _plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.None:
                    break;
            }
        }
    }
}