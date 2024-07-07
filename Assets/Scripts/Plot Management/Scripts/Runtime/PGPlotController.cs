using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace PG.PlotManagement
{
    public partial class PGPlotController : MonoBehaviour
    {
        public PGPlotAsset plotAsset;
        public PlayableDirector playableDirector;
        public int currentLanguage;
        public int CurrentChapter;
        [SerializeField] private int _currentState;
        [SerializeField] private string _saveName = "Plot.json";
        [SerializeField] private string _saveLanguageName = "Language";
        private string _loadedLanaguage;


        [HideInInspector] public List<ObjectElement> objectsElements = new List<ObjectElement>();
        [HideInInspector] public List<EventElement> eventElements = new List<EventElement>();


        [HideInInspector] public UnityEvent onStateValueChanged;
        public int CurrentState { 
            get => _currentState; 
            set
            {
                _currentState = value;
                onStateValueChanged?.Invoke();
            }
        }

        public PGPlotChapter GetChapter => plotAsset.chapters[CurrentChapter];
        public PGPlotState GetState => plotAsset.chapters[CurrentChapter].states[_currentState];

        private SavePlotController _savePlotController = new SavePlotController();

        [System.Serializable]
        public class SavePlotController
        {
            public int CurrentState;
        }
        public GameObject GetElement(int index)
        {
            for (int i = 0; i < objectsElements.Count; i++)
            {
                if (objectsElements[i].indexObject == index)
                {
                    return objectsElements[i].gameObject;
                }
            }
            return null;
        }
        public GameObject GetElement(string value)
        {
            for (int i = 0; i < plotAsset.Objects.Count; i++)
            {
                if (plotAsset.Objects[i] == value)
                {
                    for (int a = 0; a < objectsElements.Count; a++)
                    {
                        if (objectsElements[a].indexObject == i)
                        {
                            return objectsElements[i].gameObject;
                        }
                    }
                }
            }
            return null;
        }
        public UnityEvent GetEventElement(int index)
        {
            for (int i = 0; i < eventElements.Count; i++)
            {
                if (eventElements[i].indexEvent == index)
                {
                    return eventElements[i].plotEvent;
                }
            }
            return null;
        }
        public UnityEvent GetEventElement(string value)
        {
            for (int i = 0; i < plotAsset.EventObjects.Count; i++)
            {
                if (plotAsset.EventObjects[i] == value)
                {
                    for (int a = 0; a < eventElements.Count; a++)
                    {
                        if (eventElements[a].indexEvent == i)
                        {
                            return eventElements[i].plotEvent;
                        }
                    }
                }
            }
            return null;
        }
        // Start is called before the first frame update
        void Awake()
        {
            Load();
            plotAsset = Instantiate(plotAsset);
        }
        private void Start()
        {
            GetState.OnStartState(this);
        }
        // Update is called once per frame
        void Update()
        {
            if (GetState.isStarted)
            {
                GetState.OnUpdateState(this);
            }
        }
        [ContextMenu("Save")]
        void Save()
        {
            _savePlotController.CurrentState = _currentState;
            string path = Path.Combine(Application.persistentDataPath, _saveName);
            string json = JsonUtility.ToJson(_savePlotController);
            File.WriteAllText(path, json);
            Debug.Log(json);

        }
        [ContextMenu("Load")]
        void Load()
        {
            _loadedLanaguage = PlayerPrefs.GetString(_saveLanguageName);
            for (int i = 0; i < plotAsset.Languages.Count; i++)
            {
                if (plotAsset.Languages[i] == _loadedLanaguage)
                {
                    currentLanguage = i;
                    break;
                }
            }

            string path = Path.Combine(Application.persistentDataPath, _saveName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, _savePlotController);
            }

            _currentState = _savePlotController.CurrentState;
        }
        [ContextMenu("Clear Save")]
        public void ClearSave()
        {
            string path = Path.Combine(Application.persistentDataPath, _saveName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public void NextPlot(PGPlotCondition plotCondition)
        {
            switch (plotCondition.transitionType)
            {
                case PGPlotCondition.TransitionType.NextState:
                    NextState();
                    break;
                case PGPlotCondition.TransitionType.PreviousState:
                    PreviousState();
                    break;
                case PGPlotCondition.TransitionType.SetState:
                    SetState(plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.NextChapter:
                    NextChapter();
                    break;
                case PGPlotCondition.TransitionType.PreviousChapter:
                    PreviousChapter();
                    break;
                case PGPlotCondition.TransitionType.SetChapter:
                    SetChapter(plotCondition.chapterValue);
                    break;
                case PGPlotCondition.TransitionType.SetAll:
                    SetAll(plotCondition.chapterValue, plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.None:
                    break;
            }
        }
        public void NextPlot(int targetPlotCondition)
        {
            PGPlotCondition plotCondition = GetState.plotConditions[targetPlotCondition];
            switch (plotCondition.transitionType)
            {
                case PGPlotCondition.TransitionType.NextState:
                    NextState();
                    break;
                case PGPlotCondition.TransitionType.PreviousState:
                    PreviousState();
                    break;
                case PGPlotCondition.TransitionType.SetState:
                    SetState(plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.NextChapter:
                    NextChapter();
                    break;
                case PGPlotCondition.TransitionType.PreviousChapter:
                    PreviousChapter();
                    break;
                case PGPlotCondition.TransitionType.SetChapter:
                    SetChapter(plotCondition.chapterValue);
                    break;
                case PGPlotCondition.TransitionType.SetAll:
                    SetAll(plotCondition.chapterValue, plotCondition.stateValue);
                    break;
                case PGPlotCondition.TransitionType.None:
                    break;
            }
        }
        void NextState()
        {
            if (CurrentState < GetChapter.states.Count-1)
            {
                GetState.OnEndState(this);
                CurrentState++;
                GetState.OnStartState(this);
            }
            else
            {
                NextChapter();
            }
            Save();
        }
        void NextChapter()
        {
            if (CurrentChapter < plotAsset.chapters.Count-1)
            {
                GetState.OnEndState(this);
                CurrentChapter++;
                GetState.OnStartState(this);
                CurrentState = 0;
            }
            Save();
        }
        void PreviousState()
        {
            if (CurrentState > 0)
            {
                GetState.OnEndState(this);
                CurrentState--;
                GetState.OnStartState(this);
            }
            else
            {
                PreviousChapter();
            }
            Save();
        }
        void PreviousChapter()
        {
            if (CurrentChapter > 0)
            {
                GetState.OnEndState(this);
                CurrentChapter--;
                GetState.OnStartState(this);
                CurrentState = 0;
            }
            Save();
        }
        void SetState(int state)
        {
            if (state < GetChapter.states.Count)
            {
                if (CurrentState != state)
                {
                    GetState.OnEndState(this);
                }
                CurrentState = state;
                GetState.OnStartState(this);
            }
            Save();
        }
        void SetChapter(int chapter)
        {
            if (chapter < plotAsset.chapters.Count)
            {
                if (CurrentChapter != chapter)
                {
                    GetState.OnEndState(this);
                }
                CurrentChapter = chapter;
                GetState.OnStartState(this);
            }
            Save();
        }
        void SetAll(int chapter, int state)
        {
            if (chapter < plotAsset.chapters.Count)
            {
                if (CurrentChapter != chapter && CurrentState != state)
                {
                    GetState.OnEndState(this);
                }
                CurrentChapter = chapter;
                CurrentState = state;
                GetState.OnStartState(this);
            }
            Save();
        }
    }
}
