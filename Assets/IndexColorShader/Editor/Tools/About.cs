/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using UnityEngine;
using UnityEditor;

public sealed class MenuItem_IndexColorShader_About : EditorWindow
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	[MenuItem("Tools/IndexColorShader/About")]
	static void About()
	{
		EditorUtility.DisplayDialog(	Library_IndexColorShader.SignatureNameAsset,
										Library_IndexColorShader.SignatureNameAsset
										+ "\n\n"
										+ "Version: " + Library_IndexColorShader.SignatureVersionAsset
										+ "\n\n"
										+ "Copyright(C) " + Library_IndexColorShader.SignatureNameDistributor,
										"OK"
									);
	}
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
