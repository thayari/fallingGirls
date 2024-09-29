using UnityEngine;
using UnityEditor;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotAudioBehaviour))]
    public class PGPlotAudioBehaviourEditor : PGPlotBehaviourEditor
    {
        private PGPlotAudioBehaviour _audioBehaviour;
        public override void OnInspectorGUI()
        {
            _audioBehaviour = (PGPlotAudioBehaviour)target;
            GUILayout.Label("Audio Behaviour");
            PGPlotControllerEditorWindow.ObjectsPopup(ref _audioBehaviour.audioObjectIndex, "Audio Source");
            _audioBehaviour.fromClip = EditorGUILayout.Toggle("From Clip", _audioBehaviour.fromClip);
            if (_audioBehaviour.fromClip)
            {
                _audioBehaviour.clip = (AudioClip)EditorGUILayout.ObjectField("Clip", _audioBehaviour.clip, typeof(AudioClip), true);
            }
        }
    }
}