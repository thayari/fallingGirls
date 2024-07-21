using TMPro;
using UnityEngine;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;
    private int _coins = 0;
    private void Awake()
    {
        _coinsText.text = _coins.ToString();
        Coin.OnCoinTaken += () => onCoinTaken();
    }

    private void onCoinTaken()
    {
        // test
        _coins += 10;
        _coinsText.text = _coins.ToString();
    }
}
