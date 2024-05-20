using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Renge.Util;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Renge.PPB
{



    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Progress Bars/Procedural Progress Bar")]
    public partial class ProceduralProgressBar : MonoBehaviour
    {
        enum ValidationResult
        {
            Failed = 0,
            ComponentsCreated = 1,
            ComponentsOK = 2,
            MaterialCloned = 4,
            NewMaterialGenerated = 8,
            MaterialOK = 16,
            MeshGenerated = 32,
            MeshOK = 64,
        }

        #region fields


        //square
        [SerializeField] protected int squareSubdivisions = 2;

        //editor foldouts and flags
        [SerializeField] protected bool quickDocs = false;
        [SerializeField] protected bool eventsFoldout = false;

        //events
        /// <summary>
        /// Called when the progress bar's value changes. The first parameter is the new value, the second is the value delta (negative is decrease, positive is increase)
        /// </summary>
        [SerializeField] public UnityEvent<float, float> onValueChanged;

        /// <summary>
        /// Called when the progress bar is fully filled.
        /// </summary>
        [SerializeField] public UnityEvent onFilled;

        /// <summary>
        /// Called when the progress bar is fully emptied.
        /// </summary>
        [SerializeField] public UnityEvent onEmpty;

        /// <summary>
        /// Called when the progress bar's segment count increases by a full segment. The parameter is the new segment count.
        /// </summary>
        [SerializeField] public UnityEvent<int> onSegmentFilled;

        /// <summary>
        /// Called when the progress bar's segment count decreases by a full segment. The parameter is the new segment count.
        /// </summary>
        [SerializeField] public UnityEvent<int> onSegmentEmpty;

        //material and mesh storage
        [SerializeField] protected Material material;
        [SerializeField] protected Mesh mesh;

        //private/protected unserialized
        protected bool _shaderError = false;
        protected MeshRenderer _meshRenderer;
        protected MeshFilter _meshFilter;
        protected ProceduralProgressBarUI _ppbUI;
        const string SHADER_NAME = "PPB_";
        const string SHADER_NAME_BUILTIN = "BuiltIn";
        const string SHADER_NAME_URP = "URP";
        const string SHADER_NAME_HDRP = "HDRP";
        const string SHADER_NAME_UI = "UI";
        const string INSTANCED_MAT_PREFIX = "PPB_";
        protected bool _meshIsDirty = false;
#if UNITY_EDITOR
        private bool _undoRedoPerformed = false;
#endif
        private bool _validationRequired = false;

        #endregion

        #region unity callbacks

#if UNITY_EDITOR
        protected void PBPrefabInstanceUpdated(GameObject instance)
        {
            if (!this || !gameObject) return;
            var go = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
            if (instance == go)
            {
                Validate();
                ApplyAllSerializedProperties();
            }
        }
        private void OnUndoRedoPerformed()
        {
            _undoRedoPerformed = true;
        }
        private void OnAfterAssemblyReload()
        {
            Validate();
        }

        protected void OnEnable()
        {
            PrefabUtility.prefabInstanceUpdated += PBPrefabInstanceUpdated;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        protected void OnDisable()
        {
            PrefabUtility.prefabInstanceUpdated -= PBPrefabInstanceUpdated;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        private void OnValidate()
        {
            _validationRequired = true;
        }

#endif
        protected void Update()
        {
#if UNITY_EDITOR
            //this is done in place of recording the entire mesh for undo/redo
            if (_undoRedoPerformed)
            {
                RegenerateMesh();
                _undoRedoPerformed = false;
            }
            if (_validationRequired)
            {
                _validationRequired = false;
                Validate();
                ApplyAllSerializedProperties();
            }
#endif

            if (_meshIsDirty)
            {
                RegenerateMesh();
            }
        }


        protected void Start()
        {
            Validate();
        }

        protected void Reset()
        {
            Validate();

            variableWidthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 1) });

            ApplyAllSerializedProperties();
        }

        private void OnDidApplyAnimationProperties()
        {
            //this is extremely slow and needs to be optimized
            //turns out there's no other option. Thanks Unity!
            ApplyAllSerializedProperties();

            //if (!_animator) {
            //    _animator = GetComponentInParent<Animator>();
            //    //_animator.enabled = false;
            //    if (!_animator)
            //        return;

            //    var clip = _animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            //    if(_currentClip != clip) {
            //        _currentClip = clip;
            //        _currentAnimationBindings = AnimationUtility.GetCurveBindings(clip);
            //    }
            //    //EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
            //}


        }

        private void OnTransformParentChanged()
        {
            if(Validate() == ValidationResult.Failed){
                _validationRequired = true;
            }
        }

        #endregion

        internal void RefreshCanvasRenderer(CanvasRenderer cr)
        {
            cr.SetMaterial(material, null);
            cr.SetMesh(mesh);
        }

        public void PublicValidate()
        {
            Validate();
        }

        ValidationResult Validate()
        {
            //#if UNITY_EDITOR
            //            if (!HasValidGUID()) return ValidationResult.Failed;
            //#endif
            var vResult = ValidateComponents();
            if(vResult == ValidationResult.Failed) return ValidationResult.Failed;
            return vResult | ValidateMaterialAndMesh();

            ValidationResult ValidateComponents()
            {
                ValidationResult result = ValidationResult.ComponentsOK;
                //handle canvas components
                if (GetComponentInParent<Canvas>() != null
#if UNITY_EDITOR
                    || Helpers.IsAssetOnDisk(gameObject) && GetComponent<RectTransform>() != null
#endif
                    )
                {
                    //create canvas renderer for rendering custom mesh on canvas
                    _ppbUI = GetComponent<ProceduralProgressBarUI>();
                    if (!_ppbUI && !_meshRenderer && !_meshFilter)
                    {
                        //return ValidationResult.Failed;
                        result = ValidationResult.ComponentsCreated;
                        _ppbUI = gameObject.AddComponent<ProceduralProgressBarUI>();
                    }

                    //remove mesh renderer and mesh filter
                    _meshRenderer = GetComponent<MeshRenderer>();
                    _meshFilter = GetComponent<MeshFilter>();
                    Helpers.Destroy(ref _meshRenderer);
                    Helpers.Destroy(ref _meshFilter);
                    if(!_ppbUI) return ValidationResult.Failed;
                }
                //handle worldspace components
                else
                {
                    if (_meshRenderer == null)
                    {
                        _meshRenderer = GetComponent<MeshRenderer>();
                        if (_meshRenderer == null)
                        {
                            result = ValidationResult.ComponentsCreated;
                            _meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                        }
                    }

                    if (_meshFilter == null)
                    {
                        _meshFilter = GetComponent<MeshFilter>();
                        if (_meshFilter == null)
                        {
                            result = ValidationResult.ComponentsCreated;
                            _meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
                        }
                    }

                    _meshRenderer.hideFlags = HideFlags.None;
                    _meshFilter.hideFlags = HideFlags.None;

                    //remove canvasrenderer
                    _ppbUI = GetComponent<ProceduralProgressBarUI>();
                    if (_ppbUI)
                    {
                        Helpers.Destroy(ref _ppbUI);
                        var cr = GetComponent<CanvasRenderer>();
                        Helpers.Destroy(ref cr);
                    }

                }
                return result;
            }

            ValidationResult ValidateMaterialAndMesh()
            {
                bool toClone = false;
                //Material Validation
                ValidationResult materialValidationResult = ValidationResult.MaterialOK;
                //Step #1: Check if the material exists
                if (material == null)
                {
                    //Subcase #1: Material is contained within the meshrenderer
                    if (_meshRenderer != null && _meshRenderer.sharedMaterial != null)
                        material = _meshRenderer.sharedMaterial;
                    //Subcase #2: Material is contained within the UI component
                    else if (_ppbUI != null && _ppbUI.GetMaterial() != null)
                    {
                        material = _ppbUI.GetMaterial();
                    }
                    //Subcase #3: Material is completely absent, a new one needs to be generated
                    else
                    {
                        materialValidationResult = ValidationResult.NewMaterialGenerated;
                        GenerateNewMaterial();
                    }
                }
                //Step #2: Material definitely exists, ensure it is the right material
                if (!material.shader.name.Contains(GetAppropriateShaderName().shaderName))
                {
                    materialValidationResult = ValidationResult.NewMaterialGenerated;
                    GenerateNewMaterial();
                }
                //Step #2.5: if we are in prefab mode, then always clone
                else if (material.name != MaterialName()
// #if UNITY_EDITOR
//                     || EditorSceneManager.IsPreviewScene(gameObject.scene)
// #endif
                    )
                {
                    //Material is a remnant of a different object, clone it to make it unique and assign the new name
                    materialValidationResult = ValidationResult.MaterialCloned;
                    toClone = true;
                    CloneMaterial();
                }

                //Step #3: Ensure components have the material assigned
                if (_ppbUI)
                {
                    if (_ppbUI.GetMaterial() == null)
                    {
                        _ppbUI.SetMaterial(material);
                    }
                }
                else
                {
                    if (_meshRenderer.sharedMaterial == null || _meshRenderer.sharedMaterial != material)
                    {
                        _meshRenderer.sharedMaterial = material;
                    }
                }

                //if the material exists, ensure the shadererror flag is unset
                if (material)
                {
                    _shaderError = false;
                }

                //Mesh Validation
                ValidationResult meshValidationResult = ValidationResult.MeshOK;
                //Step #1: Check if mesh exists
                if (mesh == null)
                {
                    //is the mesh contained in the meshfilter?
                    if (_meshFilter != null && _meshFilter.sharedMesh != null)
                        mesh = _meshFilter.sharedMesh;
                    //canvasrenderer doesn't have a getter for meshes, so skip that case and go straight to generating a new mesh
                    else
                    {
                        meshValidationResult = ValidationResult.MeshGenerated;
                        GenerateNewMesh();
                    }
                }

                //Step #2: Is the mesh unique?
                if (toClone)
                {
                    GenerateNewMesh();
                }

                //Step #3: Ensure components have the mesh assigned
                if (_ppbUI)
                {
                    //no way to see if the canvasrenderer has a mesh assigned, so assign the mesh
                    if (mesh.indexFormat != UnityEngine.Rendering.IndexFormat.UInt16)
                    {
                        RegenerateMesh();
                    }
                    _ppbUI.SetMeshDirty();
                }
                else if (_meshFilter.sharedMesh == null)
                {
                    _meshFilter.sharedMesh = mesh;
                }

                return materialValidationResult | meshValidationResult;
            }
        }

        /// <summary>
        /// Returns the appropriate shader name as (subdirectory, shader name)
        /// </summary>
        /// <returns></returns>
        protected (string subDir, string shaderName) GetAppropriateShaderName()
        {
            string shaderName = SHADER_NAME;
            string directory = "";

            if (_ppbUI)
            {
                shaderName = $"{SHADER_NAME}{SHADER_NAME_UI}";
            }
            else if (GraphicsSettings.defaultRenderPipeline)
            {
                string rpName = GraphicsSettings.defaultRenderPipeline.GetType().Name;
                if (rpName == "HDRenderPipelineAsset")
                {
                    directory = "HDRP/";
                    shaderName = $"{SHADER_NAME}{SHADER_NAME_HDRP}";
                }
                else if (rpName == "UniversalRenderPipelineAsset")
                {
                    directory = "URP/";
                    shaderName = $"{SHADER_NAME}{SHADER_NAME_URP}";
                }
                else
                {
                    shaderName = $"{SHADER_NAME}{SHADER_NAME_BUILTIN}";
                }
            }
            else
            {
                shaderName = $"{SHADER_NAME}{SHADER_NAME_BUILTIN}";
            }
            return (directory, shaderName);
        }

        protected void GenerateNewMaterial()
        {
            bool cr = false;
            if (_ppbUI)
            {
                cr = true;
            }

            try
            {
                string[] kws = null;
                if (material)
                {
                    kws = material.shaderKeywords;
                }
                var (subDir, shaderName) = GetAppropriateShaderName();
                Shader shader = Resources.Load<Shader>(subDir + shaderName);
                material = new Material(shader);
                material.name = MaterialName();
                if (kws != null)
                {
                    material.shaderKeywords = kws;
                }
                if (!cr)
                {
#if UNITY_EDITOR
                    Undo.RecordObject(_meshRenderer, "Assign new Material");
                }
                else
                {
                    Undo.RecordObject(_ppbUI, "Assign new Material");
#endif
                }

                if (_ppbUI)
                    _ppbUI.SetMaterial(material);
                else
                {
                    _meshRenderer.sharedMaterial = material;
                }
                ApplyAllSerializedProperties();
                _shaderError = false;
            }
            catch
            {
                _shaderError = true;
            }
        }

        protected void CloneMaterial()
        {
            var kws = material.shaderKeywords;
            material = new Material(material);
            material.name = MaterialName();
            //keywords are reset on cloning, so they need to be reapplied
            material.shaderKeywords = kws;


            if (_ppbUI)
                _ppbUI.SetMaterial(material);
            else
                _meshRenderer.sharedMaterial = material;
        }

        internal Mesh GetMesh()
        {
            return mesh;
        }
        protected void MarkMeshDirty()
        {
            _meshIsDirty = true;
        }
        protected void GenerateNewMesh()
        {
            mesh = null;
            RegenerateMesh();
        }

        protected void RegenerateMesh()
        {
            bool isUI = false;
            if (_ppbUI)
                isUI = true;

            if (!_ppbUI && !_meshFilter) return;

            bool newMeshCreated = false;
            if (mesh == null)
            {
                mesh = new Mesh();
                newMeshCreated = true;
            }
            _meshIsDirty = false;

#if UNITY_EDITOR
            //Undo.RecordObject(m_mesh, "Generated Mesh");
#endif
            switch (shape)
            {
                case PBShape.Linear:
                    MeshGenerator.GenerateSquareMesh(mesh, squareSubdivisions, clockwiseFill, isUI);
                    break;
                case PBShape.Circular:
                    MeshGenerator.GenerateRingMesh(mesh, sides, radius, width, arc, autoArcOffset, autoArcOffsetThreshold, circleLength, edgeLoops, subdivisionDetail, corkScrew, faceRotation, clockwiseFill, isUI);
                    break;
                default:
                    break;

            }

            if (isUI)
                _ppbUI.SetMeshDirty();
            else if (newMeshCreated)
            {
                _meshFilter.sharedMesh = mesh;
            }
        }
        protected string MaterialName()
        {
            return INSTANCED_MAT_PREFIX + GetInstanceID();
        }

        internal Material GetMaterial() { return material; }
        public float GetMaterialFloat(string property)
        {
            return material.GetFloat(PropertyToID(property));
        }

        public void SetMaterialFloat(string property, float value)
        {
            if (property == ProgressBarProperties.Value)
            {
                Value = value;
                return;
            }
            if (property == ProgressBarProperties.SegmentCount)
            {
                SegmentCount = value;
                return;
            }
            int id = PropertyToID(property);
            material.SetFloat(id, value);
        }
        public void SetMaterialVector(string property, Vector4 value)
        {
            int id = PropertyToID(property);
            material.SetVector(id, value);
        }
        public void SetMaterialTexture(string property, Texture value)
        {
            int id = PropertyToID(property);
            material.SetTexture(id, value);
        }
        public void SetMaterialColor(string property, Color value)
        {
            int id = PropertyToID(property);
            material.SetColor(id, value);
        }

        static Dictionary<string, int> propertyIDs = new Dictionary<string, int>();
        int PropertyToID(string property)
        {
            int id;
            try
            {
                id = propertyIDs[property];
            }
            catch (KeyNotFoundException)
            {
                id = Shader.PropertyToID(property);
                propertyIDs[property] = id;
            }
            return id;
        }
    }
}
