using UnityEditor;
using UnityEngine;
namespace PG.PlotManagement
{
    public class PGPlotControllerMenu
    {
        [MenuItem("GameObject/PG/Plot Controller", false, 0)]
        static void AddPrefab()
        {
            GameObject prefab = Resources.Load("Plot Management/Plot Controller") as GameObject;
            if (Selection.gameObjects.Length > 0)
            {

                foreach (var item in Selection.gameObjects)
                {
                    GameObject.Instantiate(prefab, item.transform).name = "Plot Controller";
                }
            }
            else
            {
                GameObject.Instantiate(prefab).name = "Plot Controller";
            }
        }
    }

}