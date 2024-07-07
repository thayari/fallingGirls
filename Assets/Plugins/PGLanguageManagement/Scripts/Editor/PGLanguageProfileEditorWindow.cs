using UnityEditor;
using UnityEngine;
namespace PG.LanguageManagement
{
    public class PGLanguageProfileEditorWindow : EditorWindow
    {
        private static PGLanguageProfile _profile;
        private static SerializedObject _serializedObject;
        private Vector2 _scrollView;
        private static GUIStyle _headerStyle;
        private enum TypeElement
        {
            Text,
            Audio
        }
        private TypeElement _type;
        public static void OpenWindow(PGLanguageProfile profile)
        {
            _profile = profile;
            var window = GetWindow<PGLanguageProfileEditorWindow>(profile.name);
            window.titleContent.image = (Texture)Resources.Load("PGLanguageManagement/Icon");
            Initialize();
        }
        private void OnEnable()
        {
            Initialize();
        }
        private static void Initialize()
        {
            if (_profile != null)
            {
                _serializedObject = new SerializedObject(_profile);
            }
            _headerStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };
            _headerStyle.normal.textColor = Color.white;
        }
        private void OnGUI()
        {
            if (_profile == null)
            {
                _profile = (PGLanguageProfile)EditorGUILayout.ObjectField("Profile", _profile, typeof(PGLanguageProfile), false);
            }
            else
            {
                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("Save", GUILayout.MaxWidth(75)))
                {
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                _type = (TypeElement)EditorGUILayout.EnumPopup("Element type", _type);
                GUILayout.EndHorizontal();
                _scrollView = GUILayout.BeginScrollView(_scrollView);
                GUILayout.BeginHorizontal("box");
                OnKeysPanel();

                LanguagePanel();
                GUILayout.EndHorizontal();

                GUILayout.EndScrollView();
                EditorUtility.SetDirty(_profile);
            }
        }

        private void LanguagePanel()
        {
            GUILayout.BeginHorizontal("box");
            for (int i = 0; i < _profile.languageElements.Count; i++)
            {
                OnLanguageElementPanel(i);
            }
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 24;
            buttonStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("+", buttonStyle, GUILayout.MinWidth(50), GUILayout.MinHeight(50), GUILayout.MaxWidth(50)))
            {
                _profile.languageElements.Add(new PGLanguageProfile.LanguageElement());
            }
            GUILayout.EndHorizontal();
        }

        private void OnLanguageElementPanel(int i)
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(false), GUILayout.MinWidth(250));

            if (GUILayout.Button("X"))
            {
                _profile.languageElements.RemoveAt(i);
                i--;
            }
            GUILayout.Label($"Language (Index: {i})");
            _profile.languageElements[i].language = EditorGUILayout.TextField(_profile.languageElements[i].language);


            if (GUILayout.Button("+"))
            {
                _profile.languageElements[i].elements.Add(new PGLanguageProfile.LanguageElement.SubElement());
            }
            for (int a = 0; a < _profile.languageElements[i].elements.Count; a++)
            {
                _profile.languageElements[i].elements[a].keyIndex = EditorGUILayout.Popup("Key", _profile.languageElements[i].elements[a].keyIndex, _profile.keys.ToArray());
                switch (_type)
                {
                    case TypeElement.Text:
                        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea);
                        textAreaStyle.wordWrap = true;
                        _profile.languageElements[i].elements[a].description = EditorGUILayout.TextArea(_profile.languageElements[i].elements[a].description, textAreaStyle, GUILayout.MinHeight(50));
                        break;
                    case TypeElement.Audio:
                        _profile.languageElements[i].elements[a].audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio", _profile.languageElements[i].elements[a].audioClip, typeof(AudioClip),false);
                        break;
                    default:
                        break;
                }

                if (GUILayout.Button("X"))
                {
                    _profile.languageElements[i].elements.RemoveAt(a);
                    a--;
                }
            }
            GUILayout.EndVertical();
        }

        private void OnKeysPanel()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(false), GUILayout.MinWidth(250));
            GUILayout.Label("keys", _headerStyle);
            if (GUILayout.Button("+"))
            {
                _profile.keys.Add("");
            }
            for (int i = 0; i < _profile.keys.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                _profile.keys[i] = EditorGUILayout.TextField(_profile.keys[i]);
                if (GUILayout.Button("X"))
                {
                    _profile.keys.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
