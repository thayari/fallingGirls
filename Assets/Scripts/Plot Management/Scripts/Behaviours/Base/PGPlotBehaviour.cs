using System;
using UnityEngine;
namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotBehaviour : ScriptableObject
    {
        /// <summary>
        /// Called at the beginning of PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnStartBehaviourState(PGPlotController plotController)
        {

        }
        /// <summary>
        /// Called every frame of the active PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnUpdateBehaviourState(PGPlotController plotController)
        {

        }
        /// <summary>
        /// Called at the end of PlotState
        /// </summary>
        /// <param name="plotController"></param>
        public virtual void OnEndBehaviourState(PGPlotController plotController)
        {

        }
    }

}