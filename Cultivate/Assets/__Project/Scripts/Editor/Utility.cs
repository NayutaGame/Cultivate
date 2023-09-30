using System.IO;
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

    [MenuItem("Utility/Clear Save", false, 0)]
    static void ClearSave(MenuCommand menuCommand)
    {
        string filePath = "/EntityEditableList.json";
        // string fullFilePath = Application.persistentDataPath + filePath;
        string fullFilePath = Application.streamingAssetsPath + filePath;

        if (!File.Exists(fullFilePath))
            return;

        File.Delete(fullFilePath);
        AssetDatabase.Refresh();
    }
}
