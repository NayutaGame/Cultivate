
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EntityEditableList : ListModel<EntityEditable>
{
    public static EntityEditableList ReadFromFile()
    {
        string fileName = "/EntityCategory.json";
        string filePath = Application.persistentDataPath + fileName;

        string txt;
        if (!TryLoadFile(out txt, filePath))
        {
            string defaultTxt = Resources.Load<TextAsset>(fileName)?.text;
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
        string fileName = "/EntityCategory.json";
        string filePath = Application.persistentDataPath + fileName;

        string json = JsonUtility.ToJson(this);
        string txt = TxtFromJson(json);
        SaveFile(filePath, txt);
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
