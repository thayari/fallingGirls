using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonUp : MonoBehaviour
{
    [SerializeField] private string _scemeID = "SampleScene";
    private float _standardAlpha = 1.0f;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = _standardAlpha;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(_scemeID);
    }
}
