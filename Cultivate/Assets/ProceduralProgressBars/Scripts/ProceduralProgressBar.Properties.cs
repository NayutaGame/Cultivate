using Renge.Util;
using UnityEngine;

namespace Renge.PPB
{
    public enum PBShape
    {
        Linear,
        Circular
    }

    public partial class ProceduralProgressBar : MonoBehaviour
    {
        #region Fields

        //basic
        [SerializeField] float pbValue = 0.5f;
        [SerializeField] float segmentCount = 1;

        //circle
        [SerializeField] int sides = 50;
        [SerializeField] protected float radius = 0.3f;
        [SerializeField] protected float width = .2f;
        [SerializeField] protected float arc = 0.1f;
        [SerializeField] protected bool autoArcOffset = true;
        [SerializeField] protected float autoArcOffsetThreshold = 0.8f;
        [SerializeField] protected float circleLength = 1;
        [SerializeField] protected int edgeLoops = 5;
        [SerializeField] protected float subdivisionDetail = 1;
        [SerializeField] protected float corkScrew = 0.000001f; // this can be set to 0, but results in z-fighting when the progress bar overlaps due to circleLength > 1 or arc < 0
        [SerializeField] protected float faceRotation = 0;
        [SerializeField] protected bool clockwiseFill = true;

        //advanced material properties
        [SerializeField] protected bool ratioScaling = true;
        [SerializeField] protected bool antiAlias = true;
        [SerializeField] protected bool centerFill = false;
        [SerializeField] protected float borderWidth = 0.01f;
        [SerializeField] protected float innerBorderWidth = 0.01f;
        [SerializeField] protected float borderRadius = 0.5f;
        [SerializeField] protected Vector4 borderRadiusOffset = Vector4.zero;
        [SerializeField] protected float segmentSpacing = 0;
        [SerializeField] protected float innerRoundingPercent = 1;
        [SerializeField][ColorUsage(true, true)] protected Color innerColor = Color.red;
        [SerializeField][ColorUsage(true, true)] protected Color backgroundColor = Color.white;
        [SerializeField][ColorUsage(true, true)] protected Color borderColor = Color.black;
        [SerializeField][ColorUsage(true, true)] protected Color innerBorderColor = new Color(0.6f, 0, 0);
        [SerializeField][ColorUsage(true, true)] protected Color overlayColor = Color.white;
        [SerializeField] protected Texture2D innerTexture;
        [SerializeField] protected float innerTextureOpacity = 1;
        [SerializeField] protected Vector2 innerTextureTiling = Vector2.one;
        [SerializeField] protected Vector2 innerTextureOffset = Vector2.zero;
        [SerializeField] protected float innerTextureRotation = 0;
        [SerializeField] protected Gradient innerGradient;
        [SerializeField] protected bool innerGradientEnabled = false;
        [SerializeField] protected float innerGradientRotation = 0;
        [SerializeField] protected bool valueAsGradientTimeInner = false;
        [SerializeField] protected Texture2D overlayTexture;
        [SerializeField] protected float overlayTextureOpacity = 1;
        [SerializeField] protected Vector2 overlayTextureTiling = Vector2.one;
        [SerializeField] protected Vector2 overlayTextureOffset = Vector2.zero;
        [SerializeField] protected Texture2D borderTexture;
        [SerializeField] protected Vector2 borderTextureTiling = Vector2.one;
        [SerializeField] protected Vector2 borderTextureOffset = Vector2.zero;
        [SerializeField] protected float borderTextureOpacity = 1;
        [SerializeField] protected float borderTextureRotation = 0;
        [SerializeField] protected Texture2D backgroundTexture;
        [SerializeField] protected Vector2 backgroundTextureTiling = Vector2.one;
        [SerializeField] protected Vector2 backgroundTextureOffset = Vector2.zero;
        [SerializeField] protected float backgroundGradientRotation = 0;
        [SerializeField] protected float backgroundTextureOpacity = 1;
        [SerializeField] protected float backgroundTextureRotation = 0;
        [SerializeField] protected Gradient backgroundGradient;
        [SerializeField] protected bool backgroundTextureScaleWithSegments = true;
        [SerializeField] protected bool backgroundGradientEnabled = false;
        [SerializeField] protected bool valueAsGradientTimeBackground = false;
        [SerializeField] protected bool pulsateWhenLow = false;
        [SerializeField] protected float pulseActivationThreshold = 0.5f;
        [SerializeField] protected float pulseSpeed = 10;
        [SerializeField] protected float pulseRamp = 0.1f;
        [SerializeField][ColorUsage(true, true)] protected Color pulseColor = Color.black;
        [SerializeField] protected AnimationCurve variableWidthCurve;
        [SerializeField] protected bool adjustBorderRadiusToWidthCurve = true;
        [SerializeField] protected float slant = 0;
        [SerializeField] protected float flipbookFPS = 24;
        [SerializeField] protected Vector2 innerFlipbookDim = Vector2.one;
        [SerializeField] protected Vector2 overlayFlipbookDim = Vector2.one;
        [SerializeField] protected Vector2 borderFlipbookDim = Vector2.one;
        [SerializeField] protected Vector2 backgroundFlipbookDim = Vector2.one;
        [SerializeField] protected Vector2 valueMaskOffset = Vector2.zero;
        [SerializeField] protected bool offsetTextureWithValue = true;
        [SerializeField][ColorUsage(true, true)] protected Color valueShadowColor = Color.black;
        [SerializeField] protected float valueShadowSize = 0.5f;
        [SerializeField] protected float valueShadowFalloff = 0.995f;
        [SerializeField][ColorUsage(true, true)] protected Color borderShadowColor = Color.black;
        [SerializeField] protected float borderShadowSize = 0.1f;
        [SerializeField] protected float borderShadowFalloff = 0.995f;
        [SerializeField][ColorUsage(true, true)] protected Color valueInsetShadowColor = Color.black;
        [SerializeField] protected float valueInsetShadowSize = 0.1f;
        [SerializeField] protected float valueInsetShadowFalloff = 0.995f;
        [SerializeField][ColorUsage(true, true)] protected Color borderInsetShadowColor = Color.black;
        [SerializeField] protected float borderInsetShadowSize = 0.1f;
        [SerializeField] protected float borderInsetShadowFalloff = 0.995f;

