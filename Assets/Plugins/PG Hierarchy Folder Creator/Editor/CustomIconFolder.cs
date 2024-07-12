using UnityEditor;
using UnityEngine;

namespace PG.HierarchyFolder
{
    [InitializeOnLoad]
    public static class CustomIconFolder
    {
        private static Texture2D folderIcon;

        static CustomIconFolder()
        {
            // Загрузка вашей прозрачной иконки папки
            folderIcon = EditorGUIUtility.FindTexture("Folder Icon");// Здесь "FolderIcon" - это имя вашей прозрачной текстуры папки

            // Регистрация метода обратного вызова
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCallback;
        }

        private static void HierarchyItemCallback(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null && obj.GetComponent<HierarchyFolderTransform>() != null)
            {
                // Определяем местоположение и размер иконки
                Rect iconRect = new Rect(selectionRect.x, selectionRect.y, 16, 16);
                iconRect.x += selectionRect.width - 16; // Сдвигаем иконку вправо
                // Отрисовываем иконку папки
                GUI.DrawTexture(iconRect, folderIcon);
            }
        }
    }
}
