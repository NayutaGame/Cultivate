#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Renge.PPB {
    public class ProjectPrefs {
        private static string project_prefix = string.Empty;
        private static string Validate() {
            if (project_prefix == string.Empty) {
                project_prefix = $"{PlayerSettings.companyName}_{PlayerSettings.productName}";
            }
            return project_prefix;
        }

        public static string GenKey(string key) {
            return Validate() + key;
        }

        public static int GetInt(string key, int defaultValue = 0) {
            return EditorPrefs.GetInt(GenKey(key), defaultValue);
        }

        public static void SetInt(string key, int value) {
            EditorPrefs.SetInt(GenKey(key), value);
        }

        //TODO: implement this with a scriptable object
        public static string GetSharableString(string key, string defaultValue = "") {
            return defaultValue;
        }
    }
}
#endif
