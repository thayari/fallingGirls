using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{
    [SerializeField] private Movement playerMovement;
    [SerializeField] private float bounceDistance = 0.2f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("LeftBoundary"))
            {
                playerMovement.canMoveLeft = false;
            }
            if (gameObject.CompareTag("RightBoundary"))
            {
                playerMovement.canMoveRight = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("LeftBoundary"))
            {
                playerMovement.canMoveLeft = true;
            }
            if (gameObject.CompareTag("RightBoundary"))
            {
                playerMovement.canMoveRight = true;
            }
        }
    }
}
