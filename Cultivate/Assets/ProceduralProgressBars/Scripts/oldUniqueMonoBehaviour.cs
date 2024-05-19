using System;
using UnityEngine;

namespace Renge.PPB.Old {

    public abstract class oldUniqueMonoBehaviour : MonoBehaviour {
        [SerializeField] protected int instanceID = 0;
        [SerializeField] protected string guid = null;
        private Action callbackStart;
        
        protected void Awake() {
            var res = Validate();
            if (res.Item1 || res.Item2)
                OnCreatedNewGUIDAwake(res.Item2);
        }

        protected void Start() {
            callbackStart?.Invoke();
        }

        protected void Reset() {
            var res = Validate();
            if (res.Item1 || res.Item2)
                OnCreatedNewGUIDReset();
        }
        (bool, bool) Validate() {
            //check if this is a fresh object
            if (guid == null || instanceID == 0) {
                RegenerateGUID();
                instanceID = GetInstanceID();
                callbackStart = () => OnCreatedNewGUIDStart(false);
                return (true, false);
            }

            //check for new instanceid
            else if (instanceID != GetInstanceID()) {
                instanceID = GetInstanceID();
                //check if this is an instance of another gameobject
                if (instanceID < 0) {
                    RegenerateGUID();
                    callbackStart = () => OnCreatedNewGUIDStart(true);
                    return (false, true);
                }
            }
            return (false, false);
        }
        private void RegenerateGUID() {
            guid = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// This method is automatically called when this object has just been created or instantiated.
        /// </summary>
        /// <param name="isInstance">is this a new object or instantiated object (say, from duplication)</param>
        protected abstract void OnCreatedNewGUIDAwake(bool isInstance);

        /// <summary>
        /// This method is automatically called when this object has just been created or instantiated.
        /// </summary>
        /// <param name="isInstance">is this a new object or instantiated object (say, from duplication)</param>
        protected abstract void OnCreatedNewGUIDStart(bool isInstance);

        /// <summary>
        /// This method is automatically called when this object has just been created or instantiated.
        /// </summary>
        /// <param name="isInstance">is this a new object or instantiated object (say, from duplication)</param>
        protected abstract void OnCreatedNewGUIDReset();
    }
}