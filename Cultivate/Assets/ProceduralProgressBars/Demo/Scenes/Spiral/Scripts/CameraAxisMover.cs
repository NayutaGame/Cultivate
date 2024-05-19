using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renge.PPB.Demo {
    public class CameraAxisMover : MonoBehaviour {
        public Vector3 axis = new Vector3(0, 1, 0);
        public float dist = 1f;
        public GameObject target;

        // Update is called once per frame
        void Update() {
            Vector3 opposite = Vector3.one - axis;
            transform.position = Vector3.Scale(transform.position, opposite) + Vector3.Scale(target.transform.position, axis) + dist * axis;
        }
    }
}