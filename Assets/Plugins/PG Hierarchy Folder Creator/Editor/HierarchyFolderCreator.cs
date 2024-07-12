using UnityEngine;
using UnityEditor;

namespace PG.HierarchyFolder
{
    public class HierarchyFolderCreator : MonoBehaviour
    {
        [MenuItem("GameObject/Folder")]
        static void Create()
        {
            GameObject folder = Resources.Load("PG Hierarchy Folder Creator/Folder") as GameObject;
            GameObject folderInstance = Instantiate(folder, Selection.activeTransform);
            folderInstance.name = folder.name;
            Selection.activeGameObject = folderInstance;
        }
    }
}
