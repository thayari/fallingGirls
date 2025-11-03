using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private GameManager _gameManager;
    public void OnDamaged()
    {
        // добавить логику уменьшения жизней, сейчас пока жизнь одна
        _gameManager.ChangeState(GameManager.GameState.GameOver);
    }
}
