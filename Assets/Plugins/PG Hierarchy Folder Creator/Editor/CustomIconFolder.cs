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
            // �������� ����� ���������� ������ �����
            folderIcon = EditorGUIUtility.FindTexture("Folder Icon");// ����� "FolderIcon" - ��� ��� ����� ���������� �������� �����

            // ����������� ������ ��������� ������
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCallback;
        }

        private static void HierarchyItemCallback(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null && obj.GetComponent<HierarchyFolderTransform>() != null)
            {
                // ���������� �������������� � ������ ������
                Rect iconRect = new Rect(selectionRect.x, selectionRect.y, 16, 16);
                iconRect.x += selectionRect.width - 16; // �������� ������ ������
                // ������������ ������ �����
                GUI.DrawTexture(iconRect, folderIcon);
            }
        }
    }
}
