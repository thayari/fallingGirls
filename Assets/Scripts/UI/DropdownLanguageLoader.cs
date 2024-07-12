using PG.LocalizationManagement;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DropdownLanguageLoader : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private string _languageDropdownSaveParameter = "Localization";
    void Start()
    {
        Init();
    }

    private async void Init()
    {
        await Task.Delay(20);
        if (PlayerPrefs.HasKey(_languageDropdownSaveParameter))
        {
            for (int i = 0; i < LocalizationManager.Instance.localizationData.languages.Count; i++)
            {
                if (LocalizationManager.Instance.localizationData.languages[i].languageCode == LocalizationManager.Instance.currentLanguage)
                {
                    _dropdown.value = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < LocalizationManager.Instance.localizationData.languages.Count; i++)
            {
                if (LocalizationManager.Instance.localizationData.languages[i].languageCode == Application.systemLanguage.ToString())
                {
                    _dropdown.value = i;
                    break;
                }
            }
            LocalizationManager.Instance.SetLanguage(Application.systemLanguage.ToString());
        }
    }
}
