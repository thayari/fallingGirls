using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UnityEvent _dead;
    [SerializeField] private float _restartTimer = 1f;
    public void OnActivate()
    {
        _dead?.Invoke();
        StartCoroutine(OnRestart());
    }
    IEnumerator OnRestart()
    {
        yield return new WaitForSeconds(_restartTimer);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}