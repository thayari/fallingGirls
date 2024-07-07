using UnityEngine.Events;

namespace PG.PlotManagement
{
    public partial class PGPlotController
    {
        [System.Serializable]
        public class EventElement
        {
            public int indexEvent;
            public UnityEvent plotEvent;
        }
    }
}
