using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BtnUp : MonoBehaviour
{
    private float alpha = 1.0f;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = alpha;
    }

    public void onBtnUpClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
