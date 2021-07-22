/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

fixed4 PS_Main(InputPS In) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(In);

	/* Get Color-Index */
	float4 pixel;
	float4 indexColor;

	pixel = tex2D(_MainTex, In.Texture00UV00);
	indexColor.x = pixel.a;

	pixel = tex2D(_MainTex, In.Texture00UV01);
	indexColor.y = pixel.a;

	pixel = tex2D(_MainTex, In.Texture00UV02);
	indexColor.z = pixel.a;

	pixel = tex2D(_MainTex, In.Texture00UV03);
	indexColor.w = pixel.a;

	indexColor *= INDEX_MAX_LUT;
	indexColor = floor(indexColor);

	/* Convert to color */
	float4 pixelU = lerp(ConstantColorTable(indexColor.x), ConstantColorTable(indexColor.y), In.Property.x);
	float4 pixelD = lerp(ConstantColorTable(indexColor.z), ConstantColorTable(indexColor.w), In.Property.x);
	float4 color = lerp(pixelU, pixelD, In.Property.y);

	/* Considering Vertex-Color */
	color *= In.ColorMain;

	return(color);
}
