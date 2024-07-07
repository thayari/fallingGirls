using UnityEditor;
using UnityEngine;
namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {

        private Vector2 _menuStatesScrollView;
        void OnStatesMenu(int chapter)
        {
            PGPlotChapter plotChapter = asset.chapters[chapter];
            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.MaxWidth(300));
            _menuStatesScrollView = GUILayout.BeginScrollView(_menuStatesScrollView);
            GUILayout.Label("States");
            if (plotChapter.states.Count == 0)
            {
                plotChapter.states.Add(new PGPlotState());
                EditorUtility.SetDirty(asset);
            }
            if (GUILayout.Button("Add State"))
            {
                plotChapter.states.Add(new PGPlotState());
                EditorUtility.SetDirty(asset);
            }

            GUILayout.BeginVertical("box");
            for (int i = 0; i < plotChapter.states.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                if (i != _currentState)
                {
                    GUI.backgroundColor = Color.white;
                }
                if (i == _currentState)
                {
                    GUI.backgroundColor = _selectedButton;
                }
                if (plotChapter.states[i].languageStates.Count > 0)
                {
                    if (GUILayout.Button($"{plotChapter.states[i].languageStates[_languageIndex].name} State {i}"))
                    {
                        _currentState = i;
                        InitializeLanguages();
                        GUI.FocusControl(null);
                        EditorUtility.SetDirty(asset);
                    }
                }
                else
                {
                    if (GUILayout.Button($"State {i}"))
                    {
                        _currentState = i;
                        InitializeLanguages();
                        GUI.FocusControl(null);
                        EditorUtility.SetDirty(asset);
                    }
                }
                if (i > 0)
                {
                    if (GUILayout.Button(Resources.Load("Plot Management/Arrow Up") as Texture, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20)))
                    {
                        SortUp(plotChapter.states, i);
                        GUI.FocusControl(null);
                    }
                    if (GUILayout.Button("x", GUILayout.MaxWidth(30)))
                    {
                        plotChapter.states.RemoveAt(i);
                        GUI.FocusControl(null);
                        _currentState = 0;
                        EditorUtility.SetDirty(asset);
                    }
                }
                if (i < plotChapter.states.Count - 1)
                {
                    if (GUILayout.Button(Resources.Load("Plot Management/Arrow Down") as Texture, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20)))
                    {
                        SortDown(plotChapter.states, i);
                        EditorUtility.SetDirty(asset);
                    }
                }
                GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

        }
    }
}