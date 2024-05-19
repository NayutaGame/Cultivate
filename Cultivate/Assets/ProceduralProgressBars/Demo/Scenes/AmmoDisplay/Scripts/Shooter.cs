using Renge.PPB;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Renge.PPB.Demo {
    public class Shooter : MonoBehaviour {
        public ProceduralProgressBar progressBar;

        private void Start() {
            progressBar = GetComponent<ProceduralProgressBar>();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
            }
        }

        public void Shoot() {
            progressBar.Value -= 0.05f;
            //if (progressBar != null && progressBar.Value > 0) {


            //}
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Shooter))]
    public class ShooterEditor : Editor {
        public override void OnInspectorGUI() {
            //base.OnInspectorGUI();
            Shooter shooter = (Shooter)target;
            if (GUILayout.Button("Shoot")) {
                shooter.Shoot();
            }
            EditorGUILayout.HelpBox("To shoot, run the game and click somewhere on the screen, or click the \"Shoot\" button above.", MessageType.Info);
        }
    }
#endif
}