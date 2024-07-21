using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsContainer : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinsParent;
    [SerializeField] private float levelLength;
    [SerializeField] private float coinsDistance = 5;

    void Start()
    {
        spawnCoins();
    }

    private void spawnCoins()
    {
        float totalLength = 0;

        while (totalLength < levelLength)
        {
            totalLength += coinsDistance;

            Vector3 position = new Vector3(0, totalLength, 0);

            GameObject newCoin = Instantiate(coinPrefab, position, Quaternion.identity);

            newCoin.transform.SetParent(coinsParent.transform);

        }
    }

}
