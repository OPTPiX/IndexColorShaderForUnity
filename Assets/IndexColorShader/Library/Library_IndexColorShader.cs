/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_IndexColorShader
{
	/* ----------------------------------------------- Signatures */
	#region Signatures
	public const string SignatureNameAsset = "IndexColorShader for Unity";
	public const string SignatureVersionAsset = "1.0.1";
	public const string SignatureNameDistributor = "CRI Middleware Co., Ltd.";
	#endregion Signatures

	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class CallBack
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		#endregion Classes, Structs & Interfaces

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}

	public static partial class Data
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public static partial class Palette
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public const int CountColorMax = 256;
			#endregion Enums & Constants

			/* ----------------------------------------------- Delegates */
			#region Delegates
			#endregion Delegates
		}

		public partial class ControlMaterialPalette
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			private MaterialPropertyBlock Property = null;
			private static int IDMainTexture = -1;
			private static int IDMainTextureST = -1;
			private static int IDColorTable = -1;
			private static int IDColor = -1;
			private static int IDSetting = -1;

			private Vector4 Setting;						/* .x=Texture granularity-X, .y=Texture granularity-Y, .z=KindInterpolation, .w=Opacity */
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			/* ******************************************************** */
			//! Activate this class.
			/*!
			@param	(none)

			@retval	(none)

			*/
			public void BootUp()
			{
				if(null == Property)
				{
					Property = new MaterialPropertyBlock();
				}

				if(0 > IDMainTexture)
				{
					IDMainTexture = UnityEngine.Shader.PropertyToID("_MainTex");
				}
				if(0 > IDMainTextureST)
				{
					IDMainTextureST = UnityEngine.Shader.PropertyToID("_MainTex_ST");
				}
				if(0 > IDColorTable)
				{
					IDColorTable = UnityEngine.Shader.PropertyToID("_ColorTable");
				}
				if(0 > IDColor)
				{
					IDColor = UnityEngine.Shader.PropertyToID("_Color");
				}
				if(0 > IDSetting)
				{
					IDSetting = UnityEngine.Shader.PropertyToID("_Setting");
				}

				Setting = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			}

			/* ******************************************************** */
			//! Terminate this class
			/*!
			@param	(none)

			@retval	(none)
			*/
			public void ShutDown()
			{
			}

			/* ******************************************************** */
			//! Update
			/*!
			@param	renderer
				Renderer to use for drawing.
			@param	tableColor
				Palette(Color-LUT)<br>
				Must be Vector4[256]. Elements are as .x=R,.y=G,.z=B,.w=Alpha.
			@param	colorDiffuse
				Vertex color of primitive drawing
			@param	interpolationTexture
				Kind of Texture-Filter<br>
				KindInterpolation.NONE == Nearest-Neighbor interpolation<br>
				KindInterpolation.LINEAR == (Bi)Linear interpolation
			@param	texture
				Texture for drawing (be assigned to renderer.sharedMaterial.mainTexture)<br>
				null == current renderer.sharedMaterial.mainTexture
			@param	textureTilingOffset
				"Tiling" and "Offset" to be set for the material<br>
				.x : Tiling.x (Scale.x)
				.y : Tiling.y (Scale.y)
				.z : Offset.x
				.w : Offset.y
				null == Texture's all range (Tiling=1,1 / Offset=0,0)

			@retval	(none)

			Set Material's parameter and transfer constants to Shader.
			*/
			public void Update(	Renderer renderer,
								Vector4[] tableColor,
								Color colorDiffuse,
								KindInterpolation interpolationTexture,
								Texture texture,
								Vector4? textureTilingOffset
							)
			{
				if(null == renderer)
				{
					return;
				}

				/* Set Palette */
				if(0 <= IDColorTable)
				{
					Property.SetVectorArray(IDColorTable, tableColor);
				}

				/* Set Texture */
				if(null == texture)
				{
					texture = renderer.sharedMaterial.mainTexture;
				}
				if(0 <= IDMainTexture)
				{
					Property.SetTexture(IDMainTexture, texture);
				}
				if(0 <= IDMainTextureST)
				{
					Vector4 tilingOffset = textureTilingOffset ?? TextureTilingOffsetDefault;
					Property.SetVector(IDMainTextureST, tilingOffset);
				}

				/* Set Mesh-Color */
				if(0 <= IDColor)
				{
					Property.SetColor(IDColor, colorDiffuse);
				}

				/* Set Setting */
				Setting.x = texture.width;
				Setting.y = texture.height;
				Setting.z = (float)interpolationTexture;
				Setting.w = 1.0f;
				if(0 <= IDSetting)
				{
					Property.SetVector(IDSetting, Setting);
				}

				/* Set current material's value */
				renderer.SetPropertyBlock(Property);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindInterpolation
			{
				NONE = 0,									/* Nearest Neighbor */
				LINEAR,										/* Bi-Linear */
			}

			public readonly static Vector4 TextureTilingOffsetDefault = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
			#endregion Enums & Constants

			/* ----------------------------------------------- Delegates */
			#region Delegates
			#endregion Delegates
		}
		#endregion Classes, Structs & Interfaces

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}

	public static partial class Palette
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public static Vector4[] ColorCopyDeep(Vector4[] paletteDestination, Vector4[] paletteSource)
		{
			Vector4[] destination = null;
			if(null == paletteDestination)
			{	/* Auto (Create destination) */
				destination = new Vector4[Data.Palette.CountColorMax];
				if(null == destination)
				{
					return(null);
				}
			} else {	/* Specified destination */
				destination = paletteDestination;
			}

			/* Copy colors */
			int countColor = paletteSource.Length;
			int countColorDestination = destination.Length;
			if(countColorDestination < countColor)
			{
				countColor = countColorDestination;
			}

			for(int i=0; i<countColor; i++)
			{
				destination[i] = paletteSource[i];
			}
			for(int i=countColor; i<countColorDestination; i++)
			{
				destination[i] = Color.clear;
			}

			return(destination);
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}

	public static partial class Shader
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
