using System.Collections.Generic;
using UnityEngine;
namespace PG.LanguageManagement
{
    [CreateAssetMenu(fileName = "New LanguageProfile", menuName = "PG/Language/Profile")]
    public class PGLanguageProfile : ScriptableObject
    {
        public List<string> keys = new List<string>();
        public List<LanguageElement> languageElements = new List<LanguageElement>();
        [System.Serializable]
        public class LanguageElement
        {
            public string language;
            public List<SubElement> elements = new List<SubElement>();
            [System.Serializable]
            public class SubElement
            {
                public int keyIndex;
                [TextArea(3, 10)] public string description;
                public AudioClip audioClip;
            }
        }
    }
}
