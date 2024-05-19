using Renge.PPB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renge.PPB.Demo {
    public class SimpleCompoundPB : MonoBehaviour {
        ProceduralProgressBar[] pbs;
        private void Start() {
            pbs = GetComponentsInChildren<ProceduralProgressBar>();
        }

        public void SetValue(float value) {
            value = Mathf.Clamp01(value);
            float valuePerBar = 1f / pbs.Length;
            for (int i = 0; i < pbs.Length; i++) {
                if (value > valuePerBar) {
                    pbs[i].Value = 1;
                    value -= valuePerBar;
                } else {
                    pbs[i].Value = value / valuePerBar;
                    value = 0;
                }
            }
        }
    }
}