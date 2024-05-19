#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using Renge.Util;
using UnityEngine.UI;
using System.Linq;

namespace Renge.PPB {
    class ProgressBarEditorData {
        public Gradient innerGradient;
        public AnimationCurve widthCurve;
        public Gradient backgroundGradient;

        public ProgressBarEditorData() {
            innerGradient = new Gradient();
            widthCurve = new AnimationCurve();
            backgroundGradient = new Gradient();
        }
    }

    public class ProceduralProgressBarGUI : ShaderGUI {

        MaterialEditor m_materialEditor;
        Material m_targetMat;
        ProgressBarEditorData m_persistentData;
        GUIStyle mainHeader;
        GUIStyle sectionHeader;

        string GetIDString() {
            return m_targetMat.name;
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
            //m_materialEditor = materialEditor;
            //m_targetMat = materialEditor.target as Material;
            
            //try {
            //    m_persistentData = JsonConvert.DeserializeObject<ProgressBarEditorData>(ProjectPersistentData.GetString(GetIDString()));
            //} catch { } finally {
            //    if (m_persistentData == null) {
            //        m_persistentData = new ProgressBarEditorData();
            //    }
            //}


            EditorGUI.BeginDisabledGroup(true);
            base.OnGUI(materialEditor, properties);
            EditorGUI.EndDisabledGroup();

            //InitStyles();
            
            //ShaderPropertiesGUI(properties);

        }

        void ShaderProperty(string property, MaterialProperty[] props, string tooltip = "", bool hasName = true) {
            m_materialEditor.ShaderProperty(FindProperty(property, props), new GUIContent() { text = hasName ? PropertyName(property) : "", tooltip = tooltip });
        }

        private bool ShaderPropertyToggle(string property) {
            bool enabled = m_targetMat.GetFloat(property) > 0;
            EditorGUI.BeginChangeCheck();
            enabled = EditorGUILayout.Toggle(PropertyName(property), enabled);
            if (EditorGUI.EndChangeCheck()) {
                if (enabled) {
                    m_targetMat.SetFloat(property, 1);
                } else {
                    m_targetMat.SetFloat(property, 0);
                }
            }
            return enabled;
        }

        string PropertyName (string property) {
            string name = property.Replace("_", "");
            name = Helpers.AddSpacesToSentence(name, true);
            return name;
        }

        private void InitStyles() {
            mainHeader = new GUIStyle(EditorStyles.boldLabel);
            mainHeader.margin = new RectOffset(0, 0, 20, 20);
            mainHeader.wordWrap = true;
            mainHeader.fontSize = 16;
            mainHeader.fontStyle = FontStyle.Bold;
            mainHeader.alignment = TextAnchor.MiddleCenter;

            sectionHeader = new GUIStyle(EditorStyles.boldLabel);
        }


