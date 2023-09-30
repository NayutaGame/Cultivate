
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EntityEditableList : ListModel<RunEntity>
{
    public static EntityEditableList ReadFromFile()
    {
        string filePath = "/EntityEditableList.json";
        // string fullFilePath = Application.persistentDataPath + filePath;
        string fullFilePath = Application.streamingAssetsPath + filePath;

        string txt;
        if (!TryLoadFile(out txt, fullFilePath))
        {
            string defaultTxt = Resources.Load<TextAsset>(filePath)?.text;
            if (defaultTxt != null)
                txt = defaultTxt;
            else
                return new();
        }

        string json = JsonFromTxt(txt);
        return JsonUtility.FromJson<EntityEditableList>(json);
    }

    public void WriteToFile()
    {
        string filePath = "/EntityEditableList.json";
        // string fullFilePath = Application.persistentDataPath + filePath;
        string fullFilePath = Application.streamingAssetsPath + filePath;

        string json = JsonUtility.ToJson(this);
        string txt = TxtFromJson(json);

        SaveFile(fullFilePath, txt);
    }

    private static void SaveFile(string filePath, string txt)
    {
        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine(txt);
        sw.Close();
    }

    private static bool TryLoadFile(out string txt, string filePath)
    {
        txt = null;
        if (!File.Exists(filePath))
            return false;
        StreamReader sr = new StreamReader(filePath);
        txt = sr.ReadToEnd();
        sr.Close();
        return true;
    }

    private static string TxtFromJson(string json)
    {
        return json.Replace('\"', '\'');
    }

    private static string JsonFromTxt(string txt)
    {
        return txt.Replace('\'', '\"');
    }
}
