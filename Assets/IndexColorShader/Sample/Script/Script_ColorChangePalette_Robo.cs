/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class Script_ColorChangePalette_Robo : Script_ColorChange_Robo
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_IndexColorShader_Palette[] TablePalette;
	public float RateTime;

	private float TimeElapsed = 0.0f;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
	//	void Start()
	//	{
	//	}

	new void Update()
	{
		if(null == TablePalette)
		{
			return;
		}

		int countPalette = TablePalette.Length;
		if(0 >= countPalette)
		{
			return;
		}

		/* ICS is Booted up ? */
		bool flagBootedUpNow;
		Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette = ControlGetMaterialPalette(out flagBootedUpNow);
		if(true == flagBootedUpNow)
		{	/* Just after booted-up */
			TimeElapsed = RangeTimeStart;
		}
		else
		{	/* Running */
			TimeElapsed += Time.deltaTime * RateTime;
		}

		/* Decide palette to use */
		while(RangeTime <= TimeElapsed)
		{
			TimeElapsed -= RangeTime;
		}
		while(RangeTimeStart > TimeElapsed)
		{
			TimeElapsed += RangeTime;
		}
		int indexPalette = (int)(TimeElapsed * (float)countPalette);
		DataPalette = TablePalette[indexPalette];

		/* Update Shader-Constants */
		ConstantsUpdateShader(	controlMaterialPalette,
								Interpolation,
								TablePalette[indexPalette].Color,
								null
							);
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private const float RangeTime = 1.0f;
	private const float RangeTimeStart = 0.0f;
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
