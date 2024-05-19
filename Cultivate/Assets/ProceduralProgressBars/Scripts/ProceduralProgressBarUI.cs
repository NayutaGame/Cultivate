using UnityEngine;
using UnityEngine.UI;
using Renge.Util;

#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif


namespace Renge.PPB {
    [RequireComponent(typeof(CanvasRenderer))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Progress Bars/Procedural Progress Bar UI")]
    public class ProceduralProgressBarUI : Graphic {
        [SerializeField] public ProceduralProgressBar progressBar;

        protected override void Start() {
            base.Start();
            ValidateComponents();
            SetVerticesDirty();
            MaterialMaskingValidation();
            ValidateUIProperties();
        }

#if UNITY_EDITOR
        protected override void Reset() {
            base.Reset();
            ValidateComponents();
            SetVerticesDirty();
            MaterialMaskingValidation();
            ValidateUIProperties();
        }
#endif

        internal void ValidateComponents() {
            progressBar = GetComponent<ProceduralProgressBar>();
            if (!progressBar)
                progressBar = gameObject.AddComponent<ProceduralProgressBar>();
        }

        internal Material GetMaterial() {
            return m_Material;
        }

        internal void SetMaterial(Material m_material) {
            material = m_material;
            MaterialMaskingValidation();
            ValidateUIProperties();
        }

        internal void SetMeshDirty() {
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            base.OnPopulateMesh(vh);
            vh.Clear();
            if (!progressBar) return;
            var mesh = progressBar.GetMesh();
            if (!mesh) return;
            var vertices = mesh.vertices;
            var uv = mesh.uv;
            var triangles = mesh.triangles;
            Rect pixelAdjustedRect = GetPixelAdjustedRect();

            for (int i = 0; i < vertices.Length; i++) {
                vh.AddVert(
                    (Vector3)pixelAdjustedRect.position + (Vector3)pixelAdjustedRect.size / 2.0f + 
                    new Vector3(vertices[i].x * pixelAdjustedRect.width, vertices[i].y * pixelAdjustedRect.height, vertices[i].z), 
                    Color.white, uv[i]);
            }
            for (int i = 0; i < triangles.Length; i += 3) {
                vh.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
            }
        }

        /// <summary>
        /// Method used in place of deriving from MaskableGraphic
        /// </summary>
        protected void MaterialMaskingValidation() {
            if (!progressBar || !progressBar.GetMaterial()) return;
            if (GetComponentInParent<Mask>() != null) {
                progressBar.SetMaterialFloat("_Stencil", 1);
            } else {
                progressBar.SetMaterialFloat("_Stencil", 0);
            }
            progressBar.SetMaterialFloat("_StencilComp", 4);
            progressBar.SetMaterialFloat("_StencilOp", 0);
            progressBar.SetMaterialFloat("_StencilWriteMask", 0);
            progressBar.SetMaterialFloat("_StencilReadMask", 1);
        }

        protected void ValidateUIProperties() {
            if (!progressBar || !progressBar.GetMaterial()) return;
            progressBar.SetMaterialFloat("_UIScaling", 1);
            progressBar.SetMaterialVector("_CustomScale", new Vector4(rectTransform.rect.width, rectTransform.rect.height, 0, 0));
        }

        protected override void OnTransformParentChanged() {
            base.OnTransformParentChanged();
            MaterialMaskingValidation();
        }

        protected override void OnRectTransformDimensionsChange() {
            base.OnRectTransformDimensionsChange();
            ValidateUIProperties();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ProceduralProgressBarUI))]
        public class PPBUIEditor : Editor {
            //bool quitting = false;


            private void OnDestroy() {
                //var ppb = target as ProceduralProgressBarUI;
                //if (!ppb && !quitting) {
                //    DestroyImmediate(ppb.progressBar);
                //}
            }

            private void OnEnable() {
                var ppb = target as ProceduralProgressBarUI;
                //Application.quitting += () => quitting = true;
                //ppb.ValidateComponents();
                //Helpers.SetGizmoIconEnabled(typeof(ProceduralProgressBarUI), false);
                InternalEditorUtility.SetIsInspectorExpanded(ppb, false);
            }

            public override void OnInspectorGUI() {
                //base.OnInspectorGUI();
            }
        }
#endif
    }
}