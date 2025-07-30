
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CLLibrary
{
    public static class FileUtility
    {
        public static bool IsStreamingFileExists(string filename)
        {
            string dataPath = Application.streamingAssetsPath;
            return IsFileExists(dataPath, filename);
        }

        public static bool IsPersistentFileExists(string filename)
        {
            string dataPath = Application.persistentDataPath;
            return IsFileExists(dataPath, filename);
        }
        
        public static bool IsFileExists(string dataPath, string filename)
        {
            string fullFilePath = dataPath + filename;
            return File.Exists(fullFilePath);
        }

        public static T ReadStreamingFile<T>(string filename)
        {
            string dataPath = Application.streamingAssetsPath;
            return ReadFile<T>(dataPath, filename);
        }

        public static T ReadPersistentFile<T>(string filename)
        {
            string dataPath = Application.persistentDataPath;
            return ReadFile<T>(dataPath, filename);
        }
        
        public static T ReadFile<T>(string dataPath, string filename)
        {
            string fullFilePath = dataPath + filename;

            string txt;
            if (!TryLoadFile(out txt, fullFilePath))
            {
                string defaultTxt = Resources.Load<TextAsset>(filename)?.text;
                if (defaultTxt != null)
                    txt = defaultTxt;
                else
                    return default;
            }

            string json = JsonFromTxt(txt);
            return JsonUtility.FromJson<T>(json);
        }

        public static void WriteStreamingFile<T>(T toWrite, string filename)
        {
            string dataPath = Application.streamingAssetsPath;
            WriteFile(toWrite, dataPath, filename);
        }

        public static void WritePersistentFile<T>(T toWrite, string filename)
        {
            string dataPath = Application.persistentDataPath;
            WriteFile(toWrite, dataPath, filename);
        }
        
        public static void WriteFile<T>(T toWrite, string dataPath, string filename)
        {
            string fullFilePath = dataPath + filename;

            string json = JsonUtility.ToJson(toWrite);
            string txt = TxtFromJson(json);

            SaveFile(fullFilePath, txt);

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        public static void DeleteStreamingFile(string filename)
        {
            string dataPath = Application.streamingAssetsPath;
            DeleteFile(dataPath, filename);
        }

        public static void DeletePersistentFile(string filename)
        {
            string dataPath = Application.persistentDataPath;
            DeleteFile(dataPath, filename);
        }

        public static void DeleteFile(string dataPath, string filename)
        {
            string fullFilePath = dataPath + filename;

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
