using System.Collections.Generic;
using UnityEngine;

namespace PG.LanguageManagement
{
    public class LanguageObject : MonoBehaviour
    {
        [SerializeField] private string _languageSaveParameter = "Language";
        //[SerializeField] private Language[] _languages;
        public PGLanguageProfile profile;
        [HideInInspector] public List<Language> languages = new List<Language>();
        [System.Serializable]
        public class Language
        {
            public int languageIndex;
            public GameObject description;
        }
        public void SetObject()
        {
            string languageName = PlayerPrefs.GetString(_languageSaveParameter);

            for (int i = 0; i < languages.Count; i++)
            {
                if (profile.languageElements[i].language == languageName)
                {
                    languages[i].description.SetActive(true);
                }
                else
                {
                    languages[i].description.SetActive(false);
                }
            }
        }
        // Start is called before the first frame update
        void Awake()
        {
            SetObject();
        }
        private void OnEnable()
        {
            LanguageManager.languageChanged += delegate { SetObject(); };
        }
        private void OnDisable()
        {
            LanguageManager.languageChanged -= delegate { SetObject(); };
        }
    }
}