//
//	Shader : Indexed 256 LUT
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
Shader "Custom/IndexColorShader/UIPalette256"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Color("Tint", Color) = (1, 1, 1, 1)
		[Toggle(PIXELSNAP_ON)] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
		[HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			ZTest Always
			ZWRITE Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex VS_Main
			#pragma fragment PS_Main

			#pragma multi_compile _ PIXELSNAP_ON
//			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			#include "Base/Data_IndexColorShaderUI.cginc"
			#include "Base/Vertex_UIPalette256.cginc"
			#include "Base/Pixel_UIPalette256.cginc"
			ENDCG
		}
	}
}
