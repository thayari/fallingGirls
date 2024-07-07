using UnityEditor;
using UnityEngine.SceneManagement;
namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotLoadSceneBehaviour))]
    public class PGPlotLoadSceneBehaviourEditor : PGPlotBehaviourEditor
    {
        private PGPlotLoadSceneBehaviour _loadSceneBehaviour;
        public override void OnInspectorGUI()
        {
            _loadSceneBehaviour = (PGPlotLoadSceneBehaviour)target;
            _loadSceneBehaviour.sceneName = EditorGUILayout.TextField("Scene Name", _loadSceneBehaviour.sceneName);
            _loadSceneBehaviour.conditionIndex = EditorGUILayout.IntField("Condition Index", _loadSceneBehaviour.conditionIndex);
            _loadSceneBehaviour.sceneMode = (LoadSceneMode)EditorGUILayout.EnumPopup("Type loading", _loadSceneBehaviour.sceneMode);
            _loadSceneBehaviour.unloadScene = EditorGUILayout.Toggle("Unload scene", _loadSceneBehaviour.unloadScene);
            if (_loadSceneBehaviour.unloadScene)
            {
                _loadSceneBehaviour.unloadSceneName = EditorGUILayout.TextField("Scene Name", _loadSceneBehaviour.unloadSceneName);
            }
        }
    }
}
