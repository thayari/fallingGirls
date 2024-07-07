using UnityEditor;
using UnityEngine;
namespace PG.PlotManagement
{
    public partial class PGPlotControllerEditorWindow : EditorWindow
    {
        private Vector2 _menuObjectsScrollView;
        private Vector2 _menuObjectEventsScrollView;
        private Vector2 _leftMenuScrollView;

        private bool _objectEventsMenu;
        private bool _objectsMenu;

        private string _newObjectName;
        private string _newEventObjectName;
        public static void ObjectsPopup(ref int targetValue)
        {
            targetValue = EditorGUILayout.Popup("Object", targetValue, asset.Objects.ToArray());
        }
        public static void ObjectsPopup(ref int targetValue, string nameObject)
        {
            targetValue = EditorGUILayout.Popup(nameObject, targetValue, asset.Objects.ToArray());
        }
        public static void EventObjectsPopup(ref int targetValue)
        {
            targetValue = EditorGUILayout.Popup("Event Object", targetValue, asset.EventObjects.ToArray());
        }
        public static void EventObjectsPopup(ref int targetValue, string nameObject)
        {
            targetValue = EditorGUILayout.Popup(nameObject, targetValue, asset.EventObjects.ToArray());
        }
        void OnObjectsMenu()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Objects");
            _newObjectName = GUILayout.TextField(_newObjectName);
            if (GUILayout.Button("Create"))
            {
                CreateObject(ref _newObjectName);
                EditorUtility.SetDirty(asset);
            }

            _menuObjectsScrollView = GUILayout.BeginScrollView(_menuObjectsScrollView);
            foreach (var objectName in asset.Objects)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(objectName);
                if (GUILayout.Button("Remove"))
                {
                    asset.Objects.Remove(objectName);
                    EditorUtility.SetDirty(asset);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }
        void CreateObject(ref string value)
        {
            if (!string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value))
            {
                asset.Objects.Add(value);
                value = "";
                GUI.FocusControl(null);
                EditorUtility.SetDirty(asset);
            }
        }
        void OnEventObjectsMenu()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Event Objects");
            _newEventObjectName = GUILayout.TextField(_newEventObjectName);
            if (GUILayout.Button("Create"))
            {
                CreateEventObject(ref _newEventObjectName);
                EditorUtility.SetDirty(asset);
            }

            _menuObjectEventsScrollView = GUILayout.BeginScrollView(_menuObjectEventsScrollView);
            foreach (var objectName in asset.EventObjects)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(objectName);
                if (GUILayout.Button("Remove"))
                {
                    asset.EventObjects.Remove(objectName);
                    EditorUtility.SetDirty(asset);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }
        void CreateEventObject(ref string value)
        {
            if (!string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value))
            {
                asset.EventObjects.Add(value);
                value = "";
                GUI.FocusControl(null);
                EditorUtility.SetDirty(asset);
            }
        }
    }
}