        //keywords
        [SerializeField] PBShape shape;
        [SerializeField] protected bool innerTextureEnabled = false;
        [SerializeField] protected bool overlayTextureEnabled = false;
        [SerializeField] protected bool borderTextureEnabled = false;
        [SerializeField] protected bool backgroundTextureEnabled = false;

        #endregion

        #region Properties

        /// <summary>
        /// Set the value of the progress bar. This is based on how many segments you have, If you have one segment the range is 0 to 1. 2 segments is 0 to 2 etc.
        /// </summary>
        public float Value
        {
            get
            {
                return pbValue;
            }
            set
            {
                if (value < 0 && pbValue == 0 || value > SegmentCount && pbValue == SegmentCount) return;
                if (value - pbValue != 0)
                    onValueChanged?.Invoke(value, value - pbValue);
                int oldSegment = (int)(pbValue);
                int newSegment = (int)(value);
                if (oldSegment < newSegment && oldSegment < SegmentCount && oldSegment >= 0)
                {
                    //Debug.Log("Segment Filled");
                    int segmentDiff = 0;
                    while (segmentDiff < newSegment - oldSegment)
                    {
                        onSegmentFilled?.Invoke(oldSegment + segmentDiff);
                        segmentDiff++;
                    }
                    if (oldSegment == SegmentCount - 1)
                    {
                        //Debug.Log("PB Filled");

                        onFilled?.Invoke();
                    }
                }
                else if ((newSegment < oldSegment || value <= 0 && pbValue > 0) && oldSegment < SegmentCount && oldSegment >= 0)
                {
                    //Debug.Log("Segment Empty");

                    int segmentDiff = 0;
                    while (segmentDiff < oldSegment - newSegment)
                    {
                        onSegmentEmpty?.Invoke(oldSegment - segmentDiff);
                        segmentDiff++;
                    }
                    if (oldSegment == newSegment)
                    {
                        onSegmentEmpty?.Invoke(oldSegment);
                    }
                    if (oldSegment == 0)
                    {
                        //Debug.Log("PB Empty");

                        onEmpty?.Invoke();
                    }
                }
                pbValue = value < 0 ? 0 : value > SegmentCount ? SegmentCount : value;
                material.SetFloat(PropertyToID("_Value"), value);
            }
        }

