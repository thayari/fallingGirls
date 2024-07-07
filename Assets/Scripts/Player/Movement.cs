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

    private float _moveKeyboardValue;

    private Camera _camera;
    private RaycastHit2D _hit;
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
        switch (_playerInput.currentControlScheme)
        {
            case "Android":
                _hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(new Vector3(_moveAction.action.ReadValue<float>(), _player.position.y, _player.position.z)), Vector2.zero);

                if (_hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Target Position: " + _hit.collider.gameObject.transform.position);
                    _player.position = Vector3.Lerp(_player.position, new Vector3(_hit.point.x, _player.position.y, _player.position.z), _moveLerp * Time.deltaTime);
                }
                break;
            case "Mouse":
                _hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(new Vector3(_moveAction.action.ReadValue<float>(), _player.position.y, _player.position.z)), Vector2.zero);

                if (_hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Target Position: " + _hit.collider.gameObject.transform.position);
                    _player.position = Vector3.Lerp(_player.position, new Vector3(_hit.point.x, _player.position.y, _player.position.z), _moveLerp * Time.deltaTime);
                }
                break;
            case "Gamepad":
                _player.Translate(Vector2.right * _moveAction.action.ReadValue<float>() * _movementGamepadSpeed * Time.deltaTime);
                break;
            case "Keyboard":
                _moveKeyboardValue = Mathf.Lerp(_moveKeyboardValue, _moveAction.action.ReadValue<float>(), _moveLerp * Time.deltaTime);
                _player.Translate(Vector2.right * _moveKeyboardValue * _movementKeyboardSpeed * Time.deltaTime);
                break;
        }
    }
}
