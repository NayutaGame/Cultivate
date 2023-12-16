
using System.IO;
using UnityEngine;

namespace CLLibrary
{
    public static class FileUtility
    {
        public static T ReadFromFile<T>(string filename) where T : new()
        {
            // string fullFilePath = Application.persistentDataPath + filename;
            string fullFilePath = Application.streamingAssetsPath + filename;

            string txt;
            if (!TryLoadFile(out txt, fullFilePath))
            {
                string defaultTxt = Resources.Load<TextAsset>(filename)?.text;
                if (defaultTxt != null)
                    txt = defaultTxt;
                else
                    return new();
            }

            string json = JsonFromTxt(txt);
            return JsonUtility.FromJson<T>(json);
        }

        public static void WriteToFile<T>(T toWrite, string filename)
        {
            // string fullFilePath = Application.persistentDataPath + filename;
            string fullFilePath = Application.streamingAssetsPath + filename;

            string json = JsonUtility.ToJson(toWrite);
            string txt = TxtFromJson(json);

            SaveFile(fullFilePath, txt);
        }

        public static void DeleteFile(string filename)
        {
            // string fullFilePath = Application.persistentDataPath + filename;
            string fullFilePath = Application.streamingAssetsPath + filename;

            if (!File.Exists(fullFilePath))
                return;

            File.Delete(fullFilePath);
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
}
