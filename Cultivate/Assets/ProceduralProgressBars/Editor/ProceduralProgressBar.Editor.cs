using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Renge.Util;
using System.Linq;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace Renge.PPB {

    //partial class for private member access in editor
    public partial class ProceduralProgressBar : MonoBehaviour {

#if UNITY_EDITOR
        [CustomEditor(typeof(ProceduralProgressBar))]
        public class ProceduralProgressBarEditor : Editor {

            private ProceduralProgressBar ppb;
            private MeshRenderer m_meshRenderer;
            private MeshFilter m_meshFilter;
            private ProceduralProgressBarUI m_ppbUI;
            bool quitting = false;
            List<Action> changeActions;

            SerializedProperty hideCC;
            SerializedProperty quickDocs;
            SerializedProperty eventsFoldout;

            SerializedProperty shape;
            SerializedProperty sides;
            SerializedProperty radius;
            SerializedProperty width;
            SerializedProperty arc;
            SerializedProperty autoArcOffset;
            SerializedProperty autoArcOffsetThreshold;
            SerializedProperty circleLength;
            SerializedProperty edgeLoops;
            SerializedProperty subdivisionDetail;
            SerializedProperty corkScrew;
            SerializedProperty faceRotation;
            SerializedProperty clockwiseFill;

            SerializedProperty squareSubdivisions;

            SerializedProperty onValueChanged;
            SerializedProperty onFilled;
            SerializedProperty onEmpty;
            SerializedProperty onSegmentFilled;
            SerializedProperty onSegmentEmpty;

            SerializedProperty value;
            SerializedProperty segmentCount;

            #region advanced serialized properties
            SerializedProperty ratioScaling;
            SerializedProperty antiAlias;
            SerializedProperty centerFill;
            SerializedProperty borderWidth;
            SerializedProperty innerBorderWidth;
            SerializedProperty borderRadius;
            SerializedProperty borderRadiusOffset;
            SerializedProperty segmentSpacing;
            SerializedProperty innerRoundingPercent;
            SerializedProperty innerColor;
            SerializedProperty backgroundColor;
            SerializedProperty borderColor;
            SerializedProperty innerBorderColor;
            SerializedProperty overlayColor;
            SerializedProperty innerTexture;
            SerializedProperty innerTextureOpacity;
            SerializedProperty innerTextureTiling;
            SerializedProperty innerTextureOffset;
            SerializedProperty innerTextureRotation;
            SerializedProperty innerGradient;
            SerializedProperty innerGradientEnabled;
            SerializedProperty innerGradientRotation;
            SerializedProperty valueAsGradientTimeInner;
            SerializedProperty overlayTexture;
            SerializedProperty overlayTextureOpacity;
            SerializedProperty overlayTextureTiling;
            SerializedProperty overlayTextureOffset;
            SerializedProperty borderTexture;
            SerializedProperty borderTextureTiling;
            SerializedProperty borderTextureOffset;
            SerializedProperty borderTextureOpacity;
            SerializedProperty borderTextureRotation;
            SerializedProperty backgroundTexture;
            SerializedProperty backgroundTextureTiling;
            SerializedProperty backgroundTextureOffset;
            SerializedProperty backgroundGradientRotation;
            SerializedProperty backgroundTextureOpacity;
            SerializedProperty backgroundTextureRotation;
            SerializedProperty backgroundGradient;
            SerializedProperty backgroundTextureScaleWithSegments;
            SerializedProperty backgroundGradientEnabled;
            SerializedProperty valueAsGradientTimeBackground;
            SerializedProperty pulsateWhenLow;
            SerializedProperty pulseActivationThreshold;
            SerializedProperty pulseSpeed;
            SerializedProperty pulseRamp;
            SerializedProperty pulseColor;
            SerializedProperty variableWidthCurve;
            SerializedProperty adjustBorderRadiusToWidthCurve;
            SerializedProperty slant;
            SerializedProperty flipbookFPS;
            SerializedProperty innerFlipbookDim;
            SerializedProperty overlayFlipbookDim;
            SerializedProperty borderFlipbookDim;
            SerializedProperty backgroundFlipbookDim;
            SerializedProperty valueMaskOffset;
            SerializedProperty offsetTextureWithValue;
            SerializedProperty valueShadowColor;
            SerializedProperty valueShadowSize;
            SerializedProperty valueShadowFalloff;
            SerializedProperty borderShadowColor;
            SerializedProperty borderShadowSize;
            SerializedProperty borderShadowFalloff;
            SerializedProperty valueInsetShadowColor;
            SerializedProperty valueInsetShadowSize;
            SerializedProperty valueInsetShadowFalloff;
            SerializedProperty borderInsetShadowColor;
            SerializedProperty borderInsetShadowSize;
            SerializedProperty borderInsetShadowFalloff;

            //keywords
            SerializedProperty innerTextureEnabled;
            SerializedProperty overlayTextureEnabled;
            SerializedProperty borderTextureEnabled;
            SerializedProperty backgroundTextureEnabled;
            #endregion


            private void OnEnable() {
                ppb = (ProceduralProgressBar)target;
                ppb.Validate();
                //if(ppb.gameObject.scene.handle == 0) {
                //    ppb.Init();
                //}

                //coupled components that are unserialized
                m_meshRenderer = ppb._meshRenderer;
                m_meshFilter = ppb._meshFilter;
                m_ppbUI = ppb._ppbUI;

                //check if quitting
                Application.quitting += () => quitting = true;
                //PrefabUtility.prefabInstanceUpdated += (GameObject instance) => {

                //};

                //list of actions to be performed when values change
                //populated in OnInspectorGUI
                changeActions = new List<Action>();

                //disable script icon gizmo
                //Helpers.SetGizmoIconEnabled(typeof(ProceduralProgressBar), false);

                //find serialized properties for custom inspector
                quickDocs = serializedObject.FindProperty("quickDocs");
                eventsFoldout = serializedObject.FindProperty("eventsFoldout");

                shape = serializedObject.FindProperty("shape");
                sides = serializedObject.FindProperty("sides");
                radius = serializedObject.FindProperty("radius");
                width = serializedObject.FindProperty("width");
                arc = serializedObject.FindProperty("arc");
                autoArcOffset = serializedObject.FindProperty("autoArcOffset");
                autoArcOffsetThreshold = serializedObject.FindProperty("autoArcOffsetThreshold");
                circleLength = serializedObject.FindProperty("circleLength");
                edgeLoops = serializedObject.FindProperty("edgeLoops");
                subdivisionDetail = serializedObject.FindProperty("subdivisionDetail");
                corkScrew = serializedObject.FindProperty("corkScrew");
                faceRotation = serializedObject.FindProperty("faceRotation");
                clockwiseFill = serializedObject.FindProperty("clockwiseFill");

                squareSubdivisions = serializedObject.FindProperty("squareSubdivisions");

                onValueChanged = serializedObject.FindProperty("onValueChanged");
                onFilled = serializedObject.FindProperty("onFilled");
                onEmpty = serializedObject.FindProperty("onEmpty");
                onSegmentFilled = serializedObject.FindProperty("onSegmentFilled");
                onSegmentEmpty = serializedObject.FindProperty("onSegmentEmpty");

                value = serializedObject.FindProperty("pbValue");
                segmentCount = serializedObject.FindProperty("segmentCount");

                #region advanced serialized properties
                ratioScaling = serializedObject.FindProperty("ratioScaling");
                antiAlias = serializedObject.FindProperty("antiAlias");
                centerFill = serializedObject.FindProperty("centerFill");
                borderWidth = serializedObject.FindProperty("borderWidth");
                innerBorderWidth = serializedObject.FindProperty("innerBorderWidth");
                borderRadius = serializedObject.FindProperty("borderRadius");
                borderRadiusOffset = serializedObject.FindProperty("borderRadiusOffset");
                segmentSpacing = serializedObject.FindProperty("segmentSpacing");
                innerRoundingPercent = serializedObject.FindProperty("innerRoundingPercent");
                innerColor = serializedObject.FindProperty("innerColor");
                backgroundColor = serializedObject.FindProperty("backgroundColor");
                borderColor = serializedObject.FindProperty("borderColor");
                innerBorderColor = serializedObject.FindProperty("innerBorderColor");
                overlayColor = serializedObject.FindProperty("overlayColor");
                innerTexture = serializedObject.FindProperty("innerTexture");
                innerTextureOpacity = serializedObject.FindProperty("innerTextureOpacity");
                innerTextureTiling = serializedObject.FindProperty("innerTextureTiling");
                innerTextureOffset = serializedObject.FindProperty("innerTextureOffset");
                innerTextureRotation = serializedObject.FindProperty("innerTextureRotation");
                innerGradient = serializedObject.FindProperty("innerGradient");
                innerGradientEnabled = serializedObject.FindProperty("innerGradientEnabled");
                innerGradientRotation = serializedObject.FindProperty("innerGradientRotation");
                valueAsGradientTimeInner = serializedObject.FindProperty("valueAsGradientTimeInner");
                overlayTexture = serializedObject.FindProperty("overlayTexture");
                overlayTextureOpacity = serializedObject.FindProperty("overlayTextureOpacity");
                overlayTextureTiling = serializedObject.FindProperty("overlayTextureTiling");
                overlayTextureOffset = serializedObject.FindProperty("overlayTextureOffset");
                borderTexture = serializedObject.FindProperty("borderTexture");
                borderTextureTiling = serializedObject.FindProperty("borderTextureTiling");
                borderTextureOffset = serializedObject.FindProperty("borderTextureOffset");
                borderTextureOpacity = serializedObject.FindProperty("borderTextureOpacity");
                borderTextureRotation = serializedObject.FindProperty("borderTextureRotation");
                backgroundTexture = serializedObject.FindProperty("backgroundTexture");
                backgroundTextureTiling = serializedObject.FindProperty("backgroundTextureTiling");
                backgroundTextureOffset = serializedObject.FindProperty("backgroundTextureOffset");
                backgroundGradientRotation = serializedObject.FindProperty("backgroundGradientRotation");
                backgroundTextureOpacity = serializedObject.FindProperty("backgroundTextureOpacity");
                backgroundTextureRotation = serializedObject.FindProperty("backgroundTextureRotation");
                backgroundGradient = serializedObject.FindProperty("backgroundGradient");
                backgroundTextureScaleWithSegments = serializedObject.FindProperty("backgroundTextureScaleWithSegments");
                backgroundGradientEnabled = serializedObject.FindProperty("backgroundGradientEnabled");
                valueAsGradientTimeBackground = serializedObject.FindProperty("valueAsGradientTimeBackground");
                pulsateWhenLow = serializedObject.FindProperty("pulsateWhenLow");
                pulseActivationThreshold = serializedObject.FindProperty("pulseActivationThreshold");
                pulseSpeed = serializedObject.FindProperty("pulseSpeed");
                pulseRamp = serializedObject.FindProperty("pulseRamp");
                pulseColor = serializedObject.FindProperty("pulseColor");
                variableWidthCurve = serializedObject.FindProperty("variableWidthCurve");
                adjustBorderRadiusToWidthCurve = serializedObject.FindProperty("adjustBorderRadiusToWidthCurve");
                slant = serializedObject.FindProperty("slant");
                flipbookFPS = serializedObject.FindProperty("flipbookFPS");
                innerFlipbookDim = serializedObject.FindProperty("innerFlipbookDim");
                overlayFlipbookDim = serializedObject.FindProperty("overlayFlipbookDim");
                borderFlipbookDim = serializedObject.FindProperty("borderFlipbookDim");
                backgroundFlipbookDim = serializedObject.FindProperty("backgroundFlipbookDim");
                valueMaskOffset = serializedObject.FindProperty("valueMaskOffset");
                offsetTextureWithValue = serializedObject.FindProperty("offsetTextureWithValue");
                valueShadowColor = serializedObject.FindProperty("valueShadowColor");
                valueShadowSize = serializedObject.FindProperty("valueShadowSize");
                valueShadowFalloff = serializedObject.FindProperty("valueShadowFalloff");
                borderShadowColor = serializedObject.FindProperty("borderShadowColor");
                borderShadowSize = serializedObject.FindProperty("borderShadowSize");
                borderShadowFalloff = serializedObject.FindProperty("borderShadowFalloff");
                valueInsetShadowColor = serializedObject.FindProperty("valueInsetShadowColor");
                valueInsetShadowSize = serializedObject.FindProperty("valueInsetShadowSize");
                valueInsetShadowFalloff = serializedObject.FindProperty("valueInsetShadowFalloff");
                borderInsetShadowColor = serializedObject.FindProperty("borderInsetShadowColor");
                borderInsetShadowSize = serializedObject.FindProperty("borderInsetShadowSize");
                borderInsetShadowFalloff = serializedObject.FindProperty("borderInsetShadowFalloff");

                innerTextureEnabled = serializedObject.FindProperty("innerTextureEnabled");
                overlayTextureEnabled = serializedObject.FindProperty("overlayTextureEnabled");
                borderTextureEnabled = serializedObject.FindProperty("borderTextureEnabled");
                backgroundTextureEnabled = serializedObject.FindProperty("backgroundTextureEnabled");
                #endregion

                CSVManager.LoadCSVFromResources("tooltips");
            }

            private void OnDestroy() {
                //remove coupled components when component itself is being removed
                if (ppb == null && !quitting) {
                    DestroyImmediate(m_meshRenderer);
                    DestroyImmediate(m_meshFilter);
                    DestroyImmediate(m_ppbUI);
                }
            }

            public override void OnInspectorGUI() {
                //if (!ppb.HasValidGUID()) {
                //    EditorGUILayout.HelpBox("To change the settings for this Progress Bar, please open the prefab.", MessageType.Info);
                //    return;
                //}

                //base.OnInspectorGUI();
                serializedObject.Update();
                //GUIStyle dbg_btn = EditorStyles.miniButton;
                //dbg_btn.padding = new RectOffset(1, 1, 1, 1);
                //GUILayout.BeginArea(new Rect(Screen.width - 150, 0, 16, 16));
                //GUILayout.Button(EditorGUIUtility.IconContent("console.infoicon.sml"), dbg_btn, GUILayout.Width(16), GUILayout.Height(16));
                //GUILayout.EndArea();

                //if (onDisk && GUILayout.Button("Find All Material Dependencies")) {
                //    Debug.Log(Helpers.FindAllReferencesInProject(ppb.m_material).Length);
                //}
                //if (onDisk && GUILayout.Button("Clear unused generated assets")) {
                //    var allAssets = Helpers.FindAllAssetsInDirectory("t:material t:mesh", ppb.ValidateAndGetPrefabCachePath());
                //    Debug.Log(allAssets.Length);
                //    foreach (string guid in allAssets) {
                //        if (Helpers.FindAllReferencesInProject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid))).Length == 0) {
                //            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
                //        }
                //    }
                //}

                //if (GUILayout.Button("Manual Validation")) {
                //    ppb.Validate();
                //}

                if (ppb._shaderError) {
                    EditorGUILayout.HelpBox("When using HDRP/URP, make sure to import the appropriate package from the subfolder \"URP and HDRP\"", MessageType.Error);
                }

                EditorGUILayout.LabelField("Basic", EditorStyles.boldLabel);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(value, ppb.UpdateValue);
                    PropertyField(segmentCount, ppb.UpdateSegmentCount);
                    PropertyField(innerColor, ppb.UpdateInnerColor);
                    //ppb.Value = EditorGUILayout.FloatField("Value", ppb.Value);


                    //ppb.SegmentCount = EditorGUILayout.IntField("Segment Count", ppb.SegmentCount);
                }

                var mainHeader = new GUIStyle(EditorStyles.boldLabel);
                mainHeader.margin = new RectOffset(0, 0, 10, 10);
                mainHeader.wordWrap = true;
                mainHeader.fontSize = 16;
                mainHeader.fontStyle = FontStyle.Bold;
                mainHeader.alignment = TextAnchor.MiddleCenter;

                EditorGUILayout.LabelField("Shape Customization", mainHeader);

                PropertyField(shape, ppb.UpdateShape, "Change the shape of the progress bar. Different shapes reveal different settings.");

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    if (ppb.shape == PBShape.Circular) {
                        PropertyField(sides, ppb.UpdateSides, "How many sides should the circle have? 3 = Triangular, 4 = Square, 5 = Pentagon, >50 = Circle");
                        PropertyField(radius, ppb.UpdateRadius, "The radius of the circle.");
                        PropertyField(width, ppb.UpdateWidth, "The width of the circle.");
                        PropertyField(arc, ppb.UpdateArc, "The arc of the circle. 0 = Full circle, .5 = Half circle, 1 = Nothing");
                        PropertyField(autoArcOffset, ppb.UpdateAutoArcOffset, "When this is enabled, the progress bar's offset is adjusted automatically to center itself when the arc exceeds the value in 'Auto Arc Offset Threshold' below");
                        PropertyField(autoArcOffsetThreshold, ppb.UpdateAutoArcOffsetThreshold, "The threshold of the arc value at which exceeding this point the progress bar begins to center itself");
                        PropertyField(circleLength, ppb.UpdateCircleLength, "The length of the circle. 1 is Minimum, The circle can be extended as long as wished, limited only by performance.");
                        PropertyField(edgeLoops, ppb.UpdateEdgeLoops, "How many edge loops should the circle have? 2 is Minimum (outer and inner radius), 10 is recommended, adjust to your needs. Look at the wireframe mode to see exactly what this does.");
                        PropertyField(subdivisionDetail, ppb.UpdateSubdivisionDetail, "The 3d model is subdivided to maintain detail when the circle has a small amount of relative sides. This setting controls this detail. 1 is default amount of subdivisions. 0 disables subdivisions completely.");
                        PropertyField(corkScrew, ppb.UpdateCorkScrew, "The distance the circle is twisted in one revolution.");
                        //PropertyField(faceRotation, ppb.UpdateFaceRotation, "The rotation of the faces of the circle.");
                        PropertyField(clockwiseFill, ppb.UpdateClockwiseFill, "Fill the circle progress bar clockwise or counter-clockwise.");
                    } else if (ppb.shape == PBShape.Linear) {
                        //PropertyField(squareSubdivisions, ppb.UpdateSquareSubdivisions, "How many subdivisions should the square have?");
                        PropertyField(clockwiseFill, ppb.UpdateClockwiseFill, "Fill the progress bar left to right (true) or right to left (false)");
                        EditorGUILayout.HelpBox("New features coming soon!", MessageType.Info);
                    }

                    GUIStyle verticesStyle = new GUIStyle(EditorStyles.boldLabel);
                    int vertexCount = ppb.mesh != null ? ppb.mesh.vertexCount : 0;
                    if (vertexCount < 1000) {
                        verticesStyle.normal.textColor = Color.green;

                    } else if (vertexCount < 10000) {
                        verticesStyle.normal.textColor = Color.yellow;
                    } else {
                        verticesStyle.normal.textColor = Color.red;
                    }
                    EditorGUILayout.LabelField("Vertex Count: " + vertexCount, verticesStyle);

                    EditorGUILayout.Space(10);
                }
                //shader property modification
                EditorGUILayout.LabelField("Material Customization", mainHeader);

                GUIStyle sectionHeader = new GUIStyle(EditorStyles.boldLabel);

                GUILayout.Label("General", sectionHeader);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(segmentSpacing, ppb.UpdateSegmentSpacing, "The spacing between each segment");
                    PropertyField(borderWidth, ppb.UpdateBorderWidth, "The size of the border");
                    PropertyField(borderRadius, ppb.UpdateBorderRadius, "The rounding of the progress bar");
                    PropertyField(borderRadiusOffset, ppb.UpdateBorderRadiusOffset, "Control the rounding of each corner individually. (tr, tl, bl, br)");
                    PropertyField(innerRoundingPercent, ppb.UpdateInnerRoundingPercent, "The rounding of the inner progress bar");
                    PropertyField(variableWidthCurve, ppb.UpdateVariableWidthCurve, "The curve representing the width at any given point along the progress bar. 1 is full width, 0 is invisible.");
                    PropertyField(slant, ppb.UpdateSlant);
                    PropertyField(centerFill, ppb.UpdateCenterFill, "Fill the progress bar from the center instead of the left.");
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Value", sectionHeader);

                EditorGUILayout.EndHorizontal();

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(innerTextureEnabled, ppb.UpdateInnerTextureEnabled);
                    if (innerTextureEnabled.boolValue) {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                            PropertyField(innerTexture, ppb.UpdateInnerTexture, "The texture to use for the inner progress bar");
                            PropertyField(innerTextureTiling, ppb.UpdateInnerTextureTiling, "The tiling of the inner texture");
                            PropertyField(innerTextureOffset, ppb.UpdateInnerTextureOffset, "The offset of the inner texture");
                            PropertyField(innerTextureOpacity, ppb.UpdateInnerTextureOpacity, "The opacity of the inner texture");
                            PropertyField(innerTextureRotation, ppb.UpdateInnerTextureRotation, "The rotation of the inner texture");
                            PropertyField(offsetTextureWithValue, ppb.UpdateOffsetTextureWithValue, "Have the texture move with the value");
                            PropertyField(innerFlipbookDim, ppb.UpdateInnerFlipbookDim, "When using an animated texture, input the columns and rows of the animation");

                            EditorGUILayout.Space();

                            PropertyField(innerGradientEnabled, ppb.UpdateInnerGradientEnabled);
                            if (innerGradientEnabled.boolValue) {
                                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                                    PropertyField(innerGradient, ppb.UpdateInnerGradient);
                                    PropertyField(innerGradientRotation, ppb.UpdateInnerGradientRotation, "Rotation of the gradient");
                                    PropertyField(valueAsGradientTimeInner, ppb.UpdateValueAsGradientTimeInner, "Sample the gradient with the value as percent. E.g. Value = 0.5, Gradient is sampled at 0.5 for the whole progress bar.");
                                }
                            }
                        }
                    }
                    //if (ppb.shape == PBShape.Linear) {
                        EditorGUILayout.Space();

                        PropertyField(valueMaskOffset, ppb.UpdateValueMaskOffset, "Offset the mask used to display the value in the x and y direction");
                    //}

                    EditorGUILayout.Space();

                    PropertyField(innerBorderWidth, ppb.UpdateInnerBorderWidth, "Similar to the outer border, the inner border appears on the value portion");
                    PropertyField(innerBorderColor, ppb.UpdateInnerBorderColor, "The color of the inner border");

                    EditorGUILayout.Space();
                    PropertyField(pulsateWhenLow, ppb.UpdatePulsateWhenLow);
                    if (pulsateWhenLow.boolValue) {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                            PropertyField(pulseActivationThreshold, ppb.UpdatePulseActivationThreshold, "Threshold in percent when pulsation should trigger.");
                            PropertyField(pulseSpeed, ppb.UpdatePulseSpeed, "The speed of pulsating.");
                            PropertyField(pulseRamp, ppb.UpdatePulseRamp, "Value difference between barely pulsating and maximum pulsating.");
                            PropertyField(pulseColor, ppb.UpdatePulseColor, "The color the progress bar should pulse with");
                        }
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Background", sectionHeader);
                PropertyField(backgroundColor, ppb.UpdateBackgroundColor);
                EditorGUILayout.EndHorizontal();
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(backgroundTextureEnabled, ppb.UpdateBackgroundTextureEnabled);
                    if (backgroundTextureEnabled.boolValue) {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                            PropertyField(backgroundTexture, ppb.UpdateBackgroundTexture, "The texture for the background of the progress bar");
                            PropertyField(backgroundTextureTiling, ppb.UpdateBackgroundTextureTiling, "The tiling of the background texture");
                            PropertyField(backgroundTextureOffset, ppb.UpdateBackgroundTextureOffset, "The offset of the background texture");
                            PropertyField(backgroundTextureOpacity, ppb.UpdateBackgroundTextureOpacity, "The opacity of the background texture");
                            PropertyField(backgroundTextureRotation, ppb.UpdateBackgroundTextureRotation, "The rotation of the background texture");
                            PropertyField(backgroundFlipbookDim, ppb.UpdateBackgroundFlipbookDim, "When using an animated texture, input the columns and rows of the animation");

                            EditorGUILayout.Space();
                            PropertyField(backgroundGradientEnabled, ppb.UpdateBackgroundGradientEnabled);
                            if (backgroundGradientEnabled.boolValue) {
                                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                                    PropertyField(backgroundGradient, ppb.UpdateBackgroundGradient);
                                    PropertyField(backgroundGradientRotation, ppb.UpdateBackgroundGradientRotation, "Rotation of the gradient");
                                    PropertyField(valueAsGradientTimeBackground, ppb.UpdateValueAsGradientTimeBackground, "Sample the gradient with the value as percent. E.g. Value = 0.5, Gradient is sampled at 0.5 for the whole progress bar background.");
                                }
                            }
                        }
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Border", sectionHeader);
                PropertyField(borderColor, ppb.UpdateBorderColor);
                EditorGUILayout.EndHorizontal();
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(borderTextureEnabled, ppb.UpdateBorderTextureEnabled);
                    if (borderTextureEnabled.boolValue) {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                            PropertyField(borderTexture, ppb.UpdateBorderTexture, "The texture for the progress bar border");
                            PropertyField(borderTextureTiling, ppb.UpdateBorderTextureTiling, "The tiling of the border texture");
                            PropertyField(borderTextureOffset, ppb.UpdateBorderTextureOffset, "The offset of the border texture");
                            PropertyField(borderTextureOpacity, ppb.UpdateBorderTextureOpacity, "The opacity of the border texture");
                            PropertyField(borderTextureRotation, ppb.UpdateBorderTextureRotation, "The rotation of the border texture");
                            PropertyField(borderFlipbookDim, ppb.UpdateBorderFlipbookDim, "When using an animated texture, input the columns and rows of the animation");
                        }
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Overlay", sectionHeader);
                PropertyField(overlayColor, ppb.UpdateOverlayColor);
                EditorGUILayout.EndHorizontal();
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(overlayTextureEnabled, ppb.UpdateOverlayTextureEnabled);
                    if (overlayTextureEnabled.boolValue) {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                            PropertyField(overlayTexture, ppb.UpdateOverlayTexture, "The texture for the progress bar overlay");
                            PropertyField(overlayTextureTiling, ppb.UpdateOverlayTextureTiling, "The tiling of the overlay texture");
                            PropertyField(overlayTextureOffset, ppb.UpdateOverlayTextureOffset, "The offset of the overlay texture");
                            PropertyField(overlayTextureOpacity, ppb.UpdateOverlayTextureOpacity, "The opacity of the overlay texture");
                            PropertyField(overlayFlipbookDim, ppb.UpdateOverlayFlipbookDim, "When using an animated texture, input the columns and rows of the animation");
                        }
                    }
                }

                EditorGUILayout.Space();
                GUILayout.Label("Shadows", sectionHeader);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(valueShadowSize, ppb.UpdateValueShadowSize, "The size of the drop shadow caused by the inner progress bar");
                    PropertyField(valueShadowColor, ppb.UpdateValueShadowColor, "The color of the value shadow");
                    PropertyField(valueShadowFalloff, ppb.UpdateValueShadowFalloff, "The falloff of the value shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

                    EditorGUILayout.Space();

                    PropertyField(valueInsetShadowSize, ppb.UpdateValueInsetShadowSize, "The size of the inset shadow of the inner progress bar");
                    PropertyField(valueInsetShadowColor, ppb.UpdateValueInsetShadowColor, "The color of the value inset shadow");
                    PropertyField(valueInsetShadowFalloff, ppb.UpdateValueInsetShadowFalloff, "The falloff of the value inset shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

                    EditorGUILayout.Space();

                    PropertyField(borderShadowSize, ppb.UpdateBorderShadowSize, "The size of the shadow from the border going into the progress bar");
                    PropertyField(borderShadowColor, ppb.UpdateBorderShadowColor, "The color of the border shadow");
                    PropertyField(borderShadowFalloff, ppb.UpdateBorderShadowFalloff, "The falloff of the border shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

                    EditorGUILayout.Space();

                    PropertyField(borderInsetShadowSize, ppb.UpdateBorderInsetShadowSize, "The size of the border inset shadow");
                    PropertyField(borderInsetShadowColor, ppb.UpdateBorderInsetShadowColor, "The color of the border inset shadow");
                    PropertyField(borderInsetShadowFalloff, ppb.UpdateBorderInsetShadowFalloff, "The falloff of the border inset shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");
                }

                EditorGUILayout.Space();
                GUILayout.Label("Other", sectionHeader);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    PropertyField(flipbookFPS, ppb.UpdateFlipbookFPS, "Frames per second of animated textures, if they are being used");
                    PropertyField(antiAlias, ppb.UpdateAntiAlias, "Enable built-in anti-aliasing");
                    PropertyField(ratioScaling, ppb.UpdateRatioScaling, "When enabled, this ensures that one side of the progress bar is always internally one unit long, making it easier to scale the progress bar to any size");
                }


                EditorGUILayout.Space();

                eventsFoldout.boolValue = EditorGUILayout.Foldout(eventsFoldout.boolValue, "Events");
                if (eventsFoldout.boolValue) {
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                        PropertyField(onValueChanged);
                        EditorGUILayout.PropertyField(onFilled);
                        EditorGUILayout.PropertyField(onEmpty);
                        EditorGUILayout.PropertyField(onSegmentFilled);
                        EditorGUILayout.PropertyField(onSegmentEmpty);

                    }
                }
                EditorGUILayout.Space();


                quickDocs.boolValue = EditorGUILayout.Foldout(quickDocs.boolValue, "Quick Help");
                if (quickDocs.boolValue) {
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                        EditorGUILayout.HelpBox("Change progress bar features via script. <progress bar>.Value = 0.1f; etc.", MessageType.Info);
                        EditorGUILayout.HelpBox("Get the Component with GetComponent<ProceduralProgressBar>", MessageType.Info);
                        EditorGUILayout.HelpBox("Don't forget to use the namespace Renge.PPB when scripting", MessageType.Info);
                        //EditorGUILayout.LabelField(, EditorStyles.helpBox);
                    }
                }

                if (serializedObject.ApplyModifiedProperties()) {
                    //#if UNITY_EDITOR
                    //Undo.RecordObject(ppb, "Mesh Renderer Update");

                    Undo.RecordObject(ppb.material, "Material update");
                    //#endif
                    foreach (Action action in changeActions)
                        action();
                    changeActions.Clear();
                }
                //base.OnInspectorGUI();
            }


            void PropertyField(SerializedProperty property, Action changeAction = null, string tooltip = "") {
                EditorGUI.BeginChangeCheck();
                string language = "English";

                string displayName = CSVManager.GetCell(property.name, language + " Name", "English Name");
                string tooltipText = CSVManager.GetCell(property.name, language, "English");
                string tooltipScriptRef = CSVManager.GetCell(property.name + "_script", language, "English");

                string tooltipCombo = tooltipText + (tooltipScriptRef == "" ? "" : $"\n\n{CSVManager.GetCell("scriptReference", language + " Name" , "English Name")}:\n\n{tooltipScriptRef}");

                EditorGUILayout.PropertyField(property, new GUIContent() { text = displayName == "" ? property.displayName : displayName, tooltip = tooltipCombo});
                if (EditorGUI.EndChangeCheck() && changeAction != null) {
                    changeActions.Add(changeAction);
                }
            }

        }

        #region deprecated
        //[MenuItem("Assets/Procedural Progress Bar/Clear Unused Asset Cache (can take a while)", false, 20)]
        //this method is deprecated
        static private void ClearCache(){
            if (Helpers.IsSceneDirty()) {
                throw new Exception("Unsaved Scene detected! Please save your scene before clearing the Progress Bar cache");
            }
            string cachePath = Helpers.ValidateAndGetPrefabCachePath();
            var materialAssets = Helpers.FindAllAssetsInDirectory("t:material", cachePath + "/materials");
            int counter = 0;
            List<string> deletedNames = new List<string>();
            var matsWithReferences = Helpers.FindAllPBMatReferences(materialAssets);
            foreach (var item in matsWithReferences) {
                if (!item.Value) {
                    deletedNames.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(item.Key)).name);
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(item.Key));
                    counter++;
                }
            }

            AssetDatabase.FindAssets("t:mesh", new string[] { cachePath + "/meshes" }).ToList().ForEach(guid => {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                foreach (var name in deletedNames) {
                    if (assetPath.Contains(name)) {
                        AssetDatabase.DeleteAsset(assetPath);
                        counter++;
                        break;
                    }
                }
            });
            AssetDatabase.FindAssets("t:texture2d", new string[] { cachePath + "/textures" }).ToList().ForEach(guid => {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                foreach (var name in deletedNames) {
                    if (assetPath.Contains(name)) {
                        AssetDatabase.DeleteAsset(assetPath);
                        counter++;
                        break;
                    }
                }
            });

            Debug.Log($"Removed {counter} unused Assets");
        }
        #endregion

        [MenuItem("GameObject/2D Object/Progress Bar/Linear Progress Bar", false, 10)]
        static private void CreateLinearProgressBar(MenuCommand menuCommand) {
            var context = (menuCommand.context as GameObject);
            if (context && context.GetComponentInParent<Canvas>() != null || !context && PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot?.GetComponent<RectTransform>() != null){ 
                CreateLinearUIProgressBar(menuCommand);
                return;
            }
            var go = GenPBMenu("Linear Progress Bar", menuCommand, typeof(ProceduralProgressBar));
            go.transform.localScale = new Vector3(1, .2f, 1);
            var pb = go.GetComponent<ProceduralProgressBar>();
            pb.Shape = PBShape.Linear;
        }

        [MenuItem("GameObject/2D Object/Progress Bar/Circular Progress Bar", false, 10)]
        static private void CreateCircularProgressBar(MenuCommand menuCommand) {
            var context = (menuCommand.context as GameObject);
            if (context && context.GetComponentInParent<Canvas>() != null || !context && PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot?.GetComponent<RectTransform>() != null) {
                CreateCircularUIProgressBar(menuCommand);
                return;
            }

            var go = GenPBMenu("Circular Progress Bar", menuCommand, typeof(ProceduralProgressBar));

            var pb = go.GetComponent<ProceduralProgressBar>();
            pb.Shape = PBShape.Circular;
            pb.BorderRadius = 0.08f;

        }

        [MenuItem("GameObject/UI/Progress Bar/Linear Progress Bar", false, 10)]
        static private void CreateLinearUIProgressBar(MenuCommand menuCommand) {
            var go = GenPBMenu("Linear UI Progress Bar", menuCommand, typeof(RectTransform));
            if (go.GetComponentInParent<Canvas>() == null) {
                var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                var canvas = canvasGO.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                if (go.transform.parent)
                    GameObjectUtility.SetParentAndAlign(canvasGO, go.transform.parent.gameObject);
                GameObjectUtility.SetParentAndAlign(go, canvasGO);
            }
            if (FindObjectOfType<EventSystem>() == null) {
                var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            //go.transform.localScale = new Vector3(1, .2f, 1);
            var pb = go.AddComponent<ProceduralProgressBar>();
            pb.Shape = PBShape.Linear;
            // pb.BorderRadius = 10;
            // pb.BorderWidth = 2;
            // pb.InnerBorderWidth = 2;
            // pb.ValueShadowSize = 5;
            // pb.ValueInsetShadowSize = 5;
            // pb.BorderShadowSize = 5;
            // pb.BorderInsetShadowSize = 5;
        }

        [MenuItem("GameObject/UI/Progress Bar/Circular Progress Bar", false, 10)]
        static private void CreateCircularUIProgressBar(MenuCommand menuCommand) {
            var go = GenPBMenu("Circular UI Progress Bar", menuCommand, typeof(RectTransform));
            if (go.GetComponentInParent<Canvas>() == null) {
                var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                var canvas = canvasGO.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                if (go.transform.parent)
                    GameObjectUtility.SetParentAndAlign(canvasGO, go.transform.parent.gameObject);
                GameObjectUtility.SetParentAndAlign(go, canvasGO);
            }
            if (FindObjectOfType<EventSystem>() == null) {
                var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            var pb = go.AddComponent<ProceduralProgressBar>();
            pb.Shape = PBShape.Circular;
            pb.BorderRadius = 0.08f;
        }

        static GameObject GenPBMenu(string name, MenuCommand menuCommand, params Type[] components) {
            GameObject go = new GameObject(name, components);

            //we need to do some extra work if we are in a prefab scene
            GameObject context = menuCommand.context as GameObject;
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (context == null && prefabStage != null) {
                context = prefabStage.prefabContentsRoot;
            }
            GameObjectUtility.SetParentAndAlign(go, context);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
            return go;
        }

#endif

//        private void ApplyPersistentData() {
//#if UNITY_EDITOR
//            string persistentMatDataString = ProjectPersistentData.GetString(m_material.name);
//            ProgressBarEditorData matData = null;
//            if (persistentMatDataString == null) return;
//            matData = JsonConvert.DeserializeObject<ProgressBarEditorData>(persistentMatDataString);

//            m_material.SetTexture(ProgressBarProperties.InnerGradient, matData.innerGradient.ToTexture2D());
//            m_material.SetTexture(ProgressBarProperties.VariableWidthCurve, matData.widthCurve.ToTexture2D());
//            m_material.SetTexture(ProgressBarProperties.BackgroundGradient, matData.backgroundGradient.ToTexture2D());
//#endif
//        }

        private bool HasSerializedValueChanged() {
            //if(Value != )
            return false;
        }
    }
}