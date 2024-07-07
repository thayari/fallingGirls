using UnityEditor;
using UnityEngine;
namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private Vector2 _menuChaptersScrollView;

        void OnChaptersMenu()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.MaxWidth(350));
            _menuChaptersScrollView = GUILayout.BeginScrollView(_menuChaptersScrollView);
            GUILayout.Label("Chapters");
            if (asset.chapters.Count == 0)
            {
                asset.chapters.Add(new PGPlotChapter());
                InitializeLanguages();
                EditorUtility.SetDirty(asset);
            }
            if (GUILayout.Button("Add Chapter"))
            {
                asset.chapters.Add(new PGPlotChapter());
                InitializeLanguages();
                EditorUtility.SetDirty(asset);
            }

            GUILayout.BeginVertical("box");
            for (int i = 0; i < asset.chapters.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                if (i != _currentChapter)
                {
                    GUI.backgroundColor = Color.white;
                }
                if (i == _currentChapter)
                {
                    GUI.backgroundColor = _selectedButton;
                    InitializeLanguages();
                }
                if (GUILayout.Button($"Chapter {i}"))
                {
                    _currentChapter = i;

                    _currentState = 0;
                    InitializeLanguages();
                    EditorUtility.SetDirty(asset);
                }
                if (asset.chapters.Count > 1)
                {
                    if (GUILayout.Button("x", GUILayout.MaxWidth(30)))
                    {
                        _currentChapter = 0;
                        asset.chapters.RemoveAt(i);
                        InitializeLanguages();
                        EditorUtility.SetDirty(asset);
                    }
                }
                if (i > 0)
                {
                    if (GUILayout.Button(Resources.Load("Plot Management/Arrow Up") as Texture, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20)))
                    {
                        SortUp(asset.chapters, i);
                        InitializeLanguages();
                        EditorUtility.SetDirty(asset);
                    }
                }
                if (i < asset.chapters.Count - 1)
                {
                    if (GUILayout.Button(Resources.Load("Plot Management/Arrow Down") as Texture, GUILayout.MaxWidth(30), GUILayout.MaxHeight(20)))
                    {
                        SortDown(asset.chapters, i);
                        InitializeLanguages();
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