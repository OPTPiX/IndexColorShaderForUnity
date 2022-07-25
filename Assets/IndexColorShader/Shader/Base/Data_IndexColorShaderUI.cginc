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
	float4 Color : COLOR;
	float2 Texcoord : TEXCOORD0;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct InputPS
{
	float4 Position : SV_POSITION;
	fixed4 ColorMain : COLOR;
	float2 Texture00UV : TEXCOORD0;
	float4 PositionWorld : TEXCOORD1;
	float4 MaskUV : TEXCOORD2;
	float4 Property : TEXCOORD7;

	UNITY_VERTEX_OUTPUT_STEREO
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
		UNITY_DEFINE_INSTANCED_PROP(float, _EnableExternalAlpha)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _TextureSampleAdd)
		UNITY_DEFINE_INSTANCED_PROP(float4, _ClipRect)
		UNITY_DEFINE_INSTANCED_PROP(float, _UIMaskSoftnessX)
		UNITY_DEFINE_INSTANCED_PROP(float, _UIMaskSoftnessY)
		UNITY_DEFINE_INSTANCED_PROP(float4, _Setting)
	UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

	CBUFFER_START(IndexColorShader)
		float4 _ColorTable[SIZE_MAX_LUT];
	CBUFFER_END

	/* Accessor */
	#define ConstantColor UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Color)
	#define ConstantTextureSampleAdd UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _TextureSampleAdd)
	#define ConstantClipRect UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _ClipRect)
	#define ConstantUIMaskSoftnessX UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _UIMaskSoftnessX)
	#define ConstantUIMaskSoftnessY UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _UIMaskSoftnessY)
	#define ConstantSetting UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Setting)
	#define ConstantColorTable(idx) _ColorTable[idx]
#else
	/* Substance */
	float _EnableExternalAlpha;
	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;
	float _UIMaskSoftnessX;
	float _UIMaskSoftnessY;
	float4 _Setting;
	float4 _ColorTable[SIZE_MAX_LUT];

	/* Accessor */
	#define ConstantColor _Color
	#define ConstantTextureSampleAdd _TextureSampleAdd
	#define ConstantClipRect _ClipRect
	#define ConstantUIMaskSoftnessX _UIMaskSoftnessX
	#define ConstantUIMaskSoftnessY _UIMaskSoftnessY
	#define ConstantSetting _Setting
	#define ConstantColorTable(idx) _ColorTable[idx]
#endif
