/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
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
	float4 property = float4(	1.0f,				/* Weight X */
								1.0f,				/* Weight Y */
								4.0f,				/* Count Sampling */
								ConstantSetting.w	/* Opacity */
						);

	coordTexture = coordTexture * _MainTex_ST.xy + _MainTex_ST.zw;	/* TRANSFORM_TEX *//* Transform Texture */

	if(1.0f <= ConstantSetting.z)
	{	/* Interpolation: Bi-Linear */
		float2 adjust = float2(0.5f, 0.5f);
		float2 rateSize = ConstantSetting.xy;

		coordTexture *= rateSize;
		coordTexture -= adjust;
		property.xy = frac(coordTexture);
		coordTexture += adjust;

		coordTextureShift = coordTexture;
		coordTextureShift += float2(1.0f, 1.0f);

		rateSize = float2(1.0f, 1.0f) / rateSize;
		coordTexture *= rateSize;
		coordTextureShift *= rateSize;
	}
	else
	{	/* Interpolation: None(Nearest Neighbor) */
		coordTextureShift = coordTexture;	/* Same */
		property.xy = float2(0.5f, 0.5f);	/* Average */
	}

	/* Vertex-Colors */
//	float4 colorMain = _RendererColor * _Color;
	float4 colorMain = ConstantColor;

	/* Output */
	Out.Position = coordinateVertes;
	Out.Texture00UV00 = coordTexture;	/* LU */
	Out.Texture00UV01 = float2(coordTextureShift.x, coordTexture.y);	/* RU */
	Out.Texture00UV02 = coordTextureShift;	/* RU */
	Out.Texture00UV03 = float2(coordTexture.x, coordTextureShift.y);	/* LD */
	Out.Property = property;
	UNITY_TRANSFER_INSTANCE_ID(In, Out);

	Out.ColorMain = colorMain;

	return(Out);
}
