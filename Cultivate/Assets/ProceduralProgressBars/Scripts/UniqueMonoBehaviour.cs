using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Renge.PPB {

    // This component gives a GameObject a stable, non-replicatable Globally Unique IDentifier.
    // It can be used to reference a specific instance of an object no matter where it is.
    // This can also be used for other systems, such as Save/Load game
    public abstract class UniqueMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver {
        // System guid we use for comparison and generation
        private System.Guid guid = System.Guid.Empty;

        // Unity's serialization system doesn't know about System.Guid, so we convert to a byte array
        // Fun fact, we tried using strings at first, but that allocated memory and was twice as slow
        [SerializeField]
        protected string serializedGuid = null;

        protected abstract void OnCreatedNewGUID();
        private Action onCreatedNewGUIDStartDelay;

        // When de-serializing or creating this component, we want to either restore our serialized GUID
        // or create a new one.
        bool CreateGuid() {
            bool createdGuid = false;
#if UNITY_EDITOR
            if (IsAssetOnDisk()) {
                var tmp = GlobalObjectId.GetGlobalObjectIdSlow(this).ToString();
                if(serializedGuid != tmp && !tmp.Contains("-0-00")) {
                    serializedGuid = tmp;
                    createdGuid = true;
                }
                guid = System.Guid.Empty;
                return createdGuid;
            }
#endif
            // if our serialized data is invalid, then we are a new object and need a new GUID
            if (serializedGuid == null || serializedGuid.Length > 0 && serializedGuid[0] == 'G' || serializedGuid.Length != 36) {

#if UNITY_EDITOR
                Undo.RecordObject(this, "Added GUID");
#endif
                guid = System.Guid.NewGuid();
                serializedGuid = guid.ToString();
                createdGuid= true;

#if UNITY_EDITOR
                // If we are creating a new GUID for a prefab instance of a prefab, but we have somehow lost our prefab connection
                // force a save of the modified prefab instance properties
                if (PrefabUtility.IsPartOfNonAssetPrefabInstance(this)) {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                }
#endif
            } else if (guid == System.Guid.Empty) {
                //prefab asset needs to perform some additional checks, since it doesn't use the guidmanager

                // otherwise, we should set our system guid to our serialized guid
                guid = new System.Guid(serializedGuid);
            }


            // register with the GUID Manager so that other components can access this
            if (guid != System.Guid.Empty) {
                if (!GuidManager.Add(this)) {
                    // if registration fails, we probably have a duplicate or invalid GUID, get us a new one.
                    serializedGuid = null;
                    guid = System.Guid.Empty;
                    createdGuid = CreateGuid();
                }
            }

            return createdGuid;
        }

#if UNITY_EDITOR
        protected bool HasValidGUID() {
            return serializedGuid != null && (serializedGuid.Length == 36 || !serializedGuid.Contains("-0-00") && !serializedGuid.Contains("-1-"));
        }
        private bool IsEditingInPrefabMode() {
            if (EditorUtility.IsPersistent(this)) {
                // if the game object is stored on disk, it is a prefab of some kind, despite not returning true for IsPartOfPrefabAsset =/
                return true;
            } else {
                // If the GameObject is not persistent let's determine which stage we are in first because getting Prefab info depends on it
                var mainStage = StageUtility.GetMainStageHandle();
                var currentStage = StageUtility.GetStageHandle(gameObject);
                if (currentStage != mainStage) {
                    var prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
                    if (prefabStage != null) {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsAssetOnDisk() {
            if (!gameObject) return PrefabUtility.IsPartOfPrefabAsset(this);
            return PrefabUtility.IsPartOfPrefabAsset(this) || IsEditingInPrefabMode();
        }

#endif
        public void OnBeforeSerialize() {
            if (guid != System.Guid.Empty) {
                serializedGuid = guid.ToString();
            }
        }

        // On load, we can go head a restore our system guid for later use
        public void OnAfterDeserialize() {
            if (serializedGuid != null && serializedGuid.Length == 36) {
                guid = new System.Guid(serializedGuid);
            }
        }

        protected void Awake() {
            if (CreateGuid()) {
                onCreatedNewGUIDStartDelay = OnCreatedNewGUID;
            }
        }

        protected void Start() {
            onCreatedNewGUIDStartDelay?.Invoke();
        }

        protected void Reset() {

        }

#if UNITY_EDITOR
        protected void Update() {
            if (serializedGuid != null && serializedGuid.Length > 0 && serializedGuid[0] == 'G' && serializedGuid.Contains("-0-00")) {
                if (CreateGuid()) {
                    OnCreatedNewGUID();
                }
            }
        }
#endif


        // Never return an invalid GUID
        public System.Guid GetGuid() {

            if (guid == System.Guid.Empty && serializedGuid != null && serializedGuid.Length == 36) {
                guid = new System.Guid(serializedGuid);
            }

            return guid;
        }

        public string GetStringGUID() {
            return serializedGuid;
        }

        // let the manager know we are gone, so other objects no longer find this
        public void OnDestroy() {
            GuidManager.Remove(guid);
        }
    }
}