        /// <summary>
        /// Change how many segments your progress bar has. This also affects the value. See the value tooltip for more information.
        /// </summary>
        public float SegmentCount
        {
            get
            {
                return segmentCount;
            }
            set
            {
                material.SetFloat("_SegmentCount", value);
                segmentCount = value;
            }
        }
        /// <summary>
        /// Change the shape of the progress bar. Different shapes reveal different settings.
        /// </summary>
        public PBShape Shape
        {
            get => shape;
            set
            {
                shape = value;
                UpdateShape();
            }
        }
        /// <summary>
        /// How many sides should the circle have? 3 is triangular, 4 is square, 5 is a pentagon, >50 is a smooth circle
        /// </summary>
        public int Sides
        {
            get => sides;
            set
            {
                sides = value;
                UpdateSides();
            }
        }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                UpdateRadius();
            }
        }

        /// <summary>
        /// The thickness of the circle.
        /// </summary>
        public float Width
        {
            get => width;
            set
            {
                width = value;
                UpdateWidth();
            }
        }

        /// <summary>
        /// The arc of the circle. 0 = Full circle, .5 = Half circle, 1 = Nothing. Using values like 0.95 with a large radius can give you a more linear progress bar that still has some rounding to it. You'll also want to see the Auto Arc Offset property.
        /// </summary>
        public float Arc
        {
            get => arc;
            set
            {
                arc = value;
                UpdateArc();
            }
        }

        /// <summary>
        /// When this is enabled, the progress bar's offset is adjusted automatically to center itself when the arc exceeds the value in 'Auto Arc Offset Threshold'.
        /// </summary>
        public bool AutoArcOffset
        {
            get => autoArcOffset;
            set
            {
                autoArcOffset = value;
                UpdateAutoArcOffset();
            }
        }

        /// <summary>
        /// When the arc value exceeds this threshold, the progress bar is automatically centered. Only if the Auto Arc Offset property is checked.
        /// </summary>
        public float AutoArcOffsetThreshold
        {
            get => autoArcOffsetThreshold;
            set
            {
                autoArcOffsetThreshold = value;
                UpdateAutoArcOffsetThreshold();
            }
        }

        /// <summary>
        /// The length of the circle. 1 is Minimum, 100 is Maximum. 
        /// Keep in mind, when the circle is extended new geometry is created, 
        /// so if the circle gets too long you might end up with a very high-poly progress bar. 
        /// This value works nicely in conjunction with the CorkScrew property.
        /// </summary>
        public float CircleLength
        {
            get => circleLength;
            set
            {
                circleLength = value;
                UpdateCircleLength();
            }
        }

        /// <summary>
        /// Edge loops are groups of vertices following the length of the progress bar. 
        /// 2 is the minimum value, making up one edge loop on the inside and one on the outside of the progress bar. 
        /// The maximum value is 100 to ensure the vertex count doesn't get too crazy. If you don't understand what this does, 
        /// enable the wireframe mode in the scene and play around with this value. 
        /// If you are dealing with a very high vertex count, consider reducing this value.
        /// </summary>
        public int EdgeLoops
        {
            get => edgeLoops;
            set
            {
                edgeLoops = value;
                UpdateEdgeLoops();
            }
        }

        /// <summary>
        /// This property is only relevant when dealing with a low number of sides (less than 50). 
        /// The subdivision detail ensures the progress bar quality remains consistent even when the relative resolution is quite low. 
        /// You may wish to sacrifice quality if it isn't that important in which case you can set this value to 0. In most cases it should remain at 1.
        /// </summary>
        public float SubdivisionDetail
        {
            get => subdivisionDetail;
            set
            {
                subdivisionDetail = value;
                UpdateSubdivisionDetail();
            }
        }

        /// <summary>
        /// The progress bar takes the shape of a corkscrew. This value can be both positive and negative. 
        /// Increase the circle length above to extend this effect.
        /// </summary>
        public float CorkScrew
        {
            get => corkScrew;
            set
            {
                corkScrew = value;
                UpdateCorkScrew();
            }
        }


        /// <summary>
        /// Use anti aliasing on the progress bar.
        /// </summary>
        public bool RatioScaling { get => ratioScaling; set { ratioScaling = value; UpdateRatioScaling(); } }

        /// <summary>
        /// Use anti aliasing on the progress bar.
        /// </summary>
        public bool AntiAlias { get => antiAlias; set { antiAlias = value; UpdateAntiAlias(); } }

        /// <summary>
        /// Fill the progress bar from the center.
        /// </summary>
        private bool CenterFill { get => centerFill; set { centerFill = value; UpdateCenterFill(); } }

        /// <summary>
        /// The width of the border around the progress bar.
        /// </summary>
        public float BorderWidth { get => borderWidth; set { borderWidth = value; UpdateBorderWidth(); } }

        /// <summary>
        /// The width of the inner border around the value of the progress bar.
        /// </summary>
        public float InnerBorderWidth { get => innerBorderWidth; set { innerBorderWidth = value; UpdateInnerBorderWidth(); } }

        /// <summary>
        /// The rounding of the progress bar
        /// </summary>
        public float BorderRadius { get => borderRadius; set { borderRadius = value; UpdateBorderRadius(); } }

        /// <summary>
        /// Control the rounding of each corner individually. (tr, tl, bl, br)
        /// </summary>
        public Vector4 BorderRadiusOffset { get => borderRadiusOffset; set { borderRadiusOffset = value; UpdateBorderRadiusOffset(); } }

        /// <summary>
        /// The spacing between each segment.
        /// </summary>
        public float SegmentSpacing { get => segmentSpacing; set { segmentSpacing = value; UpdateSegmentSpacing(); } }

        /// <summary>
        /// The rounding of the inner progress bar. Lets you control if the value portion is as rounded as the entire progress bar.
        /// </summary>
        public float InnerRoundingPercent { get => innerRoundingPercent; set { innerRoundingPercent = value; UpdateInnerRoundingPercent(); } }

        /// <summary>
        /// The color of the value portion of the progress bar.
        /// </summary>
        public Color InnerColor { get => innerColor; set { innerColor = value; UpdateInnerColor(); } }

        /// <summary>
        /// The background color of the progress bar.
        /// </summary>
        public Color BackgroundColor { get => backgroundColor; set { backgroundColor = value; UpdateBackgroundColor(); } }

        /// <summary>
        /// The border color of the progress bar.
        /// </summary>
        public Color BorderColor { get => borderColor; set { borderColor = value; UpdateBorderColor(); } }

        /// <summary>
        /// The inner border color of the progress bar. This is the border around the value of the progress bar.
        /// </summary>
        public Color InnerBorderColor { get => innerBorderColor; set { innerBorderColor = value; UpdateInnerBorderColor(); } }

        /// <summary>
        /// The overlay color of the progress bar.
        /// </summary>
        public Color OverlayColor { get => overlayColor; set { overlayColor = value; UpdateOverlayColor(); } }

        /// <summary>
        /// The texture of the inner portion of the progress bar.
        /// </summary>
        public Texture2D InnerTexture { get => innerTexture; set { innerTexture = value; UpdateInnerTexture(); } }

        /// <summary>
        /// The opacity of the inner texture.
        /// </summary>
        public float InnerTextureOpacity { get => innerTextureOpacity; set { innerTextureOpacity = value; UpdateInnerTextureOpacity(); } }

        /// <summary>
        /// The tiling of the inner texture.
        /// </summary>
        public Vector2 InnerTextureTiling { get => innerTextureTiling; set { innerTextureTiling = value; UpdateInnerTextureTiling(); } }

        /// <summary>
        /// The offset of the inner texture.
        /// </summary>
        public Vector2 InnerTextureOffset { get => innerTextureOffset; set { innerTextureOffset = value; UpdateInnerTextureOffset(); } }

        /// <summary>
        /// The rotation of the inner texture.
        /// </summary>
        public float InnerTextureRotation { get => innerTextureRotation; set { innerTextureRotation = value; UpdateInnerTextureRotation(); } }

        /// <summary>
        /// The gradient of the inner portion of the progress bar.
        /// </summary>
        public Gradient InnerGradient { get => innerGradient; set { innerGradient = value; UpdateInnerGradient(); } }

        /// <summary>
        /// Enable the gradient on the value of the progress bar.
        /// </summary>
        public bool InnerGradientEnabled { get => innerGradientEnabled; set { innerGradientEnabled = value; UpdateInnerGradientEnabled(); } }

        /// <summary>
        /// The rotation of the inner gradient.
        /// </summary>
        public float InnerGradientRotation { get => innerGradientRotation; set { innerGradientRotation = value; UpdateInnerGradientRotation(); } }

        /// <summary>
        /// Use the value of the progress bar to sample the inner gradient
        /// </summary>
        public bool ValueAsGradientTimeInner { get => valueAsGradientTimeInner; set { valueAsGradientTimeInner = value; UpdateValueAsGradientTimeInner(); } }

        /// <summary>
        /// The texture of the overlay.
        /// </summary>
        public Texture2D OverlayTexture { get => overlayTexture; set { overlayTexture = value; UpdateOverlayTexture(); } }

        /// <summary>
        /// The opacity of the overlay texture.
        /// </summary>
        public float OverlayTextureOpacity { get => overlayTextureOpacity; set { overlayTextureOpacity = value; UpdateOverlayTextureOpacity(); } }

        /// <summary>
        /// The tiling of the overlay texture.
        /// </summary>
        public Vector2 OverlayTextureTiling { get => overlayTextureTiling; set { overlayTextureTiling = value; UpdateOverlayTextureTiling(); } }

        /// <summary>
        /// The offset of the overlay texture.
        /// </summary>
        public Vector2 OverlayTextureOffset { get => overlayTextureOffset; set { overlayTextureOffset = value; UpdateOverlayTextureOffset(); } }

        /// <summary>
        /// The texture of the border around the progress bar.
        /// </summary>
        public Texture2D BorderTexture { get => borderTexture; set { borderTexture = value; UpdateBorderTexture(); } }

        /// <summary>
        /// The tiling of the border texture.
        /// </summary>
        public Vector2 BorderTextureTiling { get => borderTextureTiling; set { borderTextureTiling = value; UpdateBorderTextureTiling(); } }

        /// <summary>
        /// The offset of the border texture.
        /// </summary>
        public Vector2 BorderTextureOffset { get => borderTextureOffset; set { borderTextureOffset = value; UpdateBorderTextureOffset(); } }

        /// <summary>
        /// The opacity of the border texture.
        /// </summary>
        public float BorderTextureOpacity { get => borderTextureOpacity; set { borderTextureOpacity = value; UpdateBorderTextureOpacity(); } }

        /// <summary>
        /// The rotation of the border texture.
        /// </summary>
        public float BorderTextureRotation { get => borderTextureRotation; set { borderTextureRotation = value; UpdateBorderTextureRotation(); } }

        /// <summary>
        /// The texture of the background of the progress bar.
        /// </summary>
        public Texture2D BackgroundTexture { get => backgroundTexture; set { backgroundTexture = value; UpdateBackgroundTexture(); } }

        /// <summary>
        /// The tiling of the background texture.
        /// </summary>
        public Vector2 BackgroundTextureTiling { get => backgroundTextureTiling; set { backgroundTextureTiling = value; UpdateBackgroundTextureTiling(); } }

        /// <summary>
        /// The offset of the background texture.
        /// </summary>
        public Vector2 BackgroundTextureOffset { get => backgroundTextureOffset; set { backgroundTextureOffset = value; UpdateBackgroundTextureOffset(); } }

        /// <summary>
        /// The rotation of the background texture.
        /// </summary>
        public float BackgroundGradientRotation { get => backgroundGradientRotation; set { backgroundGradientRotation = value; UpdateBackgroundGradientRotation(); } }

        /// <summary>
        /// The opacity of the background texture.
        /// </summary>
        public float BackgroundTextureOpacity { get => backgroundTextureOpacity; set { backgroundTextureOpacity = value; UpdateBackgroundTextureOpacity(); } }

        /// <summary>
        /// The rotation of the background texture.
        /// </summary>
        public float BackgroundTextureRotation { get => backgroundTextureRotation; set { backgroundTextureRotation = value; UpdateBackgroundTextureRotation(); } }

        /// <summary>
        /// The gradient of the background of the progress bar.
        /// </summary>
        public Gradient BackgroundGradient { get => backgroundGradient; set { backgroundGradient = value; UpdateBackgroundGradient(); } }

        /// <summary>
        /// Tile the background texture with the segments.
        /// </summary>
        public bool BackgroundTextureScaleWithSegments { get => backgroundTextureScaleWithSegments; set { backgroundTextureScaleWithSegments = value; UpdateBackgroundTextureScaleWithSegments(); } }

        /// <summary>
        /// Enable the gradient on the background of the progress bar.
        /// </summary>
        public bool BackgroundGradientEnabled { get => backgroundGradientEnabled; set { backgroundGradientEnabled = value; UpdateBackgroundGradientEnabled(); } }

        /// <summary>
        /// Use the value of the progress bar to sample the background gradient
        /// </summary>
        public bool ValueAsGradientTimeBackground { get => valueAsGradientTimeBackground; set { valueAsGradientTimeBackground = value; UpdateValueAsGradientTimeBackground(); } }
        
        /// <summary>
        /// Pulsate the value of the progress bar once it dips below a certain point.
        /// </summary>
        public bool PulsateWhenLow { get => pulsateWhenLow; set { pulsateWhenLow = value; UpdatePulsateWhenLow(); } }

        /// <summary>
        /// Threshold in percent when pulsation should trigger.
        /// </summary>
        public float PulseActivationThreshold { get => pulseActivationThreshold; set { pulseActivationThreshold = value; UpdatePulseActivationThreshold(); } }

        /// <summary>
        /// The speed of the pulsation.
        /// </summary>
        public float PulseSpeed { get => pulseSpeed; set { pulseSpeed = value; UpdatePulseSpeed(); } }

        /// <summary>
        /// Value difference between barely pulsating and maximum. pulsating.
        /// </summary>
        public float PulseRamp { get => pulseRamp; set { pulseRamp = value; UpdatePulseRamp(); } }

        /// <summary>
        /// The color of the pulsation.
        /// </summary>
        public Color PulseColor { get => pulseColor; set { pulseColor = value; UpdatePulseColor(); } }

        /// <summary>
        /// The curve representing the width at any given point along the progress bar. 
        /// 1 is full width, 0 is invisible. 
        /// If your progress bar ever becomes invisible and you don't know why, this might be the cause.
        /// </summary>
        public AnimationCurve VariableWidthCurve { get => variableWidthCurve; set { variableWidthCurve = value; UpdateVariableWidthCurve(); } }

        /// <summary>
        /// Adjust the border radius to the width curve. This will make the progress bar less rounded at spots where it is thinner.
        /// </summary>
        public bool AdjustBorderRadiusToWidthCurve { get => adjustBorderRadiusToWidthCurve; set { adjustBorderRadiusToWidthCurve = value; UpdateAdjustBorderRadiusToWidthCurve(); } }
        
        /// <summary>
        /// The slant of the progress bar.
        /// </summary>
        public float Slant { get => slant; set { slant = value; UpdateSlant(); } }

        /// <summary>
        /// The frames per second of the flipbook textures. Only relevant if using animated textures.
        /// </summary>
        public float FlipbookFPS { get => flipbookFPS; set { flipbookFPS = value; UpdateFlipbookFPS(); } }

        /// <summary>
        /// The dimensions of the inner texture spritesheet. use 1, 1 for a still image.
        /// </summary>
        public Vector2 InnerFlipbookDim { get => innerFlipbookDim; set { innerFlipbookDim = value; UpdateInnerFlipbookDim(); } }

        /// <summary>
        /// The dimensions of the overlay texture spritesheet. use 1, 1 for a still image.
        /// </summary>
        public Vector2 OverlayFlipbookDim { get => overlayFlipbookDim; set { overlayFlipbookDim = value; UpdateOverlayFlipbookDim(); } }

        /// <summary>
        /// The dimensions of the border texture spritesheet. use 1, 1 for a still image.
        /// </summary>
        public Vector2 BorderFlipbookDim { get => borderFlipbookDim; set { borderFlipbookDim = value; UpdateBorderFlipbookDim(); } }

        /// <summary>
        /// The dimensions of the background texture spritesheet. use 1, 1 for a still image.
        /// </summary>
        public Vector2 BackgroundFlipbookDim { get => backgroundFlipbookDim; set { backgroundFlipbookDim = value; UpdateBackgroundFlipbookDim(); } }

        /// <summary>
        /// Offset the mask used to display the value in the x and y direction. This can be used to create some interesting effects.
        /// </summary>
        public Vector2 ValueMaskOffset { get => valueMaskOffset; set { valueMaskOffset = value; UpdateValueMaskOffset(); } }

        /// <summary>
        /// Offset the texture with the value of the progress bar.
        /// </summary>
        public bool OffsetTextureWithValue { get => offsetTextureWithValue; set { offsetTextureWithValue = value; UpdateOffsetTextureWithValue(); } }

        /// <summary>
        /// The color of the value shadow.
        /// </summary>
        public Color ValueShadowColor { get => valueShadowColor; set { valueShadowColor = value; UpdateValueShadowColor(); } }

        /// <summary>
        /// The size of the value shadow.
        /// </summary>
        public float ValueShadowSize { get => valueShadowSize; set { valueShadowSize = value; UpdateValueShadowSize(); } }

        /// <summary>
        /// The falloff of the value shadow.
        /// </summary>
        public float ValueShadowFalloff { get => valueShadowFalloff; set { valueShadowFalloff = value; UpdateValueShadowFalloff(); } }

        /// <summary>
        /// The color of the border shadow.
        /// </summary>
        public Color BorderShadowColor { get => borderShadowColor; set { borderShadowColor = value; UpdateBorderShadowColor(); } }

        /// <summary>
        /// The size of the border shadow.
        /// </summary>
        public float BorderShadowSize { get => borderShadowSize; set { borderShadowSize = value; UpdateBorderShadowSize(); } }

        /// <summary>
        /// The falloff of the border shadow.
        /// </summary>
        public float BorderShadowFalloff { get => borderShadowFalloff; set { borderShadowFalloff = value; UpdateBorderShadowFalloff(); } }

        /// <summary>
        /// The color of the value inset shadow.
        /// </summary>
        public Color ValueInsetShadowColor { get => valueInsetShadowColor; set { valueInsetShadowColor = value; UpdateValueInsetShadowColor(); } }

        /// <summary>
        /// The size of the value inset shadow.
        /// </summary>
        public float ValueInsetShadowSize { get => valueInsetShadowSize; set { valueInsetShadowSize = value; UpdateValueInsetShadowSize(); } }

        /// <summary>
        /// The falloff of the value inset shadow.
        /// </summary>
        public float ValueInsetShadowFalloff { get => valueInsetShadowFalloff; set { valueInsetShadowFalloff = value; UpdateValueInsetShadowFalloff(); } }

        /// <summary>
        /// The color of the border inset shadow.
        /// </summary>
        public Color BorderInsetShadowColor { get => borderInsetShadowColor; set { borderInsetShadowColor = value; UpdateBorderInsetShadowColor(); } }

        /// <summary>
        /// The size of the border inset shadow.
        /// </summary>
        public float BorderInsetShadowSize { get => borderInsetShadowSize; set { borderInsetShadowSize = value; UpdateBorderInsetShadowSize(); } }

        /// <summary>
        /// The falloff of the border inset shadow.
        /// </summary>
        public float BorderInsetShadowFalloff { get => borderInsetShadowFalloff; set { borderInsetShadowFalloff = value; UpdateBorderInsetShadowFalloff(); } }
        #endregion

        #region Update Methods

        /// <summary>
        /// Set the fill percent of the progress bar.
        /// </summary>
        /// <param name="value">the fill percentage. A value between 0 and 1</param>
        public void SetPercent(float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            Value = value * SegmentCount;
        }

        private void UpdateShape()
        {
            switch (shape)
            {
                case PBShape.Linear:
                    material.EnableKeyword("SHAPE_LINEAR");
                    material.DisableKeyword("SHAPE_CIRCULAR");
                    break;
                case PBShape.Circular:
                    material.EnableKeyword("SHAPE_CIRCULAR");
                    material.DisableKeyword("SHAPE_LINEAR");
                    break;
            }
            MarkMeshDirty();
        }

        private void UpdateInnerTextureEnabled()
        {
            if (innerTextureEnabled)
            {
                material.EnableKeyword("INNER_TEXTURE_ON");
            }
            else
            {
                material.DisableKeyword("INNER_TEXTURE_ON");
            }
        }
        private void UpdateOverlayTextureEnabled()
        {
            if (overlayTextureEnabled)
            {
                material.EnableKeyword("OVERLAY_TEXTURE_ON");
            }
            else
            {
                material.DisableKeyword("OVERLAY_TEXTURE_ON");
            }
        }
        private void UpdateBorderTextureEnabled()
        {
            if (borderTextureEnabled)
            {
                material.EnableKeyword("BORDER_TEXTURE_ON");
            }
            else
            {
                material.DisableKeyword("BORDER_TEXTURE_ON");
            }
        }
        private void UpdateBackgroundTextureEnabled()
        {
            if (backgroundTextureEnabled)
            {
                material.EnableKeyword("BACKGROUND_TEXTURE_ON");
            }
            else
            {
                material.DisableKeyword("BACKGROUND_TEXTURE_ON");
            }
        }

        private void UpdateValue()
        {
            Value = pbValue;
        }

        private void UpdateSegmentCount()
        {
            SegmentCount = segmentCount;
        }
        private void UpdateSides()
        {
            if (shape == PBShape.Circular)
            {
                if (sides < 3)
                    sides = 3;
                else if (sides > 100)
                    sides = 100;
                MarkMeshDirty();
            }
        }

        private void UpdateRadius()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
                material.SetFloat("_Radius", radius);
            }
        }

        private void UpdateWidth()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
                material.SetFloat("_Width", width);
            }
        }

        private void UpdateArc()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
                material.SetFloat("_Arc", arc);
            }
        }

        private void UpdateAutoArcOffset()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateAutoArcOffsetThreshold()
        {
            if (shape == PBShape.Circular && arc > autoArcOffsetThreshold)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateCircleLength()
        {
            if (circleLength < 1)
                circleLength = 1;
            else if (circleLength > 100)
            {
                circleLength = 100;
            }
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
                material.SetFloat("_CircleLength", circleLength);
            }
        }
        private void UpdateEdgeLoops()
        {
            if (edgeLoops < 2)
                edgeLoops = 2;
            else if (edgeLoops > 100)
            {
                edgeLoops = 100;
            }
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateSubdivisionDetail()
        {
            if (subdivisionDetail < 0)
                subdivisionDetail = 0;
            else if (subdivisionDetail > 1)
                subdivisionDetail = 1;
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateCorkScrew()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateFaceRotation()
        {
            if (shape == PBShape.Circular)
            {
                MarkMeshDirty();
            }
        }

        private void UpdateClockwiseFill()
        {
            MarkMeshDirty();
        }

        private void UpdateSquareSubdivisions()
        {
            if (squareSubdivisions < 0)
                squareSubdivisions = 0;
            if (squareSubdivisions > 10)
                squareSubdivisions = 10;
            if (shape == PBShape.Linear)
            {
                MarkMeshDirty();
            }
        }

        public void UpdateRatioScaling() { SetMaterialFloat(ProgressBarProperties.RatioScaling, ratioScaling ? 1 : 0); }
        public void UpdateAntiAlias() { SetMaterialFloat(ProgressBarProperties.AntiAlias, antiAlias ? 1 : 0); }
        public void UpdateCenterFill() { SetMaterialFloat(ProgressBarProperties.CenterFill, centerFill ? 1 : 0); }
        public void UpdateBorderWidth() { SetMaterialFloat(ProgressBarProperties.BorderWidth, borderWidth); }
        public void UpdateInnerBorderWidth() { SetMaterialFloat(ProgressBarProperties.InnerBorderWidth, innerBorderWidth); }
        public void UpdateBorderRadius() { SetMaterialFloat(ProgressBarProperties.BorderRadius, borderRadius); }
        public void UpdateBorderRadiusOffset() { SetMaterialVector(ProgressBarProperties.BorderRadiusOffset, borderRadiusOffset); }
        public void UpdateSegmentSpacing() { SetMaterialFloat(ProgressBarProperties.SegmentSpacing, segmentSpacing); }
        public void UpdateInnerRoundingPercent() { SetMaterialFloat(ProgressBarProperties.InnerRoundingPercent, innerRoundingPercent); }
        public void UpdateInnerColor() { SetMaterialColor(ProgressBarProperties.InnerColor, innerColor); }
        public void UpdateBackgroundColor() { SetMaterialColor(ProgressBarProperties.BackgroundColor, backgroundColor); }
        public void UpdateBorderColor() { SetMaterialColor(ProgressBarProperties.BorderColor, borderColor); }
        public void UpdateInnerBorderColor() { SetMaterialColor(ProgressBarProperties.InnerBorderColor, innerBorderColor); }
        public void UpdateOverlayColor() { SetMaterialColor(ProgressBarProperties.OverlayColor, overlayColor); }
        public void UpdateInnerTexture() { SetMaterialTexture(ProgressBarProperties.InnerTexture, innerTexture); }
        public void UpdateInnerTextureOpacity() { SetMaterialFloat(ProgressBarProperties.InnerTextureOpacity, innerTextureOpacity); }
        public void UpdateInnerTextureTiling() { SetMaterialVector(ProgressBarProperties.InnerTextureTiling, innerTextureTiling); }
        public void UpdateInnerTextureOffset() { SetMaterialVector(ProgressBarProperties.InnerTextureOffset, innerTextureOffset); }
        public void UpdateInnerTextureRotation() { SetMaterialFloat(ProgressBarProperties.InnerTextureRotation, innerTextureRotation); }
        public void UpdateInnerGradient() { SetMaterialTexture(ProgressBarProperties.InnerGradient, innerGradient.ToTexture2D()); }
        public void UpdateInnerGradientEnabled() { SetMaterialFloat(ProgressBarProperties.InnerGradientEnabled, innerGradientEnabled ? 1 : 0); }
        public void UpdateInnerGradientRotation() { SetMaterialFloat(ProgressBarProperties.InnerGradientRotation, innerGradientRotation); }
        public void UpdateValueAsGradientTimeInner() { SetMaterialFloat(ProgressBarProperties.ValueAsGradientTimeInner, valueAsGradientTimeInner ? 1 : 0); }
        public void UpdateOverlayTexture() { SetMaterialTexture(ProgressBarProperties.OverlayTexture, overlayTexture); }
        public void UpdateOverlayTextureOpacity() { SetMaterialFloat(ProgressBarProperties.OverlayTextureOpacity, overlayTextureOpacity); }
        public void UpdateOverlayTextureTiling() { SetMaterialVector(ProgressBarProperties.OverlayTextureTiling, overlayTextureTiling); }
        public void UpdateOverlayTextureOffset() { SetMaterialVector(ProgressBarProperties.OverlayTextureOffset, overlayTextureOffset); }
        public void UpdateBorderTexture() { SetMaterialTexture(ProgressBarProperties.BorderTexture, borderTexture); }
        public void UpdateBorderTextureTiling() { SetMaterialVector(ProgressBarProperties.BorderTextureTiling, borderTextureTiling); }
        public void UpdateBorderTextureOffset() { SetMaterialVector(ProgressBarProperties.BorderTextureOffset, borderTextureOffset); }
        public void UpdateBorderTextureOpacity() { SetMaterialFloat(ProgressBarProperties.BorderTextureOpacity, borderTextureOpacity); }
        public void UpdateBorderTextureRotation() { SetMaterialFloat(ProgressBarProperties.BorderTextureRotation, borderTextureRotation); }
        public void UpdateBackgroundTexture() { SetMaterialTexture(ProgressBarProperties.BackgroundTexture, backgroundTexture); }
        public void UpdateBackgroundTextureTiling() { SetMaterialVector(ProgressBarProperties.BackgroundTextureTiling, backgroundTextureTiling); }
        public void UpdateBackgroundTextureOffset() { SetMaterialVector(ProgressBarProperties.BackgroundTextureOffset, backgroundTextureOffset); }
        public void UpdateBackgroundGradientRotation() { SetMaterialFloat(ProgressBarProperties.BackgroundGradientRotation, backgroundGradientRotation); }
        public void UpdateBackgroundTextureOpacity() { SetMaterialFloat(ProgressBarProperties.BackgroundTextureOpacity, backgroundTextureOpacity); }
        public void UpdateBackgroundTextureRotation() { SetMaterialFloat(ProgressBarProperties.BackgroundTextureRotation, backgroundTextureRotation); }
        public void UpdateBackgroundGradient() { SetMaterialTexture(ProgressBarProperties.BackgroundGradient, backgroundGradient.ToTexture2D()); }
        public void UpdateBackgroundTextureScaleWithSegments() { SetMaterialFloat(ProgressBarProperties.BackgroundTextureScaleWithSegments, backgroundTextureScaleWithSegments ? 1 : 0); }
        public void UpdateBackgroundGradientEnabled() { SetMaterialFloat(ProgressBarProperties.BackgroundGradientEnabled, backgroundGradientEnabled ? 1 : 0); }
        public void UpdateValueAsGradientTimeBackground() { SetMaterialFloat(ProgressBarProperties.ValueAsGradientTimeBackground, valueAsGradientTimeBackground ? 1 : 0); }
        public void UpdatePulsateWhenLow() { SetMaterialFloat(ProgressBarProperties.PulsateWhenLow, pulsateWhenLow ? 1 : 0); }
        public void UpdatePulseActivationThreshold() { SetMaterialFloat(ProgressBarProperties.PulseActivationThreshold, pulseActivationThreshold); }
        public void UpdatePulseSpeed() { SetMaterialFloat(ProgressBarProperties.PulseSpeed, pulseSpeed); }
        public void UpdatePulseRamp() { SetMaterialFloat(ProgressBarProperties.PulseRamp, pulseRamp); }
        public void UpdatePulseColor() { SetMaterialColor(ProgressBarProperties.PulseColor, pulseColor); }
        public void UpdateVariableWidthCurve() { if (variableWidthCurve != null) SetMaterialTexture(ProgressBarProperties.VariableWidthCurve, variableWidthCurve.ToTexture2D()); }
        public void UpdateAdjustBorderRadiusToWidthCurve() { SetMaterialFloat(ProgressBarProperties.AdjustBorderRadiusToWidthCurve, adjustBorderRadiusToWidthCurve ? 1 : 0); }
        public void UpdateSlant() { SetMaterialFloat(ProgressBarProperties.Slant, slant); }
        public void UpdateFlipbookFPS() { SetMaterialFloat(ProgressBarProperties.FlipbookFPS, flipbookFPS); }
        public void UpdateInnerFlipbookDim() { SetMaterialVector(ProgressBarProperties.InnerFlipbookDim, innerFlipbookDim); }
        public void UpdateOverlayFlipbookDim() { SetMaterialVector(ProgressBarProperties.OverlayFlipbookDim, overlayFlipbookDim); }
        public void UpdateBorderFlipbookDim() { SetMaterialVector(ProgressBarProperties.BorderFlipbookDim, borderFlipbookDim); }
        public void UpdateBackgroundFlipbookDim() { SetMaterialVector(ProgressBarProperties.BackgroundFlipbookDim, backgroundFlipbookDim); }
        public void UpdateValueMaskOffset() { SetMaterialVector(ProgressBarProperties.ValueMaskOffset, valueMaskOffset); }
        public void UpdateOffsetTextureWithValue() { SetMaterialFloat(ProgressBarProperties.OffsetTextureWithValue, offsetTextureWithValue ? 1 : 0); }
        public void UpdateValueShadowColor() { SetMaterialColor(ProgressBarProperties.ValueShadowColor, valueShadowColor); }
        public void UpdateValueShadowSize() { SetMaterialFloat(ProgressBarProperties.ValueShadowSize, valueShadowSize); }
        public void UpdateValueShadowFalloff() { SetMaterialFloat(ProgressBarProperties.ValueShadowFalloff, valueShadowFalloff); }
        public void UpdateBorderShadowColor() { SetMaterialColor(ProgressBarProperties.BorderShadowColor, borderShadowColor); }
        public void UpdateBorderShadowSize() { SetMaterialFloat(ProgressBarProperties.BorderShadowSize, borderShadowSize); }
        public void UpdateBorderShadowFalloff() { SetMaterialFloat(ProgressBarProperties.BorderShadowFalloff, borderShadowFalloff); }
        public void UpdateValueInsetShadowColor() { SetMaterialColor(ProgressBarProperties.ValueInsetShadowColor, valueInsetShadowColor); }
        public void UpdateValueInsetShadowSize() { SetMaterialFloat(ProgressBarProperties.ValueInsetShadowSize, valueInsetShadowSize); }
        public void UpdateValueInsetShadowFalloff() { SetMaterialFloat(ProgressBarProperties.ValueInsetShadowFalloff, valueInsetShadowFalloff); }
        public void UpdateBorderInsetShadowColor() { SetMaterialColor(ProgressBarProperties.BorderInsetShadowColor, borderInsetShadowColor); }
        public void UpdateBorderInsetShadowSize() { SetMaterialFloat(ProgressBarProperties.BorderInsetShadowSize, borderInsetShadowSize); }
        public void UpdateBorderInsetShadowFalloff() { SetMaterialFloat(ProgressBarProperties.BorderInsetShadowFalloff, borderInsetShadowFalloff); }

        private void ApplyAllSerializedProperties()
        {
            UpdateValue();
            UpdateSegmentCount();
            UpdateRadius();
            UpdateWidth();
            UpdateArc();
            UpdateAutoArcOffset();
            UpdateAutoArcOffsetThreshold();
            UpdateCircleLength();
            UpdateShape();
            UpdateInnerTextureEnabled();
            UpdateBorderTextureEnabled();
            UpdateBackgroundTextureEnabled();
            UpdateOverlayTextureEnabled();
            UpdateRatioScaling();
            UpdateAntiAlias();
            UpdateCenterFill();
            UpdateBorderWidth();
            UpdateInnerBorderWidth();
            UpdateBorderRadius();
            UpdateBorderRadiusOffset();
            UpdateSegmentSpacing();
            UpdateInnerRoundingPercent();
            UpdateInnerColor();
            UpdateBackgroundColor();
            UpdateBorderColor();
            UpdateInnerBorderColor();
            UpdateOverlayColor();
            UpdateInnerTexture();
            UpdateInnerTextureOpacity();
            UpdateInnerTextureTiling();
            UpdateInnerTextureOffset();
            UpdateInnerTextureRotation();
            UpdateInnerGradient();
            UpdateInnerGradientEnabled();
            UpdateInnerGradientRotation();
            UpdateValueAsGradientTimeInner();
            UpdateOverlayTexture();
            UpdateOverlayTextureOpacity();
            UpdateOverlayTextureTiling();
            UpdateOverlayTextureOffset();
            UpdateBorderTexture();
            UpdateBorderTextureTiling();
            UpdateBorderTextureOffset();
            UpdateBorderTextureOpacity();
            UpdateBorderTextureRotation();
            UpdateBackgroundTexture();
            UpdateBackgroundTextureTiling();
            UpdateBackgroundTextureOffset();
            UpdateBackgroundGradientRotation();
            UpdateBackgroundTextureOpacity();
            UpdateBackgroundGradient();
            UpdateBackgroundTextureScaleWithSegments();
            UpdateBackgroundGradientEnabled();
            UpdateValueAsGradientTimeBackground();
            UpdatePulsateWhenLow();
            UpdatePulseActivationThreshold();
            UpdatePulseSpeed();
            UpdatePulseRamp();
            UpdatePulseColor();
            UpdateVariableWidthCurve();
            UpdateAdjustBorderRadiusToWidthCurve();
            UpdateSlant();
            UpdateFlipbookFPS();
            UpdateInnerFlipbookDim();
            UpdateOverlayFlipbookDim();
            UpdateBorderFlipbookDim();
            UpdateBackgroundFlipbookDim();
            UpdateValueMaskOffset();
            UpdateOffsetTextureWithValue();
            UpdateValueShadowColor();
            UpdateValueShadowSize();
            UpdateValueShadowFalloff();
            UpdateBorderShadowColor();
            UpdateBorderShadowSize();
            UpdateBorderShadowFalloff();
            UpdateValueInsetShadowColor();
            UpdateValueInsetShadowSize();
            UpdateValueInsetShadowFalloff();
            UpdateBorderInsetShadowColor();
            UpdateBorderInsetShadowSize();
            UpdateBorderInsetShadowFalloff();
        }

        #endregion
    }
}
