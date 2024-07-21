using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    public static event Action OnCoinTaken;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        OnCoinTaken?.Invoke();
    }
}
