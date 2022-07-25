/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Script_ColorChange_Robo : Script_IndexColorShader_SpritePalette
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	protected Vector4[] Palette = null;
	public Vector4[] DataPaletteOverride;
	public float RateBlendOverride;
	public Library_IndexColorShader.Data.ControlMaterialPalette.KindInterpolation Interpolation;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
	//	void Start()
	//	{
	//	}

	new void Update()
	{
		if(null == DataPalette)
		{
			return;
		}

		/* ICS is Booted up ? */
		bool flagBootedUpNow;
		Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette = ControlGetMaterialPalette(out flagBootedUpNow);
		if((true == flagBootedUpNow) || (null == Palette))
		{	/* Just after booted-up */
			if(null == DataPalette.Color)
			{
				return;
			}

			/* Duplicate Color-Table */
			Palette = Library_IndexColorShader.Palette.ColorCopyDeep(null, DataPalette.Color);
		}
#if true
		/* Calculate Colors */
		if(null != DataPaletteOverride)
		{
			if(0 < DataPaletteOverride.Length)
			{
				int countColor = Palette.Length;
				Vector4 colorSource;
				Vector4 colorOverride;
				for(int i=0; i<countColor; i++)
				{
					colorSource = DataPalette.Color[i];
					colorOverride = DataPaletteOverride[i];
					if(Vector4.zero != colorOverride)
					{
						Palette[i] = (colorSource * (1.0f - RateBlendOverride)) + (colorOverride * RateBlendOverride);
					}
				}
			}
		}

		/* Update Shader-Constants */
		ConstantsUpdateShader(	controlMaterialPalette,
								Interpolation,
								Palette,
								null
							);
#else
		/* Update Shader-Constants */
		ConstantsUpdateShader(	controlMaterialPalette,
								Interpolation,
								DataPalette.Color,
								null
							);
#endif
	}
	#endregion Functions-MonoBehaviour

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
