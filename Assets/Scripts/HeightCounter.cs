using TMPro;
using UnityEngine;

public class HeightCounter : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private TMP_Text _heightText;
    [SerializeField] private string _countString = "{count}";
    [SerializeField] private int _height = 0;
    private float _heightFloat;
    private string _standartHeightString;
    private void Awake()
    {
        _standartHeightString = _heightText.text;
        _heightText.text = _standartHeightString.Replace(_countString, _height.ToString());
    }
    void Update()
    {
        enabled = _movement.enabled;
        _heightText.text = _standartHeightString.Replace(_countString, _height.ToString());
        _heightFloat += (_movement.verticalSpeed * Time.deltaTime);
        _height = (int)_heightFloat;
    }
}
