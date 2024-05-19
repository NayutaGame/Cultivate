using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using Renge.PPB;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace Renge.Util {


    internal static class Helpers {
        public const string persistentAssetName = "ppb_projectPersistentData_a5g2q"; //prefix_name_gibberish
        public static void UselessMethod(params System.Object[] obj) { }
        public static string AddSpacesToSentence(string text, bool preserveAcronyms) {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++) {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static void Destroy<T>(ref T obj) where T : UnityEngine.Object {
            if (!obj) return;
#if UNITY_EDITOR
            if (Application.isPlaying || Environment.StackTrace.Contains("OnValidate"))
                UnityEngine.Object.Destroy(obj);
            else
                UnityEngine.Object.DestroyImmediate(obj);
#else
            UnityEngine.Object.Destroy(obj);
#endif
            obj = null;
        }

#if UNITY_EDITOR

#if UNITY_2022_1_OR_NEWER
        public static void SetGizmoIconEnabled(Type type, bool on) {
            GizmoUtility.SetIconEnabled(type, on);
        }
#else
        static MethodInfo setIconEnabled;
        static MethodInfo SetIconEnabled => setIconEnabled = setIconEnabled ??
            Assembly.GetAssembly(typeof(Editor))?.GetType("UnityEditor.AnnotationUtility")?.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);
        public static void SetGizmoIconEnabled(Type type, bool on) {
            if (SetIconEnabled == null) return;
            const int MONO_BEHAVIOR_CLASS_ID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html
            SetIconEnabled.Invoke(null, new object[] { MONO_BEHAVIOR_CLASS_ID, type.Name, on ? 1 : 0 });
        }
#endif

#if UNITY_EDITOR
        private static bool IsEditingInPrefabMode(GameObject obj) {
            if (EditorUtility.IsPersistent(obj)) {
                // if the game object is stored on disk, it is a prefab of some kind, despite not returning true for IsPartOfPrefabAsset =/
                return true;
            } else {
                // If the GameObject is not persistent let's determine which stage we are in first because getting Prefab info depends on it
                var mainStage = StageUtility.GetMainStageHandle();
                var currentStage = StageUtility.GetStageHandle(obj);
                if (currentStage != mainStage) {
                    var prefabStage = PrefabStageUtility.GetPrefabStage(obj);
                    if (prefabStage != null) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsAssetOnDisk(GameObject obj) {
            if (!obj) return PrefabUtility.IsPartOfPrefabAsset(obj);
            return PrefabUtility.IsPartOfPrefabAsset(obj) || IsEditingInPrefabMode(obj);
        }
#endif

        public static void AddObjectToAsset(UnityEngine.Object obj, string assetPath) {

            //AssetDatabase.AddObjectToAsset(obj, assetPath);
        }

        public static string AssetPath(UnityEngine.Object obj) {
            return AssetDatabase.GetAssetPath(obj);
        }

        public static bool SaveObjectAsAsset(UnityEngine.Object obj, string path = null) {
            if (!AssetDatabase.Contains(obj) && AssetDatabase.AssetPathToGUID(path) == "") {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(obj, path);
                return true;
            }
            return false;
        }

        public static bool PBMatHasReferencesInProject(UnityEngine.Object obj) {
            string objAssetPath = AssetDatabase.GetAssetPath(obj);
            if (objAssetPath == null) return false;
            if(AssetDatabase.FindAssets($"ref:\"{objAssetPath}\"").Length != 0) {
                return true;
            }

            List<Scene> toCloseScenes = new List<Scene> ();
            Scene[] originalScenes = new Scene[SceneManager.sceneCount];
            for (int i = 0; i < originalScenes.Length; i++) {
                originalScenes[i] = SceneManager.GetSceneAt(i);
            }

            bool referenceFound = false;
            string[] sceneGuids = AssetDatabase.FindAssets("t:SceneAsset");
            foreach (var guid in sceneGuids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.StartsWith("Assets/")) continue;
                Scene scene = SceneManager.GetSceneByPath(path);
                if (!scene.isLoaded) {
                    scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                    toCloseScenes.Add(scene);
                }
                var sceneGos = scene.GetRootGameObjects();
                var pbComps = new List<ProceduralProgressBar>();
                foreach (var go in sceneGos) {
                    pbComps.AddRange(go.GetComponentsInChildren<ProceduralProgressBar>());
                }
                foreach (var pb in pbComps) {
                    if(AssetDatabase.GetAssetPath(pb.GetMaterial()) == objAssetPath) {
                        referenceFound = true;
                        break;
                    }
                }
                if (referenceFound) 
                    break;
            }

            foreach (var scene in toCloseScenes) {
                EditorSceneManager.CloseScene(scene, true);
            }

            return referenceFound;
        }

        public static Dictionary<string, bool> FindAllPBMatReferences(string[] guids) {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (var guid in guids) {
                string objAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                //asset doesn't exist
                if (objAssetPath == null) { 
                    result[guid] = false;
                    continue;
                }
                //find all asset references, in this case most likely only prefabs
                if (AssetDatabase.FindAssets($"ref:\"{objAssetPath}\"").Length != 0) {
                    result[guid] = true;
                } else {
                    result[guid] = false;
                }
            }

            //find all open scenes
            Scene[] originalScenes = new Scene[SceneManager.sceneCount];
            for (int i = 0; i < originalScenes.Length; i++) {
                originalScenes[i] = SceneManager.GetSceneAt(i);
            }

            //init loop
            bool allAssetsReferenced = true;
            List<Scene> toCloseScenes = new List<Scene>();
            string[] sceneGuids = AssetDatabase.FindAssets("t:SceneAsset");
            foreach (var guid in sceneGuids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.StartsWith("Assets/")) continue;
                Scene scene = SceneManager.GetSceneByPath(path);
                if (!scene.isLoaded) {
                    scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                    toCloseScenes.Add(scene);
                }
                var sceneGos = scene.GetRootGameObjects();
                var pbComps = new List<ProceduralProgressBar>();
                foreach (var go in sceneGos) {
                    pbComps.AddRange(go.GetComponentsInChildren<ProceduralProgressBar>());
                }
                allAssetsReferenced = true;
                if(pbComps.Count == 0) allAssetsReferenced = false;
                foreach (var pb in pbComps) {
                    foreach (var matGuid in guids) {
                        if (result[matGuid] == true) continue;
                        allAssetsReferenced = false;
                        if (AssetDatabase.GetAssetPath(pb.GetMaterial()) == AssetDatabase.GUIDToAssetPath(matGuid)) {
                            result[matGuid] = true;
                        }
                    }
                }
                if (allAssetsReferenced) break;
            }

            foreach (var scene in toCloseScenes) {
                EditorSceneManager.CloseScene(scene, true);
            }

            return result;
        }

        public static string[] FindAllAssetsInDirectory(string filter, params string[] paths) {
            return AssetDatabase.FindAssets(filter, paths);
        }

        public static bool IsSceneDirty() {
            for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
                if (EditorSceneManager.GetSceneAt(i).isDirty)
                    return true;
            }
            return false;
        }

        static string s_AssetPath = "Assets/ProceduralProgressBars";
        public static string GetAssetPath() {
            bool exists = true;
            if (!Directory.Exists(s_AssetPath)) {
                string guid = AssetDatabase.FindAssets("ProceduralProgressBars t:folder").FirstOrDefault();
                if (guid == null) {
                    exists = false;
                } else {
                    exists = true;
                    s_AssetPath = AssetDatabase.GUIDToAssetPath(guid);
                }
            }
            if (!exists)
                return null;
            return s_AssetPath;
        }
        public static string ValidateAndGetPrefabCachePath() {
            string cacheSubDir = "/ProgressBarData";
            string dataFolder = "_pb_data";
            string fallbackPath = "Assets";
            string assetPath = GetAssetPath();
            if (assetPath == null) {
                assetPath = fallbackPath;
            }

            string cachePath = $"{assetPath}{cacheSubDir}/{dataFolder}";

            if (!Directory.Exists(cachePath)) {
                Directory.CreateDirectory(cachePath);
                Directory.CreateDirectory(cachePath + "/meshes");
                Directory.CreateDirectory(cachePath + "/materials");
                Directory.CreateDirectory(cachePath + "/textures");
            }

            return cachePath;
        }
#endif
        private static StringBuilder CreateStringBuilderWithSpaceAfter(string message) {
            var builder = new StringBuilder(message);

            if (message.EndsWith(".")) {
                builder.Append(' ');
            } else if (!message.EndsWith(". ")) {
                builder.Append(". ");
            }

            return builder;
        }
    }
}