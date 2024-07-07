using System;
using TMPro;
using UnityEngine;

namespace PG.LanguageManagement
{
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _languageDropdown;
        [SerializeField] private string _languageSaveParameter = "Language";
        [SerializeField] private string _languageDropdownSaveParameter = "LanguageDropdown";
        [SerializeField] private PGLanguageProfile _languageProfile;
        public static Action languageChanged = default;

        public void SetLanguage(int language)
        {
            PlayerPrefs.SetString(_languageSaveParameter, _languageProfile.languageElements[language].language);
            PlayerPrefs.SetInt(_languageDropdownSaveParameter, language);
            languageChanged?.Invoke();
        }
        // Start is called before the first frame update
        void Awake()
        {
            if (PlayerPrefs.HasKey(_languageSaveParameter))
            {
                languageChanged?.Invoke();
            }
            else
            {
                PlayerPrefs.SetInt(_languageSaveParameter, 0);
                languageChanged?.Invoke();
            }
            if (_languageDropdown != null)
            {
                _languageDropdown.value = PlayerPrefs.GetInt(_languageSaveParameter);
            }
        }
        private void OnLevelWasLoaded(int level)
        {
            languageChanged = default;
        }
    }
}

