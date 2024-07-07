using System;
using UnityEngine;

namespace PG.PlotManagement
{
    [Serializable]
    public abstract class PGPlotCondition : ScriptableObject
    {
        public PGPlotController plotController;
        public enum TransitionType { NextState, PreviousState, SetState, NextChapter, PreviousChapter, SetChapter, SetAll, None }
        public TransitionType transitionType;
        public int chapterValue;
        public int stateValue;

        /// <summary>
        /// Called at the beginning of PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnStartCondition(PGPlotController plotController)
        {
        }

        /// <summary>
        /// Called every frame of the active PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnUpdateCondition(PGPlotController plotController)
        {
        }

        /// <summary>
        /// Called at the end of PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnEndCondition(PGPlotController plotController)
        {
        }
        public PGPlotCondition GetTypePlotCondition => this;
    }
}