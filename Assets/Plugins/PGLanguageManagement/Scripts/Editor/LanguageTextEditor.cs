using UnityEditor;

namespace PG.LanguageManagement
{
    [CustomEditor(typeof(LanguageText))]
    public class LanguageTextEditor : Editor
    {
        private LanguageText _languageText;
        private void OnEnable()
        {
            _languageText = (LanguageText)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_languageText.profile != null)
            {
                _languageText.key = EditorGUILayout.Popup("Key", _languageText.key, _languageText.profile.keys.ToArray());
            }
        }
    }
}

