using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playButtonObject;
    [SerializeField] private GameManager gameManager; 

    public void OnPlayButtonClick()
    {
        gameManager.ChangeState(GameManager.GameState.Playing);
        //playButtonObject.SetActive(false);
        //gameManager.movementScript.enabled = true; // gameManager.movementScript - это ваш компонент Movement
        //gameManager.menuScript.OnChangeCursorVisibleWithoutLock(false); // gameManager.menuScript - это ваш компонент Menu
    }
}
