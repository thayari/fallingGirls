using System;
using UnityEngine;
namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotEnableObjectCondition : PGPlotCondition
    {
        public int objectIndex;
        private GameObject _gameObject;
        public bool objectActive;

        public override void OnStartCondition(PGPlotController plotController)
        {
            _gameObject = plotController.GetElement(objectIndex).gameObject;
        }
        public override void OnUpdateCondition(PGPlotController plotController)
        {
            if (_gameObject.activeSelf == objectActive)
            {
                plotController.NextPlot(this);
            }
        }
    }
}
