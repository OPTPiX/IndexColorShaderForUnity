/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_IndexColorShader_Palette : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;

	/* MEMO: Color.x=R, .y=G, .z=B, .w=Alpha */
	public UnityEngine.Vector4[] Color;
	#endregion Variables & Properties

	/* ----------------------------------------------- ScriptableObject-Functions */
	#region ScriptableObject-Functions
	#endregion ScriptableObject-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Color = null;
	}

	public void BootUp()
	{
		if(null == Color)
		{
			Color = new Vector4[CountColorDefault];
		}

		int countColor = Color.Length;
		for(int i=0; i<countColor; i++)
		{
			Color[i] = UnityEngine.Color.clear;
		}
	}

	public bool VersionCheckRuntime()
	{
		return(((KindVersion.SUPPORT_EARLIEST <= Version) && (KindVersion.SUPPORT_LATEST >= Version)));	/* ? true : false */
	}

	public int CountGetColor()
	{
		return((null == Color) ? -1 : Color.Length);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000,

		CODE_010000 = 0x00010000,
	}

	public const int CountColorDefault = 256; 
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates

}