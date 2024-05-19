#if UNITY_EDITOR
using Renge.Util;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Renge.PPB {

    public class ProjectPersistentData : ScriptableObject {

        private static ProjectPersistentData instance;
        private static ProjectPersistentData instanceDev;
        private static ProjectPersistentData Instance {
            get {
                string persistentAssetNameDev = Helpers.persistentAssetName + "_Dev";
#if PPB_DEV
                if (!instance) {
                    instance = Resources.Load<ProjectPersistentData>(persistentAssetNameDev);
                    if (!instance) {
                        string assetPath = Helpers.GetAssetPath();
                        if (assetPath == null) {
                            assetPath = "Assets/Editor/Resources";
                        } else {
                            assetPath += "/Editor/Resources";
                        }
                        if (!Directory.Exists(assetPath)) {
                            Directory.CreateDirectory(assetPath);
                        }
                        instance = CreateInstance<ProjectPersistentData>();
                        AssetDatabase.CreateAsset(instance, $"{assetPath}/{persistentAssetNameDev}.asset");
                    }
                }
#else
                if (!instance || instance.name == persistentAssetNameDev) {
                    instance = Resources.Load<ProjectPersistentData>(Helpers.persistentAssetName);
                    instanceDev = Resources.Load<ProjectPersistentData>(persistentAssetNameDev);
                    if (!instance) {
                        string assetPath = Helpers.GetAssetPath();
                        if (assetPath == null) {
                            assetPath = "Assets/Editor/Resources";
                        } else {
                            assetPath += "/Editor/Resources";
                        }
                        if (!Directory.Exists(assetPath)) {
                            Directory.CreateDirectory(assetPath);
                        }
                        instance = CreateInstance<ProjectPersistentData>();
                        AssetDatabase.CreateAsset(instance, $"{assetPath}/{Helpers.persistentAssetName}.asset");
                    }
                }
#endif
                return instance;
            }

        }

        [SerializeField] private List<string> keyData;
        [SerializeField] private List<string> valueData;

        private ProjectPersistentData() {
            keyData = new List<string>();
            valueData = new List<string>();
        }

        public static void SetString(string key, string value) {
            var instance = Instance;
            int index = instance.keyData.IndexOf(key);
            if (index != -1)
                instance.valueData[index] = value;
            else {
                instance.keyData.Add(key);
                instance.valueData.Add(value);
            }
            EditorUtility.SetDirty(instance);
        }

        public static string GetString(string key) {
            var instance = Instance;
            int index = instance.keyData.IndexOf(key);
            if (index == -1) {
                if (!instanceDev) return null;
                index = instanceDev.keyData.IndexOf(key);
                if (index == -1) return null;
                return instanceDev.valueData[index];
            }
            
            return instance.valueData[index];
        }

        //[MenuItem("Assets/Procedural Progress Bar/Clear Persistent Data")]
        public static void ClearPersistentData() {
            var i = Instance;
            i.keyData.Clear();
            i.valueData.Clear();
            if (instanceDev) {
                instanceDev.keyData.Clear();
                instanceDev.valueData.Clear();
                EditorUtility.SetDirty(instanceDev);
            }
            EditorUtility.SetDirty(instance);
        }
    }
}
#endif