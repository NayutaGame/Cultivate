using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Renge.PPB.Demo {

    enum DemoAnimator {
        None,
        Loading,
        PingPong
    }

    [ExecuteAlways]
    public class DemoHealthController : MonoBehaviour {
        [SerializeField] DemoAnimator demoAnimation = DemoAnimator.None;
        [SerializeField] float speed = 1;

        ProceduralProgressBar progressBar;

        void Start() {
            progressBar = GetComponent<ProceduralProgressBar>();
        }

        private void Update() {
            if (progressBar)
                switch (demoAnimation) {
                    case DemoAnimator.Loading:
                        if (progressBar.Shape == PBShape.Circular) break;
                        progressBar.Value = Mathf.Sin(Time.time) * 0.25f + .5f;
                        progressBar.SetMaterialVector(ProgressBarProperties.ValueMaskOffset, new Vector2(2 - ((Time.time * speed) % 2f) - 1, 0));
                        break;
                    case DemoAnimator.PingPong:
                        progressBar.Value = Mathf.PingPong(Time.time * speed, progressBar.SegmentCount);
                        progressBar.SetMaterialVector(ProgressBarProperties.ValueMaskOffset, new Vector2());
                        break;
                    default:
                        progressBar.SetMaterialVector(ProgressBarProperties.ValueMaskOffset, new Vector2());
                        break;
                }
        }

        public void SetOppositeValue01(float value, float delta) {
            if (progressBar)
                progressBar.Value = 1 - value;
            else
                progressBar = GetComponent<ProceduralProgressBar>();
        }

        void OnDrawGizmos() {
#if UNITY_EDITOR
            // Ensure continuous Update calls.
            // this causes significant cpu usage within the editor
            if (!Application.isPlaying) {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }
}