using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Statistics : MonoBehaviour
{
    private static Statistics _instance;

    public static Statistics Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<Statistics>();

                if (_instance == null)
                {
                    Debug.LogError("На сцене отсутствует объект со скриптом Statistics!");
                }
            }
            return _instance;
        }
    }

    [Header("Save")]
    [SerializeField] private string fileName = "Statistics.json";
    public string path => Path.Combine(Application.persistentDataPath, fileName);

    private int _money;
    public int money
    {
        get => _money; 
        set
        {
            _money = value;
            moneyChanged?.Invoke();
        }
    }
    public delegate void MoneyChanged();
    public event MoneyChanged moneyChanged;
    public delegate void MoneyAdded(int value);
    public event MoneyAdded moneyAdded;

    private SaveStatistics _saveStatistics = new SaveStatistics();

    private static bool applicationIsQuitting = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        Load();

        Coin.OnCoinTaken += AddMoney;
    }

    public void Save()
    {
        _saveStatistics.money = money;
        string json = JsonUtility.ToJson(_saveStatistics);
        File.WriteAllText(path, json);
    }

    void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, _saveStatistics);
            money = _saveStatistics.money;
        }
    }

    public void AddMoney(int value)
    {

        money += value;
        moneyAdded?.Invoke(value);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Coin.OnCoinTaken -= AddMoney;
        }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    [System.Serializable]
    public class SaveStatistics
    {
        public int money;
    }
}
