
using CLLibrary;
using UnityEditor;
using UnityEngine;

public class Utility : MonoBehaviour
{
    [MenuItem("GameObject/Create Custom/ListView", false, 0)]
    static void CreateListView(MenuCommand menuCommand)
    {
        string path = "ListView";

        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Create Custom/AnimatedListView", false, 1)]
    static void CreateAnimatedListView(MenuCommand menuCommand)
    {
        string path = "AnimatedListView";

        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Create Custom/ComplexView", false, 1)]
    static void CreateComplexView(MenuCommand menuCommand)
    {
        string path = "ComplexView";

        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("Utility/Delete Profile", false, 0)]
    static void DeleteProfile(MenuCommand menuCommand)
    {
        FileUtility.DeleteFile(ProfileList.Filename);
        AssetDatabase.Refresh();
    }
}
