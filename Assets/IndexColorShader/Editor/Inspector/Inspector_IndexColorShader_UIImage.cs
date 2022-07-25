/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Script_IndexColorShader_UIImage))]
public class Inspector_IndexColorShader_UIImage : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* WorkArea (for Inspector) */
	private Script_IndexColorShader_UIImage InstanceObject;

	/* WorkArea (for Preview) */
	private UnityEngine.Material InstanceMaterial = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceObject = (Script_IndexColorShader_UIImage)target;

//		serializedObject.FindProperty("__DUMMY__");
	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//	}

//	private void OnDisable()
//	{
//	}
	#endregion Functions

	/* ----------------------------------------------- Functions-forPreview */
	#region Functions-forPreview
	public override bool HasPreviewGUI()
	{
		return(true);
	}

//	public override GUIContent GetPreviewTitle()
//	{
//		return(TitlePreview);
//	}

	public override bool RequiresConstantRepaint()
	{
		return(false);
	}

//	public override void OnPreviewSettings()
//	{
//	}

	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		if(null == InstanceObject)
		{
			return;
		}
		if(null == InstanceObject.DataPalette)
		{
			return;
		}

		if(null == InstanceMaterial)
		{
			if(null == InstanceObject.material)
			{
				return;
			}
			InstanceMaterial = new Material(InstanceObject.material);
		}
		Vector4[] dataLUT = InstanceObject.LUT;
		if(null == dataLUT)
		{
			return;
		}

		/* Update Material */
		Rect uvRect = InstanceObject.uvRect;
		UnityEngine.Texture texture = InstanceObject.mainTexture;
		Library_IndexColorShader.Data.ControlMaterialPalette.Update(	InstanceMaterial,
																		dataLUT,
																		InstanceObject.color,
																		InstanceObject.Interpolation,
																		texture,
																		new Vector4(uvRect.width, uvRect.height, uvRect.x, uvRect.y)
																);

		/* Draw */
		base.OnPreviewGUI(rect, background);
		Graphics.DrawTexture(rect, texture, uvRect, 0, 0, 0, 0, Color.white, InstanceMaterial);
	}
	#endregion Functions-forPreview

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	//	private readonly static GUIContent TitlePreview = new GUIContent("Preview [Script_IndexColorShader_UIImage]");
	#endregion Enums & Constants
}
