using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue;
    public static event Action<int> OnCoinTaken;

    private void OnTriggerEnter(Collider other)
    {

        Destroy(gameObject);

        OnCoinTaken?.Invoke(coinValue);
    }
}
