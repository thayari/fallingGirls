using PG.LocalizationManagement;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DropdownLanguageLoader : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private string _languageDropdownSaveParameter = "LanguageDropdown";
    void Start()
    {
        Init();
    }

    private async void Init()
    {
        await Task.Delay(20);
        if (PlayerPrefs.HasKey(_languageDropdownSaveParameter))
        {
            _dropdown.value = PlayerPrefs.GetInt(_languageDropdownSaveParameter);
        }
        else
        {
            LocalizationManager.Instance.SetLanguage(Application.systemLanguage.ToString());
            _dropdown.value = PlayerPrefs.GetInt(_languageDropdownSaveParameter);
        }
    }
}
