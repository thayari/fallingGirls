using System;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotWithoutConditions : PGPlotCondition
    {
        public override void OnStartCondition(PGPlotController plotController)
        {
            return;
        }
        public override void OnUpdateCondition(PGPlotController plotController)
        {
            return;
        }
        public override void OnEndCondition(PGPlotController plotController)
        {
            return;
        }
    }
}