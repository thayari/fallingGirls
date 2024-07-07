using System;
using System.Collections.Generic;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotChapter
    {
        public List<PGPlotState> states = new List<PGPlotState>(1);
    }
}