using TMPro;
using UnityEngine;
namespace PG.PlotManagement
{
    public class PGPlotDisplayCurrentQuest : MonoBehaviour
    {
        [SerializeField] private PGPlotController _plotController;
        [SerializeField] private TMP_Text _textQuest;
        private void OnEnable()
        {
            _plotController.onStateValueChanged.AddListener(delegate { OnDisplayText(); });
        }
        private void OnDisable()
        {
            _plotController.onStateValueChanged.RemoveListener(delegate { OnDisplayText(); });
        }
        private void Start()
        {
            OnDisplayText();
        }
        public void OnDisplayText()
        {
            _textQuest.text = _plotController.plotAsset.DisplayText(_plotController.CurrentState % _plotController.plotAsset.chapters.Count, _plotController.CurrentState, _plotController.currentLanguage);
        }
    }
}
