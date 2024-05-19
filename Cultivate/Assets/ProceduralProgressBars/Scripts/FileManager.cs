#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEditor;

namespace Renge.PPB {

    internal class FileManager {
        private static FileManager instance;

        private static FileManager Instance {
            get {
                if (instance == null) instance = new FileManager();
                return instance;
            }
        }

        Dictionary<string, UnityEngine.Object> assetCreationQueue;
        SynchronizationContext mainThread;
        Stopwatch sw;

        private FileManager() {
            assetCreationQueue = new Dictionary<string, UnityEngine.Object>();
            var timer = new Timer(AssetCreatorLoop, null, 100, 100);
            mainThread = System.Threading.SynchronizationContext.Current;
            sw = new Stopwatch();
        }

        public static void AddToAssetCreationQueue(UnityEngine.Object obj, string assetPath) {
            Instance.assetCreationQueue[assetPath] = obj;
            if (Instance.sw.ElapsedMilliseconds > 100 || !Instance.sw.IsRunning)
                Instance.sw.Restart();
        }

        private void AssetCreatorLoop(System.Object stateInfo) {
            if (sw.ElapsedMilliseconds > 500 && assetCreationQueue.Count > 0) {
                sw.Reset();
                mainThread.Post(_ => {
                    foreach (var item in assetCreationQueue) {
                        AssetDatabase.CreateAsset(item.Value, item.Key);
                    }
                    assetCreationQueue.Clear();
                }, null);
            }
        }
    }
}
#endif