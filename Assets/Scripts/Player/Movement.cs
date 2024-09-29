using System.Collections;
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
    [SerializeField] private float _moveLerp = 8f;

    public float verticalSpeed => _movementVerticalSpeed;

    private float _moveKeyboardValue;

    private Camera _camera;

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
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 worldPosition = _camera.ScreenToWorldPoint(new Vector2(touchPosition.x, touchPosition.y));
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _player.position = Vector2.Lerp(_player.position, new Vector2(worldPosition.x, _player.position.y), _moveLerp * Time.deltaTime);
            }
        }
        if (Mouse.current != null && Mouse.current.IsActuated())
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _player.position = Vector2.Lerp(_player.position, new Vector2(worldPosition.x, _player.position.y), _moveLerp * Time.deltaTime);
            }
        }
        if (Gamepad.current != null && Gamepad.current.IsActuated())
        {
            _player.Translate(Vector2.right * _moveAction.action.ReadValue<float>() * _movementGamepadSpeed * Time.deltaTime);
        }
        if (Keyboard.current != null && Keyboard.current.IsActuated())
        {
            _moveKeyboardValue = Mathf.Lerp(_moveKeyboardValue, _moveAction.action.ReadValue<float>(), _moveLerp * Time.deltaTime);
            _player.Translate(Vector2.right * _moveKeyboardValue * _movementKeyboardSpeed * Time.deltaTime);
        }
    }
}
