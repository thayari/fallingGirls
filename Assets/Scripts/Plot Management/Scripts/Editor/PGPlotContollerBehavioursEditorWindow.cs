using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private static Type _baseTypeBehaviour;
        private static IEnumerable<Type> _allTypesBehaviour;
        private static IEnumerable<Type> _derivedTypesBehaviour;
        private static List<Type> _derivedTypesBehaviourList;
        private bool _conditionsCreateBehaviourMenu;
        private Vector2 _behavioursScrollView;
        void OnBehavioursMenu(int chapter, int state)
        {
            PGPlotState plotState = asset.chapters[chapter].states[state];
            GUILayout.BeginVertical("box");
            _conditionsCreateBehaviourMenu = EditorGUILayout.Foldout(_conditionsCreateBehaviourMenu, "Create Behaviour");
            if (_conditionsCreateBehaviourMenu)
            {

                // Выводим имена найденных дочерних классов
                for (int i = 0; i < _derivedTypesBehaviourList.Count; i++)
                {
                    if (GUILayout.Button($"Create {_derivedTypesBehaviourList[i].Name}"))
                    {
                        // Создаем экземпляр объекта соответствующего типа
                        var instance = ScriptableObject.CreateInstance(_derivedTypesBehaviourList[i]);

                        // Добавляем созданный экземпляр в список
                        plotState.plotBehaviours.Add((PGPlotBehaviour)instance);
                        instance.name = instance.GetType().Name;
                        AssetDatabase.AddObjectToAsset(instance, asset);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        GUI.FocusControl(null);
                        _conditionsCreateBehaviourMenu = false;
                    }
                }
            }
            _behavioursScrollView = GUILayout.BeginScrollView(_behavioursScrollView);
            for (int i = 0; i < plotState.plotBehaviours.Count; i++)
            {
                GUILayout.BeginVertical("box");
                if (plotState.plotBehaviours[i] == null)
                {
                    GUILayout.Label("NULL");
                    if (GUILayout.Button("x"))
                    {
                        plotState.plotBehaviours.Clear();
                        EditorUtility.SetDirty(asset);
                        GUI.FocusControl(null);
                    }
                }
                if (plotState.plotBehaviours[i] != null)
                {
                    PGPlotBehaviourEditor myDataEditor = Editor.CreateEditor(plotState.plotBehaviours[i]) as PGPlotBehaviourEditor;
                    myDataEditor.OnInspectorGUI();
                    if (GUILayout.Button("x"))
                    {
                        AssetDatabase.RemoveObjectFromAsset(plotState.plotBehaviours[i]);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        plotState.plotBehaviours.RemoveAt(i);
                        GUI.FocusControl(null);
                        continue;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}