/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

fixed4 PS_Main(InputPS In) : SV_Target
{
	float2 coordTexel = In.Texture00UV;

	/* When "Nearest Neighbor" (No-Interpolation) */
	if(1.0f >= In.Property.z)
	{
		float indexColor = tex2D(_MainTex, coordTexel).a;	/* LU */
		indexColor *= INDEX_MAX_LUT;

		float4 pixel = ConstantColorTable(indexColor);
		pixel *= In.ColorMain;

		return(pixel);
	}

	/* Process UV */
	float2 sizeTexture = float2(In.Property.x, In.Property.y);
	float2 rateTexelTexture = float2(1.0f, 1.0f) / sizeTexture;
#if 1
	coordTexel *= sizeTexture;
	float2 rateInterpolation = frac(coordTexel);
	coordTexel = floor(coordTexel) * rateTexelTexture;
#else
	float2 rateInterpolation = frac(coordTexel * sizeTexture);
#endif

	/* Get Color-Index */
	float4 indexColor;
	{
		float2 coordTexelRD = coordTexel;
		coordTexelRD.x += rateTexelTexture.x;
		coordTexelRD.y -= rateTexelTexture.y;

		indexColor.x = tex2D(_MainTex, coordTexel).a;	/* LU */
		indexColor.y = tex2D(_MainTex, float2(coordTexelRD.x, coordTexel.y)).a;	/* RU */
		indexColor.z = tex2D(_MainTex, float2(coordTexel.x, coordTexelRD.y)).a;	/* LD */
		indexColor.w = tex2D(_MainTex, coordTexelRD).a;	/* RD */
	}
	indexColor *= INDEX_MAX_LUT;
	indexColor = floor(indexColor);

	/* Convert to color */
#if 0
	float4 pixelU = lerp(ConstantColorTable(indexColor.x), ConstantColorTable(indexColor.y), rateInterpolation.x);
	float4 pixelD = lerp(ConstantColorTable(indexColor.z), ConstantColorTable(indexColor.w), rateInterpolation.x);
	float4 color = lerp(pixelU, pixelD, rateInterpolation.y);
#else
	float4 pixelU = lerp(ConstantColorTable(indexColor.x), ConstantColorTable(indexColor.y), rateInterpolation.x);
	float4 pixelD = lerp(ConstantColorTable(indexColor.z), ConstantColorTable(indexColor.w), rateInterpolation.x);
	float4 color = lerp(pixelU, pixelD, 1.0f - rateInterpolation.y);
#endif

	/* Considering Vertex-Color */
	color += ConstantTextureSampleAdd;
	color *= In.ColorMain;

	/* Masking & Clipping */
#ifdef UNITY_UI_CLIP_RECT
	half2 mask = saturate((ConstantClipRect.zw - ConstantClipRect.xy - abs(In.MaskUV.xy)) * In.MaskUV.zw);
	color.a *= mask.x * mask.y;
#endif

#ifdef UNITY_UI_ALPHACLIP
	clip(color.a - 0.001);
#endif

	return(color);
}
