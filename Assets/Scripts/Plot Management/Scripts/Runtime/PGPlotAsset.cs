using System.Collections.Generic;
using UnityEngine;

namespace PG.PlotManagement
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="New PG Plot Asset", menuName ="PG/Plot/Asset")]
    public class PGPlotAsset : ScriptableObject
    {
        public List<string> EventObjects = new List<string>();
        public List<string> Objects = new List<string>();
        public List<string> Languages = new List<string>();
        public List<PGPlotChapter> chapters = new List<PGPlotChapter>(1);
        public string DisplayText(int chapter, int state, int language)
        {
            return chapters[chapter].states[state].languageStates[language].name;
        }
        public string DisplayDescription(int chapter, int state, int language)
        {
            return chapters[chapter].states[state].languageStates[language].description;
        }
    }
}