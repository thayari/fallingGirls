using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private GameOver _gameOver;
    public void OnDamaged()
    {
        _gameOver?.OnActivate();
    }
}