        //void ShaderPropertiesGUI(MaterialProperty[] props) {
        //    //m_materialEditor.ShaderProperty(FindProperty("_UseUIAlphaClip", props), new GUIContent() { text = "UseUIAlphaClip" });
        //    bool isCircle = Array.IndexOf(m_targetMat.shaderKeywords, "SHAPE_CIRCULAR") != -1;
        //    GUILayout.Label("Procedural Progress Bar Customization", mainHeader);
        //    GUILayout.Label("General", sectionHeader);
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        //ShaderProperty(ProgressBarProperties.Value, props, "The value of the progress bar. 0 is empty, <segmentCount> is full.");
        //        ShaderProperty(ProgressBarProperties.CenterFill, props, "Fill the progress bar from the center instead of the left.");
        //        //if (isCircle) {
        //        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        //        ShaderProperty(ProgressBarProperties.Radius, props);
        //        //        ShaderProperty(ProgressBarProperties.Arc, props);
        //        //        ShaderProperty(ProgressBarProperties.Width, props);
        //        //        ShaderProperty(ProgressBarProperties.CircleLength, props);
        //        //    }
        //        //}
        //        //ShaderProperty(ProgressBarProperties.SegmentCount, props, "The number of segments making up the progress bar");
        //        ShaderProperty(ProgressBarProperties.SegmentSpacing, props, "The spacing between each segment");
        //        ShaderProperty(ProgressBarProperties.BorderWidth, props, "The size of the border");
        //        ShaderProperty(ProgressBarProperties.BorderRadius, props, "The rounding of the progress bar");
        //        ShaderProperty(ProgressBarProperties.BorderRadiusOffset, props, "Control the rounding of each corner individually. (tr, tl, bl, br)");
        //        ShaderProperty(ProgressBarProperties.InnerRoundingPercent, props, "The rounding of the inner progress bar");
        //        EditorGUI.BeginChangeCheck();
        //        m_persistentData.widthCurve = EditorGUILayout.CurveField(new GUIContent() { text = "Width Curve", tooltip = "The curve representing the width at any given point along the progress bar. 1 is full width, 0 is invisible."}, m_persistentData.widthCurve);
        //        if (EditorGUI.EndChangeCheck()) {
        //            var tex = m_persistentData.widthCurve.ToTexture2D();
        //            if (AssetDatabase.Contains(m_targetMat)) {
        //                string assetPath = Helpers.ValidateAndGetPrefabCachePath() + "/textures/" + GetIDString() + "_widthCurve.asset";
        //                FileManager.AddToAssetCreationQueue(tex, assetPath);
        //            }
        //            m_targetMat.SetTexture(ProgressBarProperties.VariableWidthCurve, tex);
        //            string ser = JsonConvert.SerializeObject(m_persistentData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Converters = new[] { new ColorConverter() } });
        //            ProjectPersistentData.SetString(GetIDString(), ser);
        //            //EditorPrefs.SetString(GetIDString(), ser);
        //        }
        //        ShaderProperty(ProgressBarProperties.Slant, props);
        //    }

        //    EditorGUILayout.Space();
        //    EditorGUILayout.BeginHorizontal();
        //    GUILayout.Label("Value", sectionHeader);
        //    ShaderProperty(ProgressBarProperties.InnerColor, props, "", false);
        //    EditorGUILayout.EndHorizontal();

        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        bool innerTextureEnabled = Array.IndexOf(m_targetMat.shaderKeywords, "INNER_TEXTURE_ON") != -1;
        //        EditorGUI.BeginChangeCheck();
        //        innerTextureEnabled = EditorGUILayout.Toggle("Inner Texture Enabled", innerTextureEnabled);
        //        if (EditorGUI.EndChangeCheck()) {
        //            if (innerTextureEnabled) {
        //                m_targetMat.EnableKeyword("INNER_TEXTURE_ON");
        //            } else {
        //                m_targetMat.DisableKeyword("INNER_TEXTURE_ON");
        //            }
        //        }
        //        if (innerTextureEnabled) {
        //            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                ShaderProperty(ProgressBarProperties.InnerTexture, props, "The texture to use for the inner progress bar");
        //                ShaderProperty(ProgressBarProperties.InnerTextureTiling, props, "The tiling of the inner texture");
        //                ShaderProperty(ProgressBarProperties.InnerTextureOffset, props, "The offset of the inner texture");
        //                ShaderProperty(ProgressBarProperties.InnerTextureOpacity, props, "The opacity of the inner texture");
        //                ShaderProperty(ProgressBarProperties.InnerTextureRotation, props, "The rotation of the inner texture");
        //                ShaderProperty(ProgressBarProperties.OffsetTextureWithValue, props, "Have the texture move with the value");
        //                ShaderProperty(ProgressBarProperties.InnerFlipbookDim, props, "When using an animated texture, input the columns and rows of the animation");

        //                EditorGUILayout.Space();

