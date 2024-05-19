// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Renge/PPB_BuiltIn"
{
	Properties
	{
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
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "DisableBatching" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha , One OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.0
		#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
		#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
		#pragma multi_compile_local __ BORDER_TEXTURE_ON
		#pragma multi_compile_local __ INNER_TEXTURE_ON
		#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

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

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 appendResult582_g421 = (float3(( ( ( v.texcoord.xy.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
			#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g421 = appendResult582_g421;
			#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g421 = float3(0,0,0);
			#else
				float3 staticSwitch581_g421 = appendResult582_g421;
			#endif
			v.vertex.xyz += staticSwitch581_g421;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float Pixelate531_g421 = _Pixelate;
			float PixelCount545_g421 = _PixelCount;
			#if defined(SHAPE_LINEAR)
				float staticSwitch687_g421 = 0.0;
			#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g421 = 1.0;
			#else
				float staticSwitch687_g421 = 0.0;
			#endif
			float temp_output_588_0_g421 = ( staticSwitch687_g421 > 0.0 ? 1.0 : 0.0 );
			float Arc539_g421 = _Arc;
			float Width537_g421 = _Width;
			float temp_output_9_0_g459 = Width537_g421;
			float Radius536_g421 = _Radius;
			float2 appendResult587_g421 = (float2(( saturate( ( 1.0 - Arc539_g421 ) ) * ( ( ( i.uv_texcoord.y * temp_output_9_0_g459 ) + ( Radius536_g421 - ( temp_output_9_0_g459 / 2.0 ) ) ) * ( 6.28318548202515 * _CircleLength ) ) ) , Width537_g421));
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float3 appendResult28_g490 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
			float3 appendResult29_g490 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
			float3 appendResult30_g490 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
			float3 appendResult24_g490 = (float3(length( appendResult28_g490 ) , length( appendResult29_g490 ) , length( appendResult30_g490 )));
			float3 temp_output_38_0_g490 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g490 );
			float3 temp_output_16_0_g490 = ( ( ( temp_output_588_0_g421 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g421 > 0.0 ? appendResult587_g421 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g490 );
			float3 break9_g490 = temp_output_16_0_g490;
			float3 break48_g490 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g490 / min( break9_g490.x , break9_g490.y ) ) : temp_output_16_0_g490 );
			float2 appendResult10_g490 = (float2(break48_g490.x , break48_g490.y));
			float2 OSXY554_g421 = appendResult10_g490;
			float2 temp_output_6_0_g488 = ( PixelCount545_g421 * OSXY554_g421 );
			float2 PixelationUV559_g421 = ( Pixelate531_g421 > 0.0 ? ( floor( ( i.uv_texcoord * temp_output_6_0_g488 ) ) / ( temp_output_6_0_g488 - float2( 1,1 ) ) ) : i.uv_texcoord );
			float2 temp_output_2_0_g424 = ( ( PixelationUV559_g421 * _OverlayTextureTiling ) + _OverlayTextureOffset );
			float2 break51_g421 = max( _OverlayFlipbookDim , float2( 1,1 ) );
			float fps541_g421 = _FlipbookFPS;
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles13_g423 = break51_g421.x * break51_g421.y;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset13_g423 = 1.0f / break51_g421.x;
			float fbrowsoffset13_g423 = 1.0f / break51_g421.y;
			// Speed of animation
			float fbspeed13_g423 = _Time.y * fps541_g421;
			// UV Tiling (col and row offset)
			float2 fbtiling13_g423 = float2(fbcolsoffset13_g423, fbrowsoffset13_g423);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex13_g423 = round( fmod( fbspeed13_g423 + 0.0, fbtotaltiles13_g423) );
			fbcurrenttileindex13_g423 += ( fbcurrenttileindex13_g423 < 0) ? fbtotaltiles13_g423 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox13_g423 = round ( fmod ( fbcurrenttileindex13_g423, break51_g421.x ) );
			// Multiply Offset X by coloffset
			float fboffsetx13_g423 = fblinearindextox13_g423 * fbcolsoffset13_g423;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy13_g423 = round( fmod( ( fbcurrenttileindex13_g423 - fblinearindextox13_g423 ) / break51_g421.x, break51_g421.y ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy13_g423 = (int)(break51_g421.y-1) - fblinearindextoy13_g423;
			// Multiply Offset Y by rowoffset
			float fboffsety13_g423 = fblinearindextoy13_g423 * fbrowsoffset13_g423;
			// UV Offset
			float2 fboffset13_g423 = float2(fboffsetx13_g423, fboffsety13_g423);
			// Flipbook UV
			half2 fbuv13_g423 = temp_output_2_0_g424 * fbtiling13_g423 + fboffset13_g423;
			// *** END Flipbook UV Animation vars ***
			float4 lerpResult45_g421 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g423 ) ) , saturate( _OverlayTextureOpacity ));
			#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g421 = lerpResult45_g421;
			#else
				float4 staticSwitch44_g421 = _OverlayColor;
			#endif
			float4 OverlayColorProcessed524_g421 = staticSwitch44_g421;
			float BorderWidth529_g421 = _BorderWidth;
			float4 break4_g450 = _BorderColor;
			float4 appendResult17_g450 = (float4(break4_g450.r , break4_g450.g , break4_g450.b , 1.0));
			float4 temp_output_738_0_g421 = ( ( saturate( ceil( BorderWidth529_g421 ) ) * ( 1.0 > 0.0 ? break4_g450.a : 1.0 ) ) * appendResult17_g450 );
			float segment_count527_g421 = _SegmentCount;
			float2 appendResult345_g421 = (float2(segment_count527_g421 , 1.0));
			float2 temp_output_2_0_g442 = ( ( PixelationUV559_g421 * appendResult345_g421 ) + float2( 0,0 ) );
			float2 break10_g442 = temp_output_2_0_g442;
			float2 appendResult352_g421 = (float2(( break10_g442.x % 1.0 ) , break10_g442.y));
			float2 ScaledTextureUV349_g421 = appendResult352_g421;
			float2 temp_output_2_0_g441 = ( ( PixelationUV559_g421 * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 UnscaledTextureUV350_g421 = temp_output_2_0_g441;
			float2 break77_g421 = _BorderTextureTiling;
			float2 uv_VariableWidthCurve = i.uv_texcoord * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
			float width_curve532_g421 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
			float temp_output_75_0_g421 = ( break77_g421.y / width_curve532_g421 );
			float2 appendResult74_g421 = (float2(break77_g421.x , temp_output_75_0_g421));
			float2 appendResult70_g421 = (float2(0.0 , ( -( temp_output_75_0_g421 / 2.0 ) + 0.5 )));
			float2 temp_output_2_0_g425 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g421 : UnscaledTextureUV350_g421 ) * appendResult74_g421 ) + ( _BorderTextureOffset + appendResult70_g421 ) );
			float cos63_g421 = cos( radians( _BorderTextureRotation ) );
			float sin63_g421 = sin( radians( _BorderTextureRotation ) );
			float2 rotator63_g421 = mul( temp_output_2_0_g425 - float2( 0.5,0.5 ) , float2x2( cos63_g421 , -sin63_g421 , sin63_g421 , cos63_g421 )) + float2( 0.5,0.5 );
			float2 break39_g421 = max( _BorderFlipbookDim , float2( 1,1 ) );
			float fbtotaltiles13_g422 = break39_g421.x * break39_g421.y;
			float fbcolsoffset13_g422 = 1.0f / break39_g421.x;
			float fbrowsoffset13_g422 = 1.0f / break39_g421.y;
			float fbspeed13_g422 = _Time.y * fps541_g421;
			float2 fbtiling13_g422 = float2(fbcolsoffset13_g422, fbrowsoffset13_g422);
			float fbcurrenttileindex13_g422 = round( fmod( fbspeed13_g422 + 0.0, fbtotaltiles13_g422) );
			fbcurrenttileindex13_g422 += ( fbcurrenttileindex13_g422 < 0) ? fbtotaltiles13_g422 : 0;
			float fblinearindextox13_g422 = round ( fmod ( fbcurrenttileindex13_g422, break39_g421.x ) );
			float fboffsetx13_g422 = fblinearindextox13_g422 * fbcolsoffset13_g422;
			float fblinearindextoy13_g422 = round( fmod( ( fbcurrenttileindex13_g422 - fblinearindextox13_g422 ) / break39_g421.x, break39_g421.y ) );
			fblinearindextoy13_g422 = (int)(break39_g421.y-1) - fblinearindextoy13_g422;
			float fboffsety13_g422 = fblinearindextoy13_g422 * fbrowsoffset13_g422;
			float2 fboffset13_g422 = float2(fboffsetx13_g422, fboffsety13_g422);
			half2 fbuv13_g422 = rotator63_g421 * fbtiling13_g422 + fboffset13_g422;
			float4 lerpResult35_g421 = lerp( temp_output_738_0_g421 , ( tex2D( _BorderTexture, fbuv13_g422 ) * temp_output_738_0_g421 ) , saturate( _BorderTextureOpacity ));
			#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g421 = lerpResult35_g421;
			#else
				float4 staticSwitch496_g421 = temp_output_738_0_g421;
			#endif
			float4 BorderColorProcessed497_g421 = staticSwitch496_g421;
			float InnerBorderWidth250_g421 = _InnerBorderWidth;
			float4 break4_g456 = _InnerBorderColor;
			float4 appendResult17_g456 = (float4(break4_g456.r , break4_g456.g , break4_g456.b , 1.0));
			float4 temp_output_745_0_g421 = ( ( saturate( ceil( InnerBorderWidth250_g421 ) ) * ( 1.0 > 0.0 ? break4_g456.a : 1.0 ) ) * appendResult17_g456 );
			float4 break4_g458 = _PulseColor;
			float4 appendResult17_g458 = (float4(break4_g458.r , break4_g458.g , break4_g458.b , 1.0));
			float4 PulseColorProcessed384_g421 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g458.a : 1.0 ) ) * appendResult17_g458 );
			float Value574_g421 = _Value;
			float temp_output_1_0_g440 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
			float PulseAlpha382_g421 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _Time.y * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g421 / segment_count527_g421 ) - temp_output_1_0_g440 ) / ( _PulseActivationThreshold - temp_output_1_0_g440 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
			float4 lerpResult389_g421 = lerp( temp_output_745_0_g421 , PulseColorProcessed384_g421 , PulseAlpha382_g421);
			float2 temp_cast_4 = (saturate( ( Value574_g421 / segment_count527_g421 ) )).xx;
			float OSX553_g421 = break48_g490.x;
			float temp_output_444_0_g421 = ( ( BorderWidth529_g421 * segment_count527_g421 ) * -2.0 );
			float Segment_Spacing533_g421 = _SegmentSpacing;
			float temp_output_449_0_g421 = ( Segment_Spacing533_g421 * OSX553_g421 );
			float temp_output_408_0_g421 = ( ( segment_count527_g421 * OSX553_g421 ) / ( ( temp_output_444_0_g421 + ( OSX553_g421 * segment_count527_g421 ) ) - temp_output_449_0_g421 ) );
			float2 appendResult422_g421 = (float2(temp_output_408_0_g421 , 1.0));
			float2 appendResult407_g421 = (float2(-( ( temp_output_408_0_g421 - 1.0 ) / 2.0 ) , 0.0));
			float2 temp_output_2_0_g445 = ( ( PixelationUV559_g421 * appendResult422_g421 ) + appendResult407_g421 );
			float2 GradientUV479_g421 = temp_output_2_0_g445;
			float cos363_g421 = cos( radians( _InnerGradientRotation ) );
			float sin363_g421 = sin( radians( _InnerGradientRotation ) );
			float2 rotator363_g421 = mul( GradientUV479_g421 - float2( 0.5,0.5 ) , float2x2( cos363_g421 , -sin363_g421 , sin363_g421 , cos363_g421 )) + float2( 0.5,0.5 );
			float4 break4_g452 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g421 ) );
			float4 appendResult17_g452 = (float4(break4_g452.r , break4_g452.g , break4_g452.b , 1.0));
			float4 temp_output_740_0_g421 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g452.a : 1.0 ) ) * appendResult17_g452 );
			float4 lerpResult390_g421 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g421 * temp_output_740_0_g421 ) : temp_output_745_0_g421 ) , PulseColorProcessed384_g421 , PulseAlpha382_g421);
			#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g421 = lerpResult390_g421;
			#else
				float4 staticSwitch388_g421 = lerpResult389_g421;
			#endif
			float4 ValueBorderColorProcessed525_g421 = staticSwitch388_g421;
			float4 break679_g421 = ValueBorderColorProcessed525_g421;
			float4 appendResult675_g421 = (float4(break679_g421.x , break679_g421.y , break679_g421.z , 1.0));
			float4 break4_g457 = _InnerColor;
			float4 appendResult17_g457 = (float4(break4_g457.r , break4_g457.g , break4_g457.b , 1.0));
			float4 temp_output_746_0_g421 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g457.a : 1.0 ) ) * appendResult17_g457 );
			float4 lerpResult369_g421 = lerp( temp_output_746_0_g421 , PulseColorProcessed384_g421 , PulseAlpha382_g421);
			float4 lerpResult367_g421 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g421 * temp_output_746_0_g421 ) : temp_output_746_0_g421 ) , PulseColorProcessed384_g421 , PulseAlpha382_g421);
			float Inner_Tex_Scale_w_Segments252_g421 = _InnerTextureScaleWithSegments;
			float2 Inner_Tex_Tiling254_g421 = _InnerTextureTiling;
			float temp_output_330_0_g421 = ( Inner_Tex_Scale_w_Segments252_g421 > 0.0 ? 1.0 : segment_count527_g421 );
			float temp_output_324_0_g421 = ( ( ( Inner_Tex_Tiling254_g421.x * OSX553_g421 ) * temp_output_330_0_g421 ) / ( ( ( temp_output_330_0_g421 * OSX553_g421 ) + ( ( BorderWidth529_g421 * segment_count527_g421 ) * -2.0 ) ) - ( OSX553_g421 * Segment_Spacing533_g421 ) ) );
			float OSY552_g421 = break48_g490.y;
			float temp_output_270_0_g421 = ( Inner_Tex_Tiling254_g421.y / ( width_curve532_g421 - ( BorderWidth529_g421 * ( 2.0 / OSY552_g421 ) ) ) );
			float2 appendResult276_g421 = (float2(temp_output_324_0_g421 , temp_output_270_0_g421));
			float CenterFill562_g421 = _CenterFill;
			float2 temp_output_2_0_g438 = ( ( i.uv_texcoord * float2( 1,1 ) ) + float2( 0,0 ) );
			float2 break10_g438 = temp_output_2_0_g438;
			float lerpResult321_g421 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g421 > 0.0 ? ( 1.0 - ( min( Value574_g421 , segment_count527_g421 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g421 / segment_count527_g421 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g421 > 0.0 ? ( floor( Value574_g421 ) / segment_count527_g421 ) : 0.0 ) , break10_g438.x ));
			float2 appendResult277_g421 = (float2(( ( -( ( temp_output_324_0_g421 - Inner_Tex_Tiling254_g421.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g421.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g421 > 0.0 ? 0.0 : lerpResult321_g421 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g421 / 2.0 ) + 0.5 ) )));
			float2 temp_output_2_0_g437 = ( ( ( Inner_Tex_Scale_w_Segments252_g421 > 0.0 ? ScaledTextureUV349_g421 : UnscaledTextureUV350_g421 ) * appendResult276_g421 ) + appendResult277_g421 );
			float cos299_g421 = cos( radians( _InnerTextureRotation ) );
			float sin299_g421 = sin( radians( _InnerTextureRotation ) );
			float2 rotator299_g421 = mul( temp_output_2_0_g437 - float2( 0.5,0.5 ) , float2x2( cos299_g421 , -sin299_g421 , sin299_g421 , cos299_g421 )) + float2( 0.5,0.5 );
			float2 break275_g421 = max( _InnerFlipbookDim , float2( 1,1 ) );
			float fbtotaltiles13_g439 = break275_g421.x * break275_g421.y;
			float fbcolsoffset13_g439 = 1.0f / break275_g421.x;
			float fbrowsoffset13_g439 = 1.0f / break275_g421.y;
			float fbspeed13_g439 = _Time.y * fps541_g421;
			float2 fbtiling13_g439 = float2(fbcolsoffset13_g439, fbrowsoffset13_g439);
			float fbcurrenttileindex13_g439 = round( fmod( fbspeed13_g439 + 0.0, fbtotaltiles13_g439) );
			fbcurrenttileindex13_g439 += ( fbcurrenttileindex13_g439 < 0) ? fbtotaltiles13_g439 : 0;
			float fblinearindextox13_g439 = round ( fmod ( fbcurrenttileindex13_g439, break275_g421.x ) );
			float fboffsetx13_g439 = fblinearindextox13_g439 * fbcolsoffset13_g439;
			float fblinearindextoy13_g439 = round( fmod( ( fbcurrenttileindex13_g439 - fblinearindextox13_g439 ) / break275_g421.x, break275_g421.y ) );
			fblinearindextoy13_g439 = (int)(break275_g421.y-1) - fblinearindextoy13_g439;
			float fboffsety13_g439 = fblinearindextoy13_g439 * fbrowsoffset13_g439;
			float2 fboffset13_g439 = float2(fboffsetx13_g439, fboffsety13_g439);
			half2 fbuv13_g439 = rotator299_g421 * fbtiling13_g439 + fboffset13_g439;
			float4 break4_g451 = tex2D( _InnerTexture, fbuv13_g439 );
			float4 appendResult17_g451 = (float4(break4_g451.r , break4_g451.g , break4_g451.b , 1.0));
			float4 lerpResult314_g421 = lerp( lerpResult367_g421 , ( lerpResult367_g421 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g451.a : 1.0 ) ) * appendResult17_g451 ) ) , saturate( _InnerTextureOpacity ));
			#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g421 = lerpResult314_g421;
			#else
				float4 staticSwitch686_g421 = lerpResult369_g421;
			#endif
			float4 ValueColorProcessed398_g421 = staticSwitch686_g421;
			float AA530_g421 = _AntiAlias;
			float temp_output_234_0_g421 = ( ( ( ( segment_count527_g421 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g421 ) > 0.0 ? 1.0 : 0.0 );
			float temp_output_220_0_g421 = ( OSX553_g421 / segment_count527_g421 );
			float temp_output_221_0_g421 = ( temp_output_220_0_g421 - ( temp_output_220_0_g421 * ( ( ( ( segment_count527_g421 * BorderWidth529_g421 ) * 2.0 ) / OSX553_g421 ) + Segment_Spacing533_g421 ) ) );
			float temp_output_188_0_g421 = max( 0.0 , Value574_g421 );
			float temp_output_181_0_g421 = ( max( ( segment_count527_g421 - temp_output_188_0_g421 ) , 0.0 ) / 2.0 );
			float temp_output_180_0_g421 = floor( temp_output_181_0_g421 );
			float temp_output_179_0_g421 = ( ( temp_output_180_0_g421 + 1.0 ) / segment_count527_g421 );
			float2 break11_g435 = i.uv_texcoord;
			float temp_output_2_0_g435 = ( 1.0 > 0.0 ? ( ( break11_g435.x * -1.0 ) + 1.0 ) : break11_g435.x );
			float temp_output_171_0_g421 = step( temp_output_179_0_g421 , temp_output_2_0_g435 );
			float2 break11_g434 = i.uv_texcoord;
			float temp_output_2_0_g434 = ( 0.0 > 0.0 ? ( ( break11_g434.x * -1.0 ) + 1.0 ) : break11_g434.x );
			float temp_output_173_0_g421 = step( temp_output_179_0_g421 , temp_output_2_0_g434 );
			float temp_output_215_0_g421 = ( temp_output_221_0_g421 * ( 1.0 - ( temp_output_181_0_g421 % 1.0 ) ) );
			float temp_output_176_0_g421 = ( temp_output_180_0_g421 / segment_count527_g421 );
			float temp_output_175_0_g421 = ( step( temp_output_176_0_g421 , temp_output_2_0_g435 ) - temp_output_171_0_g421 );
			float temp_output_174_0_g421 = ( step( temp_output_176_0_g421 , temp_output_2_0_g434 ) - temp_output_173_0_g421 );
			float temp_output_192_0_g421 = min( temp_output_175_0_g421 , temp_output_174_0_g421 );
			float2 appendResult196_g421 = (float2(( ( ( -temp_output_221_0_g421 * temp_output_171_0_g421 ) + ( temp_output_221_0_g421 * temp_output_173_0_g421 ) ) + ( ( -temp_output_215_0_g421 * ( temp_output_175_0_g421 - temp_output_192_0_g421 ) ) + ( temp_output_215_0_g421 * ( temp_output_174_0_g421 - temp_output_192_0_g421 ) ) ) ) , 0.0));
			float temp_output_151_0_g421 = ( OSX553_g421 / segment_count527_g421 );
			float temp_output_159_0_g421 = min( segment_count527_g421 , Value574_g421 );
			float temp_output_135_0_g421 = ( ( ( ( BorderWidth529_g421 * segment_count527_g421 ) * 2.0 ) / OSX553_g421 ) + Segment_Spacing533_g421 );
			float temp_output_160_0_g421 = floor( temp_output_159_0_g421 );
			float temp_output_154_0_g421 = step( ( ( temp_output_160_0_g421 + 1.0 ) / segment_count527_g421 ) , i.uv_texcoord.x );
			float2 appendResult149_g421 = (float2(max( ( ( temp_output_151_0_g421 - ( temp_output_151_0_g421 * (temp_output_135_0_g421 + (( temp_output_159_0_g421 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g421) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g421 / segment_count527_g421 ) , i.uv_texcoord.x ) - temp_output_154_0_g421 ) ) , ( ( temp_output_151_0_g421 - ( temp_output_135_0_g421 * temp_output_151_0_g421 ) ) * temp_output_154_0_g421 ) ) , 0.0));
			float2 temp_output_128_0_g421 = ( temp_output_234_0_g421 > 0.0 ? appendResult196_g421 : appendResult149_g421 );
			float2 temp_output_2_0_g485 = OSXY554_g421;
			float2 break22_g485 = -( temp_output_2_0_g485 / float2( 2,2 ) );
			float2 appendResult29_g485 = (float2(( 0.0 > 0.0 ? break22_g485.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g485.y : 0.0 )));
			float2 temp_output_2_0_g486 = ( ( PixelationUV559_g421 * temp_output_2_0_g485 ) + appendResult29_g485 );
			float temp_output_701_0_g421 = ( OSX553_g421 / segment_count527_g421 );
			float2 appendResult705_g421 = (float2(temp_output_701_0_g421 , OSY552_g421));
			float2 temp_output_11_0_g448 = appendResult705_g421;
			float2 temp_output_12_0_g448 = ( temp_output_2_0_g486 % temp_output_11_0_g448 );
			float2 break13_g448 = ( temp_output_12_0_g448 - ( temp_output_11_0_g448 / float2( 2,2 ) ) );
			float2 break14_g448 = temp_output_12_0_g448;
			float2 appendResult1_g448 = (float2(( 1.0 > 0.0 ? break13_g448.x : break14_g448.x ) , ( 1.0 > 0.0 ? break13_g448.y : break14_g448.y )));
			float2 SegmentUV521_g421 = appendResult1_g448;
			float2 temp_output_20_0_g433 = ( ( temp_output_128_0_g421 + SegmentUV521_g421 ) + ( OSXY554_g421 * _ValueMaskOffset ) );
			float2 break23_g433 = temp_output_20_0_g433;
			float BorderRadius548_g421 = _BorderRadius;
			float InnerRoundingPercent720_g421 = _InnerRoundingPercent;
			float temp_output_718_0_g421 = ( ( width_curve532_g421 * BorderRadius548_g421 ) * InnerRoundingPercent720_g421 );
			float temp_output_9_0_g436 = Width537_g421;
			float temp_output_118_0_g421 = ( ( saturate( ( 1.0 - Arc539_g421 ) ) * ( ( ( i.uv_texcoord.y * temp_output_9_0_g436 ) + ( Radius536_g421 - ( temp_output_9_0_g436 / 2.0 ) ) ) * ( 6.28318548202515 * 1.0 ) ) ) / Radius536_g421 );
			#if defined(SHAPE_LINEAR)
				float staticSwitch249_g421 = temp_output_718_0_g421;
			#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g421 = ( temp_output_118_0_g421 * temp_output_718_0_g421 );
			#else
				float staticSwitch249_g421 = temp_output_718_0_g421;
			#endif
			float Rounding13_g433 = staticSwitch249_g421;
			float4 BorderRadiusOffset547_g421 = _BorderRadiusOffset;
			float4 temp_output_717_0_g421 = ( ( width_curve532_g421 * BorderRadiusOffset547_g421 ) * InnerRoundingPercent720_g421 );
			#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g421 = temp_output_717_0_g421;
			#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g421 = ( temp_output_118_0_g421 * temp_output_717_0_g421 );
			#else
				float4 staticSwitch246_g421 = temp_output_717_0_g421;
			#endif
			float4 break27_g433 = ( Rounding13_g433 + staticSwitch246_g421 );
			float2 appendResult25_g433 = (float2(break27_g433.x , break27_g433.w));
			float2 appendResult26_g433 = (float2(break27_g433.y , break27_g433.z));
			float2 break32_g433 = ( break23_g433.x > 0.0 ? appendResult25_g433 : appendResult26_g433 );
			float temp_output_31_0_g433 = ( break23_g433.y > 0.0 ? break32_g433.x : break32_g433.y );
			float2 appendResult520_g421 = (float2(temp_output_701_0_g421 , ( OSY552_g421 * width_curve532_g421 )));
			float2 appendResult512_g421 = (float2(( 0.5 - ( Segment_Spacing533_g421 / 2.0 ) ) , 0.5));
			float2 SegmentSize619_g421 = ( ( appendResult520_g421 * appendResult512_g421 ) + float2( 0,-0.01 ) );
			float temp_output_211_0_g421 = ( segment_count527_g421 * 2.0 );
			float2 appendResult710_g421 = (float2(( temp_output_192_0_g421 * ( ( 1.0 - temp_output_188_0_g421 ) * ( ( ( OSX553_g421 / temp_output_211_0_g421 ) - BorderWidth529_g421 ) - ( ( OSX553_g421 * Segment_Spacing533_g421 ) / temp_output_211_0_g421 ) ) ) ) , 0.0));
			float2 temp_output_10_0_g433 = ( ( float2( 1,1 ) * temp_output_31_0_g433 ) + ( abs( temp_output_20_0_g433 ) - ( SegmentSize619_g421 - ( temp_output_234_0_g421 > 0.0 ? appendResult710_g421 : float2( 0,0 ) ) ) ) );
			float2 break8_g433 = temp_output_10_0_g433;
			float2 temp_output_20_0_g432 = SegmentUV521_g421;
			float2 break23_g432 = temp_output_20_0_g432;
			float AdjustBorderRadiusToWidthCurve557_g421 = _AdjustBorderRadiusToWidthCurve;
			float temp_output_9_0_g446 = Width537_g421;
			float temp_output_507_0_g421 = ( ( saturate( ( 1.0 - Arc539_g421 ) ) * ( ( ( i.uv_texcoord.y * temp_output_9_0_g446 ) + ( Radius536_g421 - ( temp_output_9_0_g446 / 2.0 ) ) ) * ( 6.28318548202515 * 1.0 ) ) ) / Radius536_g421 );
			#if defined(SHAPE_LINEAR)
				float staticSwitch523_g421 = BorderRadius548_g421;
			#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g421 = ( BorderRadius548_g421 * temp_output_507_0_g421 );
			#else
				float staticSwitch523_g421 = BorderRadius548_g421;
			#endif
			float SegmentRounding518_g421 = ( AdjustBorderRadiusToWidthCurve557_g421 > 0.0 ? ( staticSwitch523_g421 * width_curve532_g421 ) : staticSwitch523_g421 );
			float Rounding13_g432 = ( SegmentRounding518_g421 * 1.0 );
			#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g421 = BorderRadiusOffset547_g421;
			#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g421 = ( BorderRadiusOffset547_g421 * temp_output_507_0_g421 );
			#else
				float4 staticSwitch723_g421 = BorderRadiusOffset547_g421;
			#endif
			float4 SegmentRoundingOffset519_g421 = ( AdjustBorderRadiusToWidthCurve557_g421 > 0.0 ? ( width_curve532_g421 * staticSwitch723_g421 ) : staticSwitch723_g421 );
			float4 break27_g432 = ( Rounding13_g432 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g421 ) );
			float2 appendResult25_g432 = (float2(break27_g432.x , break27_g432.w));
			float2 appendResult26_g432 = (float2(break27_g432.y , break27_g432.z));
			float2 break32_g432 = ( break23_g432.x > 0.0 ? appendResult25_g432 : appendResult26_g432 );
			float temp_output_31_0_g432 = ( break23_g432.y > 0.0 ? break32_g432.x : break32_g432.y );
			float2 temp_output_10_0_g432 = ( ( float2( 1,1 ) * temp_output_31_0_g432 ) + ( abs( temp_output_20_0_g432 ) - SegmentSize619_g421 ) );
			float2 break8_g432 = temp_output_10_0_g432;
			float temp_output_89_0_g421 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g433 ) ) + min( max( break8_g433.x , break8_g433.y ) , 0.0 ) ) - temp_output_31_0_g433 ) + BorderWidth529_g421 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g432 ) ) + min( max( break8_g432.x , break8_g432.y ) , 0.0 ) ) - temp_output_31_0_g432 ) + BorderWidth529_g421 ) ) - InnerBorderWidth250_g421 );
			float temp_output_3_0_g426 = ( 0.0 + 0.0 + temp_output_89_0_g421 );
			float InnerValue240_g421 = ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g426 / fwidth( temp_output_89_0_g421 ) ) ) : step( 0.0 , temp_output_3_0_g426 ) );
			float4 lerpResult674_g421 = lerp( appendResult675_g421 , ValueColorProcessed398_g421 , max( ( 1.0 - break679_g421.w ) , InnerValue240_g421 ));
			float temp_output_15_0_g465 = _ValueInsetShadowSize;
			float temp_output_4_0_g465 = saturate( ceil( temp_output_15_0_g465 ) );
			float4 break4_g467 = _ValueInsetShadowColor;
			float4 appendResult17_g467 = (float4(break4_g467.r , break4_g467.g , break4_g467.b , 1.0));
			float temp_output_86_0_g421 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g433 ) ) + min( max( break8_g433.x , break8_g433.y ) , 0.0 ) ) - temp_output_31_0_g433 ) + BorderWidth529_g421 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g432 ) ) + min( max( break8_g432.x , break8_g432.y ) , 0.0 ) ) - temp_output_31_0_g432 ) + BorderWidth529_g421 ) ) + 0.0 + 0.0 );
			float temp_output_3_0_g427 = temp_output_86_0_g421;
			float ValueView242_g421 = ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g427 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g433 ) ) + min( max( break8_g433.x , break8_g433.y ) , 0.0 ) ) - temp_output_31_0_g433 ) + BorderWidth529_g421 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g432 ) ) + min( max( break8_g432.x , break8_g432.y ) , 0.0 ) ) - temp_output_31_0_g432 ) + BorderWidth529_g421 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g427 ) );
			float ValueSDF241_g421 = temp_output_86_0_g421;
			float temp_output_2_0_g466 = ValueSDF241_g421;
			float4 lerpResult673_g421 = lerp( ( InnerBorderWidth250_g421 > 0.0 ? lerpResult674_g421 : ValueColorProcessed398_g421 ) , ( ( saturate( temp_output_4_0_g465 ) * ( 1.0 > 0.0 ? break4_g467.a : 1.0 ) ) * appendResult17_g467 ) , ( temp_output_4_0_g465 * min( ValueView242_g421 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g466 : temp_output_2_0_g466 ) / max( temp_output_15_0_g465 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
			float4 zzLerp_Value685_g421 = lerpResult673_g421;
			float4 lerpResult657_g421 = lerp( BorderColorProcessed497_g421 , zzLerp_Value685_g421 , ValueView242_g421);
			float temp_output_15_0_g480 = _BorderInsetShadowSize;
			float temp_output_4_0_g480 = saturate( ceil( temp_output_15_0_g480 ) );
			float4 break4_g482 = _BorderInsetShadowColor;
			float4 appendResult17_g482 = (float4(break4_g482.r , break4_g482.g , break4_g482.b , 1.0));
			float2 temp_output_20_0_g447 = SegmentUV521_g421;
			float2 break23_g447 = temp_output_20_0_g447;
			float Rounding13_g447 = SegmentRounding518_g421;
			float4 break27_g447 = ( Rounding13_g447 + SegmentRoundingOffset519_g421 );
			float2 appendResult25_g447 = (float2(break27_g447.x , break27_g447.w));
			float2 appendResult26_g447 = (float2(break27_g447.y , break27_g447.z));
			float2 break32_g447 = ( break23_g447.x > 0.0 ? appendResult25_g447 : appendResult26_g447 );
			float temp_output_31_0_g447 = ( break23_g447.y > 0.0 ? break32_g447.x : break32_g447.y );
			float2 temp_output_10_0_g447 = ( ( float2( 1,1 ) * temp_output_31_0_g447 ) + ( abs( temp_output_20_0_g447 ) - SegmentSize619_g421 ) );
			float2 break8_g447 = temp_output_10_0_g447;
			float temp_output_615_0_g421 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g447 ) ) + min( max( break8_g447.x , break8_g447.y ) , 0.0 ) ) - temp_output_31_0_g447 );
			float PB_SDF_Negated618_g421 = -temp_output_615_0_g421;
			float temp_output_654_0_g421 = ( PB_SDF_Negated618_g421 - BorderWidth529_g421 );
			float temp_output_3_0_g463 = temp_output_654_0_g421;
			float temp_output_2_0_g481 = temp_output_654_0_g421;
			float4 lerpResult645_g421 = lerp( lerpResult657_g421 , ( ( saturate( temp_output_4_0_g480 ) * ( 1.0 > 0.0 ? break4_g482.a : 1.0 ) ) * appendResult17_g482 ) , ( temp_output_4_0_g480 * min( ( 1.0 - ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g463 / fwidth( temp_output_654_0_g421 ) ) ) : step( 0.0 , temp_output_3_0_g463 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g481 : temp_output_2_0_g481 ) / max( temp_output_15_0_g480 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
			float4 zzLerp_Border666_g421 = lerpResult645_g421;
			float4 break4_g454 = _BackgroundColor;
			float4 appendResult17_g454 = (float4(break4_g454.r , break4_g454.g , break4_g454.b , 1.0));
			float4 temp_output_743_0_g421 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g454.a : 1.0 ) ) * appendResult17_g454 );
			float2 temp_cast_5 = (saturate( ( Value574_g421 / segment_count527_g421 ) )).xx;
			float cos478_g421 = cos( radians( _BackgroundGradientRotation ) );
			float sin478_g421 = sin( radians( _BackgroundGradientRotation ) );
			float2 rotator478_g421 = mul( GradientUV479_g421 - float2( 0.5,0.5 ) , float2x2( cos478_g421 , -sin478_g421 , sin478_g421 , cos478_g421 )) + float2( 0.5,0.5 );
			float4 break4_g453 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g421 ) );
			float4 appendResult17_g453 = (float4(break4_g453.r , break4_g453.g , break4_g453.b , 1.0));
			float4 temp_output_403_0_g421 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g453.a : 1.0 ) ) * appendResult17_g453 ) * temp_output_743_0_g421 ) : temp_output_743_0_g421 );
			float BG_Tex_Scale_w_Segments414_g421 = _BackgroundTextureScaleWithSegments;
			float2 BG_Tex_Tiling417_g421 = _BackgroundTextureTiling;
			float temp_output_453_0_g421 = ( BG_Tex_Scale_w_Segments414_g421 > 0.0 ? 1.0 : segment_count527_g421 );
			float temp_output_462_0_g421 = ( ( ( BG_Tex_Tiling417_g421.x * OSX553_g421 ) * temp_output_453_0_g421 ) / ( ( ( temp_output_453_0_g421 * OSX553_g421 ) + temp_output_444_0_g421 ) - temp_output_449_0_g421 ) );
			float temp_output_429_0_g421 = ( BG_Tex_Tiling417_g421.y / ( width_curve532_g421 - ( BorderWidth529_g421 * ( 2.0 / OSY552_g421 ) ) ) );
			float2 appendResult483_g421 = (float2(temp_output_462_0_g421 , temp_output_429_0_g421));
			float2 appendResult486_g421 = (float2(( -( ( temp_output_462_0_g421 - BG_Tex_Tiling417_g421.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g421 / 2.0 ) + 0.5 ) )));
			float2 temp_output_2_0_g444 = ( ( ( BG_Tex_Scale_w_Segments414_g421 > 0.0 ? ScaledTextureUV349_g421 : UnscaledTextureUV350_g421 ) * appendResult483_g421 ) + appendResult486_g421 );
			float cos472_g421 = cos( radians( _BackgroundTextureRotation ) );
			float sin472_g421 = sin( radians( _BackgroundTextureRotation ) );
			float2 rotator472_g421 = mul( temp_output_2_0_g444 - float2( 0.5,0.5 ) , float2x2( cos472_g421 , -sin472_g421 , sin472_g421 , cos472_g421 )) + float2( 0.5,0.5 );
			float2 break468_g421 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
			float fbtotaltiles13_g443 = break468_g421.x * break468_g421.y;
			float fbcolsoffset13_g443 = 1.0f / break468_g421.x;
			float fbrowsoffset13_g443 = 1.0f / break468_g421.y;
			float fbspeed13_g443 = _Time.y * fps541_g421;
			float2 fbtiling13_g443 = float2(fbcolsoffset13_g443, fbrowsoffset13_g443);
			float fbcurrenttileindex13_g443 = round( fmod( fbspeed13_g443 + 0.0, fbtotaltiles13_g443) );
			fbcurrenttileindex13_g443 += ( fbcurrenttileindex13_g443 < 0) ? fbtotaltiles13_g443 : 0;
			float fblinearindextox13_g443 = round ( fmod ( fbcurrenttileindex13_g443, break468_g421.x ) );
			float fboffsetx13_g443 = fblinearindextox13_g443 * fbcolsoffset13_g443;
			float fblinearindextoy13_g443 = round( fmod( ( fbcurrenttileindex13_g443 - fblinearindextox13_g443 ) / break468_g421.x, break468_g421.y ) );
			fblinearindextoy13_g443 = (int)(break468_g421.y-1) - fblinearindextoy13_g443;
			float fboffsety13_g443 = fblinearindextoy13_g443 * fbrowsoffset13_g443;
			float2 fboffset13_g443 = float2(fboffsetx13_g443, fboffsety13_g443);
			half2 fbuv13_g443 = rotator472_g421 * fbtiling13_g443 + fboffset13_g443;
			float4 break4_g455 = tex2D( _BackgroundTexture, fbuv13_g443 );
			float4 appendResult17_g455 = (float4(break4_g455.r , break4_g455.g , break4_g455.b , 1.0));
			float4 lerpResult400_g421 = lerp( temp_output_403_0_g421 , ( temp_output_403_0_g421 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g455.a : 1.0 ) ) * appendResult17_g455 ) ) , saturate( _BackgroundTextureOpacity ));
			#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g421 = lerpResult400_g421;
			#else
				float4 staticSwitch494_g421 = temp_output_743_0_g421;
			#endif
			float4 BackgroundColorProcessed495_g421 = staticSwitch494_g421;
			float temp_output_639_0_g421 = ( PB_SDF_Negated618_g421 - BorderWidth529_g421 );
			float temp_output_3_0_g464 = temp_output_639_0_g421;
			float temp_output_638_0_g421 = ( ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g464 / fwidth( temp_output_639_0_g421 ) ) ) : step( 0.0 , temp_output_3_0_g464 ) ) - ValueView242_g421 );
			float4 lerpResult636_g421 = lerp( zzLerp_Border666_g421 , BackgroundColorProcessed495_g421 , temp_output_638_0_g421);
			float temp_output_15_0_g475 = _ValueShadowSize;
			float temp_output_4_0_g475 = saturate( ceil( temp_output_15_0_g475 ) );
			float4 break4_g477 = _ValueShadowColor;
			float4 appendResult17_g477 = (float4(break4_g477.r , break4_g477.g , break4_g477.b , 1.0));
			float temp_output_2_0_g476 = ValueSDF241_g421;
			float4 lerpResult634_g421 = lerp( lerpResult636_g421 , ( ( saturate( temp_output_4_0_g475 ) * ( 1.0 > 0.0 ? break4_g477.a : 1.0 ) ) * appendResult17_g477 ) , ( temp_output_4_0_g475 * min( temp_output_638_0_g421 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g476 : temp_output_2_0_g476 ) / max( temp_output_15_0_g475 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
			float4 zzLerp_Background642_g421 = lerpResult634_g421;
			float temp_output_15_0_g470 = _BorderShadowSize;
			float temp_output_4_0_g470 = saturate( ceil( temp_output_15_0_g470 ) );
			float4 break4_g472 = _BorderShadowColor;
			float4 appendResult17_g472 = (float4(break4_g472.r , break4_g472.g , break4_g472.b , 1.0));
			float temp_output_625_0_g421 = ( PB_SDF_Negated618_g421 - BorderWidth529_g421 );
			float temp_output_3_0_g462 = temp_output_625_0_g421;
			float temp_output_2_0_g471 = temp_output_625_0_g421;
			float4 lerpResult620_g421 = lerp( zzLerp_Background642_g421 , ( ( saturate( temp_output_4_0_g470 ) * ( 1.0 > 0.0 ? break4_g472.a : 1.0 ) ) * appendResult17_g472 ) , ( temp_output_4_0_g470 * min( ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g462 / fwidth( temp_output_625_0_g421 ) ) ) : step( 0.0 , temp_output_3_0_g462 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g471 : temp_output_2_0_g471 ) / max( temp_output_15_0_g470 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
			float4 zzLerp_Border_Shadow629_g421 = lerpResult620_g421;
			float4 temp_output_608_0_g421 = ( OverlayColorProcessed524_g421 * zzLerp_Border_Shadow629_g421 );
			float4 break726_g421 = temp_output_608_0_g421;
			float3 appendResult727_g421 = (float3(break726_g421.r , break726_g421.g , break726_g421.b));
			o.Emission = appendResult727_g421;
			float PB_SDF616_g421 = temp_output_615_0_g421;
			float temp_output_3_0_g460 = PB_SDF616_g421;
			float temp_output_534_0_g421 = min( temp_output_608_0_g421.a , ( 1.0 - ( AA530_g421 > 0.0 ? saturate( ( temp_output_3_0_g460 / fwidth( PB_SDF616_g421 ) ) ) : step( 0.0 , temp_output_3_0_g460 ) ) ) );
			o.Alpha = temp_output_534_0_g421;
		}

		ENDCG
	}
	CustomEditor "Renge.PPB.ProceduralProgressBarGUI"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.FunctionNode;35;-3248.34,202.6756;Inherit;True;The Whole Shebang;0;;421;2d6870fee17216f4db3628575a74016f;0;0;3;FLOAT3;0;FLOAT;728;FLOAT3;729
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;14;-2900.045,72.98727;Float;False;True;-1;4;Renge.PPB.ProceduralProgressBarGUI;0;0;Unlit;Renge/PPB_BuiltIn;False;False;False;False;True;True;True;True;True;False;True;True;False;True;False;False;False;False;False;False;False;Off;2;False;;3;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;3;1;False;;10;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;90;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;2;35;0
WireConnection;14;9;35;728
WireConnection;14;11;35;729
ASEEND*/
//CHKSM=BEB84552B57AAEB7C8F32826D95C77BDB6948484