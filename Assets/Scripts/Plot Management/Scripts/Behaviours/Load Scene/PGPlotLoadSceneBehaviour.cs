using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.PlotManagement
{
    public class PGPlotLoadSceneBehaviour : PGPlotBehaviour
    {
        public string sceneName;
        public int conditionIndex;
        public LoadSceneMode sceneMode;
        public bool unloadScene;
        public string unloadSceneName;
        private AsyncOperation _asyncOperation;
        public override void OnStartBehaviourState(PGPlotController plotController)
        {
            switch (sceneMode)
            {
                case LoadSceneMode.Single:
                    _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
                    plotController.StartCoroutine(LoadScene(plotController));
                    break;
                case LoadSceneMode.Additive:
                    _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    plotController.StartCoroutine(LoadScene(plotController));
                    break;
            }
            if (unloadScene)
            {
                SceneManager.UnloadSceneAsync(unloadSceneName);
            }

        }
        IEnumerator LoadScene(PGPlotController plotController)
        {
            while (!_asyncOperation.isDone)
            {
                yield return null;
            }
            plotController.NextPlot(conditionIndex);
        }
        public override void OnUpdateBehaviourState(PGPlotController plotController)
        {
            base.OnUpdateBehaviourState(plotController);
        }
        public override void OnEndBehaviourState(PGPlotController plotController)
        {
            base.OnEndBehaviourState(plotController);
        }
    }
}
