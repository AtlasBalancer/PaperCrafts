#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace com.ab.papercrafts
{
    public class CustomHotkey
    {
        [MenuItem("GameObject/Create Custom Empty Child %&n", false, 0)] // ctr+alt+n
        static void CreateEmptyChild()
        {
            if (Selection.activeTransform != null)
            {
                GameObject newObj = new GameObject("GameObject");
                Transform parent = Selection.activeTransform;
                newObj.transform.SetParent(parent);
                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
                newObj.transform.localScale = Vector3.one;
                Selection.activeGameObject = newObj;
            }
            else
            {
                GameObject newObj = new GameObject("GameObject");
                newObj.transform.position = Vector3.zero;
                Selection.activeGameObject = newObj;
            }
        }
    }
}
#endif