using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renge.PPB;
using Renge.Util;

namespace Renge.PPB.Demo {

    [ExecuteAlways]
    public class CurveToShader : MonoBehaviour {
        public AnimationCurve curve;
        public string shaderID;
        public Material mat;

        private void Update() {
            if (curve != null)
                mat.SetTexture(shaderID, curve.ToTexture2D());
        }
    }
}