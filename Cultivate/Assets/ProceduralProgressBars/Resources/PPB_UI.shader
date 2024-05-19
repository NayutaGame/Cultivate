// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Renge/PPB_UI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Value("Value", Float) = 0.64
        _SegmentCount("SegmentCount", Float) = 1
        _SegmentSpacing("SegmentSpacing", Float) = 0
        _Width("Width", Float) = 0.2
        _Radius("Radius", Float) = 0.3
        _Arc("Arc", Float) = 0
        _Slant("Slant", Float) = 0
        _AntiAlias("AntiAlias", Float) = 1
        _Pixelate("Pixelate", Float) = 0
        _PixelCount("PixelCount", Float) = 0
        _FlipbookFPS("FlipbookFPS", Float) = 24
        _VariableWidthCurve("VariableWidthCurve", 2D) = "white" {}
        [HDR]_OverlayColor("OverlayColor", Color) = (1,1,1,1)
        _OverlayTexture("OverlayTexture", 2D) = "white" {}
        _OverlayFlipbookDim("OverlayFlipbookDim", Vector) = (1,1,0,0)
        _OverlayTextureOffset("OverlayTextureOffset", Vector) = (0,0,0,0)
        _OverlayTextureTiling("OverlayTextureTiling", Vector) = (1,1,0,0)
        _OverlayTextureOpacity("OverlayTextureOpacity", Float) = 1
        [HDR]_BorderColor("BorderColor", Color) = (1,1,1,1)
        _BorderWidth("BorderWidth", Float) = 0.06
        _BorderRadius("BorderRadius", Float) = 0.3
        _BorderTextureOpacity("BorderTextureOpacity", Float) = 1
        _BorderTexture("BorderTexture", 2D) = "white" {}
        _BorderTextureTiling("BorderTextureTiling", Vector) = (1,1,0,0)
        _AdjustBorderRadiusToWidthCurve("AdjustBorderRadiusToWidthCurve", Float) = 0
        _BorderTextureOffset("BorderTextureOffset", Vector) = (0,0,0,0)
        _BorderFlipbookDim("BorderFlipbookDim", Vector) = (1,1,0,0)
        _BorderRadiusOffset("BorderRadiusOffset", Vector) = (0,0,0,0)
        [HDR]_BorderInsetShadowColor("BorderInsetShadowColor", Color) = (0,0,0,1)
        _BorderInsetShadowSize("BorderInsetShadowSize", Float) = 0.04
        _BorderInsetShadowFalloff("BorderInsetShadowFalloff", Float) = 0.99
        [HDR]_BorderShadowColor("BorderShadowColor", Color) = (0,0,0,1)
        _BorderShadowSize("BorderShadowSize", Float) = 0.05
        _BorderShadowFalloff("BorderShadowFalloff", Float) = 0.99
        _InnerTextureOpacity("InnerTextureOpacity", Float) = 0
        _BackgroundTextureOpacity("BackgroundTextureOpacity", Float) = 0
        _ValueAsGradientTimeBackground("ValueAsGradientTimeBackground", Float) = 0
        _ValueAsGradientTimeInner("ValueAsGradientTimeInner", Float) = 0
        _InnerGradientEnabled("InnerGradientEnabled", Float) = 0
        _BackgroundGradientEnabled("BackgroundGradientEnabled", Float) = 0
        [HDR]_InnerColor("InnerColor", Color) = (1,1,1,1)
        [HDR]_BackgroundColor("BackgroundColor", Color) = (1,1,1,1)
        _BackgroundGradient("BackgroundGradient", 2D) = "white" {}
        _InnerGradient("InnerGradient", 2D) = "white" {}
        _InnerGradientRotation("InnerGradientRotation", Float) = 0
        _BackgroundGradientRotation("BackgroundGradientRotation", Float) = 0
        _BackgroundTextureScaleWithSegments("BackgroundTextureScaleWithSegments", Float) = 0
        _InnerTextureScaleWithSegments("InnerTextureScaleWithSegments", Float) = 1
        _BackgroundTextureTiling("BackgroundTextureTiling", Vector) = (1,1,0,0)
        _InnerTextureTiling("InnerTextureTiling", Vector) = (1,1,0,0)
        _BackgroundTexture("BackgroundTexture", 2D) = "white" {}
        _InnerTexture("InnerTexture", 2D) = "white" {}
        _InnerTextureRotation("InnerTextureRotation", Float) = 0
        _BackgroundTextureRotation("BackgroundTextureRotation", Float) = 0
        _InnerFlipbookDim("InnerFlipbookDim", Vector) = (1,1,0,0)
        _BackgroundFlipbookDim("BackgroundFlipbookDim", Vector) = (1,1,0,0)
        _InnerTextureOffset("InnerTextureOffset", Vector) = (0,0,0,0)
        _BackgroundTextureOffset("BackgroundTextureOffset", Vector) = (0,0,0,0)
        [HDR]_InnerBorderColor("InnerBorderColor", Color) = (0.745283,0,0,1)
        _InnerBorderWidth("InnerBorderWidth", Float) = 0.02
        _InnerRoundingPercent("InnerRoundingPercent", Float) = 0
        _UIScaling("UIScaling", Float) = 0
        _CustomScale("CustomScale", Vector) = (1,1,0,0)
        _CircleLength("CircleLength", Float) = 0.2
        _CenterFill("CenterFill", Float) = 0
        _OffsetTextureWithValue("OffsetTextureWithValue", Float) = 1
        _PulsateWhenLow("PulsateWhenLow", Float) = 1
        _PulseSpeed("PulseSpeed", Float) = 10
        _PulseActivationThreshold("PulseActivationThreshold", Range( 0 , 1)) = 0.5
        _PulseRamp("PulseRamp", Range( 0 , 1)) = 0.1
        _ValueMaskOffset("ValueMaskOffset", Vector) = (0,0,0,0)
        [HDR]_PulseColor("PulseColor", Color) = (0,0,0,1)
        [HDR]_ValueInsetShadowColor("ValueInsetShadowColor", Color) = (0,0,0,1)
        [HDR]_ValueShadowColor("ValueShadowColor", Color) = (0,0,0,1)
        _ValueInsetShadowSize("ValueInsetShadowSize", Float) = 0.1
        _ValueShadowSize("ValueShadowSize", Float) = 0.1
        _ValueInsetShadowFalloff("ValueInsetShadowFalloff", Float) = 0.99
        _ValueShadowFalloff("ValueShadowFalloff", Float) = 0.99
        _BorderTextureRotation("BorderTextureRotation", Float) = 0
        _BorderTextureScaleWithSegments("BorderTextureScaleWithSegments", Float) = 0
        _RatioScaling("RatioScaling", Float) = 1
        [HideInInspector] _texcoord( "", 2D ) = "white" {}

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"
            #pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
            #pragma multi_compile_local __ OVERLAY_TEXTURE_ON
            #pragma multi_compile_local __ BORDER_TEXTURE_ON
            #pragma multi_compile_local __ INNER_TEXTURE_ON
            #pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float _Slant;
            uniform float4 _OverlayColor;
            uniform sampler2D _OverlayTexture;
            uniform float _Pixelate;
            uniform float _PixelCount;
            uniform float _RatioScaling;
            uniform float _UIScaling;
            uniform float _Arc;
            uniform float _Width;
            uniform float _Radius;
            uniform float _CircleLength;
            uniform float2 _CustomScale;
            uniform float2 _OverlayTextureTiling;
            uniform float2 _OverlayTextureOffset;
            uniform float2 _OverlayFlipbookDim;
            uniform float _FlipbookFPS;
            uniform float _OverlayTextureOpacity;
            uniform float _BorderWidth;
            uniform float4 _BorderColor;
            uniform sampler2D _BorderTexture;
            uniform float _BorderTextureScaleWithSegments;
            uniform float _SegmentCount;
            uniform float2 _BorderTextureTiling;
            uniform sampler2D _VariableWidthCurve;
            uniform float4 _VariableWidthCurve_ST;
            uniform float2 _BorderTextureOffset;
            uniform float _BorderTextureRotation;
            uniform float2 _BorderFlipbookDim;
            uniform float _BorderTextureOpacity;
            uniform float _InnerBorderWidth;
            uniform float4 _InnerBorderColor;
            uniform float4 _PulseColor;
            uniform float _PulsateWhenLow;
            uniform float _PulseSpeed;
            uniform float _Value;
            uniform float _PulseActivationThreshold;
            uniform float _PulseRamp;
            uniform float _InnerGradientEnabled;
            uniform sampler2D _InnerGradient;
            uniform float _ValueAsGradientTimeInner;
            uniform float _SegmentSpacing;
            uniform float _InnerGradientRotation;
            uniform float4 _InnerColor;
            uniform sampler2D _InnerTexture;
            uniform float _InnerTextureScaleWithSegments;
            uniform float2 _InnerTextureTiling;
            uniform float2 _InnerTextureOffset;
            uniform float _OffsetTextureWithValue;
            uniform float _CenterFill;
            uniform float _InnerTextureRotation;
            uniform float2 _InnerFlipbookDim;
            uniform float _InnerTextureOpacity;
            uniform float _AntiAlias;
            uniform float2 _ValueMaskOffset;
            uniform float _BorderRadius;
            uniform float _InnerRoundingPercent;
            uniform float4 _BorderRadiusOffset;
            uniform float _AdjustBorderRadiusToWidthCurve;
            uniform float _ValueInsetShadowSize;
            uniform float4 _ValueInsetShadowColor;
            uniform float _ValueInsetShadowFalloff;
            uniform float _BorderInsetShadowSize;
            uniform float4 _BorderInsetShadowColor;
            uniform float _BorderInsetShadowFalloff;
            uniform float4 _BackgroundColor;
            uniform float _BackgroundGradientEnabled;
            uniform sampler2D _BackgroundGradient;
            uniform float _ValueAsGradientTimeBackground;
            uniform float _BackgroundGradientRotation;
            uniform sampler2D _BackgroundTexture;
            uniform float _BackgroundTextureScaleWithSegments;
            uniform float2 _BackgroundTextureTiling;
            uniform float2 _BackgroundTextureOffset;
            uniform float _BackgroundTextureRotation;
            uniform float2 _BackgroundFlipbookDim;
            uniform float _BackgroundTextureOpacity;
            uniform float _ValueShadowSize;
            uniform float4 _ValueShadowColor;
            uniform float _ValueShadowFalloff;
            uniform float _BorderShadowSize;
            uniform float4 _BorderShadowColor;
            uniform float _BorderShadowFalloff;

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 appendResult582_g880 = (float3(( ( ( v.texcoord.xy.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
                #if defined(SHAPE_LINEAR)
                float3 staticSwitch581_g880 = appendResult582_g880;
                #elif defined(SHAPE_CIRCULAR)
                float3 staticSwitch581_g880 = float3(0,0,0);
                #else
                float3 staticSwitch581_g880 = appendResult582_g880;
                #endif
                

                v.vertex.xyz += staticSwitch581_g880;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float Pixelate531_g880 = _Pixelate;
                float PixelCount545_g880 = _PixelCount;
                #if defined(SHAPE_LINEAR)
                float staticSwitch687_g880 = 0.0;
                #elif defined(SHAPE_CIRCULAR)
                float staticSwitch687_g880 = 1.0;
                #else
                float staticSwitch687_g880 = 0.0;
                #endif
                float temp_output_588_0_g880 = ( staticSwitch687_g880 > 0.0 ? 1.0 : 0.0 );
                float Arc539_g880 = _Arc;
                float Width537_g880 = _Width;
                float temp_output_9_0_g918 = Width537_g880;
                float Radius536_g880 = _Radius;
                float2 appendResult587_g880 = (float2(( saturate( ( 1.0 - Arc539_g880 ) ) * ( ( ( IN.texcoord.xy.y * temp_output_9_0_g918 ) + ( Radius536_g880 - ( temp_output_9_0_g918 / 2.0 ) ) ) * ( 6.28318548202515 * _CircleLength ) ) ) , Width537_g880));
                float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
                float3 appendResult28_g949 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
                float3 appendResult29_g949 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
                float3 appendResult30_g949 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
                float3 appendResult24_g949 = (float3(length( appendResult28_g949 ) , length( appendResult29_g949 ) , length( appendResult30_g949 )));
                float3 temp_output_38_0_g949 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g949 );
                float3 temp_output_16_0_g949 = ( ( ( temp_output_588_0_g880 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g880 > 0.0 ? appendResult587_g880 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g949 );
                float3 break9_g949 = temp_output_16_0_g949;
                float3 break48_g949 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g949 / min( break9_g949.x , break9_g949.y ) ) : temp_output_16_0_g949 );
                float2 appendResult10_g949 = (float2(break48_g949.x , break48_g949.y));
                float2 OSXY554_g880 = appendResult10_g949;
                float2 temp_output_6_0_g947 = ( PixelCount545_g880 * OSXY554_g880 );
                float2 PixelationUV559_g880 = ( Pixelate531_g880 > 0.0 ? ( floor( ( IN.texcoord.xy * temp_output_6_0_g947 ) ) / ( temp_output_6_0_g947 - float2( 1,1 ) ) ) : IN.texcoord.xy );
                float2 temp_output_2_0_g883 = ( ( PixelationUV559_g880 * _OverlayTextureTiling ) + _OverlayTextureOffset );
                float2 break51_g880 = max( _OverlayFlipbookDim , float2( 1,1 ) );
                float fps541_g880 = _FlipbookFPS;
                // *** BEGIN Flipbook UV Animation vars ***
                // Total tiles of Flipbook Texture
                float fbtotaltiles13_g882 = break51_g880.x * break51_g880.y;
                // Offsets for cols and rows of Flipbook Texture
                float fbcolsoffset13_g882 = 1.0f / break51_g880.x;
                float fbrowsoffset13_g882 = 1.0f / break51_g880.y;
                // Speed of animation
                float fbspeed13_g882 = _Time.y * fps541_g880;
                // UV Tiling (col and row offset)
                float2 fbtiling13_g882 = float2(fbcolsoffset13_g882, fbrowsoffset13_g882);
                // UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
                // Calculate current tile linear index
                float fbcurrenttileindex13_g882 = round( fmod( fbspeed13_g882 + 0.0, fbtotaltiles13_g882) );
                fbcurrenttileindex13_g882 += ( fbcurrenttileindex13_g882 < 0) ? fbtotaltiles13_g882 : 0;
                // Obtain Offset X coordinate from current tile linear index
                float fblinearindextox13_g882 = round ( fmod ( fbcurrenttileindex13_g882, break51_g880.x ) );
                // Multiply Offset X by coloffset
                float fboffsetx13_g882 = fblinearindextox13_g882 * fbcolsoffset13_g882;
                // Obtain Offset Y coordinate from current tile linear index
                float fblinearindextoy13_g882 = round( fmod( ( fbcurrenttileindex13_g882 - fblinearindextox13_g882 ) / break51_g880.x, break51_g880.y ) );
                // Reverse Y to get tiles from Top to Bottom
                fblinearindextoy13_g882 = (int)(break51_g880.y-1) - fblinearindextoy13_g882;
                // Multiply Offset Y by rowoffset
                float fboffsety13_g882 = fblinearindextoy13_g882 * fbrowsoffset13_g882;
                // UV Offset
                float2 fboffset13_g882 = float2(fboffsetx13_g882, fboffsety13_g882);
                // Flipbook UV
                half2 fbuv13_g882 = temp_output_2_0_g883 * fbtiling13_g882 + fboffset13_g882;
                // *** END Flipbook UV Animation vars ***
                float4 lerpResult45_g880 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g882 ) ) , saturate( _OverlayTextureOpacity ));
                #ifdef OVERLAY_TEXTURE_ON
                float4 staticSwitch44_g880 = lerpResult45_g880;
                #else
                float4 staticSwitch44_g880 = _OverlayColor;
                #endif
                float4 OverlayColorProcessed524_g880 = staticSwitch44_g880;
                float BorderWidth529_g880 = _BorderWidth;
                float4 break4_g909 = _BorderColor;
                float4 appendResult17_g909 = (float4(break4_g909.r , break4_g909.g , break4_g909.b , 1.0));
                float4 temp_output_738_0_g880 = ( ( saturate( ceil( BorderWidth529_g880 ) ) * ( 1.0 > 0.0 ? break4_g909.a : 1.0 ) ) * appendResult17_g909 );
                float segment_count527_g880 = _SegmentCount;
                float2 appendResult345_g880 = (float2(segment_count527_g880 , 1.0));
                float2 temp_output_2_0_g901 = ( ( PixelationUV559_g880 * appendResult345_g880 ) + float2( 0,0 ) );
                float2 break10_g901 = temp_output_2_0_g901;
                float2 appendResult352_g880 = (float2(( break10_g901.x % 1.0 ) , break10_g901.y));
                float2 ScaledTextureUV349_g880 = appendResult352_g880;
                float2 temp_output_2_0_g900 = ( ( PixelationUV559_g880 * float2( 1,1 ) ) + float2( 0,0 ) );
                float2 UnscaledTextureUV350_g880 = temp_output_2_0_g900;
                float2 break77_g880 = _BorderTextureTiling;
                float2 uv_VariableWidthCurve = IN.texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
                float width_curve532_g880 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
                float temp_output_75_0_g880 = ( break77_g880.y / width_curve532_g880 );
                float2 appendResult74_g880 = (float2(break77_g880.x , temp_output_75_0_g880));
                float2 appendResult70_g880 = (float2(0.0 , ( -( temp_output_75_0_g880 / 2.0 ) + 0.5 )));
                float2 temp_output_2_0_g884 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g880 : UnscaledTextureUV350_g880 ) * appendResult74_g880 ) + ( _BorderTextureOffset + appendResult70_g880 ) );
                float cos63_g880 = cos( radians( _BorderTextureRotation ) );
                float sin63_g880 = sin( radians( _BorderTextureRotation ) );
                float2 rotator63_g880 = mul( temp_output_2_0_g884 - float2( 0.5,0.5 ) , float2x2( cos63_g880 , -sin63_g880 , sin63_g880 , cos63_g880 )) + float2( 0.5,0.5 );
                float2 break39_g880 = max( _BorderFlipbookDim , float2( 1,1 ) );
                float fbtotaltiles13_g881 = break39_g880.x * break39_g880.y;
                float fbcolsoffset13_g881 = 1.0f / break39_g880.x;
                float fbrowsoffset13_g881 = 1.0f / break39_g880.y;
                float fbspeed13_g881 = _Time.y * fps541_g880;
                float2 fbtiling13_g881 = float2(fbcolsoffset13_g881, fbrowsoffset13_g881);
                float fbcurrenttileindex13_g881 = round( fmod( fbspeed13_g881 + 0.0, fbtotaltiles13_g881) );
                fbcurrenttileindex13_g881 += ( fbcurrenttileindex13_g881 < 0) ? fbtotaltiles13_g881 : 0;
                float fblinearindextox13_g881 = round ( fmod ( fbcurrenttileindex13_g881, break39_g880.x ) );
                float fboffsetx13_g881 = fblinearindextox13_g881 * fbcolsoffset13_g881;
                float fblinearindextoy13_g881 = round( fmod( ( fbcurrenttileindex13_g881 - fblinearindextox13_g881 ) / break39_g880.x, break39_g880.y ) );
                fblinearindextoy13_g881 = (int)(break39_g880.y-1) - fblinearindextoy13_g881;
                float fboffsety13_g881 = fblinearindextoy13_g881 * fbrowsoffset13_g881;
                float2 fboffset13_g881 = float2(fboffsetx13_g881, fboffsety13_g881);
                half2 fbuv13_g881 = rotator63_g880 * fbtiling13_g881 + fboffset13_g881;
                float4 lerpResult35_g880 = lerp( temp_output_738_0_g880 , ( tex2D( _BorderTexture, fbuv13_g881 ) * temp_output_738_0_g880 ) , saturate( _BorderTextureOpacity ));
                #ifdef BORDER_TEXTURE_ON
                float4 staticSwitch496_g880 = lerpResult35_g880;
                #else
                float4 staticSwitch496_g880 = temp_output_738_0_g880;
                #endif
                float4 BorderColorProcessed497_g880 = staticSwitch496_g880;
                float InnerBorderWidth250_g880 = _InnerBorderWidth;
                float4 break4_g915 = _InnerBorderColor;
                float4 appendResult17_g915 = (float4(break4_g915.r , break4_g915.g , break4_g915.b , 1.0));
                float4 temp_output_745_0_g880 = ( ( saturate( ceil( InnerBorderWidth250_g880 ) ) * ( 1.0 > 0.0 ? break4_g915.a : 1.0 ) ) * appendResult17_g915 );
                float4 break4_g917 = _PulseColor;
                float4 appendResult17_g917 = (float4(break4_g917.r , break4_g917.g , break4_g917.b , 1.0));
                float4 PulseColorProcessed384_g880 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g917.a : 1.0 ) ) * appendResult17_g917 );
                float Value574_g880 = _Value;
                float temp_output_1_0_g899 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
                float PulseAlpha382_g880 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _Time.y * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g880 / segment_count527_g880 ) - temp_output_1_0_g899 ) / ( _PulseActivationThreshold - temp_output_1_0_g899 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
                float4 lerpResult389_g880 = lerp( temp_output_745_0_g880 , PulseColorProcessed384_g880 , PulseAlpha382_g880);
                float2 temp_cast_4 = (saturate( ( Value574_g880 / segment_count527_g880 ) )).xx;
                float OSX553_g880 = break48_g949.x;
                float temp_output_444_0_g880 = ( ( BorderWidth529_g880 * segment_count527_g880 ) * -2.0 );
                float Segment_Spacing533_g880 = _SegmentSpacing;
                float temp_output_449_0_g880 = ( Segment_Spacing533_g880 * OSX553_g880 );
                float temp_output_408_0_g880 = ( ( segment_count527_g880 * OSX553_g880 ) / ( ( temp_output_444_0_g880 + ( OSX553_g880 * segment_count527_g880 ) ) - temp_output_449_0_g880 ) );
                float2 appendResult422_g880 = (float2(temp_output_408_0_g880 , 1.0));
                float2 appendResult407_g880 = (float2(-( ( temp_output_408_0_g880 - 1.0 ) / 2.0 ) , 0.0));
                float2 temp_output_2_0_g904 = ( ( PixelationUV559_g880 * appendResult422_g880 ) + appendResult407_g880 );
                float2 GradientUV479_g880 = temp_output_2_0_g904;
                float cos363_g880 = cos( radians( _InnerGradientRotation ) );
                float sin363_g880 = sin( radians( _InnerGradientRotation ) );
                float2 rotator363_g880 = mul( GradientUV479_g880 - float2( 0.5,0.5 ) , float2x2( cos363_g880 , -sin363_g880 , sin363_g880 , cos363_g880 )) + float2( 0.5,0.5 );
                float4 break4_g911 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g880 ) );
                float4 appendResult17_g911 = (float4(break4_g911.r , break4_g911.g , break4_g911.b , 1.0));
                float4 temp_output_740_0_g880 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g911.a : 1.0 ) ) * appendResult17_g911 );
                float4 lerpResult390_g880 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g880 * temp_output_740_0_g880 ) : temp_output_745_0_g880 ) , PulseColorProcessed384_g880 , PulseAlpha382_g880);
                #ifdef INNER_TEXTURE_ON
                float4 staticSwitch388_g880 = lerpResult390_g880;
                #else
                float4 staticSwitch388_g880 = lerpResult389_g880;
                #endif
                float4 ValueBorderColorProcessed525_g880 = staticSwitch388_g880;
                float4 break679_g880 = ValueBorderColorProcessed525_g880;
                float4 appendResult675_g880 = (float4(break679_g880.x , break679_g880.y , break679_g880.z , 1.0));
                float4 break4_g916 = _InnerColor;
                float4 appendResult17_g916 = (float4(break4_g916.r , break4_g916.g , break4_g916.b , 1.0));
                float4 temp_output_746_0_g880 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g916.a : 1.0 ) ) * appendResult17_g916 );
                float4 lerpResult369_g880 = lerp( temp_output_746_0_g880 , PulseColorProcessed384_g880 , PulseAlpha382_g880);
                float4 lerpResult367_g880 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g880 * temp_output_746_0_g880 ) : temp_output_746_0_g880 ) , PulseColorProcessed384_g880 , PulseAlpha382_g880);
                float Inner_Tex_Scale_w_Segments252_g880 = _InnerTextureScaleWithSegments;
                float2 Inner_Tex_Tiling254_g880 = _InnerTextureTiling;
                float temp_output_330_0_g880 = ( Inner_Tex_Scale_w_Segments252_g880 > 0.0 ? 1.0 : segment_count527_g880 );
                float temp_output_324_0_g880 = ( ( ( Inner_Tex_Tiling254_g880.x * OSX553_g880 ) * temp_output_330_0_g880 ) / ( ( ( temp_output_330_0_g880 * OSX553_g880 ) + ( ( BorderWidth529_g880 * segment_count527_g880 ) * -2.0 ) ) - ( OSX553_g880 * Segment_Spacing533_g880 ) ) );
                float OSY552_g880 = break48_g949.y;
                float temp_output_270_0_g880 = ( Inner_Tex_Tiling254_g880.y / ( width_curve532_g880 - ( BorderWidth529_g880 * ( 2.0 / OSY552_g880 ) ) ) );
                float2 appendResult276_g880 = (float2(temp_output_324_0_g880 , temp_output_270_0_g880));
                float CenterFill562_g880 = _CenterFill;
                float2 temp_output_2_0_g897 = ( ( IN.texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
                float2 break10_g897 = temp_output_2_0_g897;
                float lerpResult321_g880 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g880 > 0.0 ? ( 1.0 - ( min( Value574_g880 , segment_count527_g880 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g880 / segment_count527_g880 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g880 > 0.0 ? ( floor( Value574_g880 ) / segment_count527_g880 ) : 0.0 ) , break10_g897.x ));
                float2 appendResult277_g880 = (float2(( ( -( ( temp_output_324_0_g880 - Inner_Tex_Tiling254_g880.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g880.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g880 > 0.0 ? 0.0 : lerpResult321_g880 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g880 / 2.0 ) + 0.5 ) )));
                float2 temp_output_2_0_g896 = ( ( ( Inner_Tex_Scale_w_Segments252_g880 > 0.0 ? ScaledTextureUV349_g880 : UnscaledTextureUV350_g880 ) * appendResult276_g880 ) + appendResult277_g880 );
                float cos299_g880 = cos( radians( _InnerTextureRotation ) );
                float sin299_g880 = sin( radians( _InnerTextureRotation ) );
                float2 rotator299_g880 = mul( temp_output_2_0_g896 - float2( 0.5,0.5 ) , float2x2( cos299_g880 , -sin299_g880 , sin299_g880 , cos299_g880 )) + float2( 0.5,0.5 );
                float2 break275_g880 = max( _InnerFlipbookDim , float2( 1,1 ) );
                float fbtotaltiles13_g898 = break275_g880.x * break275_g880.y;
                float fbcolsoffset13_g898 = 1.0f / break275_g880.x;
                float fbrowsoffset13_g898 = 1.0f / break275_g880.y;
                float fbspeed13_g898 = _Time.y * fps541_g880;
                float2 fbtiling13_g898 = float2(fbcolsoffset13_g898, fbrowsoffset13_g898);
                float fbcurrenttileindex13_g898 = round( fmod( fbspeed13_g898 + 0.0, fbtotaltiles13_g898) );
                fbcurrenttileindex13_g898 += ( fbcurrenttileindex13_g898 < 0) ? fbtotaltiles13_g898 : 0;
                float fblinearindextox13_g898 = round ( fmod ( fbcurrenttileindex13_g898, break275_g880.x ) );
                float fboffsetx13_g898 = fblinearindextox13_g898 * fbcolsoffset13_g898;
                float fblinearindextoy13_g898 = round( fmod( ( fbcurrenttileindex13_g898 - fblinearindextox13_g898 ) / break275_g880.x, break275_g880.y ) );
                fblinearindextoy13_g898 = (int)(break275_g880.y-1) - fblinearindextoy13_g898;
                float fboffsety13_g898 = fblinearindextoy13_g898 * fbrowsoffset13_g898;
                float2 fboffset13_g898 = float2(fboffsetx13_g898, fboffsety13_g898);
                half2 fbuv13_g898 = rotator299_g880 * fbtiling13_g898 + fboffset13_g898;
                float4 break4_g910 = tex2D( _InnerTexture, fbuv13_g898 );
                float4 appendResult17_g910 = (float4(break4_g910.r , break4_g910.g , break4_g910.b , 1.0));
                float4 lerpResult314_g880 = lerp( lerpResult367_g880 , ( lerpResult367_g880 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g910.a : 1.0 ) ) * appendResult17_g910 ) ) , saturate( _InnerTextureOpacity ));
                #ifdef INNER_TEXTURE_ON
                float4 staticSwitch686_g880 = lerpResult314_g880;
                #else
                float4 staticSwitch686_g880 = lerpResult369_g880;
                #endif
                float4 ValueColorProcessed398_g880 = staticSwitch686_g880;
                float AA530_g880 = _AntiAlias;
                float temp_output_234_0_g880 = ( ( ( ( segment_count527_g880 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g880 ) > 0.0 ? 1.0 : 0.0 );
                float temp_output_220_0_g880 = ( OSX553_g880 / segment_count527_g880 );
                float temp_output_221_0_g880 = ( temp_output_220_0_g880 - ( temp_output_220_0_g880 * ( ( ( ( segment_count527_g880 * BorderWidth529_g880 ) * 2.0 ) / OSX553_g880 ) + Segment_Spacing533_g880 ) ) );
                float temp_output_188_0_g880 = max( 0.0 , Value574_g880 );
                float temp_output_181_0_g880 = ( max( ( segment_count527_g880 - temp_output_188_0_g880 ) , 0.0 ) / 2.0 );
                float temp_output_180_0_g880 = floor( temp_output_181_0_g880 );
                float temp_output_179_0_g880 = ( ( temp_output_180_0_g880 + 1.0 ) / segment_count527_g880 );
                float2 break11_g894 = IN.texcoord.xy;
                float temp_output_2_0_g894 = ( 1.0 > 0.0 ? ( ( break11_g894.x * -1.0 ) + 1.0 ) : break11_g894.x );
                float temp_output_171_0_g880 = step( temp_output_179_0_g880 , temp_output_2_0_g894 );
                float2 break11_g893 = IN.texcoord.xy;
                float temp_output_2_0_g893 = ( 0.0 > 0.0 ? ( ( break11_g893.x * -1.0 ) + 1.0 ) : break11_g893.x );
                float temp_output_173_0_g880 = step( temp_output_179_0_g880 , temp_output_2_0_g893 );
                float temp_output_215_0_g880 = ( temp_output_221_0_g880 * ( 1.0 - ( temp_output_181_0_g880 % 1.0 ) ) );
                float temp_output_176_0_g880 = ( temp_output_180_0_g880 / segment_count527_g880 );
                float temp_output_175_0_g880 = ( step( temp_output_176_0_g880 , temp_output_2_0_g894 ) - temp_output_171_0_g880 );
                float temp_output_174_0_g880 = ( step( temp_output_176_0_g880 , temp_output_2_0_g893 ) - temp_output_173_0_g880 );
                float temp_output_192_0_g880 = min( temp_output_175_0_g880 , temp_output_174_0_g880 );
                float2 appendResult196_g880 = (float2(( ( ( -temp_output_221_0_g880 * temp_output_171_0_g880 ) + ( temp_output_221_0_g880 * temp_output_173_0_g880 ) ) + ( ( -temp_output_215_0_g880 * ( temp_output_175_0_g880 - temp_output_192_0_g880 ) ) + ( temp_output_215_0_g880 * ( temp_output_174_0_g880 - temp_output_192_0_g880 ) ) ) ) , 0.0));
                float temp_output_151_0_g880 = ( OSX553_g880 / segment_count527_g880 );
                float temp_output_159_0_g880 = min( segment_count527_g880 , Value574_g880 );
                float temp_output_135_0_g880 = ( ( ( ( BorderWidth529_g880 * segment_count527_g880 ) * 2.0 ) / OSX553_g880 ) + Segment_Spacing533_g880 );
                float temp_output_160_0_g880 = floor( temp_output_159_0_g880 );
                float temp_output_154_0_g880 = step( ( ( temp_output_160_0_g880 + 1.0 ) / segment_count527_g880 ) , IN.texcoord.xy.x );
                float2 appendResult149_g880 = (float2(max( ( ( temp_output_151_0_g880 - ( temp_output_151_0_g880 * (temp_output_135_0_g880 + (( temp_output_159_0_g880 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g880) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g880 / segment_count527_g880 ) , IN.texcoord.xy.x ) - temp_output_154_0_g880 ) ) , ( ( temp_output_151_0_g880 - ( temp_output_135_0_g880 * temp_output_151_0_g880 ) ) * temp_output_154_0_g880 ) ) , 0.0));
                float2 temp_output_128_0_g880 = ( temp_output_234_0_g880 > 0.0 ? appendResult196_g880 : appendResult149_g880 );
                float2 temp_output_2_0_g944 = OSXY554_g880;
                float2 break22_g944 = -( temp_output_2_0_g944 / float2( 2,2 ) );
                float2 appendResult29_g944 = (float2(( 0.0 > 0.0 ? break22_g944.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g944.y : 0.0 )));
                float2 temp_output_2_0_g945 = ( ( PixelationUV559_g880 * temp_output_2_0_g944 ) + appendResult29_g944 );
                float temp_output_701_0_g880 = ( OSX553_g880 / segment_count527_g880 );
                float2 appendResult705_g880 = (float2(temp_output_701_0_g880 , OSY552_g880));
                float2 temp_output_11_0_g907 = appendResult705_g880;
                float2 temp_output_12_0_g907 = ( temp_output_2_0_g945 % temp_output_11_0_g907 );
                float2 break13_g907 = ( temp_output_12_0_g907 - ( temp_output_11_0_g907 / float2( 2,2 ) ) );
                float2 break14_g907 = temp_output_12_0_g907;
                float2 appendResult1_g907 = (float2(( 1.0 > 0.0 ? break13_g907.x : break14_g907.x ) , ( 1.0 > 0.0 ? break13_g907.y : break14_g907.y )));
                float2 SegmentUV521_g880 = appendResult1_g907;
                float2 temp_output_20_0_g892 = ( ( temp_output_128_0_g880 + SegmentUV521_g880 ) + ( OSXY554_g880 * _ValueMaskOffset ) );
                float2 break23_g892 = temp_output_20_0_g892;
                float BorderRadius548_g880 = _BorderRadius;
                float InnerRoundingPercent720_g880 = _InnerRoundingPercent;
                float temp_output_718_0_g880 = ( ( width_curve532_g880 * BorderRadius548_g880 ) * InnerRoundingPercent720_g880 );
                float temp_output_9_0_g895 = Width537_g880;
                float temp_output_118_0_g880 = ( ( saturate( ( 1.0 - Arc539_g880 ) ) * ( ( ( IN.texcoord.xy.y * temp_output_9_0_g895 ) + ( Radius536_g880 - ( temp_output_9_0_g895 / 2.0 ) ) ) * ( 6.28318548202515 * 1.0 ) ) ) / Radius536_g880 );
                #if defined(SHAPE_LINEAR)
                float staticSwitch249_g880 = temp_output_718_0_g880;
                #elif defined(SHAPE_CIRCULAR)
                float staticSwitch249_g880 = ( temp_output_118_0_g880 * temp_output_718_0_g880 );
                #else
                float staticSwitch249_g880 = temp_output_718_0_g880;
                #endif
                float Rounding13_g892 = staticSwitch249_g880;
                float4 BorderRadiusOffset547_g880 = _BorderRadiusOffset;
                float4 temp_output_717_0_g880 = ( ( width_curve532_g880 * BorderRadiusOffset547_g880 ) * InnerRoundingPercent720_g880 );
                #if defined(SHAPE_LINEAR)
                float4 staticSwitch246_g880 = temp_output_717_0_g880;
                #elif defined(SHAPE_CIRCULAR)
                float4 staticSwitch246_g880 = ( temp_output_118_0_g880 * temp_output_717_0_g880 );
                #else
                float4 staticSwitch246_g880 = temp_output_717_0_g880;
                #endif
                float4 break27_g892 = ( Rounding13_g892 + staticSwitch246_g880 );
                float2 appendResult25_g892 = (float2(break27_g892.x , break27_g892.w));
                float2 appendResult26_g892 = (float2(break27_g892.y , break27_g892.z));
                float2 break32_g892 = ( break23_g892.x > 0.0 ? appendResult25_g892 : appendResult26_g892 );
                float temp_output_31_0_g892 = ( break23_g892.y > 0.0 ? break32_g892.x : break32_g892.y );
                float2 appendResult520_g880 = (float2(temp_output_701_0_g880 , ( OSY552_g880 * width_curve532_g880 )));
                float2 appendResult512_g880 = (float2(( 0.5 - ( Segment_Spacing533_g880 / 2.0 ) ) , 0.5));
                float2 SegmentSize619_g880 = ( ( appendResult520_g880 * appendResult512_g880 ) + float2( 0,-0.01 ) );
                float temp_output_211_0_g880 = ( segment_count527_g880 * 2.0 );
                float2 appendResult710_g880 = (float2(( temp_output_192_0_g880 * ( ( 1.0 - temp_output_188_0_g880 ) * ( ( ( OSX553_g880 / temp_output_211_0_g880 ) - BorderWidth529_g880 ) - ( ( OSX553_g880 * Segment_Spacing533_g880 ) / temp_output_211_0_g880 ) ) ) ) , 0.0));
                float2 temp_output_10_0_g892 = ( ( float2( 1,1 ) * temp_output_31_0_g892 ) + ( abs( temp_output_20_0_g892 ) - ( SegmentSize619_g880 - ( temp_output_234_0_g880 > 0.0 ? appendResult710_g880 : float2( 0,0 ) ) ) ) );
                float2 break8_g892 = temp_output_10_0_g892;
                float2 temp_output_20_0_g891 = SegmentUV521_g880;
                float2 break23_g891 = temp_output_20_0_g891;
                float AdjustBorderRadiusToWidthCurve557_g880 = _AdjustBorderRadiusToWidthCurve;
                float temp_output_9_0_g905 = Width537_g880;
                float temp_output_507_0_g880 = ( ( saturate( ( 1.0 - Arc539_g880 ) ) * ( ( ( IN.texcoord.xy.y * temp_output_9_0_g905 ) + ( Radius536_g880 - ( temp_output_9_0_g905 / 2.0 ) ) ) * ( 6.28318548202515 * 1.0 ) ) ) / Radius536_g880 );
                #if defined(SHAPE_LINEAR)
                float staticSwitch523_g880 = BorderRadius548_g880;
                #elif defined(SHAPE_CIRCULAR)
                float staticSwitch523_g880 = ( BorderRadius548_g880 * temp_output_507_0_g880 );
                #else
                float staticSwitch523_g880 = BorderRadius548_g880;
                #endif
                float SegmentRounding518_g880 = ( AdjustBorderRadiusToWidthCurve557_g880 > 0.0 ? ( staticSwitch523_g880 * width_curve532_g880 ) : staticSwitch523_g880 );
                float Rounding13_g891 = ( SegmentRounding518_g880 * 1.0 );
                #if defined(SHAPE_LINEAR)
                float4 staticSwitch723_g880 = BorderRadiusOffset547_g880;
                #elif defined(SHAPE_CIRCULAR)
                float4 staticSwitch723_g880 = ( BorderRadiusOffset547_g880 * temp_output_507_0_g880 );
                #else
                float4 staticSwitch723_g880 = BorderRadiusOffset547_g880;
                #endif
                float4 SegmentRoundingOffset519_g880 = ( AdjustBorderRadiusToWidthCurve557_g880 > 0.0 ? ( width_curve532_g880 * staticSwitch723_g880 ) : staticSwitch723_g880 );
                float4 break27_g891 = ( Rounding13_g891 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g880 ) );
                float2 appendResult25_g891 = (float2(break27_g891.x , break27_g891.w));
                float2 appendResult26_g891 = (float2(break27_g891.y , break27_g891.z));
                float2 break32_g891 = ( break23_g891.x > 0.0 ? appendResult25_g891 : appendResult26_g891 );
                float temp_output_31_0_g891 = ( break23_g891.y > 0.0 ? break32_g891.x : break32_g891.y );
                float2 temp_output_10_0_g891 = ( ( float2( 1,1 ) * temp_output_31_0_g891 ) + ( abs( temp_output_20_0_g891 ) - SegmentSize619_g880 ) );
                float2 break8_g891 = temp_output_10_0_g891;
                float temp_output_89_0_g880 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g892 ) ) + min( max( break8_g892.x , break8_g892.y ) , 0.0 ) ) - temp_output_31_0_g892 ) + BorderWidth529_g880 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g891 ) ) + min( max( break8_g891.x , break8_g891.y ) , 0.0 ) ) - temp_output_31_0_g891 ) + BorderWidth529_g880 ) ) - InnerBorderWidth250_g880 );
                float temp_output_3_0_g885 = ( 0.0 + 0.0 + temp_output_89_0_g880 );
                float InnerValue240_g880 = ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g885 / fwidth( temp_output_89_0_g880 ) ) ) : step( 0.0 , temp_output_3_0_g885 ) );
                float4 lerpResult674_g880 = lerp( appendResult675_g880 , ValueColorProcessed398_g880 , max( ( 1.0 - break679_g880.w ) , InnerValue240_g880 ));
                float temp_output_15_0_g924 = _ValueInsetShadowSize;
                float temp_output_4_0_g924 = saturate( ceil( temp_output_15_0_g924 ) );
                float4 break4_g926 = _ValueInsetShadowColor;
                float4 appendResult17_g926 = (float4(break4_g926.r , break4_g926.g , break4_g926.b , 1.0));
                float temp_output_86_0_g880 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g892 ) ) + min( max( break8_g892.x , break8_g892.y ) , 0.0 ) ) - temp_output_31_0_g892 ) + BorderWidth529_g880 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g891 ) ) + min( max( break8_g891.x , break8_g891.y ) , 0.0 ) ) - temp_output_31_0_g891 ) + BorderWidth529_g880 ) ) + 0.0 + 0.0 );
                float temp_output_3_0_g886 = temp_output_86_0_g880;
                float ValueView242_g880 = ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g886 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g892 ) ) + min( max( break8_g892.x , break8_g892.y ) , 0.0 ) ) - temp_output_31_0_g892 ) + BorderWidth529_g880 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g891 ) ) + min( max( break8_g891.x , break8_g891.y ) , 0.0 ) ) - temp_output_31_0_g891 ) + BorderWidth529_g880 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g886 ) );
                float ValueSDF241_g880 = temp_output_86_0_g880;
                float temp_output_2_0_g925 = ValueSDF241_g880;
                float4 lerpResult673_g880 = lerp( ( InnerBorderWidth250_g880 > 0.0 ? lerpResult674_g880 : ValueColorProcessed398_g880 ) , ( ( saturate( temp_output_4_0_g924 ) * ( 1.0 > 0.0 ? break4_g926.a : 1.0 ) ) * appendResult17_g926 ) , ( temp_output_4_0_g924 * min( ValueView242_g880 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g925 : temp_output_2_0_g925 ) / max( temp_output_15_0_g924 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
                float4 zzLerp_Value685_g880 = lerpResult673_g880;
                float4 lerpResult657_g880 = lerp( BorderColorProcessed497_g880 , zzLerp_Value685_g880 , ValueView242_g880);
                float temp_output_15_0_g939 = _BorderInsetShadowSize;
                float temp_output_4_0_g939 = saturate( ceil( temp_output_15_0_g939 ) );
                float4 break4_g941 = _BorderInsetShadowColor;
                float4 appendResult17_g941 = (float4(break4_g941.r , break4_g941.g , break4_g941.b , 1.0));
                float2 temp_output_20_0_g906 = SegmentUV521_g880;
                float2 break23_g906 = temp_output_20_0_g906;
                float Rounding13_g906 = SegmentRounding518_g880;
                float4 break27_g906 = ( Rounding13_g906 + SegmentRoundingOffset519_g880 );
                float2 appendResult25_g906 = (float2(break27_g906.x , break27_g906.w));
                float2 appendResult26_g906 = (float2(break27_g906.y , break27_g906.z));
                float2 break32_g906 = ( break23_g906.x > 0.0 ? appendResult25_g906 : appendResult26_g906 );
                float temp_output_31_0_g906 = ( break23_g906.y > 0.0 ? break32_g906.x : break32_g906.y );
                float2 temp_output_10_0_g906 = ( ( float2( 1,1 ) * temp_output_31_0_g906 ) + ( abs( temp_output_20_0_g906 ) - SegmentSize619_g880 ) );
                float2 break8_g906 = temp_output_10_0_g906;
                float temp_output_615_0_g880 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g906 ) ) + min( max( break8_g906.x , break8_g906.y ) , 0.0 ) ) - temp_output_31_0_g906 );
                float PB_SDF_Negated618_g880 = -temp_output_615_0_g880;
                float temp_output_654_0_g880 = ( PB_SDF_Negated618_g880 - BorderWidth529_g880 );
                float temp_output_3_0_g922 = temp_output_654_0_g880;
                float temp_output_2_0_g940 = temp_output_654_0_g880;
                float4 lerpResult645_g880 = lerp( lerpResult657_g880 , ( ( saturate( temp_output_4_0_g939 ) * ( 1.0 > 0.0 ? break4_g941.a : 1.0 ) ) * appendResult17_g941 ) , ( temp_output_4_0_g939 * min( ( 1.0 - ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g922 / fwidth( temp_output_654_0_g880 ) ) ) : step( 0.0 , temp_output_3_0_g922 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g940 : temp_output_2_0_g940 ) / max( temp_output_15_0_g939 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
                float4 zzLerp_Border666_g880 = lerpResult645_g880;
                float4 break4_g913 = _BackgroundColor;
                float4 appendResult17_g913 = (float4(break4_g913.r , break4_g913.g , break4_g913.b , 1.0));
                float4 temp_output_743_0_g880 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g913.a : 1.0 ) ) * appendResult17_g913 );
                float2 temp_cast_5 = (saturate( ( Value574_g880 / segment_count527_g880 ) )).xx;
                float cos478_g880 = cos( radians( _BackgroundGradientRotation ) );
                float sin478_g880 = sin( radians( _BackgroundGradientRotation ) );
                float2 rotator478_g880 = mul( GradientUV479_g880 - float2( 0.5,0.5 ) , float2x2( cos478_g880 , -sin478_g880 , sin478_g880 , cos478_g880 )) + float2( 0.5,0.5 );
                float4 break4_g912 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g880 ) );
                float4 appendResult17_g912 = (float4(break4_g912.r , break4_g912.g , break4_g912.b , 1.0));
                float4 temp_output_403_0_g880 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g912.a : 1.0 ) ) * appendResult17_g912 ) * temp_output_743_0_g880 ) : temp_output_743_0_g880 );
                float BG_Tex_Scale_w_Segments414_g880 = _BackgroundTextureScaleWithSegments;
                float2 BG_Tex_Tiling417_g880 = _BackgroundTextureTiling;
                float temp_output_453_0_g880 = ( BG_Tex_Scale_w_Segments414_g880 > 0.0 ? 1.0 : segment_count527_g880 );
                float temp_output_462_0_g880 = ( ( ( BG_Tex_Tiling417_g880.x * OSX553_g880 ) * temp_output_453_0_g880 ) / ( ( ( temp_output_453_0_g880 * OSX553_g880 ) + temp_output_444_0_g880 ) - temp_output_449_0_g880 ) );
                float temp_output_429_0_g880 = ( BG_Tex_Tiling417_g880.y / ( width_curve532_g880 - ( BorderWidth529_g880 * ( 2.0 / OSY552_g880 ) ) ) );
                float2 appendResult483_g880 = (float2(temp_output_462_0_g880 , temp_output_429_0_g880));
                float2 appendResult486_g880 = (float2(( -( ( temp_output_462_0_g880 - BG_Tex_Tiling417_g880.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g880 / 2.0 ) + 0.5 ) )));
                float2 temp_output_2_0_g903 = ( ( ( BG_Tex_Scale_w_Segments414_g880 > 0.0 ? ScaledTextureUV349_g880 : UnscaledTextureUV350_g880 ) * appendResult483_g880 ) + appendResult486_g880 );
                float cos472_g880 = cos( radians( _BackgroundTextureRotation ) );
                float sin472_g880 = sin( radians( _BackgroundTextureRotation ) );
                float2 rotator472_g880 = mul( temp_output_2_0_g903 - float2( 0.5,0.5 ) , float2x2( cos472_g880 , -sin472_g880 , sin472_g880 , cos472_g880 )) + float2( 0.5,0.5 );
                float2 break468_g880 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
                float fbtotaltiles13_g902 = break468_g880.x * break468_g880.y;
                float fbcolsoffset13_g902 = 1.0f / break468_g880.x;
                float fbrowsoffset13_g902 = 1.0f / break468_g880.y;
                float fbspeed13_g902 = _Time.y * fps541_g880;
                float2 fbtiling13_g902 = float2(fbcolsoffset13_g902, fbrowsoffset13_g902);
                float fbcurrenttileindex13_g902 = round( fmod( fbspeed13_g902 + 0.0, fbtotaltiles13_g902) );
                fbcurrenttileindex13_g902 += ( fbcurrenttileindex13_g902 < 0) ? fbtotaltiles13_g902 : 0;
                float fblinearindextox13_g902 = round ( fmod ( fbcurrenttileindex13_g902, break468_g880.x ) );
                float fboffsetx13_g902 = fblinearindextox13_g902 * fbcolsoffset13_g902;
                float fblinearindextoy13_g902 = round( fmod( ( fbcurrenttileindex13_g902 - fblinearindextox13_g902 ) / break468_g880.x, break468_g880.y ) );
                fblinearindextoy13_g902 = (int)(break468_g880.y-1) - fblinearindextoy13_g902;
                float fboffsety13_g902 = fblinearindextoy13_g902 * fbrowsoffset13_g902;
                float2 fboffset13_g902 = float2(fboffsetx13_g902, fboffsety13_g902);
                half2 fbuv13_g902 = rotator472_g880 * fbtiling13_g902 + fboffset13_g902;
                float4 break4_g914 = tex2D( _BackgroundTexture, fbuv13_g902 );
                float4 appendResult17_g914 = (float4(break4_g914.r , break4_g914.g , break4_g914.b , 1.0));
                float4 lerpResult400_g880 = lerp( temp_output_403_0_g880 , ( temp_output_403_0_g880 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g914.a : 1.0 ) ) * appendResult17_g914 ) ) , saturate( _BackgroundTextureOpacity ));
                #ifdef BACKGROUND_TEXTURE_ON
                float4 staticSwitch494_g880 = lerpResult400_g880;
                #else
                float4 staticSwitch494_g880 = temp_output_743_0_g880;
                #endif
                float4 BackgroundColorProcessed495_g880 = staticSwitch494_g880;
                float temp_output_639_0_g880 = ( PB_SDF_Negated618_g880 - BorderWidth529_g880 );
                float temp_output_3_0_g923 = temp_output_639_0_g880;
                float temp_output_638_0_g880 = ( ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g923 / fwidth( temp_output_639_0_g880 ) ) ) : step( 0.0 , temp_output_3_0_g923 ) ) - ValueView242_g880 );
                float4 lerpResult636_g880 = lerp( zzLerp_Border666_g880 , BackgroundColorProcessed495_g880 , temp_output_638_0_g880);
                float temp_output_15_0_g934 = _ValueShadowSize;
                float temp_output_4_0_g934 = saturate( ceil( temp_output_15_0_g934 ) );
                float4 break4_g936 = _ValueShadowColor;
                float4 appendResult17_g936 = (float4(break4_g936.r , break4_g936.g , break4_g936.b , 1.0));
                float temp_output_2_0_g935 = ValueSDF241_g880;
                float4 lerpResult634_g880 = lerp( lerpResult636_g880 , ( ( saturate( temp_output_4_0_g934 ) * ( 1.0 > 0.0 ? break4_g936.a : 1.0 ) ) * appendResult17_g936 ) , ( temp_output_4_0_g934 * min( temp_output_638_0_g880 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g935 : temp_output_2_0_g935 ) / max( temp_output_15_0_g934 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
                float4 zzLerp_Background642_g880 = lerpResult634_g880;
                float temp_output_15_0_g929 = _BorderShadowSize;
                float temp_output_4_0_g929 = saturate( ceil( temp_output_15_0_g929 ) );
                float4 break4_g931 = _BorderShadowColor;
                float4 appendResult17_g931 = (float4(break4_g931.r , break4_g931.g , break4_g931.b , 1.0));
                float temp_output_625_0_g880 = ( PB_SDF_Negated618_g880 - BorderWidth529_g880 );
                float temp_output_3_0_g921 = temp_output_625_0_g880;
                float temp_output_2_0_g930 = temp_output_625_0_g880;
                float4 lerpResult620_g880 = lerp( zzLerp_Background642_g880 , ( ( saturate( temp_output_4_0_g929 ) * ( 1.0 > 0.0 ? break4_g931.a : 1.0 ) ) * appendResult17_g931 ) , ( temp_output_4_0_g929 * min( ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g921 / fwidth( temp_output_625_0_g880 ) ) ) : step( 0.0 , temp_output_3_0_g921 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g930 : temp_output_2_0_g930 ) / max( temp_output_15_0_g929 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
                float4 zzLerp_Border_Shadow629_g880 = lerpResult620_g880;
                float4 temp_output_608_0_g880 = ( OverlayColorProcessed524_g880 * zzLerp_Border_Shadow629_g880 );
                float PB_SDF616_g880 = temp_output_615_0_g880;
                float temp_output_3_0_g919 = PB_SDF616_g880;
                float temp_output_534_0_g880 = min( temp_output_608_0_g880.a , ( 1.0 - ( AA530_g880 > 0.0 ? saturate( ( temp_output_3_0_g919 / fwidth( PB_SDF616_g880 ) ) ) : step( 0.0 , temp_output_3_0_g919 ) ) ) );
                float4 break726_g880 = temp_output_608_0_g880;
                float3 appendResult727_g880 = (float3(break726_g880.r , break726_g880.g , break726_g880.b));
                float4 break4_g496 = float4( appendResult727_g880 , 0.0 );
                float4 appendResult17_g496 = (float4(break4_g496.r , break4_g496.g , break4_g496.b , 1.0));
                

                half4 color = ( ( saturate( temp_output_534_0_g880 ) * ( 0.0 > 0.0 ? break4_g496.a : 1.0 ) ) * appendResult17_g496 );

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "Renge.PPB.ProceduralProgressBarGUI"
	
	Fallback Off
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1020;1267.325,-195.0483;Float;False;True;-1;2;Renge.PPB.ProceduralProgressBarGUI;0;3;Renge/PPB_UI;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;4;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.FunctionNode;1031;869.4007,-400.8072;Inherit;True;ColorMul;-1;;496;6d00832ca1d99d6498e080fa6963521f;0;3;5;FLOAT;1;False;6;COLOR;0,0.3821156,1,1;False;7;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;1042;503.4006,-231.8072;Inherit;True;The Whole Shebang;0;;880;2d6870fee17216f4db3628575a74016f;0;0;3;FLOAT3;0;FLOAT;728;FLOAT3;729
WireConnection;1020;0;1031;0
WireConnection;1020;1;1042;729
WireConnection;1031;5;1042;728
WireConnection;1031;6;1042;0
ASEEND*/
//CHKSM=D5FD2D67C0BEAD36305494A2889E4388B6BF3B49