        //                if (ShaderPropertyToggle(ProgressBarProperties.InnerGradientEnabled)) {
        //                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                        EditorGUI.BeginChangeCheck();
        //                        m_persistentData.innerGradient = EditorGUILayout.GradientField("Inner Gradient", m_persistentData.innerGradient);
        //                        if (EditorGUI.EndChangeCheck()) {
        //                            Texture2D tex = m_persistentData.innerGradient.ToTexture2D();
        //                            if (AssetDatabase.Contains(m_targetMat)) {
        //                                string assetPath = Helpers.ValidateAndGetPrefabCachePath() + "/textures/" + GetIDString() + "_innerGradient.asset";
        //                                FileManager.AddToAssetCreationQueue(tex, assetPath);
        //                            }
        //                            m_targetMat.SetTexture(ProgressBarProperties.InnerGradient, tex);
        //                            string ser = JsonConvert.SerializeObject(m_persistentData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Converters = new[] { new ColorConverter() } });
        //                            ProjectPersistentData.SetString(GetIDString(), ser);
        //                            //EditorPrefs.SetString(GetIDString(), ser);
        //                        }
        //                        ShaderProperty(ProgressBarProperties.InnerGradientRotation, props, "Rotation of the gradient");
        //                        ShaderProperty(ProgressBarProperties.ValueAsGradientTimeInner, props, "Sample the gradient with the value as percent. E.g. Value = 0.5, Gradient is sampled at 0.5 for the whole progress bar.");
        //                    }
        //                }
        //            }
        //        }
        //        if (!isCircle) {
        //            EditorGUILayout.Space();

        //            ShaderProperty(ProgressBarProperties.ValueMaskOffset, props, "Offset the mask used to display the value in the x and y direction");
        //        }

        //        EditorGUILayout.Space();

        //        ShaderProperty(ProgressBarProperties.InnerBorderWidth, props, "Similar to the outer border, the inner border appears on the value portion");
        //        ShaderProperty(ProgressBarProperties.InnerBorderColor, props, "The color of the inner border");

        //        EditorGUILayout.Space();

        //        if (ShaderPropertyToggle(ProgressBarProperties.PulsateWhenLow)) {
        //            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                ShaderProperty(ProgressBarProperties.PulseActivationThreshold, props, "Threshold in percent when pulsation should trigger.");
        //                ShaderProperty(ProgressBarProperties.PulseSpeed, props, "The speed of pulsating.");
        //                ShaderProperty(ProgressBarProperties.PulseRamp, props, "Value difference between barely pulsating and maximum pulsating.");
        //                ShaderProperty(ProgressBarProperties.PulseColor, props, "The color the progress bar should pulse with");
        //            }
        //        }
        //    }

        //    EditorGUILayout.Space();
        //    EditorGUILayout.BeginHorizontal();
        //    GUILayout.Label("Background", sectionHeader);
        //    ShaderProperty(ProgressBarProperties.BackgroundColor, props, "", false);
        //    EditorGUILayout.EndHorizontal();
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        bool textureEnabled = Array.IndexOf(m_targetMat.shaderKeywords, "BACKGROUND_TEXTURE_ON") != -1;
        //        EditorGUI.BeginChangeCheck();
        //        textureEnabled = EditorGUILayout.Toggle("Background Texture Enabled", textureEnabled);
        //        if (EditorGUI.EndChangeCheck()) {
        //            if (textureEnabled) {
        //                m_targetMat.EnableKeyword("BACKGROUND_TEXTURE_ON");
        //            } else {
        //                m_targetMat.DisableKeyword("BACKGROUND_TEXTURE_ON");
        //            }
        //        }
        //        if (textureEnabled) {
        //            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                ShaderProperty(ProgressBarProperties.BackgroundTexture, props, "The texture for the background of the progress bar");
        //                ShaderProperty(ProgressBarProperties.BackgroundTextureTiling, props, "The tiling of the background texture");
        //                ShaderProperty(ProgressBarProperties.BackgroundTextureOffset, props, "The offset of the background texture");
        //                ShaderProperty(ProgressBarProperties.BackgroundTextureOpacity, props, "The opacity of the background texture");
        //                ShaderProperty(ProgressBarProperties.BackgroundFlipbookDim, props, "When using an animated texture, input the columns and rows of the animation");

