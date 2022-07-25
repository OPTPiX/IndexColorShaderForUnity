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
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(Out);

	/* Transform Coordinates */
	float4 vertex = In.Vertex;
	float4 coordVertex = UnityObjectToClipPos(vertex);
	Out.PositionWorld = vertex;
	Out.Position = coordVertex;

	/* Calculate Clipping & Masking */
	float2 sizePixel = coordVertex.w;
	sizePixel /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

	float4 rectangleClamped = clamp(ConstantClipRect, -2e10, 2e10);
	float2 maskUV = (vertex.xy - rectangleClamped.xy) / (rectangleClamped.zw - rectangleClamped.xy);
	Out.MaskUV = float4(vertex.xy * 2.0 - rectangleClamped.xy - rectangleClamped.zw, 0.25 / (0.25 * float2(ConstantUIMaskSoftnessX, ConstantUIMaskSoftnessY) + abs(sizePixel.xy)));

	/* Transform Texture-UV */
	Out.Texture00UV = TRANSFORM_TEX(In.Texcoord, _MainTex);

	/* Vertex Color */
	Out.ColorMain = In.Color * ConstantColor;

	/* Properties */
	float4 property = float4(	ConstantSetting.x,	/* Texture Width */
								ConstantSetting.x,	/* Texture Height */
								1.0f,				/* Count Sampling */
								ConstantSetting.w	/* Opacity */
						);
	if(1.0f > ConstantSetting.z)
	{	/* Interpolation: None(Nearest Neighbor) */
		property.z = 1.0f;
	}
	else
	{	/* Interpolation: Bi-Linear */
		property.z = 4.0f;
	}
	Out.Property = property;

	return(Out);
}
