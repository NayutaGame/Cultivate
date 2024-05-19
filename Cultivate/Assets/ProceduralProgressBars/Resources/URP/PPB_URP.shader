// Made with Amplify Shader Editor v1.9.3.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Renge/PPB_URP"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
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


		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

		[HideInInspector][ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "UniversalMaterialType"="Unlit" }

		Cull Back
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 4.5
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForwardOnly" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3

			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif

				o.positionCS = positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord3.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord3.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord3.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord3.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord3.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord3.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord3.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord3.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float4 break726_g1 = temp_output_608_0_g1;
				float3 appendResult727_g1 = (float3(break726_g1.r , break726_g1.g , break726_g1.b));
				
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = appendResult727_g1;
				float Alpha = temp_output_534_0_g1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.positionCS, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off
			ColorMask 0

			HLSLPROGRAM

			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_FOG 1
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ASE_SRP_VERSION 120113


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile _ DOTS_INSTANCING_ON

			#define SHADERPASS SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.normalOS );

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					float3 lightDirectionWS = _LightDirection;
				#endif

				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord2.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord2.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord2.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord2.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord2.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord2.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord2.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord2.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				float Alpha = temp_output_534_0_g1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #define ASE_FOG 1
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ASE_SRP_VERSION 120113


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				o.positionCS = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord2.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord2.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord2.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord2.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord2.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord2.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord2.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord2.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord2.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				float Alpha = temp_output_534_0_g1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass // -- DEPRECATED --
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define ASE_FOG 1
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ASE_SRP_VERSION 120113


			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ DEBUG_DISPLAY

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;
			CBUFFER_START( UnityPerMaterial )
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			CBUFFER_END


			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );

				#endif

				o.positionCS = positionCS;

				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord3.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord3.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord3.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord3.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord3.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord3.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord3.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord3.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord3.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float4 break726_g1 = temp_output_608_0_g1;
				float3 appendResult727_g1 = (float3(break726_g1.r , break726_g1.g , break726_g1.b));
				
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = appendResult727_g1;
				float Alpha = temp_output_534_0_g1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.positionCS, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off
			AlphaToMask Off

			HLSLPROGRAM

            #define ASE_FOG 1
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ASE_SRP_VERSION 120113


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				surfaceDescription.Alpha = temp_output_534_0_g1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "ScenePickingPass"
			Tags { "LightMode"="Picking" }

			AlphaToMask Off

			HLSLPROGRAM

            #define ASE_FOG 1
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ASE_SRP_VERSION 120113


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT

			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			float4 _SelectionID;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				o.positionCS = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				surfaceDescription.Alpha = temp_output_534_0_g1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthNormals"
			Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On

			HLSLPROGRAM

			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_FOG 1
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ASE_SRP_VERSION 120113


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_NORMAL_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BorderRadiusOffset;
			float4 _VariableWidthCurve_ST;
			float4 _BackgroundColor;
			float4 _BorderInsetShadowColor;
			float4 _InnerBorderColor;
			float4 _PulseColor;
			float4 _ValueInsetShadowColor;
			float4 _BorderColor;
			float4 _InnerColor;
			float4 _ValueShadowColor;
			float4 _OverlayColor;
			float4 _BorderShadowColor;
			float2 _BorderTextureTiling;
			float2 _ValueMaskOffset;
			float2 _InnerTextureTiling;
			float2 _BorderFlipbookDim;
			float2 _InnerTextureOffset;
			float2 _BorderTextureOffset;
			float2 _InnerFlipbookDim;
			float2 _BackgroundTextureOffset;
			float2 _BackgroundTextureTiling;
			float2 _OverlayFlipbookDim;
			float2 _OverlayTextureOffset;
			float2 _OverlayTextureTiling;
			float2 _CustomScale;
			float2 _BackgroundFlipbookDim;
			float _AntiAlias;
			float _BorderShadowSize;
			float _ValueShadowFalloff;
			float _ValueShadowSize;
			float _BackgroundTextureOpacity;
			float _BorderRadius;
			float _InnerRoundingPercent;
			float _BackgroundTextureRotation;
			float _BackgroundGradientEnabled;
			float _ValueInsetShadowSize;
			float _ValueAsGradientTimeBackground;
			float _ValueInsetShadowFalloff;
			float _BorderInsetShadowSize;
			float _BorderInsetShadowFalloff;
			float _InnerTextureOpacity;
			float _BackgroundTextureScaleWithSegments;
			float _BackgroundGradientRotation;
			float _AdjustBorderRadiusToWidthCurve;
			float _Slant;
			float _InnerGradientRotation;
			float _CenterFill;
			float _Pixelate;
			float _PixelCount;
			float _RatioScaling;
			float _UIScaling;
			float _Arc;
			float _Width;
			float _Radius;
			float _CircleLength;
			float _FlipbookFPS;
			float _OverlayTextureOpacity;
			float _BorderWidth;
			float _BorderTextureScaleWithSegments;
			float _InnerTextureRotation;
			float _SegmentCount;
			float _BorderTextureOpacity;
			float _InnerBorderWidth;
			float _PulsateWhenLow;
			float _PulseSpeed;
			float _Value;
			float _PulseActivationThreshold;
			float _PulseRamp;
			float _InnerGradientEnabled;
			float _ValueAsGradientTimeInner;
			float _SegmentSpacing;
			float _InnerTextureScaleWithSegments;
			float _OffsetTextureWithValue;
			float _BorderTextureRotation;
			float _BorderShadowFalloff;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _OverlayTexture;
			sampler2D _BorderTexture;
			sampler2D _VariableWidthCurve;
			sampler2D _InnerGradient;
			sampler2D _InnerTexture;
			sampler2D _BackgroundGradient;
			sampler2D _BackgroundTexture;


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 appendResult582_g1 = (float3(( ( ( v.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g1 = appendResult582_g1;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g1 = float3(0,0,0);
				#else
				float3 staticSwitch581_g1 = appendResult582_g1;
				#endif
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = staticSwitch581_g1;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.normalOS);

				o.positionCS = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float Pixelate531_g1 = _Pixelate;
				float PixelCount545_g1 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g1 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g1 = 1.0;
				#else
				float staticSwitch687_g1 = 0.0;
				#endif
				float temp_output_588_0_g1 = ( staticSwitch687_g1 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g1 = _Arc;
				float Width537_g1 = _Width;
				float temp_output_9_0_g349 = Width537_g1;
				float Radius536_g1 = _Radius;
				float2 appendResult587_g1 = (float2(( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord1.xy.y * temp_output_9_0_g349 ) + ( Radius536_g1 - ( temp_output_9_0_g349 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g1));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g389 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g389 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g389 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g389 = (float3(length( appendResult28_g389 ) , length( appendResult29_g389 ) , length( appendResult30_g389 )));
				float3 temp_output_38_0_g389 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g389 );
				float3 temp_output_16_0_g389 = ( ( ( temp_output_588_0_g1 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g1 > 0.0 ? appendResult587_g1 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g389 );
				float3 break9_g389 = temp_output_16_0_g389;
				float3 break48_g389 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g389 / min( break9_g389.x , break9_g389.y ) ) : temp_output_16_0_g389 );
				float2 appendResult10_g389 = (float2(break48_g389.x , break48_g389.y));
				float2 OSXY554_g1 = appendResult10_g389;
				float2 temp_output_6_0_g387 = ( PixelCount545_g1 * OSXY554_g1 );
				float2 PixelationUV559_g1 = ( Pixelate531_g1 > 0.0 ? ( floor( ( IN.ase_texcoord1.xy * temp_output_6_0_g387 ) ) / ( temp_output_6_0_g387 - float2( 1,1 ) ) ) : IN.ase_texcoord1.xy );
				float2 temp_output_2_0_g194 = ( ( PixelationUV559_g1 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g1 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g1 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g193 = break51_g1.x * break51_g1.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g193 = 1.0f / break51_g1.x;
				float fbrowsoffset13_g193 = 1.0f / break51_g1.y;
				// Speed of animation
				float fbspeed13_g193 = _TimeParameters.x * fps541_g1;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g193 = float2(fbcolsoffset13_g193, fbrowsoffset13_g193);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g193 = round( fmod( fbspeed13_g193 + 0.0, fbtotaltiles13_g193) );
				fbcurrenttileindex13_g193 += ( fbcurrenttileindex13_g193 < 0) ? fbtotaltiles13_g193 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g193 = round ( fmod ( fbcurrenttileindex13_g193, break51_g1.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g193 = fblinearindextox13_g193 * fbcolsoffset13_g193;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g193 = round( fmod( ( fbcurrenttileindex13_g193 - fblinearindextox13_g193 ) / break51_g1.x, break51_g1.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g193 = (int)(break51_g1.y-1) - fblinearindextoy13_g193;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g193 = fblinearindextoy13_g193 * fbrowsoffset13_g193;
				// UV Offset
				float2 fboffset13_g193 = float2(fboffsetx13_g193, fboffsety13_g193);
				// Flipbook UV
				half2 fbuv13_g193 = temp_output_2_0_g194 * fbtiling13_g193 + fboffset13_g193;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g1 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g193 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g1 = lerpResult45_g1;
				#else
				float4 staticSwitch44_g1 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g1 = staticSwitch44_g1;
				float BorderWidth529_g1 = _BorderWidth;
				float4 break4_g283 = _BorderColor;
				float4 appendResult17_g283 = (float4(break4_g283.r , break4_g283.g , break4_g283.b , 1.0));
				float4 temp_output_738_0_g1 = ( ( saturate( ceil( BorderWidth529_g1 ) ) * ( 1.0 > 0.0 ? break4_g283.a : 1.0 ) ) * appendResult17_g283 );
				float segment_count527_g1 = _SegmentCount;
				float2 appendResult345_g1 = (float2(segment_count527_g1 , 1.0));
				float2 temp_output_2_0_g212 = ( ( PixelationUV559_g1 * appendResult345_g1 ) + float2( 0,0 ) );
				float2 break10_g212 = temp_output_2_0_g212;
				float2 appendResult352_g1 = (float2(( break10_g212.x % 1.0 ) , break10_g212.y));
				float2 ScaledTextureUV349_g1 = appendResult352_g1;
				float2 temp_output_2_0_g211 = ( ( PixelationUV559_g1 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g1 = temp_output_2_0_g211;
				float2 break77_g1 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = IN.ase_texcoord1.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g1 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g1 = ( break77_g1.y / width_curve532_g1 );
				float2 appendResult74_g1 = (float2(break77_g1.x , temp_output_75_0_g1));
				float2 appendResult70_g1 = (float2(0.0 , ( -( temp_output_75_0_g1 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g195 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult74_g1 ) + ( _BorderTextureOffset + appendResult70_g1 ) );
				float cos63_g1 = cos( radians( _BorderTextureRotation ) );
				float sin63_g1 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g1 = mul( temp_output_2_0_g195 - float2( 0.5,0.5 ) , float2x2( cos63_g1 , -sin63_g1 , sin63_g1 , cos63_g1 )) + float2( 0.5,0.5 );
				float2 break39_g1 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g188 = break39_g1.x * break39_g1.y;
				float fbcolsoffset13_g188 = 1.0f / break39_g1.x;
				float fbrowsoffset13_g188 = 1.0f / break39_g1.y;
				float fbspeed13_g188 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g188 = float2(fbcolsoffset13_g188, fbrowsoffset13_g188);
				float fbcurrenttileindex13_g188 = round( fmod( fbspeed13_g188 + 0.0, fbtotaltiles13_g188) );
				fbcurrenttileindex13_g188 += ( fbcurrenttileindex13_g188 < 0) ? fbtotaltiles13_g188 : 0;
				float fblinearindextox13_g188 = round ( fmod ( fbcurrenttileindex13_g188, break39_g1.x ) );
				float fboffsetx13_g188 = fblinearindextox13_g188 * fbcolsoffset13_g188;
				float fblinearindextoy13_g188 = round( fmod( ( fbcurrenttileindex13_g188 - fblinearindextox13_g188 ) / break39_g1.x, break39_g1.y ) );
				fblinearindextoy13_g188 = (int)(break39_g1.y-1) - fblinearindextoy13_g188;
				float fboffsety13_g188 = fblinearindextoy13_g188 * fbrowsoffset13_g188;
				float2 fboffset13_g188 = float2(fboffsetx13_g188, fboffsety13_g188);
				half2 fbuv13_g188 = rotator63_g1 * fbtiling13_g188 + fboffset13_g188;
				float4 lerpResult35_g1 = lerp( temp_output_738_0_g1 , ( tex2D( _BorderTexture, fbuv13_g188 ) * temp_output_738_0_g1 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g1 = lerpResult35_g1;
				#else
				float4 staticSwitch496_g1 = temp_output_738_0_g1;
				#endif
				float4 BorderColorProcessed497_g1 = staticSwitch496_g1;
				float InnerBorderWidth250_g1 = _InnerBorderWidth;
				float4 break4_g290 = _InnerBorderColor;
				float4 appendResult17_g290 = (float4(break4_g290.r , break4_g290.g , break4_g290.b , 1.0));
				float4 temp_output_745_0_g1 = ( ( saturate( ceil( InnerBorderWidth250_g1 ) ) * ( 1.0 > 0.0 ? break4_g290.a : 1.0 ) ) * appendResult17_g290 );
				float4 break4_g331 = _PulseColor;
				float4 appendResult17_g331 = (float4(break4_g331.r , break4_g331.g , break4_g331.b , 1.0));
				float4 PulseColorProcessed384_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g331.a : 1.0 ) ) * appendResult17_g331 );
				float Value574_g1 = _Value;
				float temp_output_1_0_g210 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g1 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g1 / segment_count527_g1 ) - temp_output_1_0_g210 ) / ( _PulseActivationThreshold - temp_output_1_0_g210 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g1 = lerp( temp_output_745_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float2 temp_cast_4 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float OSX553_g1 = break48_g389.x;
				float temp_output_444_0_g1 = ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 );
				float Segment_Spacing533_g1 = _SegmentSpacing;
				float temp_output_449_0_g1 = ( Segment_Spacing533_g1 * OSX553_g1 );
				float temp_output_408_0_g1 = ( ( segment_count527_g1 * OSX553_g1 ) / ( ( temp_output_444_0_g1 + ( OSX553_g1 * segment_count527_g1 ) ) - temp_output_449_0_g1 ) );
				float2 appendResult422_g1 = (float2(temp_output_408_0_g1 , 1.0));
				float2 appendResult407_g1 = (float2(-( ( temp_output_408_0_g1 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g215 = ( ( PixelationUV559_g1 * appendResult422_g1 ) + appendResult407_g1 );
				float2 GradientUV479_g1 = temp_output_2_0_g215;
				float cos363_g1 = cos( radians( _InnerGradientRotation ) );
				float sin363_g1 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos363_g1 , -sin363_g1 , sin363_g1 , cos363_g1 )) + float2( 0.5,0.5 );
				float4 break4_g285 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g1 ) );
				float4 appendResult17_g285 = (float4(break4_g285.r , break4_g285.g , break4_g285.b , 1.0));
				float4 temp_output_740_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g285.a : 1.0 ) ) * appendResult17_g285 );
				float4 lerpResult390_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g1 * temp_output_740_0_g1 ) : temp_output_745_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g1 = lerpResult390_g1;
				#else
				float4 staticSwitch388_g1 = lerpResult389_g1;
				#endif
				float4 ValueBorderColorProcessed525_g1 = staticSwitch388_g1;
				float4 break679_g1 = ValueBorderColorProcessed525_g1;
				float4 appendResult675_g1 = (float4(break679_g1.x , break679_g1.y , break679_g1.z , 1.0));
				float4 break4_g291 = _InnerColor;
				float4 appendResult17_g291 = (float4(break4_g291.r , break4_g291.g , break4_g291.b , 1.0));
				float4 temp_output_746_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g291.a : 1.0 ) ) * appendResult17_g291 );
				float4 lerpResult369_g1 = lerp( temp_output_746_0_g1 , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float4 lerpResult367_g1 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g1 * temp_output_746_0_g1 ) : temp_output_746_0_g1 ) , PulseColorProcessed384_g1 , PulseAlpha382_g1);
				float Inner_Tex_Scale_w_Segments252_g1 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g1 = _InnerTextureTiling;
				float temp_output_330_0_g1 = ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_324_0_g1 = ( ( ( Inner_Tex_Tiling254_g1.x * OSX553_g1 ) * temp_output_330_0_g1 ) / ( ( ( temp_output_330_0_g1 * OSX553_g1 ) + ( ( BorderWidth529_g1 * segment_count527_g1 ) * -2.0 ) ) - ( OSX553_g1 * Segment_Spacing533_g1 ) ) );
				float OSY552_g1 = break48_g389.y;
				float temp_output_270_0_g1 = ( Inner_Tex_Tiling254_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult276_g1 = (float2(temp_output_324_0_g1 , temp_output_270_0_g1));
				float CenterFill562_g1 = _CenterFill;
				float2 temp_output_2_0_g208 = ( ( IN.ase_texcoord1.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g208 = temp_output_2_0_g208;
				float lerpResult321_g1 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( 1.0 - ( min( Value574_g1 , segment_count527_g1 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g1 / segment_count527_g1 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ( floor( Value574_g1 ) / segment_count527_g1 ) : 0.0 ) , break10_g208.x ));
				float2 appendResult277_g1 = (float2(( ( -( ( temp_output_324_0_g1 - Inner_Tex_Tiling254_g1.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g1.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g1 > 0.0 ? 0.0 : lerpResult321_g1 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g207 = ( ( ( Inner_Tex_Scale_w_Segments252_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult276_g1 ) + appendResult277_g1 );
				float cos299_g1 = cos( radians( _InnerTextureRotation ) );
				float sin299_g1 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g1 = mul( temp_output_2_0_g207 - float2( 0.5,0.5 ) , float2x2( cos299_g1 , -sin299_g1 , sin299_g1 , cos299_g1 )) + float2( 0.5,0.5 );
				float2 break275_g1 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g209 = break275_g1.x * break275_g1.y;
				float fbcolsoffset13_g209 = 1.0f / break275_g1.x;
				float fbrowsoffset13_g209 = 1.0f / break275_g1.y;
				float fbspeed13_g209 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g209 = float2(fbcolsoffset13_g209, fbrowsoffset13_g209);
				float fbcurrenttileindex13_g209 = round( fmod( fbspeed13_g209 + 0.0, fbtotaltiles13_g209) );
				fbcurrenttileindex13_g209 += ( fbcurrenttileindex13_g209 < 0) ? fbtotaltiles13_g209 : 0;
				float fblinearindextox13_g209 = round ( fmod ( fbcurrenttileindex13_g209, break275_g1.x ) );
				float fboffsetx13_g209 = fblinearindextox13_g209 * fbcolsoffset13_g209;
				float fblinearindextoy13_g209 = round( fmod( ( fbcurrenttileindex13_g209 - fblinearindextox13_g209 ) / break275_g1.x, break275_g1.y ) );
				fblinearindextoy13_g209 = (int)(break275_g1.y-1) - fblinearindextoy13_g209;
				float fboffsety13_g209 = fblinearindextoy13_g209 * fbrowsoffset13_g209;
				float2 fboffset13_g209 = float2(fboffsetx13_g209, fboffsety13_g209);
				half2 fbuv13_g209 = rotator299_g1 * fbtiling13_g209 + fboffset13_g209;
				float4 break4_g284 = tex2D( _InnerTexture, fbuv13_g209 );
				float4 appendResult17_g284 = (float4(break4_g284.r , break4_g284.g , break4_g284.b , 1.0));
				float4 lerpResult314_g1 = lerp( lerpResult367_g1 , ( lerpResult367_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g284.a : 1.0 ) ) * appendResult17_g284 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g1 = lerpResult314_g1;
				#else
				float4 staticSwitch686_g1 = lerpResult369_g1;
				#endif
				float4 ValueColorProcessed398_g1 = staticSwitch686_g1;
				float AA530_g1 = _AntiAlias;
				float temp_output_234_0_g1 = ( ( ( ( segment_count527_g1 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g1 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_221_0_g1 = ( temp_output_220_0_g1 - ( temp_output_220_0_g1 * ( ( ( ( segment_count527_g1 * BorderWidth529_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 ) ) );
				float temp_output_188_0_g1 = max( 0.0 , Value574_g1 );
				float temp_output_181_0_g1 = ( max( ( segment_count527_g1 - temp_output_188_0_g1 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g1 = floor( temp_output_181_0_g1 );
				float temp_output_179_0_g1 = ( ( temp_output_180_0_g1 + 1.0 ) / segment_count527_g1 );
				float2 break11_g205 = IN.ase_texcoord1.xy;
				float temp_output_2_0_g205 = ( 1.0 > 0.0 ? ( ( break11_g205.x * -1.0 ) + 1.0 ) : break11_g205.x );
				float temp_output_171_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g205 );
				float2 break11_g204 = IN.ase_texcoord1.xy;
				float temp_output_2_0_g204 = ( 0.0 > 0.0 ? ( ( break11_g204.x * -1.0 ) + 1.0 ) : break11_g204.x );
				float temp_output_173_0_g1 = step( temp_output_179_0_g1 , temp_output_2_0_g204 );
				float temp_output_215_0_g1 = ( temp_output_221_0_g1 * ( 1.0 - ( temp_output_181_0_g1 % 1.0 ) ) );
				float temp_output_176_0_g1 = ( temp_output_180_0_g1 / segment_count527_g1 );
				float temp_output_175_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g205 ) - temp_output_171_0_g1 );
				float temp_output_174_0_g1 = ( step( temp_output_176_0_g1 , temp_output_2_0_g204 ) - temp_output_173_0_g1 );
				float temp_output_192_0_g1 = min( temp_output_175_0_g1 , temp_output_174_0_g1 );
				float2 appendResult196_g1 = (float2(( ( ( -temp_output_221_0_g1 * temp_output_171_0_g1 ) + ( temp_output_221_0_g1 * temp_output_173_0_g1 ) ) + ( ( -temp_output_215_0_g1 * ( temp_output_175_0_g1 - temp_output_192_0_g1 ) ) + ( temp_output_215_0_g1 * ( temp_output_174_0_g1 - temp_output_192_0_g1 ) ) ) ) , 0.0));
				float temp_output_151_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float temp_output_159_0_g1 = min( segment_count527_g1 , Value574_g1 );
				float temp_output_135_0_g1 = ( ( ( ( BorderWidth529_g1 * segment_count527_g1 ) * 2.0 ) / OSX553_g1 ) + Segment_Spacing533_g1 );
				float temp_output_160_0_g1 = floor( temp_output_159_0_g1 );
				float temp_output_154_0_g1 = step( ( ( temp_output_160_0_g1 + 1.0 ) / segment_count527_g1 ) , IN.ase_texcoord1.xy.x );
				float2 appendResult149_g1 = (float2(max( ( ( temp_output_151_0_g1 - ( temp_output_151_0_g1 * (temp_output_135_0_g1 + (( temp_output_159_0_g1 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g1) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g1 / segment_count527_g1 ) , IN.ase_texcoord1.xy.x ) - temp_output_154_0_g1 ) ) , ( ( temp_output_151_0_g1 - ( temp_output_135_0_g1 * temp_output_151_0_g1 ) ) * temp_output_154_0_g1 ) ) , 0.0));
				float2 temp_output_128_0_g1 = ( temp_output_234_0_g1 > 0.0 ? appendResult196_g1 : appendResult149_g1 );
				float2 temp_output_2_0_g384 = OSXY554_g1;
				float2 break22_g384 = -( temp_output_2_0_g384 / float2( 2,2 ) );
				float2 appendResult29_g384 = (float2(( 0.0 > 0.0 ? break22_g384.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g384.y : 0.0 )));
				float2 temp_output_2_0_g385 = ( ( PixelationUV559_g1 * temp_output_2_0_g384 ) + appendResult29_g384 );
				float temp_output_701_0_g1 = ( OSX553_g1 / segment_count527_g1 );
				float2 appendResult705_g1 = (float2(temp_output_701_0_g1 , OSY552_g1));
				float2 temp_output_11_0_g267 = appendResult705_g1;
				float2 temp_output_12_0_g267 = ( temp_output_2_0_g385 % temp_output_11_0_g267 );
				float2 break13_g267 = ( temp_output_12_0_g267 - ( temp_output_11_0_g267 / float2( 2,2 ) ) );
				float2 break14_g267 = temp_output_12_0_g267;
				float2 appendResult1_g267 = (float2(( 1.0 > 0.0 ? break13_g267.x : break14_g267.x ) , ( 1.0 > 0.0 ? break13_g267.y : break14_g267.y )));
				float2 SegmentUV521_g1 = appendResult1_g267;
				float2 temp_output_20_0_g203 = ( ( temp_output_128_0_g1 + SegmentUV521_g1 ) + ( OSXY554_g1 * _ValueMaskOffset ) );
				float2 break23_g203 = temp_output_20_0_g203;
				float BorderRadius548_g1 = _BorderRadius;
				float InnerRoundingPercent720_g1 = _InnerRoundingPercent;
				float temp_output_718_0_g1 = ( ( width_curve532_g1 * BorderRadius548_g1 ) * InnerRoundingPercent720_g1 );
				float temp_output_9_0_g206 = Width537_g1;
				float temp_output_118_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord1.xy.y * temp_output_9_0_g206 ) + ( Radius536_g1 - ( temp_output_9_0_g206 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g1 = ( temp_output_118_0_g1 * temp_output_718_0_g1 );
				#else
				float staticSwitch249_g1 = temp_output_718_0_g1;
				#endif
				float Rounding13_g203 = staticSwitch249_g1;
				float4 BorderRadiusOffset547_g1 = _BorderRadiusOffset;
				float4 temp_output_717_0_g1 = ( ( width_curve532_g1 * BorderRadiusOffset547_g1 ) * InnerRoundingPercent720_g1 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g1 = ( temp_output_118_0_g1 * temp_output_717_0_g1 );
				#else
				float4 staticSwitch246_g1 = temp_output_717_0_g1;
				#endif
				float4 break27_g203 = ( Rounding13_g203 + staticSwitch246_g1 );
				float2 appendResult25_g203 = (float2(break27_g203.x , break27_g203.w));
				float2 appendResult26_g203 = (float2(break27_g203.y , break27_g203.z));
				float2 break32_g203 = ( break23_g203.x > 0.0 ? appendResult25_g203 : appendResult26_g203 );
				float temp_output_31_0_g203 = ( break23_g203.y > 0.0 ? break32_g203.x : break32_g203.y );
				float2 appendResult520_g1 = (float2(temp_output_701_0_g1 , ( OSY552_g1 * width_curve532_g1 )));
				float2 appendResult512_g1 = (float2(( 0.5 - ( Segment_Spacing533_g1 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g1 = ( ( appendResult520_g1 * appendResult512_g1 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g1 = ( segment_count527_g1 * 2.0 );
				float2 appendResult710_g1 = (float2(( temp_output_192_0_g1 * ( ( 1.0 - temp_output_188_0_g1 ) * ( ( ( OSX553_g1 / temp_output_211_0_g1 ) - BorderWidth529_g1 ) - ( ( OSX553_g1 * Segment_Spacing533_g1 ) / temp_output_211_0_g1 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g203 = ( ( float2( 1,1 ) * temp_output_31_0_g203 ) + ( abs( temp_output_20_0_g203 ) - ( SegmentSize619_g1 - ( temp_output_234_0_g1 > 0.0 ? appendResult710_g1 : float2( 0,0 ) ) ) ) );
				float2 break8_g203 = temp_output_10_0_g203;
				float2 temp_output_20_0_g202 = SegmentUV521_g1;
				float2 break23_g202 = temp_output_20_0_g202;
				float AdjustBorderRadiusToWidthCurve557_g1 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g221 = Width537_g1;
				float temp_output_507_0_g1 = ( ( saturate( ( 1.0 - Arc539_g1 ) ) * ( ( ( IN.ase_texcoord1.xy.y * temp_output_9_0_g221 ) + ( Radius536_g1 - ( temp_output_9_0_g221 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g1 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g1 = BorderRadius548_g1;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g1 = ( BorderRadius548_g1 * temp_output_507_0_g1 );
				#else
				float staticSwitch523_g1 = BorderRadius548_g1;
				#endif
				float SegmentRounding518_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( staticSwitch523_g1 * width_curve532_g1 ) : staticSwitch523_g1 );
				float Rounding13_g202 = ( SegmentRounding518_g1 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g1 = ( BorderRadiusOffset547_g1 * temp_output_507_0_g1 );
				#else
				float4 staticSwitch723_g1 = BorderRadiusOffset547_g1;
				#endif
				float4 SegmentRoundingOffset519_g1 = ( AdjustBorderRadiusToWidthCurve557_g1 > 0.0 ? ( width_curve532_g1 * staticSwitch723_g1 ) : staticSwitch723_g1 );
				float4 break27_g202 = ( Rounding13_g202 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g1 ) );
				float2 appendResult25_g202 = (float2(break27_g202.x , break27_g202.w));
				float2 appendResult26_g202 = (float2(break27_g202.y , break27_g202.z));
				float2 break32_g202 = ( break23_g202.x > 0.0 ? appendResult25_g202 : appendResult26_g202 );
				float temp_output_31_0_g202 = ( break23_g202.y > 0.0 ? break32_g202.x : break32_g202.y );
				float2 temp_output_10_0_g202 = ( ( float2( 1,1 ) * temp_output_31_0_g202 ) + ( abs( temp_output_20_0_g202 ) - SegmentSize619_g1 ) );
				float2 break8_g202 = temp_output_10_0_g202;
				float temp_output_89_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) - InnerBorderWidth250_g1 );
				float temp_output_3_0_g196 = ( 0.0 + 0.0 + temp_output_89_0_g1 );
				float InnerValue240_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g196 / fwidth( temp_output_89_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g196 ) );
				float4 lerpResult674_g1 = lerp( appendResult675_g1 , ValueColorProcessed398_g1 , max( ( 1.0 - break679_g1.w ) , InnerValue240_g1 ));
				float temp_output_15_0_g358 = _ValueInsetShadowSize;
				float temp_output_4_0_g358 = saturate( ceil( temp_output_15_0_g358 ) );
				float4 break4_g360 = _ValueInsetShadowColor;
				float4 appendResult17_g360 = (float4(break4_g360.r , break4_g360.g , break4_g360.b , 1.0));
				float temp_output_86_0_g1 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g197 = temp_output_86_0_g1;
				float ValueView242_g1 = ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g197 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g203 ) ) + min( max( break8_g203.x , break8_g203.y ) , 0.0 ) ) - temp_output_31_0_g203 ) + BorderWidth529_g1 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g202 ) ) + min( max( break8_g202.x , break8_g202.y ) , 0.0 ) ) - temp_output_31_0_g202 ) + BorderWidth529_g1 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g197 ) );
				float ValueSDF241_g1 = temp_output_86_0_g1;
				float temp_output_2_0_g359 = ValueSDF241_g1;
				float4 lerpResult673_g1 = lerp( ( InnerBorderWidth250_g1 > 0.0 ? lerpResult674_g1 : ValueColorProcessed398_g1 ) , ( ( saturate( temp_output_4_0_g358 ) * ( 1.0 > 0.0 ? break4_g360.a : 1.0 ) ) * appendResult17_g360 ) , ( temp_output_4_0_g358 * min( ValueView242_g1 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g359 : temp_output_2_0_g359 ) / max( temp_output_15_0_g358 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g1 = lerpResult673_g1;
				float4 lerpResult657_g1 = lerp( BorderColorProcessed497_g1 , zzLerp_Value685_g1 , ValueView242_g1);
				float temp_output_15_0_g373 = _BorderInsetShadowSize;
				float temp_output_4_0_g373 = saturate( ceil( temp_output_15_0_g373 ) );
				float4 break4_g375 = _BorderInsetShadowColor;
				float4 appendResult17_g375 = (float4(break4_g375.r , break4_g375.g , break4_g375.b , 1.0));
				float2 temp_output_20_0_g236 = SegmentUV521_g1;
				float2 break23_g236 = temp_output_20_0_g236;
				float Rounding13_g236 = SegmentRounding518_g1;
				float4 break27_g236 = ( Rounding13_g236 + SegmentRoundingOffset519_g1 );
				float2 appendResult25_g236 = (float2(break27_g236.x , break27_g236.w));
				float2 appendResult26_g236 = (float2(break27_g236.y , break27_g236.z));
				float2 break32_g236 = ( break23_g236.x > 0.0 ? appendResult25_g236 : appendResult26_g236 );
				float temp_output_31_0_g236 = ( break23_g236.y > 0.0 ? break32_g236.x : break32_g236.y );
				float2 temp_output_10_0_g236 = ( ( float2( 1,1 ) * temp_output_31_0_g236 ) + ( abs( temp_output_20_0_g236 ) - SegmentSize619_g1 ) );
				float2 break8_g236 = temp_output_10_0_g236;
				float temp_output_615_0_g1 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g236 ) ) + min( max( break8_g236.x , break8_g236.y ) , 0.0 ) ) - temp_output_31_0_g236 );
				float PB_SDF_Negated618_g1 = -temp_output_615_0_g1;
				float temp_output_654_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g356 = temp_output_654_0_g1;
				float temp_output_2_0_g374 = temp_output_654_0_g1;
				float4 lerpResult645_g1 = lerp( lerpResult657_g1 , ( ( saturate( temp_output_4_0_g373 ) * ( 1.0 > 0.0 ? break4_g375.a : 1.0 ) ) * appendResult17_g375 ) , ( temp_output_4_0_g373 * min( ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_654_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g374 : temp_output_2_0_g374 ) / max( temp_output_15_0_g373 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g1 = lerpResult645_g1;
				float4 break4_g288 = _BackgroundColor;
				float4 appendResult17_g288 = (float4(break4_g288.r , break4_g288.g , break4_g288.b , 1.0));
				float4 temp_output_743_0_g1 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g288.a : 1.0 ) ) * appendResult17_g288 );
				float2 temp_cast_5 = (saturate( ( Value574_g1 / segment_count527_g1 ) )).xx;
				float cos478_g1 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g1 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g1 = mul( GradientUV479_g1 - float2( 0.5,0.5 ) , float2x2( cos478_g1 , -sin478_g1 , sin478_g1 , cos478_g1 )) + float2( 0.5,0.5 );
				float4 break4_g287 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g1 ) );
				float4 appendResult17_g287 = (float4(break4_g287.r , break4_g287.g , break4_g287.b , 1.0));
				float4 temp_output_403_0_g1 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g287.a : 1.0 ) ) * appendResult17_g287 ) * temp_output_743_0_g1 ) : temp_output_743_0_g1 );
				float BG_Tex_Scale_w_Segments414_g1 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g1 = _BackgroundTextureTiling;
				float temp_output_453_0_g1 = ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? 1.0 : segment_count527_g1 );
				float temp_output_462_0_g1 = ( ( ( BG_Tex_Tiling417_g1.x * OSX553_g1 ) * temp_output_453_0_g1 ) / ( ( ( temp_output_453_0_g1 * OSX553_g1 ) + temp_output_444_0_g1 ) - temp_output_449_0_g1 ) );
				float temp_output_429_0_g1 = ( BG_Tex_Tiling417_g1.y / ( width_curve532_g1 - ( BorderWidth529_g1 * ( 2.0 / OSY552_g1 ) ) ) );
				float2 appendResult483_g1 = (float2(temp_output_462_0_g1 , temp_output_429_0_g1));
				float2 appendResult486_g1 = (float2(( -( ( temp_output_462_0_g1 - BG_Tex_Tiling417_g1.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g1 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g214 = ( ( ( BG_Tex_Scale_w_Segments414_g1 > 0.0 ? ScaledTextureUV349_g1 : UnscaledTextureUV350_g1 ) * appendResult483_g1 ) + appendResult486_g1 );
				float cos472_g1 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g1 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g1 = mul( temp_output_2_0_g214 - float2( 0.5,0.5 ) , float2x2( cos472_g1 , -sin472_g1 , sin472_g1 , cos472_g1 )) + float2( 0.5,0.5 );
				float2 break468_g1 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g213 = break468_g1.x * break468_g1.y;
				float fbcolsoffset13_g213 = 1.0f / break468_g1.x;
				float fbrowsoffset13_g213 = 1.0f / break468_g1.y;
				float fbspeed13_g213 = _TimeParameters.x * fps541_g1;
				float2 fbtiling13_g213 = float2(fbcolsoffset13_g213, fbrowsoffset13_g213);
				float fbcurrenttileindex13_g213 = round( fmod( fbspeed13_g213 + 0.0, fbtotaltiles13_g213) );
				fbcurrenttileindex13_g213 += ( fbcurrenttileindex13_g213 < 0) ? fbtotaltiles13_g213 : 0;
				float fblinearindextox13_g213 = round ( fmod ( fbcurrenttileindex13_g213, break468_g1.x ) );
				float fboffsetx13_g213 = fblinearindextox13_g213 * fbcolsoffset13_g213;
				float fblinearindextoy13_g213 = round( fmod( ( fbcurrenttileindex13_g213 - fblinearindextox13_g213 ) / break468_g1.x, break468_g1.y ) );
				fblinearindextoy13_g213 = (int)(break468_g1.y-1) - fblinearindextoy13_g213;
				float fboffsety13_g213 = fblinearindextoy13_g213 * fbrowsoffset13_g213;
				float2 fboffset13_g213 = float2(fboffsetx13_g213, fboffsety13_g213);
				half2 fbuv13_g213 = rotator472_g1 * fbtiling13_g213 + fboffset13_g213;
				float4 break4_g289 = tex2D( _BackgroundTexture, fbuv13_g213 );
				float4 appendResult17_g289 = (float4(break4_g289.r , break4_g289.g , break4_g289.b , 1.0));
				float4 lerpResult400_g1 = lerp( temp_output_403_0_g1 , ( temp_output_403_0_g1 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g289.a : 1.0 ) ) * appendResult17_g289 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g1 = lerpResult400_g1;
				#else
				float4 staticSwitch494_g1 = temp_output_743_0_g1;
				#endif
				float4 BackgroundColorProcessed495_g1 = staticSwitch494_g1;
				float temp_output_639_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g357 = temp_output_639_0_g1;
				float temp_output_638_0_g1 = ( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( temp_output_639_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g357 ) ) - ValueView242_g1 );
				float4 lerpResult636_g1 = lerp( zzLerp_Border666_g1 , BackgroundColorProcessed495_g1 , temp_output_638_0_g1);
				float temp_output_15_0_g368 = _ValueShadowSize;
				float temp_output_4_0_g368 = saturate( ceil( temp_output_15_0_g368 ) );
				float4 break4_g370 = _ValueShadowColor;
				float4 appendResult17_g370 = (float4(break4_g370.r , break4_g370.g , break4_g370.b , 1.0));
				float temp_output_2_0_g369 = ValueSDF241_g1;
				float4 lerpResult634_g1 = lerp( lerpResult636_g1 , ( ( saturate( temp_output_4_0_g368 ) * ( 1.0 > 0.0 ? break4_g370.a : 1.0 ) ) * appendResult17_g370 ) , ( temp_output_4_0_g368 * min( temp_output_638_0_g1 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g369 : temp_output_2_0_g369 ) / max( temp_output_15_0_g368 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g1 = lerpResult634_g1;
				float temp_output_15_0_g363 = _BorderShadowSize;
				float temp_output_4_0_g363 = saturate( ceil( temp_output_15_0_g363 ) );
				float4 break4_g365 = _BorderShadowColor;
				float4 appendResult17_g365 = (float4(break4_g365.r , break4_g365.g , break4_g365.b , 1.0));
				float temp_output_625_0_g1 = ( PB_SDF_Negated618_g1 - BorderWidth529_g1 );
				float temp_output_3_0_g355 = temp_output_625_0_g1;
				float temp_output_2_0_g364 = temp_output_625_0_g1;
				float4 lerpResult620_g1 = lerp( zzLerp_Background642_g1 , ( ( saturate( temp_output_4_0_g363 ) * ( 1.0 > 0.0 ? break4_g365.a : 1.0 ) ) * appendResult17_g365 ) , ( temp_output_4_0_g363 * min( ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g355 / fwidth( temp_output_625_0_g1 ) ) ) : step( 0.0 , temp_output_3_0_g355 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g364 : temp_output_2_0_g364 ) / max( temp_output_15_0_g363 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g1 = lerpResult620_g1;
				float4 temp_output_608_0_g1 = ( OverlayColorProcessed524_g1 * zzLerp_Border_Shadow629_g1 );
				float PB_SDF616_g1 = temp_output_615_0_g1;
				float temp_output_3_0_g350 = PB_SDF616_g1;
				float temp_output_534_0_g1 = min( temp_output_608_0_g1.a , ( 1.0 - ( AA530_g1 > 0.0 ? saturate( ( temp_output_3_0_g350 / fwidth( PB_SDF616_g1 ) ) ) : step( 0.0 , temp_output_3_0_g350 ) ) ) );
				

				surfaceDescription.Alpha = temp_output_534_0_g1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;

				return half4(NormalizeNormalPerPixel(normalWS), 0.0);
			}

			ENDHLSL
		}

	
	}
	
	CustomEditor "Renge.PPB.ProceduralProgressBarGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback "Hidden/InternalErrorShader"
}
/*ASEBEGIN
Version=19301
Node;AmplifyShaderEditor.FunctionNode;1032;483.9313,-80.146;Inherit;True;The Whole Shebang;0;;1;2d6870fee17216f4db3628575a74016f;0;0;3;FLOAT3;0;FLOAT;728;FLOAT3;729
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1020;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1022;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1023;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1024;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1025;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;5;False;;10;False;;1;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1026;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1027;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1028;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1029;794.0754,-184.3586;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;True;9;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps4;ps5;switch;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1021;808.0289,-79.8474;Float;False;True;-1;2;Renge.PPB.ProceduralProgressBarGUI;0;13;Renge/PPB_URP;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;5;False;;10;False;;1;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;21;Surface;1;638353324861505637;  Blend;0;0;Two Sided;1;0;Forward Only;0;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;GPU Instancing;0;638353324978412636;LOD CrossFade;1;0;Built-in Fog;1;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;638353367609337349;0;10;False;True;True;True;False;True;True;True;True;False;False;;False;0
WireConnection;1021;2;1032;0
WireConnection;1021;3;1032;728
WireConnection;1021;5;1032;729
ASEEND*/
//CHKSM=1793860443AD78A070B3788260D7D34E2B1FAA36