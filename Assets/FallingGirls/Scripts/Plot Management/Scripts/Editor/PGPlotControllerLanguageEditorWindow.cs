using UnityEditor;
using UnityEngine;

namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private bool _languageLeftMenuActive = true;
        private Vector2 _menuLanguagesScrollView;
        private Vector2 _menuLanguageScrollView;

        private string _newLanguageName;

        void OnLanguagesMenu()
        {
            if (_languageLeftMenuActive)
            {
                GUILayout.BeginVertical("box", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(_maxWidthLeftMenu));
                GUILayout.Label("Languages");
                _newLanguageName = GUILayout.TextField(_newLanguageName);
                if (GUILayout.Button("Create"))
                {
                    CreateLanguage(ref _newLanguageName);
                    InitializeLanguages();
                }

                _menuLanguagesScrollView = GUILayout.BeginScrollView(_menuLanguagesScrollView, GUILayout.ExpandWidth(false));
                foreach (var languageName in asset.Languages)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(languageName);
                    if (GUILayout.Button("Remove"))
                    {
                        asset.Languages.Remove(languageName);
                        InitializeLanguages();
                        EditorUtility.SetDirty(asset);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

                GUILayout.EndVertical();
            }
        }
        void CreateLanguage(ref string value)
        {
            if (!string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value))
            {
                asset.Languages.Add(value);
                value = "";
                GUI.FocusControl(null);
                EditorUtility.SetDirty(asset);
            }
        }

    }
}