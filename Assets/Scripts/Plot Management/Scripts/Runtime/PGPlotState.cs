using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Events;
using System;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotState
    {
        public List<PGLanguageState> languageStates = new List<PGLanguageState>(1);
        public bool startTimelineEnable = true;
        public PlayableAsset startTimelineAsset;
        public List<PGPlotObjectActive> plotObjectActives = new List<PGPlotObjectActive>();
        public bool endTimelineEnable = true;
        public PlayableAsset endTimelineAsset;
        public List<PGPlotCondition> plotConditions = new List<PGPlotCondition>(1);
        public List<PGPlotBehaviour> plotBehaviours = new List<PGPlotBehaviour>();


        public PGPlotState nextState;
        public bool isStarted = false;
        public bool startEventEnable = true;
        public int startEventObjectIndex;
        public bool endEventEnable = true;
        public int endEventObjectIndex;
        public UnityEvent endState;
        public void OnStartState(PGPlotController plotController)
        {
            if (startTimelineEnable)
            {
                plotController.playableDirector.Play(startTimelineAsset);
            }
            OnChangeObjectsActive(plotController);
            OnStartObjectEventInvoke(plotController);
            foreach (var item in plotConditions)
            {
                item.OnStartCondition(plotController);
            }
            if (plotBehaviours.Count > 0)
            {
                foreach (var item in plotBehaviours)
                {
                    item.OnStartBehaviourState(plotController);
                }
            }
            endState.AddListener(delegate { OnEndObjectEventInvoke(plotController); });
            isStarted = true;
        }
        void OnChangeObjectsActive(PGPlotController plotGraph)
        {
            foreach (var plotObjectActive in plotObjectActives)
            {
                plotGraph.GetElement(plotObjectActive.index).SetActive(plotObjectActive.active);
            }
        }
        void OnStartObjectEventInvoke(PGPlotController plotGraph)
        {
            if (startEventEnable)
            {
                plotGraph.GetEventElement(startEventObjectIndex)?.Invoke();
            }
        }
        void OnEndObjectEventInvoke(PGPlotController plotController)
        {
            if (endTimelineEnable)
            {
                plotController.playableDirector.Play(endTimelineAsset);
            }
            if (endEventEnable)
            {
                plotController.GetEventElement(endEventObjectIndex)?.Invoke();
            }
        }
        public void OnUpdateState(PGPlotController plotController)
        {
            foreach (var item in plotConditions)
            {
                item.OnUpdateCondition(plotController);
            }
            if (plotBehaviours.Count > 0)
            {
                foreach (var item in plotBehaviours)
                {
                    item.OnUpdateBehaviourState(plotController);
                }
            }
        }
        public void OnEndState(PGPlotController plotController)
        {
            foreach (var item in plotConditions)
            {
                item.OnEndCondition(plotController);
            }
            if (plotBehaviours.Count > 0)
            {
                foreach (var item in plotBehaviours)
                {
                    item.OnEndBehaviourState(plotController);
                }
            }
        }
    }
}