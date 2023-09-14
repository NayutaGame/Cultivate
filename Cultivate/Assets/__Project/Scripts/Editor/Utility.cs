using UnityEditor;
using UnityEngine;

public class Utility : MonoBehaviour
{
    [MenuItem("GameObject/Create Custom/ListView", false, 0)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        string path = "ListView";

        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
