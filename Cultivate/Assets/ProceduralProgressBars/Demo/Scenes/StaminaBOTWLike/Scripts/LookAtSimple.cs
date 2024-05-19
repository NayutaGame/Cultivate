using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renge.PPB.Demo {

    public class LookAtSimple : MonoBehaviour {
        public Transform target;

        private void Start() {
            transform.forward = new Vector3(0f, 0f, -1f);
        }

        void Update() {
            transform.LookAt(target);
        }
    }
}
