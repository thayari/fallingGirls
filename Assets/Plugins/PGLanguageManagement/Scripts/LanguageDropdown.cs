using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PG.LanguageManagement
{

    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private string _languageSaveParameter = "Language";

        [field: SerializeField] public PGLanguageProfile profile { get; private set; }
        [HideInInspector] public List<int> keys = new List<int>();
        private int _value;
        [ContextMenu("Get Cache")]
        void GetCache()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }
        public void SetText()
        {
            _value = _dropdown.value;
            string languageName = PlayerPrefs.GetString(_languageSaveParameter);
            for (int i = 0; i < profile.languageElements.Count; i++)
            {
                if (profile.languageElements[i].language == languageName)
                {
                    _dropdown.ClearOptions();
                    List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
                    for (int a = 0; a < keys.Count; a++)
                    {
                        options.Add(new TMP_Dropdown.OptionData(profile.languageElements[i].elements[keys[a]].description));
                    }
                    _dropdown.AddOptions(options);
                    break;
                }
            }
            _dropdown.value = _value;
        }
        // Start is called before the first frame update
        void Start()
        {
            SetText();
        }
        private void OnEnable()
        {
            LanguageManager.languageChanged += delegate { SetText(); };
        }
        private void OnDisable()
        {
            LanguageManager.languageChanged -= delegate { SetText(); };
        }
    }
}
