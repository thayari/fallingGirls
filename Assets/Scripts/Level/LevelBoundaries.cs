using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{
    private Movement _managerMovement;
    [SerializeField] private float bounceDistance = 0.2f;

    private void Start()
    {
        _managerMovement = GameObject.FindWithTag("Manager").GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("LeftBoundary"))
            {
                _managerMovement.canMoveLeft = false;
            }
            if (gameObject.CompareTag("RightBoundary"))
            {
                _managerMovement.canMoveRight = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("LeftBoundary"))
            {
                _managerMovement.canMoveLeft = true;
            }
            if (gameObject.CompareTag("RightBoundary"))
            {
                _managerMovement.canMoveRight = true;
            }
        }
    }
}
