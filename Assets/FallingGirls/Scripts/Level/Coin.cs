using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue;
    public static event Action<int> OnCoinTaken;

    private void OnTriggerEnter2D(Collider2D other)
    {

        Destroy(gameObject);

        OnCoinTaken?.Invoke(coinValue);
    }
}
