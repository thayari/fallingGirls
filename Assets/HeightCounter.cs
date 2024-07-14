using TMPro;
using UnityEngine;

public class HeightCounter : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private TMP_Text _heightText;
    [SerializeField] private string _countString = "{count}";
    [SerializeField] private float _height = 0f;
    private string _standartHeightString;
    private void Awake()
    {
        _standartHeightString = _heightText.text;
        _heightText.text = _standartHeightString.Replace(_countString, string.Format("{0:0.000}", _height));
    }
    void Update()
    {
        enabled = _movement.enabled;
        _heightText.text = _standartHeightString.Replace(_countString, string.Format("{0:0.000}",_height));
        _height += _movement.verticalSpeed * Time.deltaTime;
    }
}
