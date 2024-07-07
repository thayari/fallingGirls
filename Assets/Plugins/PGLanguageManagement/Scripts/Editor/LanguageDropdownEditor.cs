using UnityEditor;
using UnityEngine;

namespace PG.LanguageManagement
{
    [CustomEditor(typeof(LanguageDropdown))]
    public class LanguageDropdownEditor : Editor
    {

        private LanguageDropdown _languageDropdown;
        private void OnEnable()
        {
            _languageDropdown = (LanguageDropdown)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_languageDropdown.profile != null)
            {
                if (GUILayout.Button("+"))
                {
                    _languageDropdown.keys.Add(0);
                }
                for (int i = 0; i < _languageDropdown.keys.Count; i++)
                {
                    _languageDropdown.keys[i] = EditorGUILayout.Popup($"Key {i}", _languageDropdown.keys[i], _languageDropdown.profile.keys.ToArray());
                    if (GUILayout.Button("X")) 
                    {
                        _languageDropdown.keys.RemoveAt(i);
                        i--;
                    }
                    GUILayout.Space(10);
                }
            }
        }
    }
}
