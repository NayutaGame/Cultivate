using UnityEngine;

namespace Renge.PPB.Demo {
    public class DestroyAfterSeconds : MonoBehaviour {
        public float seconds = 2f;

        private void Start() {
            if (seconds >= 0)
                Destroy(gameObject, seconds);
        }
    }
}