        //                EditorGUILayout.Space();

        //                if (ShaderPropertyToggle(ProgressBarProperties.BackgroundGradientEnabled)) {
        //                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                        EditorGUI.BeginChangeCheck();
        //                        m_persistentData.backgroundGradient = EditorGUILayout.GradientField("Background Gradient", m_persistentData.backgroundGradient);
        //                        if (EditorGUI.EndChangeCheck()) {
        //                            var tex = m_persistentData.backgroundGradient.ToTexture2D();
        //                            if (AssetDatabase.Contains(m_targetMat)) {
        //                                string assetPath = Helpers.ValidateAndGetPrefabCachePath() + "/textures/" + GetIDString() + "_backgroundGradient.asset";
        //                                FileManager.AddToAssetCreationQueue(tex, assetPath);
        //                            }
        //                            m_targetMat.SetTexture(ProgressBarProperties.BackgroundGradient, tex);
        //                            string ser = JsonConvert.SerializeObject(m_persistentData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Converters = new[] { new ColorConverter() } });
        //                            ProjectPersistentData.SetString(GetIDString(), ser);
        //                            //EditorPrefs.SetString(GetIDString(), ser);
        //                        }
        //                        ShaderProperty(ProgressBarProperties.BackgroundGradientRotation, props, "Rotation of the gradient");
        //                        ShaderProperty(ProgressBarProperties.ValueAsGradientTimeBackground, props, "Sample the gradient with the value as percent. E.g. Value = 0.5, Gradient is sampled at 0.5 for the whole progress bar background.");
        //                    }
        //                }
        //            } 
        //        }
        //    }

        //    EditorGUILayout.Space();
        //    EditorGUILayout.BeginHorizontal();
        //    GUILayout.Label("Border", sectionHeader);
        //    ShaderProperty(ProgressBarProperties.BorderColor, props, "", false);
        //    EditorGUILayout.EndHorizontal();
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        bool textureEnabled = Array.IndexOf(m_targetMat.shaderKeywords, "BORDER_TEXTURE_ON") != -1;
        //        EditorGUI.BeginChangeCheck();
        //        textureEnabled = EditorGUILayout.Toggle("Border Texture Enabled", textureEnabled);
        //        if (EditorGUI.EndChangeCheck()) {
        //            if (textureEnabled) {
        //                m_targetMat.EnableKeyword("BORDER_TEXTURE_ON");
        //            } else {
        //                m_targetMat.DisableKeyword("BORDER_TEXTURE_ON");
        //            }
        //        }
        //        if (textureEnabled) {
        //            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                ShaderProperty(ProgressBarProperties.BorderTexture, props, "The texture for the progress bar border");
        //                ShaderProperty(ProgressBarProperties.BorderTextureTiling, props, "The tiling of the border texture");
        //                ShaderProperty(ProgressBarProperties.BorderTextureOffset, props, "The offset of the border texture");
        //                ShaderProperty(ProgressBarProperties.BorderTextureOpacity, props, "The opacity of the border texture");
        //                ShaderProperty(ProgressBarProperties.BorderFlipbookDim, props, "When using an animated texture, input the columns and rows of the animation");
        //            }
        //        }
        //    }

