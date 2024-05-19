using UnityEngine;

namespace Renge.PPB.Demo {
    public class CameraRotaryMovement : MonoBehaviour {

        public Vector2 minLookAtTarget;
        public Vector2 maxLookAtTarget;
        public float time = 3f;

        Vector2 oldLookAtTarget;
        Vector2 lookAtTarget;
        float counter = 0;

        private void Start() {
            lookAtTarget = new Vector2(Random.Range(minLookAtTarget.x, maxLookAtTarget.x), Random.Range(minLookAtTarget.y, maxLookAtTarget.y));
            oldLookAtTarget = lookAtTarget;
        }

        void Update() {
            counter -= Time.deltaTime;
            if (counter <= 0) {
                oldLookAtTarget = lookAtTarget;
                lookAtTarget = new Vector2(Random.Range(minLookAtTarget.x, maxLookAtTarget.x), Random.Range(minLookAtTarget.y, maxLookAtTarget.y));
                counter = time;
            }
            var target = Vector2.Lerp(oldLookAtTarget, lookAtTarget, 1 - counter / time);
            transform.LookAt(transform.position + new Vector3(target.x, target.y, 3));
        }
    }
}
