using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private static Type _baseTypeCondition;
        private static IEnumerable<Type> _allTypesCondition;
        private static IEnumerable<Type> _derivedTypesCondition;
        private static List<Type> _derivedTypesConditionList;
        private bool _conditionsCreateStateMenu;
        private Vector2 _conditionsScrollView;
        void OnConditionsMenu(int chapter, int state)
        {
            PGPlotState plotState = asset.chapters[chapter].states[state];
            if (plotState.plotConditions.Count == 0)
            {
                PGPlotNextStateCondition stateCondition = ScriptableObject.CreateInstance<PGPlotNextStateCondition>();
                plotState.plotConditions.Add(stateCondition);
                stateCondition.name = stateCondition.GetType().Name;
                AssetDatabase.AddObjectToAsset(stateCondition, asset);
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                GUI.FocusControl(null);
            }



            GUILayout.BeginVertical("box");
            _conditionsCreateStateMenu = EditorGUILayout.Foldout(_conditionsCreateStateMenu, "Create condition");
            if (_conditionsCreateStateMenu)
            {

                for (int i = 0; i < _derivedTypesConditionList.Count; i++)
                {
                    if (GUILayout.Button($"Create {_derivedTypesConditionList[i].Name}"))
                    {
                        // Создаем экземпляр объекта соответствующего типа
                        var instance = ScriptableObject.CreateInstance(_derivedTypesConditionList[i]);

                        // Добавляем созданный экземпляр в список
                        plotState.plotConditions.Add((PGPlotCondition)instance);
                        instance.name = instance.GetType().Name;
                        AssetDatabase.AddObjectToAsset(instance, asset);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        GUI.FocusControl(null);
                        _conditionsCreateStateMenu = false;
                    }
                }
            }
            _conditionsScrollView = GUILayout.BeginScrollView(_conditionsScrollView);
            for (int i = 0; i < plotState.plotConditions.Count; i++)
            {
                GUILayout.BeginVertical("box");
                if (plotState.plotConditions[i] == null)
                {
                    GUILayout.Label("NULL");
                    if (GUILayout.Button("x"))
                    {
                        plotState.plotConditions.RemoveAt(i);
                        PGPlotNextStateCondition stateCondition = ScriptableObject.CreateInstance<PGPlotNextStateCondition>();
                        stateCondition.name = stateCondition.GetType().Name;
                        AssetDatabase.AddObjectToAsset(stateCondition, asset);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        plotState.plotConditions.Add(stateCondition);
                        GUI.FocusControl(null);
                        continue;
                    }
                }
                else
                {
                    GUILayout.Label($"Index: {i}");
                    PGPlotConditionEditor myDataEditor = Editor.CreateEditor(plotState.plotConditions[i]) as PGPlotConditionEditor;
                    myDataEditor.OnInspectorGUI();
                    if (GUILayout.Button("x"))
                    {
                        PGPlotCondition pGPlotCondition = plotState.plotConditions[i];
                        AssetDatabase.RemoveObjectFromAsset(pGPlotCondition);
                        plotState.plotConditions.Remove(pGPlotCondition);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
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