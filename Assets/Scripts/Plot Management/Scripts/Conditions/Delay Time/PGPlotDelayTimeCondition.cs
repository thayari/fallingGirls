using System;
using UnityEngine;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotDelayTimeCondition : PGPlotCondition
    {
        public float duration;
        private float _timer;
        public override void OnStartCondition(PGPlotController plotController)
        {
            _timer = 0;
        }
        public override void OnUpdateCondition(PGPlotController plotController)
        {
            _timer += Time.deltaTime;
            if (_timer > duration)
            {
                plotController.NextPlot(this);
            }
        }
    }
}