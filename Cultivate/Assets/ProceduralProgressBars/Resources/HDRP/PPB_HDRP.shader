// Made with Amplify Shader Editor v1.9.3.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Renge/PPB_HDRP"
{
	Properties
	{
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

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		[HideInInspector][ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		//[HideInInspector] _ShadowMatteFilter("Shadow Matte Filter", Float) = 2.006836
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0 // StencilUsage.Clear
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0 // Nothing
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8 // StencilUsage.TraceReflectionRay
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector][ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector][ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] _SurfaceType("Surface Type", Float) = 1
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector][ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector][ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 0
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Float) = 0
		[HideInInspector][ToggleUI] _EnableFogOnTransparent("Enable Fog", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("Transparent Cull Mode", Int) = 2// Back culling by default
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4 // Less equal
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Int) = 4// Less equal
		[HideInInspector][ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector][ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector][ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector][ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		[HideInInspector] _DistortionEnable("_DistortionEnable",Float) = 0
		[HideInInspector] _DistortionOnly("_DistortionOnly",Float) = 0

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleUI] _TransparentWritingMotionVec("Transparent Writing MotionVec", Float) = 0
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("Opaque Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector][ToggleUI] _SupportDecals("Support Decals", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0
		[HideInInspector] _EmissionColor("Color", Color) = (1, 1, 1)
		[HideInInspector] _UnlitColorMap_MipInfo("_UnlitColorMap_MipInfo", Vector) = (0, 0, 0, 0)

		[HideInInspector][Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0 //DoubleSidedGIMode added in api 12x and higher
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Transparent" }

		HLSLINCLUDE
		#pragma target 4.5
		

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

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
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
			
			Name "Forward Unlit"
			Tags { "LightMode"="ForwardOnly" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]

			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			ColorMask [_ColorMaskTransparentVel] 1

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

	        #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
	        #define _WRITE_TRANSPARENT_MOTION_VECTOR
	        #endif

			#define SHADERPASS SHADERPASS_FORWARD_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
				#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
				#define HAS_LIGHTLOOP
				#define SHADOW_OPTIMIZE_REGISTER_USAGE 1

				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Shadow/HDShadowContext.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadow.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/PunctualLightCommon.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadowLoop.hlsl"
			#endif

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float4 ShadowTint;
				float Alpha;
				float AlphaClipThreshold;
				float4 VTPackedFeedback;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				surfaceData.color = surfaceDescription.Color;
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription , FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif

				#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
					HDShadowContext shadowContext = InitShadowContext();
					float shadow;
					float3 shadow3;
					posInput = GetPositionInput(fragInputs.positionSS.xy, _ScreenSize.zw, fragInputs.positionSS.z, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
					float3 normalWS = normalize(fragInputs.tangentToWorld[1]);
					uint renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
					ShadowLoopMin(shadowContext, posInput, normalWS, asuint(_ShadowMatteFilter), renderingLayers, shadow3);
					shadow = dot(shadow3, float3(1.0f/3.0f, 1.0f/3.0f, 1.0f/3.0f));

					float4 shadowColor = (1 - shadow)*surfaceDescription.ShadowTint.rgba;
					float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);

					#ifdef _SURFACE_TYPE_TRANSPARENT
						surfaceData.color = lerp(shadowColor.rgb*surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow), surfaceDescription.Alpha);
					#else
						surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow);
					#endif
					localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
					surfaceDescription.Alpha = localAlpha;
				#endif

				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			float GetDeExposureMultiplier()
			{
			#if defined(DISABLE_UNLIT_DEEXPOSURE)
				return 1.0;
			#else
				return _DeExposureMultiplier;
			#endif
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				o.positionRWS = positionRWS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
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
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#ifdef UNITY_VIRTUAL_TEXTURING
			#define VT_BUFFER_TARGET SV_Target1
			#define EXTRA_BUFFER_TARGET SV_Target2
			#else
			#define EXTRA_BUFFER_TARGET SV_Target1
			#endif

			void Frag( VertexOutput packedInput,
						out float4 outColor : SV_Target0
						#ifdef UNITY_VIRTUAL_TEXTURING
						,out float4 outVTFeedback : VT_BUFFER_TARGET
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.positionRWS;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord1.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord1.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord1.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord1.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord1.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord1.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord1.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord1.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord1.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord1.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord1.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float4 break726_g351 = temp_output_608_0_g351;
				float3 appendResult727_g351 = (float3(break726_g351.r , break726_g351.g , break726_g351.b));
				
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Color = appendResult727_g351;
				surfaceDescription.Emission = 0;
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				surfaceDescription.ShadowTint = float4( 0, 0 ,0 ,1 );
				float2 Distortion = float2 ( 0, 0 );
				float DistortionBlur = 0;

				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );

				#if defined(_ENABLE_SHADOW_MATTE)
				bsdfData.color *= GetScreenSpaceAmbientOcclusion(input.positionSS.xy);
				#endif


			#ifdef DEBUG_DISPLAY
				if (_DebugLightingMode >= DEBUGLIGHTINGMODE_DIFFUSE_LIGHTING && _DebugLightingMode <= DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
				{
					if (_DebugLightingMode != DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
					{
						builtinData.emissiveColor = 0.0;
					}
					else
					{
						bsdfData.color = 0.0;
					}
				}
			#endif

				float4 outResult = ApplyBlendMode(bsdfData.color * GetDeExposureMultiplier() + builtinData.emissiveColor * GetCurrentExposureMultiplier(), builtinData.opacity);
				outResult = EvaluateAtmosphericScattering(posInput, V, outResult);

				#ifdef DEBUG_DISPLAY
					int bufferSize = int(_DebugViewMaterialArray[0].x);
					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index].x);
						if (indexMaterialProperty != 0)
						{
							float3 result = float3(1.0, 0.0, 1.0);
							bool needLinearToSRGB = false;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);

							if (!needLinearToSRGB)
								result = SRGBToLinear(max(0, result));

							outResult = float4(result, 1.0);
						}
					}

					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outResult = result;
					}
				#endif

				outColor = outResult;

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
					outVTFeedback = builtinData.vtPackedFeedback;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ColorMask 0

			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_SHADOWS
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				ZERO_INITIALIZE (BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
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
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
							#endif
						#else
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target0
							#endif
						#endif
						#if defined(_DEPTHOFFSET_ON)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				#if defined(_DEPTHOFFSET_ON)
				outputDepth = posInput.deviceDepth;
				float bias = max(abs(ddx(posInput.deviceDepth)), abs(ddy(posInput.deviceDepth))) * _SlopeScaleDepthBias;
				outputDepth += bias;
				#endif

				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.vmesh.positionCS.z;

				#ifdef _ALPHATOMASK_ON
				depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
				#endif
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

				#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				DecalPrepassData decalPrepassData;
				decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
				decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
				EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma shader_feature EDITOR_VISUALIZATION

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD0;
				float4 LightCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			
			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData( FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );
				surfaceData.color = surfaceDescription.Color;

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData )
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData( fragInputs, surfaceDescription, V, surfaceData );
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			#define SCENEPICKINGPASS
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.uv0.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord2.xy = inputMesh.uv0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

			#ifdef EDITOR_VISUALIZATION
				float2 vizUV = 0;
				float4 lightCoord = 0;
				UnityEditorVizData(inputMesh.positionOS.xyz, inputMesh.uv0.xy, inputMesh.uv1.xy, inputMesh.uv2.xy, vizUV, lightCoord);
			#endif

				float2 uv = float2( 0.0, 0.0 );
				if( unity_MetaVertexControl.x )
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if( unity_MetaVertexControl.y )
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				#ifdef EDITOR_VISUALIZATION
					o.VizUV.xy = vizUV;
					o.LightCoord = lightCoord;
				#endif

				o.positionCS = float4( uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0 );
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.uv3 = v.uv3;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.uv3 = patch[0].uv3 * bary.x + patch[1].uv3 * bary.y + patch[2].uv3 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag( VertexOutput packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS );

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord2.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord2.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord2.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord2.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord2.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord2.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord2.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord2.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float4 break726_g351 = temp_output_608_0_g351;
				float3 appendResult727_g351 = (float3(break726_g351.r , break726_g351.g , break726_g351.b));
				
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Color = appendResult727_g351;
				surfaceDescription.Emission = 0;
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData( surfaceDescription,input, V, posInput, surfaceData, builtinData );

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );
				LightTransportData lightTransportData = GetLightTransportData( surfaceData, builtinData, bsdfData );

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				UnityMetaInput metaInput;
				metaInput.Albedo = lightTransportData.diffuseColor.rgb;
				metaInput.Emission = lightTransportData.emissiveColor;
			#ifdef EDITOR_VISUALIZATION
				metaInput.VizUV = packedInput.VizUV;
				metaInput.LightCoord = packedInput.LightCoord;
			#endif
				res = UnityMetaFragment(metaInput);

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma editor_sync_compilation

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			int _ObjectId;
			int _PassValue;

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
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
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					, out float4 outColor : SV_Target0
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthForwardOnly"
			Tags { "LightMode"="DepthForwardOnly" }

			Cull [_CullMode]
			ZWrite On
			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
			}


			ColorMask 0 0

			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
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
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
							#endif
						#else
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target0
							#endif
						#endif
						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.positionCS.z;
					#ifdef _ALPHATOMASK_ON
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
					#endif
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "MotionVectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_MOTION_VECTORS

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
                    builtinData.renderingLayers = GetMeshRenderingLightLayer();
                #endif


                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif


                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexInput ApplyMeshModification(VertexInput inputMesh, float3 timeParameters, inout VertexOutput o )
			{
				_TimeParameters.xyz = timeParameters;
				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = staticSwitch581_g351;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS =  inputMesh.normalOS ;
				return inputMesh;
			}

			VertexOutput VertexFunction(VertexInput inputMesh)
			{
				VertexOutput o = (VertexOutput)0;
				VertexInput defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, o);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				//#if defined(UNITY_REVERSED_Z)
				//	VMESHpositionCS.z -= unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#else
				//	VMESHpositionCS.z += unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#endif

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						VertexInput previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						VertexOutput test = (VertexOutput)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				o.vmeshPositionCS = VMESHpositionCS;
				o.vmeshInterp00.xyz = VMESHpositionRWS;

				o.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				o.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return o;
			}

			#if ( 0 ) // TEMPORARY: defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_DECAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target3
			#elif defined(WRITE_DECAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target2
			#else
			#define SV_TARGET_NORMAL SV_Target1
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
						, out float4 outMotionVector : SV_Target1
							#ifdef WRITE_DECAL_BUFFER
							, out float4 outDecalBuffer : SV_Target2
							#endif
						#else
						, out float4 outMotionVector : SV_Target0
							#ifdef WRITE_DECAL_BUFFER
							, out float4 outDecalBuffer : SV_Target1
							#endif
						#endif

						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_TARGET_NORMAL
						#endif

						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord3.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord3.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord3.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord3.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord3.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord3.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord3.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord3.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord3.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord3.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord3.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				// Depth and Alpha to coverage
				#ifdef WRITE_MSAA_DEPTH
					// In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
					depthColor = packedInput.vmeshPositionCS.z;
					#ifdef _ALPHATOMASK_ON
					// Alpha channel is used for alpha to coverage
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
					#endif
				#endif

				// Normal Buffer Processing
				#ifdef WRITE_NORMAL_BUFFER
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

				#if defined(WRITE_DECAL_BUFFER)
					DecalPrepassData decalPrepassData;
					#ifdef _DISABLE_DECALS
					ZERO_INITIALIZE(DecalPrepassData, decalPrepassData);
					#else
					decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
					decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
					#endif
					EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);

					#if ASE_SRP_VERSION >= 120107
					// make sure we don't overwrite light layers
					outDecalBuffer.w = (GetMeshRenderingLightLayer() & 0x000000FF) / 255.0;
					#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
        Pass
		{
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull [_CullMode]

			HLSLPROGRAM

			#pragma shader_feature_local_fragment _ENABLE_FOG_ON_TRANSPARENT
			#define HAVE_MESH_MODIFICATION 1
			#define ASE_SRP_VERSION 120113


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _TRANSPARENT_WRITES_MOTION_VEC

			#pragma editor_sync_compilation

			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_TANGENT_TO_WORLD

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENEPICKINGPASS 1

			#define SHADER_UNLIT

			float4 _SelectionID;

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
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
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


            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile_local SHAPE_LINEAR SHAPE_CIRCULAR
			#pragma multi_compile_local __ OVERLAY_TEXTURE_ON
			#pragma multi_compile_local __ BORDER_TEXTURE_ON
			#pragma multi_compile_local __ INNER_TEXTURE_ON
			#pragma multi_compile_local __ BACKGROUND_TEXTURE_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 tangentWS : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
            struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};


            void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                #ifdef LOD_FADE_CROSSFADE
			        LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif

                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif


				ZERO_INITIALIZE(SurfaceData, surfaceData);

				ZERO_BUILTIN_INITIALIZE(builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif

                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);

            }


			VertexOutput VertexFunction(VertexInput inputMesh  )
			{

				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o );

				float3 appendResult582_g351 = (float3(( ( ( inputMesh.ase_texcoord.y - 0.5 ) * 2.0 ) * _Slant ) , 0.0 , 0.0));
				#if defined(SHAPE_LINEAR)
				float3 staticSwitch581_g351 = appendResult582_g351;
				#elif defined(SHAPE_CIRCULAR)
				float3 staticSwitch581_g351 = float3(0,0,0);
				#else
				float3 staticSwitch581_g351 = appendResult582_g351;
				#endif
				
				o.ase_texcoord2.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  staticSwitch581_g351;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				o.positionCS = TransformWorldToHClip(positionRWS);
				o.normalWS.xyz =  normalWS;
				o.tangentWS.xyzw =  tangentWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(	VertexOutput packedInput
						, out float4 outColor : SV_Target0
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				UNITY_SETUP_INSTANCE_ID(packedInput);

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.tangentToWorld = BuildTangentToWorld(packedInput.tangentWS.xyzw, packedInput.normalWS.xyz);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float Pixelate531_g351 = _Pixelate;
				float PixelCount545_g351 = _PixelCount;
				#if defined(SHAPE_LINEAR)
				float staticSwitch687_g351 = 0.0;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch687_g351 = 1.0;
				#else
				float staticSwitch687_g351 = 0.0;
				#endif
				float temp_output_588_0_g351 = ( staticSwitch687_g351 > 0.0 ? 1.0 : 0.0 );
				float Arc539_g351 = _Arc;
				float Width537_g351 = _Width;
				float temp_output_9_0_g389 = Width537_g351;
				float Radius536_g351 = _Radius;
				float2 appendResult587_g351 = (float2(( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g389 ) + ( Radius536_g351 - ( temp_output_9_0_g389 / 2.0 ) ) ) * ( TWO_PI * _CircleLength ) ) ) , Width537_g351));
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 appendResult28_g420 = (float3(float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).x , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).y , float4( UNITY_MATRIX_M[0][0],UNITY_MATRIX_M[1][0],UNITY_MATRIX_M[2][0],UNITY_MATRIX_M[3][0] ).z));
				float3 appendResult29_g420 = (float3(float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).x , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).y , float4( UNITY_MATRIX_M[0][1],UNITY_MATRIX_M[1][1],UNITY_MATRIX_M[2][1],UNITY_MATRIX_M[3][1] ).z));
				float3 appendResult30_g420 = (float3(float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).x , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).y , float4( UNITY_MATRIX_M[0][2],UNITY_MATRIX_M[1][2],UNITY_MATRIX_M[2][2],UNITY_MATRIX_M[3][2] ).z));
				float3 appendResult24_g420 = (float3(length( appendResult28_g420 ) , length( appendResult29_g420 ) , length( appendResult30_g420 )));
				float3 temp_output_38_0_g420 = ( 0.0 > 0.0 ? ase_objectScale : appendResult24_g420 );
				float3 temp_output_16_0_g420 = ( ( ( temp_output_588_0_g351 + _UIScaling ) > 0.0 ? 1.0 : 0.0 ) > 0.0 ? float3( ( temp_output_588_0_g351 > 0.0 ? appendResult587_g351 : _CustomScale ) ,  0.0 ) : temp_output_38_0_g420 );
				float3 break9_g420 = temp_output_16_0_g420;
				float3 break48_g420 = ( _RatioScaling > 0.0 ? ( temp_output_16_0_g420 / min( break9_g420.x , break9_g420.y ) ) : temp_output_16_0_g420 );
				float2 appendResult10_g420 = (float2(break48_g420.x , break48_g420.y));
				float2 OSXY554_g351 = appendResult10_g420;
				float2 temp_output_6_0_g418 = ( PixelCount545_g351 * OSXY554_g351 );
				float2 PixelationUV559_g351 = ( Pixelate531_g351 > 0.0 ? ( floor( ( packedInput.ase_texcoord2.xy * temp_output_6_0_g418 ) ) / ( temp_output_6_0_g418 - float2( 1,1 ) ) ) : packedInput.ase_texcoord2.xy );
				float2 temp_output_2_0_g354 = ( ( PixelationUV559_g351 * _OverlayTextureTiling ) + _OverlayTextureOffset );
				float2 break51_g351 = max( _OverlayFlipbookDim , float2( 1,1 ) );
				float fps541_g351 = _FlipbookFPS;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles13_g353 = break51_g351.x * break51_g351.y;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset13_g353 = 1.0f / break51_g351.x;
				float fbrowsoffset13_g353 = 1.0f / break51_g351.y;
				// Speed of animation
				float fbspeed13_g353 = _TimeParameters.x * fps541_g351;
				// UV Tiling (col and row offset)
				float2 fbtiling13_g353 = float2(fbcolsoffset13_g353, fbrowsoffset13_g353);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex13_g353 = round( fmod( fbspeed13_g353 + 0.0, fbtotaltiles13_g353) );
				fbcurrenttileindex13_g353 += ( fbcurrenttileindex13_g353 < 0) ? fbtotaltiles13_g353 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox13_g353 = round ( fmod ( fbcurrenttileindex13_g353, break51_g351.x ) );
				// Multiply Offset X by coloffset
				float fboffsetx13_g353 = fblinearindextox13_g353 * fbcolsoffset13_g353;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy13_g353 = round( fmod( ( fbcurrenttileindex13_g353 - fblinearindextox13_g353 ) / break51_g351.x, break51_g351.y ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy13_g353 = (int)(break51_g351.y-1) - fblinearindextoy13_g353;
				// Multiply Offset Y by rowoffset
				float fboffsety13_g353 = fblinearindextoy13_g353 * fbrowsoffset13_g353;
				// UV Offset
				float2 fboffset13_g353 = float2(fboffsetx13_g353, fboffsety13_g353);
				// Flipbook UV
				half2 fbuv13_g353 = temp_output_2_0_g354 * fbtiling13_g353 + fboffset13_g353;
				// *** END Flipbook UV Animation vars ***
				float4 lerpResult45_g351 = lerp( _OverlayColor , ( _OverlayColor * tex2D( _OverlayTexture, fbuv13_g353 ) ) , saturate( _OverlayTextureOpacity ));
				#ifdef OVERLAY_TEXTURE_ON
				float4 staticSwitch44_g351 = lerpResult45_g351;
				#else
				float4 staticSwitch44_g351 = _OverlayColor;
				#endif
				float4 OverlayColorProcessed524_g351 = staticSwitch44_g351;
				float BorderWidth529_g351 = _BorderWidth;
				float4 break4_g380 = _BorderColor;
				float4 appendResult17_g380 = (float4(break4_g380.r , break4_g380.g , break4_g380.b , 1.0));
				float4 temp_output_738_0_g351 = ( ( saturate( ceil( BorderWidth529_g351 ) ) * ( 1.0 > 0.0 ? break4_g380.a : 1.0 ) ) * appendResult17_g380 );
				float segment_count527_g351 = _SegmentCount;
				float2 appendResult345_g351 = (float2(segment_count527_g351 , 1.0));
				float2 temp_output_2_0_g372 = ( ( PixelationUV559_g351 * appendResult345_g351 ) + float2( 0,0 ) );
				float2 break10_g372 = temp_output_2_0_g372;
				float2 appendResult352_g351 = (float2(( break10_g372.x % 1.0 ) , break10_g372.y));
				float2 ScaledTextureUV349_g351 = appendResult352_g351;
				float2 temp_output_2_0_g371 = ( ( PixelationUV559_g351 * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 UnscaledTextureUV350_g351 = temp_output_2_0_g371;
				float2 break77_g351 = _BorderTextureTiling;
				float2 uv_VariableWidthCurve = packedInput.ase_texcoord2.xy * _VariableWidthCurve_ST.xy + _VariableWidthCurve_ST.zw;
				float width_curve532_g351 = tex2D( _VariableWidthCurve, uv_VariableWidthCurve ).r;
				float temp_output_75_0_g351 = ( break77_g351.y / width_curve532_g351 );
				float2 appendResult74_g351 = (float2(break77_g351.x , temp_output_75_0_g351));
				float2 appendResult70_g351 = (float2(0.0 , ( -( temp_output_75_0_g351 / 2.0 ) + 0.5 )));
				float2 temp_output_2_0_g355 = ( ( ( _BorderTextureScaleWithSegments > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult74_g351 ) + ( _BorderTextureOffset + appendResult70_g351 ) );
				float cos63_g351 = cos( radians( _BorderTextureRotation ) );
				float sin63_g351 = sin( radians( _BorderTextureRotation ) );
				float2 rotator63_g351 = mul( temp_output_2_0_g355 - float2( 0.5,0.5 ) , float2x2( cos63_g351 , -sin63_g351 , sin63_g351 , cos63_g351 )) + float2( 0.5,0.5 );
				float2 break39_g351 = max( _BorderFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g352 = break39_g351.x * break39_g351.y;
				float fbcolsoffset13_g352 = 1.0f / break39_g351.x;
				float fbrowsoffset13_g352 = 1.0f / break39_g351.y;
				float fbspeed13_g352 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g352 = float2(fbcolsoffset13_g352, fbrowsoffset13_g352);
				float fbcurrenttileindex13_g352 = round( fmod( fbspeed13_g352 + 0.0, fbtotaltiles13_g352) );
				fbcurrenttileindex13_g352 += ( fbcurrenttileindex13_g352 < 0) ? fbtotaltiles13_g352 : 0;
				float fblinearindextox13_g352 = round ( fmod ( fbcurrenttileindex13_g352, break39_g351.x ) );
				float fboffsetx13_g352 = fblinearindextox13_g352 * fbcolsoffset13_g352;
				float fblinearindextoy13_g352 = round( fmod( ( fbcurrenttileindex13_g352 - fblinearindextox13_g352 ) / break39_g351.x, break39_g351.y ) );
				fblinearindextoy13_g352 = (int)(break39_g351.y-1) - fblinearindextoy13_g352;
				float fboffsety13_g352 = fblinearindextoy13_g352 * fbrowsoffset13_g352;
				float2 fboffset13_g352 = float2(fboffsetx13_g352, fboffsety13_g352);
				half2 fbuv13_g352 = rotator63_g351 * fbtiling13_g352 + fboffset13_g352;
				float4 lerpResult35_g351 = lerp( temp_output_738_0_g351 , ( tex2D( _BorderTexture, fbuv13_g352 ) * temp_output_738_0_g351 ) , saturate( _BorderTextureOpacity ));
				#ifdef BORDER_TEXTURE_ON
				float4 staticSwitch496_g351 = lerpResult35_g351;
				#else
				float4 staticSwitch496_g351 = temp_output_738_0_g351;
				#endif
				float4 BorderColorProcessed497_g351 = staticSwitch496_g351;
				float InnerBorderWidth250_g351 = _InnerBorderWidth;
				float4 break4_g386 = _InnerBorderColor;
				float4 appendResult17_g386 = (float4(break4_g386.r , break4_g386.g , break4_g386.b , 1.0));
				float4 temp_output_745_0_g351 = ( ( saturate( ceil( InnerBorderWidth250_g351 ) ) * ( 1.0 > 0.0 ? break4_g386.a : 1.0 ) ) * appendResult17_g386 );
				float4 break4_g388 = _PulseColor;
				float4 appendResult17_g388 = (float4(break4_g388.r , break4_g388.g , break4_g388.b , 1.0));
				float4 PulseColorProcessed384_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g388.a : 1.0 ) ) * appendResult17_g388 );
				float Value574_g351 = _Value;
				float temp_output_1_0_g370 = ( _PulseActivationThreshold - ( _PulseRamp * _PulseActivationThreshold ) );
				float PulseAlpha382_g351 = ( _PulsateWhenLow > 0.0 ? (0.0 + (sin( ( _TimeParameters.x * _PulseSpeed ) ) - -1.0) * (( 1.0 - saturate( ( ( ( Value574_g351 / segment_count527_g351 ) - temp_output_1_0_g370 ) / ( _PulseActivationThreshold - temp_output_1_0_g370 ) ) ) ) - 0.0) / (1.0 - -1.0)) : 0.0 );
				float4 lerpResult389_g351 = lerp( temp_output_745_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float2 temp_cast_4 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float OSX553_g351 = break48_g420.x;
				float temp_output_444_0_g351 = ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 );
				float Segment_Spacing533_g351 = _SegmentSpacing;
				float temp_output_449_0_g351 = ( Segment_Spacing533_g351 * OSX553_g351 );
				float temp_output_408_0_g351 = ( ( segment_count527_g351 * OSX553_g351 ) / ( ( temp_output_444_0_g351 + ( OSX553_g351 * segment_count527_g351 ) ) - temp_output_449_0_g351 ) );
				float2 appendResult422_g351 = (float2(temp_output_408_0_g351 , 1.0));
				float2 appendResult407_g351 = (float2(-( ( temp_output_408_0_g351 - 1.0 ) / 2.0 ) , 0.0));
				float2 temp_output_2_0_g375 = ( ( PixelationUV559_g351 * appendResult422_g351 ) + appendResult407_g351 );
				float2 GradientUV479_g351 = temp_output_2_0_g375;
				float cos363_g351 = cos( radians( _InnerGradientRotation ) );
				float sin363_g351 = sin( radians( _InnerGradientRotation ) );
				float2 rotator363_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos363_g351 , -sin363_g351 , sin363_g351 , cos363_g351 )) + float2( 0.5,0.5 );
				float4 break4_g382 = tex2D( _InnerGradient, ( _ValueAsGradientTimeInner > 0.0 ? temp_cast_4 : rotator363_g351 ) );
				float4 appendResult17_g382 = (float4(break4_g382.r , break4_g382.g , break4_g382.b , 1.0));
				float4 temp_output_740_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g382.a : 1.0 ) ) * appendResult17_g382 );
				float4 lerpResult390_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_745_0_g351 * temp_output_740_0_g351 ) : temp_output_745_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch388_g351 = lerpResult390_g351;
				#else
				float4 staticSwitch388_g351 = lerpResult389_g351;
				#endif
				float4 ValueBorderColorProcessed525_g351 = staticSwitch388_g351;
				float4 break679_g351 = ValueBorderColorProcessed525_g351;
				float4 appendResult675_g351 = (float4(break679_g351.x , break679_g351.y , break679_g351.z , 1.0));
				float4 break4_g387 = _InnerColor;
				float4 appendResult17_g387 = (float4(break4_g387.r , break4_g387.g , break4_g387.b , 1.0));
				float4 temp_output_746_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g387.a : 1.0 ) ) * appendResult17_g387 );
				float4 lerpResult369_g351 = lerp( temp_output_746_0_g351 , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float4 lerpResult367_g351 = lerp( ( _InnerGradientEnabled > 0.0 ? ( temp_output_740_0_g351 * temp_output_746_0_g351 ) : temp_output_746_0_g351 ) , PulseColorProcessed384_g351 , PulseAlpha382_g351);
				float Inner_Tex_Scale_w_Segments252_g351 = _InnerTextureScaleWithSegments;
				float2 Inner_Tex_Tiling254_g351 = _InnerTextureTiling;
				float temp_output_330_0_g351 = ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_324_0_g351 = ( ( ( Inner_Tex_Tiling254_g351.x * OSX553_g351 ) * temp_output_330_0_g351 ) / ( ( ( temp_output_330_0_g351 * OSX553_g351 ) + ( ( BorderWidth529_g351 * segment_count527_g351 ) * -2.0 ) ) - ( OSX553_g351 * Segment_Spacing533_g351 ) ) );
				float OSY552_g351 = break48_g420.y;
				float temp_output_270_0_g351 = ( Inner_Tex_Tiling254_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult276_g351 = (float2(temp_output_324_0_g351 , temp_output_270_0_g351));
				float CenterFill562_g351 = _CenterFill;
				float2 temp_output_2_0_g368 = ( ( packedInput.ase_texcoord2.xy * float2( 1,1 ) ) + float2( 0,0 ) );
				float2 break10_g368 = temp_output_2_0_g368;
				float lerpResult321_g351 = lerp( 0.0 , ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( 1.0 - ( min( Value574_g351 , segment_count527_g351 ) % 1.0 ) ) : ( 1.0 - saturate( ( Value574_g351 / segment_count527_g351 ) ) ) ) , step( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ( floor( Value574_g351 ) / segment_count527_g351 ) : 0.0 ) , break10_g368.x ));
				float2 appendResult277_g351 = (float2(( ( -( ( temp_output_324_0_g351 - Inner_Tex_Tiling254_g351.x ) / 2.0 ) + _InnerTextureOffset.x ) + ( Inner_Tex_Tiling254_g351.x * ( _OffsetTextureWithValue > 0.0 ? ( CenterFill562_g351 > 0.0 ? 0.0 : lerpResult321_g351 ) : 0.0 ) ) ) , ( _InnerTextureOffset.y + ( -( temp_output_270_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g367 = ( ( ( Inner_Tex_Scale_w_Segments252_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult276_g351 ) + appendResult277_g351 );
				float cos299_g351 = cos( radians( _InnerTextureRotation ) );
				float sin299_g351 = sin( radians( _InnerTextureRotation ) );
				float2 rotator299_g351 = mul( temp_output_2_0_g367 - float2( 0.5,0.5 ) , float2x2( cos299_g351 , -sin299_g351 , sin299_g351 , cos299_g351 )) + float2( 0.5,0.5 );
				float2 break275_g351 = max( _InnerFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g369 = break275_g351.x * break275_g351.y;
				float fbcolsoffset13_g369 = 1.0f / break275_g351.x;
				float fbrowsoffset13_g369 = 1.0f / break275_g351.y;
				float fbspeed13_g369 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g369 = float2(fbcolsoffset13_g369, fbrowsoffset13_g369);
				float fbcurrenttileindex13_g369 = round( fmod( fbspeed13_g369 + 0.0, fbtotaltiles13_g369) );
				fbcurrenttileindex13_g369 += ( fbcurrenttileindex13_g369 < 0) ? fbtotaltiles13_g369 : 0;
				float fblinearindextox13_g369 = round ( fmod ( fbcurrenttileindex13_g369, break275_g351.x ) );
				float fboffsetx13_g369 = fblinearindextox13_g369 * fbcolsoffset13_g369;
				float fblinearindextoy13_g369 = round( fmod( ( fbcurrenttileindex13_g369 - fblinearindextox13_g369 ) / break275_g351.x, break275_g351.y ) );
				fblinearindextoy13_g369 = (int)(break275_g351.y-1) - fblinearindextoy13_g369;
				float fboffsety13_g369 = fblinearindextoy13_g369 * fbrowsoffset13_g369;
				float2 fboffset13_g369 = float2(fboffsetx13_g369, fboffsety13_g369);
				half2 fbuv13_g369 = rotator299_g351 * fbtiling13_g369 + fboffset13_g369;
				float4 break4_g381 = tex2D( _InnerTexture, fbuv13_g369 );
				float4 appendResult17_g381 = (float4(break4_g381.r , break4_g381.g , break4_g381.b , 1.0));
				float4 lerpResult314_g351 = lerp( lerpResult367_g351 , ( lerpResult367_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g381.a : 1.0 ) ) * appendResult17_g381 ) ) , saturate( _InnerTextureOpacity ));
				#ifdef INNER_TEXTURE_ON
				float4 staticSwitch686_g351 = lerpResult314_g351;
				#else
				float4 staticSwitch686_g351 = lerpResult369_g351;
				#endif
				float4 ValueColorProcessed398_g351 = staticSwitch686_g351;
				float AA530_g351 = _AntiAlias;
				float temp_output_234_0_g351 = ( ( ( ( segment_count527_g351 % 1.0 ) == 0.0 ? 1.0 : 0.0 ) * CenterFill562_g351 ) > 0.0 ? 1.0 : 0.0 );
				float temp_output_220_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_221_0_g351 = ( temp_output_220_0_g351 - ( temp_output_220_0_g351 * ( ( ( ( segment_count527_g351 * BorderWidth529_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 ) ) );
				float temp_output_188_0_g351 = max( 0.0 , Value574_g351 );
				float temp_output_181_0_g351 = ( max( ( segment_count527_g351 - temp_output_188_0_g351 ) , 0.0 ) / 2.0 );
				float temp_output_180_0_g351 = floor( temp_output_181_0_g351 );
				float temp_output_179_0_g351 = ( ( temp_output_180_0_g351 + 1.0 ) / segment_count527_g351 );
				float2 break11_g365 = packedInput.ase_texcoord2.xy;
				float temp_output_2_0_g365 = ( 1.0 > 0.0 ? ( ( break11_g365.x * -1.0 ) + 1.0 ) : break11_g365.x );
				float temp_output_171_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g365 );
				float2 break11_g364 = packedInput.ase_texcoord2.xy;
				float temp_output_2_0_g364 = ( 0.0 > 0.0 ? ( ( break11_g364.x * -1.0 ) + 1.0 ) : break11_g364.x );
				float temp_output_173_0_g351 = step( temp_output_179_0_g351 , temp_output_2_0_g364 );
				float temp_output_215_0_g351 = ( temp_output_221_0_g351 * ( 1.0 - ( temp_output_181_0_g351 % 1.0 ) ) );
				float temp_output_176_0_g351 = ( temp_output_180_0_g351 / segment_count527_g351 );
				float temp_output_175_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g365 ) - temp_output_171_0_g351 );
				float temp_output_174_0_g351 = ( step( temp_output_176_0_g351 , temp_output_2_0_g364 ) - temp_output_173_0_g351 );
				float temp_output_192_0_g351 = min( temp_output_175_0_g351 , temp_output_174_0_g351 );
				float2 appendResult196_g351 = (float2(( ( ( -temp_output_221_0_g351 * temp_output_171_0_g351 ) + ( temp_output_221_0_g351 * temp_output_173_0_g351 ) ) + ( ( -temp_output_215_0_g351 * ( temp_output_175_0_g351 - temp_output_192_0_g351 ) ) + ( temp_output_215_0_g351 * ( temp_output_174_0_g351 - temp_output_192_0_g351 ) ) ) ) , 0.0));
				float temp_output_151_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float temp_output_159_0_g351 = min( segment_count527_g351 , Value574_g351 );
				float temp_output_135_0_g351 = ( ( ( ( BorderWidth529_g351 * segment_count527_g351 ) * 2.0 ) / OSX553_g351 ) + Segment_Spacing533_g351 );
				float temp_output_160_0_g351 = floor( temp_output_159_0_g351 );
				float temp_output_154_0_g351 = step( ( ( temp_output_160_0_g351 + 1.0 ) / segment_count527_g351 ) , packedInput.ase_texcoord2.xy.x );
				float2 appendResult149_g351 = (float2(max( ( ( temp_output_151_0_g351 - ( temp_output_151_0_g351 * (temp_output_135_0_g351 + (( temp_output_159_0_g351 % 1.0 ) - 0.0) * (1.0 - temp_output_135_0_g351) / (1.0 - 0.0)) ) ) * ( step( ( temp_output_160_0_g351 / segment_count527_g351 ) , packedInput.ase_texcoord2.xy.x ) - temp_output_154_0_g351 ) ) , ( ( temp_output_151_0_g351 - ( temp_output_135_0_g351 * temp_output_151_0_g351 ) ) * temp_output_154_0_g351 ) ) , 0.0));
				float2 temp_output_128_0_g351 = ( temp_output_234_0_g351 > 0.0 ? appendResult196_g351 : appendResult149_g351 );
				float2 temp_output_2_0_g415 = OSXY554_g351;
				float2 break22_g415 = -( temp_output_2_0_g415 / float2( 2,2 ) );
				float2 appendResult29_g415 = (float2(( 0.0 > 0.0 ? break22_g415.x : 0.0 ) , ( 0.0 > 0.0 ? break22_g415.y : 0.0 )));
				float2 temp_output_2_0_g416 = ( ( PixelationUV559_g351 * temp_output_2_0_g415 ) + appendResult29_g415 );
				float temp_output_701_0_g351 = ( OSX553_g351 / segment_count527_g351 );
				float2 appendResult705_g351 = (float2(temp_output_701_0_g351 , OSY552_g351));
				float2 temp_output_11_0_g378 = appendResult705_g351;
				float2 temp_output_12_0_g378 = ( temp_output_2_0_g416 % temp_output_11_0_g378 );
				float2 break13_g378 = ( temp_output_12_0_g378 - ( temp_output_11_0_g378 / float2( 2,2 ) ) );
				float2 break14_g378 = temp_output_12_0_g378;
				float2 appendResult1_g378 = (float2(( 1.0 > 0.0 ? break13_g378.x : break14_g378.x ) , ( 1.0 > 0.0 ? break13_g378.y : break14_g378.y )));
				float2 SegmentUV521_g351 = appendResult1_g378;
				float2 temp_output_20_0_g363 = ( ( temp_output_128_0_g351 + SegmentUV521_g351 ) + ( OSXY554_g351 * _ValueMaskOffset ) );
				float2 break23_g363 = temp_output_20_0_g363;
				float BorderRadius548_g351 = _BorderRadius;
				float InnerRoundingPercent720_g351 = _InnerRoundingPercent;
				float temp_output_718_0_g351 = ( ( width_curve532_g351 * BorderRadius548_g351 ) * InnerRoundingPercent720_g351 );
				float temp_output_9_0_g366 = Width537_g351;
				float temp_output_118_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g366 ) + ( Radius536_g351 - ( temp_output_9_0_g366 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch249_g351 = ( temp_output_118_0_g351 * temp_output_718_0_g351 );
				#else
				float staticSwitch249_g351 = temp_output_718_0_g351;
				#endif
				float Rounding13_g363 = staticSwitch249_g351;
				float4 BorderRadiusOffset547_g351 = _BorderRadiusOffset;
				float4 temp_output_717_0_g351 = ( ( width_curve532_g351 * BorderRadiusOffset547_g351 ) * InnerRoundingPercent720_g351 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch246_g351 = ( temp_output_118_0_g351 * temp_output_717_0_g351 );
				#else
				float4 staticSwitch246_g351 = temp_output_717_0_g351;
				#endif
				float4 break27_g363 = ( Rounding13_g363 + staticSwitch246_g351 );
				float2 appendResult25_g363 = (float2(break27_g363.x , break27_g363.w));
				float2 appendResult26_g363 = (float2(break27_g363.y , break27_g363.z));
				float2 break32_g363 = ( break23_g363.x > 0.0 ? appendResult25_g363 : appendResult26_g363 );
				float temp_output_31_0_g363 = ( break23_g363.y > 0.0 ? break32_g363.x : break32_g363.y );
				float2 appendResult520_g351 = (float2(temp_output_701_0_g351 , ( OSY552_g351 * width_curve532_g351 )));
				float2 appendResult512_g351 = (float2(( 0.5 - ( Segment_Spacing533_g351 / 2.0 ) ) , 0.5));
				float2 SegmentSize619_g351 = ( ( appendResult520_g351 * appendResult512_g351 ) + float2( 0,-0.01 ) );
				float temp_output_211_0_g351 = ( segment_count527_g351 * 2.0 );
				float2 appendResult710_g351 = (float2(( temp_output_192_0_g351 * ( ( 1.0 - temp_output_188_0_g351 ) * ( ( ( OSX553_g351 / temp_output_211_0_g351 ) - BorderWidth529_g351 ) - ( ( OSX553_g351 * Segment_Spacing533_g351 ) / temp_output_211_0_g351 ) ) ) ) , 0.0));
				float2 temp_output_10_0_g363 = ( ( float2( 1,1 ) * temp_output_31_0_g363 ) + ( abs( temp_output_20_0_g363 ) - ( SegmentSize619_g351 - ( temp_output_234_0_g351 > 0.0 ? appendResult710_g351 : float2( 0,0 ) ) ) ) );
				float2 break8_g363 = temp_output_10_0_g363;
				float2 temp_output_20_0_g362 = SegmentUV521_g351;
				float2 break23_g362 = temp_output_20_0_g362;
				float AdjustBorderRadiusToWidthCurve557_g351 = _AdjustBorderRadiusToWidthCurve;
				float temp_output_9_0_g376 = Width537_g351;
				float temp_output_507_0_g351 = ( ( saturate( ( 1.0 - Arc539_g351 ) ) * ( ( ( packedInput.ase_texcoord2.xy.y * temp_output_9_0_g376 ) + ( Radius536_g351 - ( temp_output_9_0_g376 / 2.0 ) ) ) * ( TWO_PI * 1.0 ) ) ) / Radius536_g351 );
				#if defined(SHAPE_LINEAR)
				float staticSwitch523_g351 = BorderRadius548_g351;
				#elif defined(SHAPE_CIRCULAR)
				float staticSwitch523_g351 = ( BorderRadius548_g351 * temp_output_507_0_g351 );
				#else
				float staticSwitch523_g351 = BorderRadius548_g351;
				#endif
				float SegmentRounding518_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( staticSwitch523_g351 * width_curve532_g351 ) : staticSwitch523_g351 );
				float Rounding13_g362 = ( SegmentRounding518_g351 * 1.0 );
				#if defined(SHAPE_LINEAR)
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#elif defined(SHAPE_CIRCULAR)
				float4 staticSwitch723_g351 = ( BorderRadiusOffset547_g351 * temp_output_507_0_g351 );
				#else
				float4 staticSwitch723_g351 = BorderRadiusOffset547_g351;
				#endif
				float4 SegmentRoundingOffset519_g351 = ( AdjustBorderRadiusToWidthCurve557_g351 > 0.0 ? ( width_curve532_g351 * staticSwitch723_g351 ) : staticSwitch723_g351 );
				float4 break27_g362 = ( Rounding13_g362 + ( float4( 1,1,1,1 ) * SegmentRoundingOffset519_g351 ) );
				float2 appendResult25_g362 = (float2(break27_g362.x , break27_g362.w));
				float2 appendResult26_g362 = (float2(break27_g362.y , break27_g362.z));
				float2 break32_g362 = ( break23_g362.x > 0.0 ? appendResult25_g362 : appendResult26_g362 );
				float temp_output_31_0_g362 = ( break23_g362.y > 0.0 ? break32_g362.x : break32_g362.y );
				float2 temp_output_10_0_g362 = ( ( float2( 1,1 ) * temp_output_31_0_g362 ) + ( abs( temp_output_20_0_g362 ) - SegmentSize619_g351 ) );
				float2 break8_g362 = temp_output_10_0_g362;
				float temp_output_89_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) - InnerBorderWidth250_g351 );
				float temp_output_3_0_g356 = ( 0.0 + 0.0 + temp_output_89_0_g351 );
				float InnerValue240_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g356 / fwidth( temp_output_89_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g356 ) );
				float4 lerpResult674_g351 = lerp( appendResult675_g351 , ValueColorProcessed398_g351 , max( ( 1.0 - break679_g351.w ) , InnerValue240_g351 ));
				float temp_output_15_0_g395 = _ValueInsetShadowSize;
				float temp_output_4_0_g395 = saturate( ceil( temp_output_15_0_g395 ) );
				float4 break4_g397 = _ValueInsetShadowColor;
				float4 appendResult17_g397 = (float4(break4_g397.r , break4_g397.g , break4_g397.b , 1.0));
				float temp_output_86_0_g351 = ( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) + 0.0 + 0.0 );
				float temp_output_3_0_g357 = temp_output_86_0_g351;
				float ValueView242_g351 = ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g357 / fwidth( -max( ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g363 ) ) + min( max( break8_g363.x , break8_g363.y ) , 0.0 ) ) - temp_output_31_0_g363 ) + BorderWidth529_g351 ) , ( ( ( length( max( float2( 0,0 ) , temp_output_10_0_g362 ) ) + min( max( break8_g362.x , break8_g362.y ) , 0.0 ) ) - temp_output_31_0_g362 ) + BorderWidth529_g351 ) ) ) ) ) : step( 0.0 , temp_output_3_0_g357 ) );
				float ValueSDF241_g351 = temp_output_86_0_g351;
				float temp_output_2_0_g396 = ValueSDF241_g351;
				float4 lerpResult673_g351 = lerp( ( InnerBorderWidth250_g351 > 0.0 ? lerpResult674_g351 : ValueColorProcessed398_g351 ) , ( ( saturate( temp_output_4_0_g395 ) * ( 1.0 > 0.0 ? break4_g397.a : 1.0 ) ) * appendResult17_g397 ) , ( temp_output_4_0_g395 * min( ValueView242_g351 , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g396 : temp_output_2_0_g396 ) / max( temp_output_15_0_g395 , 0.0 ) ) ) , ( ( 1.0 - _ValueInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Value685_g351 = lerpResult673_g351;
				float4 lerpResult657_g351 = lerp( BorderColorProcessed497_g351 , zzLerp_Value685_g351 , ValueView242_g351);
				float temp_output_15_0_g410 = _BorderInsetShadowSize;
				float temp_output_4_0_g410 = saturate( ceil( temp_output_15_0_g410 ) );
				float4 break4_g412 = _BorderInsetShadowColor;
				float4 appendResult17_g412 = (float4(break4_g412.r , break4_g412.g , break4_g412.b , 1.0));
				float2 temp_output_20_0_g377 = SegmentUV521_g351;
				float2 break23_g377 = temp_output_20_0_g377;
				float Rounding13_g377 = SegmentRounding518_g351;
				float4 break27_g377 = ( Rounding13_g377 + SegmentRoundingOffset519_g351 );
				float2 appendResult25_g377 = (float2(break27_g377.x , break27_g377.w));
				float2 appendResult26_g377 = (float2(break27_g377.y , break27_g377.z));
				float2 break32_g377 = ( break23_g377.x > 0.0 ? appendResult25_g377 : appendResult26_g377 );
				float temp_output_31_0_g377 = ( break23_g377.y > 0.0 ? break32_g377.x : break32_g377.y );
				float2 temp_output_10_0_g377 = ( ( float2( 1,1 ) * temp_output_31_0_g377 ) + ( abs( temp_output_20_0_g377 ) - SegmentSize619_g351 ) );
				float2 break8_g377 = temp_output_10_0_g377;
				float temp_output_615_0_g351 = ( ( length( max( float2( 0,0 ) , temp_output_10_0_g377 ) ) + min( max( break8_g377.x , break8_g377.y ) , 0.0 ) ) - temp_output_31_0_g377 );
				float PB_SDF_Negated618_g351 = -temp_output_615_0_g351;
				float temp_output_654_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g393 = temp_output_654_0_g351;
				float temp_output_2_0_g411 = temp_output_654_0_g351;
				float4 lerpResult645_g351 = lerp( lerpResult657_g351 , ( ( saturate( temp_output_4_0_g410 ) * ( 1.0 > 0.0 ? break4_g412.a : 1.0 ) ) * appendResult17_g412 ) , ( temp_output_4_0_g410 * min( ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g393 / fwidth( temp_output_654_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g393 ) ) ) , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g411 : temp_output_2_0_g411 ) / max( temp_output_15_0_g410 , 0.0 ) ) ) , ( ( 1.0 - _BorderInsetShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border666_g351 = lerpResult645_g351;
				float4 break4_g384 = _BackgroundColor;
				float4 appendResult17_g384 = (float4(break4_g384.r , break4_g384.g , break4_g384.b , 1.0));
				float4 temp_output_743_0_g351 = ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g384.a : 1.0 ) ) * appendResult17_g384 );
				float2 temp_cast_5 = (saturate( ( Value574_g351 / segment_count527_g351 ) )).xx;
				float cos478_g351 = cos( radians( _BackgroundGradientRotation ) );
				float sin478_g351 = sin( radians( _BackgroundGradientRotation ) );
				float2 rotator478_g351 = mul( GradientUV479_g351 - float2( 0.5,0.5 ) , float2x2( cos478_g351 , -sin478_g351 , sin478_g351 , cos478_g351 )) + float2( 0.5,0.5 );
				float4 break4_g383 = tex2D( _BackgroundGradient, ( _ValueAsGradientTimeBackground > 0.0 ? temp_cast_5 : rotator478_g351 ) );
				float4 appendResult17_g383 = (float4(break4_g383.r , break4_g383.g , break4_g383.b , 1.0));
				float4 temp_output_403_0_g351 = ( _BackgroundGradientEnabled > 0.0 ? ( ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g383.a : 1.0 ) ) * appendResult17_g383 ) * temp_output_743_0_g351 ) : temp_output_743_0_g351 );
				float BG_Tex_Scale_w_Segments414_g351 = _BackgroundTextureScaleWithSegments;
				float2 BG_Tex_Tiling417_g351 = _BackgroundTextureTiling;
				float temp_output_453_0_g351 = ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? 1.0 : segment_count527_g351 );
				float temp_output_462_0_g351 = ( ( ( BG_Tex_Tiling417_g351.x * OSX553_g351 ) * temp_output_453_0_g351 ) / ( ( ( temp_output_453_0_g351 * OSX553_g351 ) + temp_output_444_0_g351 ) - temp_output_449_0_g351 ) );
				float temp_output_429_0_g351 = ( BG_Tex_Tiling417_g351.y / ( width_curve532_g351 - ( BorderWidth529_g351 * ( 2.0 / OSY552_g351 ) ) ) );
				float2 appendResult483_g351 = (float2(temp_output_462_0_g351 , temp_output_429_0_g351));
				float2 appendResult486_g351 = (float2(( -( ( temp_output_462_0_g351 - BG_Tex_Tiling417_g351.x ) / 2.0 ) + _BackgroundTextureOffset.x ) , ( _BackgroundTextureOffset.y + ( -( temp_output_429_0_g351 / 2.0 ) + 0.5 ) )));
				float2 temp_output_2_0_g374 = ( ( ( BG_Tex_Scale_w_Segments414_g351 > 0.0 ? ScaledTextureUV349_g351 : UnscaledTextureUV350_g351 ) * appendResult483_g351 ) + appendResult486_g351 );
				float cos472_g351 = cos( radians( _BackgroundTextureRotation ) );
				float sin472_g351 = sin( radians( _BackgroundTextureRotation ) );
				float2 rotator472_g351 = mul( temp_output_2_0_g374 - float2( 0.5,0.5 ) , float2x2( cos472_g351 , -sin472_g351 , sin472_g351 , cos472_g351 )) + float2( 0.5,0.5 );
				float2 break468_g351 = max( _BackgroundFlipbookDim , float2( 1,1 ) );
				float fbtotaltiles13_g373 = break468_g351.x * break468_g351.y;
				float fbcolsoffset13_g373 = 1.0f / break468_g351.x;
				float fbrowsoffset13_g373 = 1.0f / break468_g351.y;
				float fbspeed13_g373 = _TimeParameters.x * fps541_g351;
				float2 fbtiling13_g373 = float2(fbcolsoffset13_g373, fbrowsoffset13_g373);
				float fbcurrenttileindex13_g373 = round( fmod( fbspeed13_g373 + 0.0, fbtotaltiles13_g373) );
				fbcurrenttileindex13_g373 += ( fbcurrenttileindex13_g373 < 0) ? fbtotaltiles13_g373 : 0;
				float fblinearindextox13_g373 = round ( fmod ( fbcurrenttileindex13_g373, break468_g351.x ) );
				float fboffsetx13_g373 = fblinearindextox13_g373 * fbcolsoffset13_g373;
				float fblinearindextoy13_g373 = round( fmod( ( fbcurrenttileindex13_g373 - fblinearindextox13_g373 ) / break468_g351.x, break468_g351.y ) );
				fblinearindextoy13_g373 = (int)(break468_g351.y-1) - fblinearindextoy13_g373;
				float fboffsety13_g373 = fblinearindextoy13_g373 * fbrowsoffset13_g373;
				float2 fboffset13_g373 = float2(fboffsetx13_g373, fboffsety13_g373);
				half2 fbuv13_g373 = rotator472_g351 * fbtiling13_g373 + fboffset13_g373;
				float4 break4_g385 = tex2D( _BackgroundTexture, fbuv13_g373 );
				float4 appendResult17_g385 = (float4(break4_g385.r , break4_g385.g , break4_g385.b , 1.0));
				float4 lerpResult400_g351 = lerp( temp_output_403_0_g351 , ( temp_output_403_0_g351 * ( ( saturate( 1.0 ) * ( 1.0 > 0.0 ? break4_g385.a : 1.0 ) ) * appendResult17_g385 ) ) , saturate( _BackgroundTextureOpacity ));
				#ifdef BACKGROUND_TEXTURE_ON
				float4 staticSwitch494_g351 = lerpResult400_g351;
				#else
				float4 staticSwitch494_g351 = temp_output_743_0_g351;
				#endif
				float4 BackgroundColorProcessed495_g351 = staticSwitch494_g351;
				float temp_output_639_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g394 = temp_output_639_0_g351;
				float temp_output_638_0_g351 = ( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g394 / fwidth( temp_output_639_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g394 ) ) - ValueView242_g351 );
				float4 lerpResult636_g351 = lerp( zzLerp_Border666_g351 , BackgroundColorProcessed495_g351 , temp_output_638_0_g351);
				float temp_output_15_0_g405 = _ValueShadowSize;
				float temp_output_4_0_g405 = saturate( ceil( temp_output_15_0_g405 ) );
				float4 break4_g407 = _ValueShadowColor;
				float4 appendResult17_g407 = (float4(break4_g407.r , break4_g407.g , break4_g407.b , 1.0));
				float temp_output_2_0_g406 = ValueSDF241_g351;
				float4 lerpResult634_g351 = lerp( lerpResult636_g351 , ( ( saturate( temp_output_4_0_g405 ) * ( 1.0 > 0.0 ? break4_g407.a : 1.0 ) ) * appendResult17_g407 ) , ( temp_output_4_0_g405 * min( temp_output_638_0_g351 , ( 1.0 - pow( saturate( ( ( 1.0 > 0.0 ? -temp_output_2_0_g406 : temp_output_2_0_g406 ) / max( temp_output_15_0_g405 , 0.0 ) ) ) , ( ( 1.0 - _ValueShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Background642_g351 = lerpResult634_g351;
				float temp_output_15_0_g400 = _BorderShadowSize;
				float temp_output_4_0_g400 = saturate( ceil( temp_output_15_0_g400 ) );
				float4 break4_g402 = _BorderShadowColor;
				float4 appendResult17_g402 = (float4(break4_g402.r , break4_g402.g , break4_g402.b , 1.0));
				float temp_output_625_0_g351 = ( PB_SDF_Negated618_g351 - BorderWidth529_g351 );
				float temp_output_3_0_g392 = temp_output_625_0_g351;
				float temp_output_2_0_g401 = temp_output_625_0_g351;
				float4 lerpResult620_g351 = lerp( zzLerp_Background642_g351 , ( ( saturate( temp_output_4_0_g400 ) * ( 1.0 > 0.0 ? break4_g402.a : 1.0 ) ) * appendResult17_g402 ) , ( temp_output_4_0_g400 * min( ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g392 / fwidth( temp_output_625_0_g351 ) ) ) : step( 0.0 , temp_output_3_0_g392 ) ) , ( 1.0 - pow( saturate( ( ( 0.0 > 0.0 ? -temp_output_2_0_g401 : temp_output_2_0_g401 ) / max( temp_output_15_0_g400 , 0.0 ) ) ) , ( ( 1.0 - _BorderShadowFalloff ) * 20.0 ) ) ) ) ));
				float4 zzLerp_Border_Shadow629_g351 = lerpResult620_g351;
				float4 temp_output_608_0_g351 = ( OverlayColorProcessed524_g351 * zzLerp_Border_Shadow629_g351 );
				float PB_SDF616_g351 = temp_output_615_0_g351;
				float temp_output_3_0_g390 = PB_SDF616_g351;
				float temp_output_534_0_g351 = min( temp_output_608_0_g351.a , ( 1.0 - ( AA530_g351 > 0.0 ? saturate( ( temp_output_3_0_g390 / fwidth( PB_SDF616_g351 ) ) ) : step( 0.0 , temp_output_3_0_g390 ) ) ) );
				
				surfaceDescription.Alpha = temp_output_534_0_g351;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;


				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);
				outColor = _SelectionID;
			}

            ENDHLSL
        }

		Pass
		{
			Name "FullScreenDebug"
			Tags 
			{ 
				"LightMode" = "FullScreenDebug" 
			}

			Cull [_CullMode]
			ZTest LEqual
			ZWrite Off

			HLSLPROGRAM

			/*ase_pragma_before*/

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
	
			#define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			struct VaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
			{
				VaryingsMeshToPS output;
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

			PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
			{
				PackedVaryingsMeshToPS output;
				ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

			FragInputs BuildFragInputs(VaryingsMeshToPS input)
			{
				FragInputs output;
				ZERO_INITIALIZE(FragInputs, output);

				output.tangentToWorld = k_identity3x3;
				output.positionSS = input.positionCS;

				return output;
			}

			FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
			{
				UNITY_SETUP_INSTANCE_ID(input);
				VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
				return BuildFragInputs(unpacked);
			}

			#define DEBUG_DISPLAY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/FullScreenDebug.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

			PackedVaryingsType Vert(AttributesMesh inputMesh)
			{
				VaryingsType varyingsType;
				varyingsType.vmesh = VertMesh(inputMesh);
				return PackVaryingsType(varyingsType);
			}

			#if !defined(_DEPTHOFFSET_ON)
			[earlydepthstencil] // quad overshading debug mode writes to UAV
			#endif
			void Frag(PackedVaryingsToPS packedInput)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				FragInputs input = UnpackVaryingsToFragInputs(packedInput);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz);

			#ifdef PLATFORM_SUPPORTS_PRIMITIVE_ID_IN_PIXEL_SHADER
				if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_QUAD_OVERDRAW)
				{
					IncrementQuadOverdrawCounter(posInput.positionSS.xy, input.primitiveID);
				}
			#endif
			}

			ENDHLSL
		}
		
	}
	
	CustomEditor "Renge.PPB.ProceduralProgressBarGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19301
