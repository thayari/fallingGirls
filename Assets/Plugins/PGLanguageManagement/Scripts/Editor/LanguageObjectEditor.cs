using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PG.LanguageManagement
{
    [CustomEditor(typeof(LanguageObject))]
    public class LanguageObjectEditor : Editor
    {
        private LanguageObject _languageObject;
        private List<string> _languages = new List<string>();
        private void OnEnable()
        {
            _languageObject = (LanguageObject)target;

            _languages.Clear();
            if (_languageObject.profile != null)
            {
                for (int i = 0; i < _languageObject.profile.languageElements.Count; i++)
                {
                    _languages.Add(_languageObject.profile.languageElements[i].language);
                }
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_languageObject.profile != null)
            {
                if (GUILayout.Button("+"))
                {
                    _languageObject.languages.Add(new LanguageObject.Language());
                }
                for (int i = 0; i < _languageObject.languages.Count; i++)
                {
                    GUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"Index: {i}");
                    _languageObject.languages[i].languageIndex = EditorGUILayout.Popup("Language", _languageObject.languages[i].languageIndex, _languages.ToArray());
                    _languageObject.languages[i].description = (GameObject)EditorGUILayout.ObjectField("Object", _languageObject.languages[i].description, typeof(GameObject), true);
                    if (GUILayout.Button("X"))
                    {
                        _languageObject.languages.RemoveAt(i);
                        i--;
                    }
                    GUILayout.EndVertical();
                }
            }
        }
    }
}