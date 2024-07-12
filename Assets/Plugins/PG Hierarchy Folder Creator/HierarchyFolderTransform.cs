using UnityEditor;
using UnityEngine;

namespace PG.HierarchyFolder
{
    public class HierarchyFolderTransform : MonoBehaviour
    {
        private void OnValidate()
        {
            ResetTransform();
        }

        public void ResetTransform()
        {
            if (transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.zero;
            }

            if (transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.one;
            }

            if (transform.localRotation != Quaternion.identity)
            {
                transform.localRotation = Quaternion.identity;
            }
        }
        // Этот метод вызывается, когда контекстное меню объекта создается
        // Мы используем его, чтобы добавить пользовательскую опцию в меню
        [ContextMenu("Remove Component")]
        private void PreventRemoval()
        {
            DestroyImmediate(gameObject);
        }

    }
}