Node;AmplifyShaderEditor.FunctionNode;1043;316.9703,-60.89844;Inherit;True;The Whole Shebang;0;;351;2d6870fee17216f4db3628575a74016f;0;0;3;FLOAT3;0;FLOAT;728;FLOAT3;729
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1033;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1034;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;META;0;2;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1035;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1036;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DepthForwardOnly;0;4;DepthForwardOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=DepthForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1037;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;MotionVectors;0;5;MotionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefMV;255;False;;255;True;_StencilWriteMaskMV;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=MotionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1038;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DistortionVectors;0;6;DistortionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;4;1;False;;1;False;;4;1;False;;1;False;;True;1;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDistortionVec;255;False;;255;True;_StencilWriteMaskDistortionVec;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;False;True;1;LightMode=DistortionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1039;796.0289,-121.8474;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;15;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1032;631.0289,-59.8474;Float;False;True;-1;2;Renge.PPB.ProceduralProgressBarGUI;0;15;Renge/PPB_HDRP;7f5cb9c3ea6481f469fdd856555439ef;True;Forward Unlit;0;0;Forward Unlit;9;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;5;True;12;all;0;False;True;1;0;True;_SrcBlend;0;True;_DstBlend;1;0;True;_AlphaSrcBlend;0;True;_AlphaDstBlend;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullModeForward;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVel;False;False;False;False;False;True;True;0;True;_StencilRef;255;False;;255;True;_StencilWriteMask;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;0;True;_ZWrite;True;0;True;_ZTestDepthEqualForOpaque;False;True;1;LightMode=ForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;30;Surface Type;1;638353371163089803;  Rendering Pass ;0;0;  Rendering Pass;1;0;  Blending Mode;0;0;  Receive Fog;1;0;  Distortion;0;0;    Distortion Mode;0;0;    Distortion Only;1;0;  Depth Write;1;0;  Cull Mode;0;0;  Depth Test;4;0;Double-Sided;0;0;Alpha Clipping;0;0;Receive Decals;1;0;Motion Vectors;1;0;  Add Precomputed Velocity;0;0;Shadow Matte;0;0;Cast Shadows;1;0;GPU Instancing;0;638353371432164743;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;LOD CrossFade;0;0;0;8;True;True;True;True;True;True;False;True;False;;False;0
WireConnection;1032;0;1043;0
WireConnection;1032;2;1043;728
WireConnection;1032;6;1043;729
ASEEND*/
//CHKSM=D8A9F75795CCA2811C96984F698B5E3AF614F852