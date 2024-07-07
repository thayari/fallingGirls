using System;
using UnityEngine;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGLanguageState
    {
        public string name;
        [TextArea(3,10)] public string description;
    }
}