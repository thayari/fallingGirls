using System.IO;
using UnityEditor;
using UnityEngine;

namespace PG.PlotManagement
{
    [CustomEditor(typeof(PGPlotController))]
    public class PGPlotControllerEditor : Editor
    {
        private PGPlotController plotController;
        private bool _objectsPanel;
        private bool _eventsPanel;
        private void OnEnable()
        {
            plotController = (PGPlotController)target;
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(25);
            if (plotController.plotAsset != null)
            {
                if (GUILayout.Button("Open Asset"))
                {
                    PGPlotControllerEditorWindow.OpenWindow(plotController.plotAsset);
                }
            }
            GUILayout.Space(15);

            if (GUILayout.Button("Create Asset"))
            {
                // Создаем новый экземпляр вашего ассета (в данном случае, ScriptableObject)
                PGPlotAsset asset = ScriptableObject.CreateInstance<PGPlotAsset>();

                // Убедитесь, что путь куда вы сохраняете ассет существует
                string path = "Assets/New Plot Asset.asset";

                // Создаем ассет и сохраняем его в проекте
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                plotController.plotAsset = asset;
                PGPlotControllerEditorWindow.OpenWindow(plotController.plotAsset);
            }
            GUILayout.Space(25);
            if (plotController.plotAsset != null)
            {
                GUILayout.BeginVertical("box");



                EventsPanel();
                GUILayout.Space(15);
                ObjectsPanel();

                GUILayout.EndVertical();
            }
        }
        void ObjectsPanel()
        {
            _objectsPanel = EditorGUILayout.Foldout(_objectsPanel, "Objects");
            if (_objectsPanel)
            {
                if (GUILayout.Button("Add Object"))
                {
                    plotController.objectsElements.Add(new PGPlotController.ObjectElement());
                    EditorUtility.SetDirty(plotController.plotAsset);
                }
                for (int i = 0; i < plotController.objectsElements.Count; i++)
                {
                    GUILayout.BeginVertical("box");
                    plotController.objectsElements[i].indexObject = EditorGUILayout.Popup("Name", plotController.objectsElements[i].indexObject, plotController.plotAsset.Objects.ToArray());
                    plotController.objectsElements[i].gameObject = (GameObject)EditorGUILayout.ObjectField("Object", plotController.objectsElements[i].gameObject, typeof(GameObject), true);
                    if (GUILayout.Button("X"))
                    {
                        plotController.objectsElements.RemoveAt(i);
                        if (plotController.objectsElements.Count > 0)
                        {
                            i--;
                        }
                        EditorUtility.SetDirty(plotController.plotAsset);
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }
                EditorUtility.SetDirty(plotController.plotAsset);
            }
        }
        void EventsPanel()
        {
            _eventsPanel = EditorGUILayout.Foldout(_eventsPanel, "Events");
            if (_eventsPanel)
            {
                if (GUILayout.Button("Add Event"))
                {
                    plotController.eventElements.Add(new PGPlotController.EventElement());
                    EditorUtility.SetDirty(plotController.plotAsset);
                }
                for (int i = 0; i < plotController.eventElements.Count; i++)
                {
                    GUILayout.BeginVertical("box");
                    plotController.eventElements[i].indexEvent = EditorGUILayout.Popup("Name", plotController.eventElements[i].indexEvent, plotController.plotAsset.EventObjects.ToArray());

                    EditorGUILayout.Separator();
                    SerializedProperty eventElementProperty = serializedObject.FindProperty("eventElements").GetArrayElementAtIndex(i);
                    SerializedProperty eventProperty = eventElementProperty.FindPropertyRelative("plotEvent");

                    serializedObject.Update();
                    EditorGUILayout.PropertyField(eventProperty, true);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(plotController.plotAsset);


                    //plotController.eventElements[i].plotEvent = (GameObject)EditorGUILayout.ObjectField("Event", plotController.eventElements[i].plotEvent, typeof(GameObject), true);
                    if (GUILayout.Button("X"))
                    {
                        plotController.eventElements.RemoveAt(i);
                        if (plotController.eventElements.Count > 0)
                        {
                            i--;
                        }
                        EditorUtility.SetDirty(plotController.plotAsset);
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }
            }
        }
    }
}