using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private UnityEvent _winEvent;
    [Header("Restart")]
    [SerializeField] private bool _restartAfterWin = true;
    [SerializeField] private float _restartTimer = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            _winEvent?.Invoke();
            if (_restartAfterWin)
            {
                StartCoroutine(OnRestart());
            }
            Debug.Log("Win!!!");
        }
    }
    IEnumerator OnRestart()
    {
        yield return new WaitForSeconds(_restartTimer);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
