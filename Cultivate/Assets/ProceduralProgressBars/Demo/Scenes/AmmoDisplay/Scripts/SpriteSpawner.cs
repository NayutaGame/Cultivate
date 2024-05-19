using UnityEngine;

namespace Renge.PPB.Demo {
    public class SpriteSpawner : MonoBehaviour {
        public GameObject toSpawn;

        public float initialRadian;
        public float maxRadian;
        public float radius = 1.9f;
        float radianStep;

        private void Start() {
            radianStep = Mathf.Abs(initialRadian - maxRadian) / 20f;
        }

        public void Spawn(float val, float delta) {
            float stepCount = (1 - val) / (1 / 20f);
            var go = Instantiate(toSpawn, transform.position + new Vector3(Mathf.Cos(initialRadian - stepCount * radianStep), Mathf.Sin(initialRadian - stepCount * radianStep), 0) * radius, toSpawn.transform.rotation);
            var rb = go.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3((Random.value - 0.5f) * 2 * 100, 100, -50));
            rb.AddTorque(new Vector3(0, 0, (Random.value - 0.5f) * 2 * 200));
        }

        private void OnDrawGizmos() {

            Gizmos.DrawCube(transform.position + new Vector3(Mathf.Cos(initialRadian), Mathf.Sin(initialRadian), 0) * radius, new Vector3(0.01f, 0.01f, 0.01f));
            Gizmos.DrawCube(transform.position + new Vector3(Mathf.Cos(maxRadian), Mathf.Sin(maxRadian), 0) * radius, new Vector3(0.01f, 0.01f, 0.01f));
        }
    }
}