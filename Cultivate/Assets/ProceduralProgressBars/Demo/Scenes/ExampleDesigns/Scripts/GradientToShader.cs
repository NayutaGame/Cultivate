using System.Collections;
using System.Collections.Generic;
using Renge.PPB;
using UnityEngine;
using Renge.Util;

namespace Renge.PPB.Demo {
    [ExecuteAlways]
    public class GradientToShader : MonoBehaviour {
        public Gradient gradient;
        public string shaderID;
        public Material mat;

        private void Update() {
            if (gradient != null)
                mat.SetTexture(shaderID, gradient.ToTexture2D());
        }
    }
}
