using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    private void OnEnable()
    {
        _actionAsset.Enable();
    }
    private void OnDisable()
    {
        _actionAsset.Disable();
    }
}
