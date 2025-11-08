using TMPro;
using UnityEngine;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;
    private int _coins = 0;
    private void Awake()
    {
        _coinsText.text = _coins.ToString();
        Statistics.Instance.moneyChanged += UpdateCoinsCounter;
    }

    //private void OnDisable()
    //{
    //    Statistics.Instance.moneyChanged -= UpdateCoinsCounter;
    //}

    private void UpdateCoinsCounter()
    {
        _coins = Statistics.Instance.money;
        _coinsText.text = _coins.ToString();
    }
}
