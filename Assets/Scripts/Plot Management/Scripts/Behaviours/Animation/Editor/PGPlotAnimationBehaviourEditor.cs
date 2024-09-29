using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotAnimationBehaviour))]
    public class PGPlotAnimationBehaviourEditor : PGPlotBehaviourEditor
    {
        private PGPlotAnimationBehaviour _animationBehaviour;
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Animation Behaviour");
            _animationBehaviour = (PGPlotAnimationBehaviour)target;
            PGPlotControllerEditorWindow.ObjectsPopup(ref _animationBehaviour.animatorObjectIndex, "Animator");
            _animationBehaviour.animatorType = (PGPlotAnimationBehaviour.AnimatorType)EditorGUILayout.EnumPopup("Type", _animationBehaviour.animatorType);
            GUILayout.BeginHorizontal();
            _animationBehaviour.parameterName = EditorGUILayout.TextField("Parameter", _animationBehaviour.parameterName);
            switch (_animationBehaviour.animatorType)
            {
                case PGPlotAnimationBehaviour.AnimatorType.Trigger:
                    break;
                case PGPlotAnimationBehaviour.AnimatorType.Bool:
                    _animationBehaviour.parameterBool = EditorGUILayout.Toggle(_animationBehaviour.parameterBool);
                    break;
                case PGPlotAnimationBehaviour.AnimatorType.Int:
                    _animationBehaviour.parameterInt = EditorGUILayout.IntField(_animationBehaviour.parameterInt);
                    break;
                case PGPlotAnimationBehaviour.AnimatorType.Float:
                    _animationBehaviour.parameterFloat = EditorGUILayout.FloatField(_animationBehaviour.parameterFloat);
                    break;
            }
            GUILayout.EndHorizontal();

        }
    }
}