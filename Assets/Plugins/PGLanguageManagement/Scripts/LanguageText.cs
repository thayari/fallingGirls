using TMPro;
using UnityEngine;

namespace PG.LanguageManagement
{
    public class LanguageText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _languageSaveParameter = "Language";

        [field:SerializeField] public PGLanguageProfile profile {  get; private set; }
        [HideInInspector] public string preTextValue;
        [HideInInspector] public string afterTextValue;
        [HideInInspector] public int key;
        [ContextMenu("Get Cache")]
        void GetCache()
        {
            _text = GetComponent<TMP_Text>();
        }
        public void SetText()
        {
            int languageIndex = 0;
            string languageName = PlayerPrefs.GetString(_languageSaveParameter);
            for (int i = 0; i < profile.languageElements.Count; i++)
            {
                if (profile.languageElements[i].language == languageName)
                {
                    languageIndex = i;
                    break;
                }
            }

            _text.text = preTextValue + profile.languageElements[languageIndex].elements[key].description + afterTextValue;
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