        //    EditorGUILayout.Space();
        //    EditorGUILayout.BeginHorizontal();
        //    GUILayout.Label("Overlay", sectionHeader);
        //    ShaderProperty(ProgressBarProperties.OverlayColor, props, "", false);
        //    EditorGUILayout.EndHorizontal();
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        bool textureEnabled = Array.IndexOf(m_targetMat.shaderKeywords, "OVERLAY_TEXTURE_ON") != -1;
        //        EditorGUI.BeginChangeCheck();
        //        textureEnabled = EditorGUILayout.Toggle("Overlay Texture Enabled", textureEnabled);
        //        if (EditorGUI.EndChangeCheck()) {
        //            if (textureEnabled) {
        //                m_targetMat.EnableKeyword("OVERLAY_TEXTURE_ON");
        //            } else {
        //                m_targetMat.DisableKeyword("OVERLAY_TEXTURE_ON");
        //            }
        //        }
        //        if (textureEnabled) {
        //            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //                ShaderProperty(ProgressBarProperties.OverlayTexture, props, "The texture for the progress bar overlay");
        //                ShaderProperty(ProgressBarProperties.OverlayTextureTiling, props, "The tiling of the overlay texture");
        //                ShaderProperty(ProgressBarProperties.OverlayTextureOffset, props, "The offset of the overlay texture");
        //                ShaderProperty(ProgressBarProperties.OverlayTextureOpacity, props, "The opacity of the overlay texture");
        //                ShaderProperty(ProgressBarProperties.OverlayFlipbookDim, props, "When using an animated texture, input the columns and rows of the animation");
        //            }
        //        }
        //    }

        //    EditorGUILayout.Space();
        //    GUILayout.Label("Shadows", sectionHeader);
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        ShaderProperty(ProgressBarProperties.ValueShadowSize, props, "The size of the drop shadow caused by the inner progress bar");
        //        ShaderProperty(ProgressBarProperties.ValueShadowColor, props, "The color of the value shadow");
        //        ShaderProperty(ProgressBarProperties.ValueShadowFalloff, props, "The falloff of the value shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

        //        EditorGUILayout.Space();

        //        ShaderProperty(ProgressBarProperties.ValueInsetShadowSize, props, "The size of the inset shadow of the inner progress bar");
        //        ShaderProperty(ProgressBarProperties.ValueInsetShadowColor, props, "The color of the value inset shadow");
        //        ShaderProperty(ProgressBarProperties.ValueInsetShadowFalloff, props, "The falloff of the value inset shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

        //        EditorGUILayout.Space();

        //        ShaderProperty(ProgressBarProperties.BorderShadowSize, props, "The size of the shadow from the border going into the progress bar");
        //        ShaderProperty(ProgressBarProperties.BorderShadowColor, props, "The color of the border shadow");
        //        ShaderProperty(ProgressBarProperties.BorderShadowFalloff, props, "The falloff of the border shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");

        //        EditorGUILayout.Space();

        //        ShaderProperty(ProgressBarProperties.BorderInsetShadowSize, props, "The size of the border inset shadow");
        //        ShaderProperty(ProgressBarProperties.BorderInsetShadowColor, props, "The color of the border inset shadow");
        //        ShaderProperty(ProgressBarProperties.BorderInsetShadowFalloff, props, "The falloff of the border inset shadow. Higher values lead to smooth transitions. A Value of >0.95 is recommended for a very smooth and subtle shadow");
        //    }

        //    EditorGUILayout.Space();
        //    GUILayout.Label("Other", sectionHeader);
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
        //        ShaderProperty(ProgressBarProperties.FlipbookFPS, props, "Frames per second of animated textures, if they are being used");
        //    }

        //}
    }

    //public class ColorConverter : PartialConverter<Color> {
    //    protected override void ReadValue(ref Color value, string name, JsonReader reader, JsonSerializer serializer) {
    //        switch (name) {
    //            case nameof(value.r):
    //                value.r = reader.ReadAsFloat() ?? 0f;
    //                break;
    //            case nameof(value.g):
    //                value.g = reader.ReadAsFloat() ?? 0f;
    //                break;
    //            case nameof(value.b):
    //                value.b = reader.ReadAsFloat() ?? 0f;
    //                break;
    //            case nameof(value.a):
    //                value.a = reader.ReadAsFloat() ?? 0f;
    //                break;
    //        }
    //    }

