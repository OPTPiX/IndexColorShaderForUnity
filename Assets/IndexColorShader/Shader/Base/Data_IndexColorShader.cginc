/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

/* Shader Inputs */
struct InputVS
{
	float4 Vertex : POSITION;
	float2 Texture00UV : TEXCOORD0;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct InputPS
{
	float4 Position : SV_POSITION;
	float2 Texture00UV : TEXCOORD0;
	float4 Property : TEXCOORD7;
	float4 ColorMain : COLOR0;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

/* Texture Sampler */
sampler2D _MainTex;
float4 _MainTex_ST;
#if defined(ETC1_EXTERNAL_ALPHA)
sampler2D _AlphaTex;	/* not used. */
#endif

/* Constants */
#define SIZE_MAX_LUT (256)
#define INDEX_MAX_LUT (SIZE_MAX_LUT - 1)

/* Constant Buffer */
#if defined(UNITY_INSTANCING_ENABLED)
	/* Substance */
	UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _RendererColor)
		UNITY_DEFINE_INSTANCED_PROP(float4, _Flip)
		UNITY_DEFINE_INSTANCED_PROP(float, _EnableExternalAlpha)
		UNITY_DEFINE_INSTANCED_PROP(float4, _Setting)
	UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

	CBUFFER_START(IndexColorShader)
		float4 _ColorTable[SIZE_MAX_LUT];
	CBUFFER_END

	/* Accessor */
	#define ConstantColor UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Color)
	#define ConstantRendererColor UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _RendererColor)
	#define ConstantFlip UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Flip)
	#define ConstantEnableExternalAlpha UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _EnableExternalAlpha)
	#define ConstantSetting UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Setting)
	#define ConstantColorTable(idx) _ColorTable[idx]
#else
	/* Substance */
	fixed4 _Color;	/* Material Color. */
	fixed4 _RendererColor;
	float4 _Flip;
	float _EnableExternalAlpha;	/* not used. */
	float4 _Setting;
	float4 _ColorTable[SIZE_MAX_LUT];

	/* Accessor */
	#define ConstantColor _Color
	#define ConstantRendererColor _RendererColor
	#define ConstantFlip _Flip
	#define ConstantEnableExternalAlpha _EnableExternalAlpha
	#define ConstantSetting _Setting
	#define ConstantColorTable(idx) _ColorTable[idx]
#endif
