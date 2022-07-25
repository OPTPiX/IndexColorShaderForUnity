/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

InputPS VS_Main(InputVS In)
{
	InputPS Out;

	UNITY_SETUP_INSTANCE_ID(In);

	/* Transform Vertex-Coordinate */
	float4 coordinateVertes = UnityObjectToClipPos(In.Vertex);
#if defined(PIXELSNAP_ON)
	coordinateVertes = UnityPixelSnap(coordinateVertes);
#else
#endif

	/* Texture's UV */
	float2 coordTexture = In.Texture00UV;
	float2 coordTextureShift;
	float4 property = float4(	ConstantSetting.x,	/* Texture Width */
								ConstantSetting.x,	/* Texture Height */
								1.0f,				/* Count Sampling */
								ConstantSetting.w	/* Opacity */
						);

	/* Transform UV */
	coordTexture = coordTexture * _MainTex_ST.xy + _MainTex_ST.zw;	/* TRANSFORM_TEX *//* Transform Texture */

	if(1.0f > ConstantSetting.z)
	{	/* Interpolation: None(Nearest Neighbor) */
		property.z = 1.0f;
	}
	else
	{	/* Interpolation: Bi-Linear */
		property.z = 4.0f;
	}

	/* Vertex-Colors */
//	float4 colorMain = _RendererColor * _Color;
	float4 colorMain = ConstantColor;

	/* Output */
	Out.Position = coordinateVertes;
	Out.Texture00UV = coordTexture;
	Out.Property = property;
	UNITY_TRANSFER_INSTANCE_ID(In, Out);

	Out.ColorMain = colorMain;

	return(Out);
}
