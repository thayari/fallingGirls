using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionProperty _moveAction;
    [SerializeField] private Transform _player;

    [Header("Speed")]
    [SerializeField] private float _movementGamepadSpeed = 4f;
    [SerializeField] private float _movementKeyboardSpeed = 4f;
    [SerializeField] private float _movementVerticalSpeed = 5f;
    [SerializeField] private float _moveLerp = 1f;

    public float verticalSpeed => _movementVerticalSpeed;

    private Camera _camera;

    [HideInInspector] public bool canMoveLeft = true;
    [HideInInspector] public bool canMoveRight = true;

    private void OnEnable()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        _player.Translate(Vector3.up * _movementVerticalSpeed * Time.deltaTime);
        OnMove();
    }

    void OnMove()
    {
        if (Touchscreen.current != null && Touchscreen.current.IsActuated())
        {
            Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Mathf.Abs(_camera.transform.position.z)));
            //Debug.Log("Touch Position: " + touchPosition + ", World Position: " + worldPosition);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                float targetX = worldPosition.x;

                if ((targetX < _player.position.x && canMoveLeft) || (targetX > _player.position.x && canMoveRight))
                {
                    _player.position = Vector3.Lerp(_player.position, new Vector3(targetX, _player.position.y, _player.position.z), _moveLerp * Time.deltaTime);
                }
            }
        }

        if (Mouse.current != null && Mouse.current.IsActuated())
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(_camera.transform.position.z)));
            //Debug.Log("Mouse Position: " + mousePosition + ", World Position: " + worldPosition);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                float targetX = worldPosition.x;

                if ((targetX < _player.position.x && canMoveLeft) || (targetX > _player.position.x && canMoveRight))
                {
                    _player.position = Vector3.Lerp(_player.position, new Vector3(targetX, _player.position.y, _player.position.z), _moveLerp * Time.deltaTime);
                }
            }
        }

        if (Gamepad.current != null && Gamepad.current.IsActuated())
        {
            float moveInput = _moveAction.action.ReadValue<float>();

            if ((moveInput < 0 && canMoveLeft) || (moveInput > 0 && canMoveRight))
            {
                _player.Translate(Vector2.right * moveInput * _movementGamepadSpeed * Time.deltaTime);
            }
        }

        if (Keyboard.current != null && Keyboard.current.IsActuated())
        {
            float _moveKeyboardValue = Mathf.Lerp(0, _moveAction.action.ReadValue<float>(), _moveLerp * Time.deltaTime);
            _player.Translate(Vector2.right * _moveKeyboardValue * _movementKeyboardSpeed * Time.deltaTime);
        }
    }
}
