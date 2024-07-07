using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private static PGPlotAsset asset;

        private float _maxWidthLeftMenu = 250;

        private int _currentChapter;
        private int _currentState;


        // Убедитесь, что путь куда вы сохраняете ассет существует
        private const string path = "Assets/New Plot Asset.asset";

        private GUIStyle _headerStyle;


        private Color _selectedButton = new Color(0.5613208f, 0.7228894f, 1);

        private StateMenu _stateMenu;
        public enum StateMenu
        {
            Data, Conditions, Behaviours
        }

        private MainData _mainData;
        public enum MainData
        {
            Objects, Languages
        }


        static void Initialize()
        {
            _derivedTypesBehaviourList = new List<Type>();
            _derivedTypesConditionList = new List<Type>();


            // Замените YourBaseClass на интересующий вас базовый класс
            _baseTypeCondition = typeof(PGPlotCondition);

            // Получаем все типы в текущей сцене
            _allTypesCondition = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

            // Выбираем только те типы, которые являются дочерними для базового класса
            _derivedTypesCondition = _allTypesCondition.Where(type => _baseTypeCondition.IsAssignableFrom(type) && type != _baseTypeCondition);

            foreach (var item in _derivedTypesCondition)
            {
                _derivedTypesConditionList.Add(item);
            }



            _baseTypeBehaviour = typeof(PGPlotBehaviour);

            // Получаем все типы в текущей сцене
            _allTypesBehaviour = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

            // Выбираем только те типы, которые являются дочерними для базового класса
            _derivedTypesBehaviour = _allTypesBehaviour.Where(type => _baseTypeBehaviour.IsAssignableFrom(type) && type != _baseTypeBehaviour);

            foreach (var item in _derivedTypesBehaviour)
            {
                _derivedTypesBehaviourList.Add(item);
            }
        }
        public void InitializeLanguages()
        {
            for (int i = 0; i < asset.chapters.Count; i++)
            {
                for (int j = 0; j < asset.chapters[i].states.Count; j++)
                {
                    while (asset.chapters[i].states[j].languageStates.Count != asset.Languages.Count)
                    {
                        if (asset.chapters[i].states[j].languageStates.Count < asset.Languages.Count)
                        {
                            asset.chapters[i].states[j].languageStates.Add(new PGLanguageState());
                        }
                        if (asset.chapters[i].states[j].languageStates.Count > asset.Languages.Count)
                        {
                            asset.chapters[i].states[j].languageStates.RemoveAt(asset.chapters[i].states[j].languageStates.Count - 1);
                        }
                    }
                }
            }
            EditorUtility.SetDirty(asset);
        }


        [MenuItem("Window/Plot Controller")]
        public static void OpenWindow()
        {
            var window = GetWindow<PGPlotControllerEditorWindow>("Plot Controller");
            window.titleContent.image = (Texture)Resources.Load("Plot Management/Icon");
            Initialize();
        }
        public static void OpenWindow(PGPlotAsset targetAsset)
        {
            asset = targetAsset;
            var window = GetWindow<PGPlotControllerEditorWindow>(asset.name);
            window.titleContent.image = (Texture)Resources.Load("Plot Management/Icon");
            Initialize();
        }
        private void OnGUI()
        {
            OnHeaderWithoutAsset();
            asset = (PGPlotAsset)EditorGUILayout.ObjectField("Asset", asset, typeof(PGPlotAsset), false);
            if (asset)
            {
                OnHeaderWithAsset();
                GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));

                switch (_mainData)
                {
                    case MainData.Objects:
                        if (_objectsMenu || _objectEventsMenu)
                        {
                            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(false));
                            _leftMenuScrollView = GUILayout.BeginScrollView(_leftMenuScrollView, GUILayout.MaxWidth(_maxWidthLeftMenu), GUILayout.ExpandWidth(false));
                            if (_objectsMenu)
                            {
                                OnObjectsMenu();
                            }
                            if (_objectEventsMenu)
                            {
                                OnEventObjectsMenu();
                            }
                            GUILayout.EndScrollView();

                            GUILayout.EndVertical();
                        }
                        break;
                    case MainData.Languages:
                        OnLanguagesMenu();
                        break;
                }

                OnChaptersMenu();

                OnStatesMenu(_currentChapter);

                switch (_stateMenu)
                {
                    case StateMenu.Data:
                        OnStateMenu(_currentChapter, _currentState);
                        break;
                    case StateMenu.Conditions:
                        OnConditionsMenu(_currentChapter, _currentState);
                        break;
                    case StateMenu.Behaviours:
                        OnBehavioursMenu(_currentChapter, _currentState);
                        break;
                    default:
                        OnStateMenu(_currentChapter, _currentState);
                        break;
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                CreateAsset();
            }
        }
        void SortUp<T>(List<T> types, int index)
        {
            int pastIndex = index - 1;
            (types[index], types[pastIndex]) = (types[pastIndex], types[index]);
        }
        void SortDown<T>(List<T> types, int index)
        {
            int pastIndex = index + 1;
            (types[index], types[pastIndex]) = (types[pastIndex], types[index]);
        }
        void CreateAsset()
        {
            if (GUILayout.Button("Create Asset"))
            {
                // Создаем новый экземпляр вашего ассета (в данном случае, ScriptableObject)
                PGPlotAsset newAsset = ScriptableObject.CreateInstance<PGPlotAsset>();


                // Создаем ассет и сохраняем его в проекте
                AssetDatabase.CreateAsset(newAsset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                asset = newAsset;
            }
        }
        void OnHeaderWithoutAsset()
        {
            if (!asset)
            {
                GUILayout.Space(30);
                _headerStyle = new GUIStyle(EditorStyles.label)
                {
                    fontSize = 24,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                };
                GUILayout.Label("PG Plot Controller", _headerStyle);
                GUILayout.Space(10);
            }
        }
        void OnHeaderWithAsset()
        {
            GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Save"))
            {
                AssetDatabase.SaveAssets();
            }
            _mainData = (MainData)EditorGUILayout.EnumPopup("Main Data", _mainData);
            switch (_mainData)
            {
                case MainData.Objects:
                    _objectEventsMenu = EditorGUILayout.Toggle("Object Events Panel", _objectEventsMenu);
                    _objectsMenu = EditorGUILayout.Toggle("Object Panel", _objectsMenu);
                    break;
                case MainData.Languages:
                    _languageLeftMenuActive = EditorGUILayout.Toggle("Languages Panel", _languageLeftMenuActive);
                    break;
            }
            _stateMenu = (StateMenu)EditorGUILayout.EnumPopup("State Menu", _stateMenu);
            GUILayout.EndHorizontal();
        }
    }
}