    //    protected override void WriteJsonProperties(JsonWriter writer, Color value, JsonSerializer serializer) {
    //        writer.WritePropertyName(nameof(value.r));
    //        writer.WriteValue(value.r);
    //        writer.WritePropertyName(nameof(value.g));
    //        writer.WriteValue(value.g);
    //        writer.WritePropertyName(nameof(value.b));
    //        writer.WriteValue(value.b);
    //        writer.WritePropertyName(nameof(value.a));
    //        writer.WriteValue(value.a);
    //    }
    //}

    //public abstract class PartialConverter<T> : JsonConverter
    //    where T : new() {
    //    protected abstract void ReadValue(ref T value, string name, JsonReader reader, JsonSerializer serializer);

    //    protected abstract void WriteJsonProperties(JsonWriter writer, T value, JsonSerializer serializer);

    //    /// <summary>
    //    /// Determine if the object type is <typeparamref name="T"/>
    //    /// </summary>
    //    /// <param name="objectType">Type of the object.</param>
    //    /// <returns><c>true</c> if this can convert the specified type; otherwise, <c>false</c>.</returns>
    //    public override bool CanConvert(Type objectType) {
    //        return objectType == typeof(T)
    //            || (objectType.IsGenericType
    //                && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
    //                && objectType.GenericTypeArguments[0] == typeof(T));
    //    }

    //    /// <summary>
    //    /// Read the specified properties to the object.
    //    /// </summary>
    //    /// <returns>The object value.</returns>
    //    /// <param name="reader">The <c>Newtonsoft.Json.JsonReader</c> to read from.</param>
    //    /// <param name="objectType">Type of the object.</param>
    //    /// <param name="existingValue">The existing value of object being read.</param>
    //    /// <param name="serializer">The calling serializer.</param>
    //    public override object ReadJson(
    //        JsonReader reader,
    //        Type objectType,
    //        object existingValue,
    //        JsonSerializer serializer) {
    //        if (reader.TokenType == JsonToken.Null) {
    //            bool isNullableStruct = objectType.IsGenericType
    //                && objectType.GetGenericTypeDefinition() == typeof(Nullable<>);

    //            return isNullableStruct ? null : (object)default(T);
    //        }

    //        return InternalReadJson(reader, serializer, existingValue);
    //    }

    //    private T InternalReadJson(JsonReader reader, JsonSerializer serializer, object existingValue) {
    //        if (reader.TokenType != JsonToken.StartObject) {
    //            throw reader.CreateSerializationException($"Failed to read type '{typeof(T).Name}'. Expected object start, got '{reader.TokenType}' <{reader.Value}>");
    //        }

    //        reader.Read();

    //        if (!(existingValue is T value)) {
    //            value = new T();
    //        }

    //        string previousName = null;

    //        while (reader.TokenType == JsonToken.PropertyName) {
    //            if (reader.Value is string name) {
    //                if (name == previousName) {
    //                    throw reader.CreateSerializationException($"Failed to read type '{typeof(T).Name}'. Possible loop when reading property '{name}'");
    //                }

    //                previousName = name;
    //                ReadValue(ref value, name, reader, serializer);
    //            } else {
    //                reader.Skip();
    //            }

    //            reader.Read();
    //        }

    //        return value;
    //    }

    //    /// <summary>
    //    /// Write the specified properties of the object.
    //    /// </summary>
    //    /// <param name="writer">The <c>Newtonsoft.Json.JsonWriter</c> to write to.</param>
    //    /// <param name="value">The value.</param>
    //    /// <param name="serializer">The calling serializer.</param>
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
    //        if (value == null) {
    //            writer.WriteNull();
    //            return;
    //        }

    //        writer.WriteStartObject();

    //        var typed = (T)value;
    //        WriteJsonProperties(writer, typed, serializer);

    //        writer.WriteEndObject();
    //    }
    //}
}
#endif