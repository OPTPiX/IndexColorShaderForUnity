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

[CustomEditor(typeof(Script_IndexColorShader_Sprite))]
public class Inspector_IndexColorShader_Sprite : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* WorkArea (for Inspector) */
	private Script_IndexColorShader_Sprite InstanceObject;

	/* WorkArea (for Preview) */
	private UnityEngine.Material InstanceMaterial = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceObject = (Script_IndexColorShader_Sprite)target;

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
			if(null == InstanceObject.MaterialMaster)
			{
				return;
			}
			InstanceMaterial = new Material(InstanceObject.MaterialMaster);
		}
		Vector4[] dataLUT = InstanceObject.LUT;
		if(null == dataLUT)
		{
			return;
		}

		/* Update Material */
		UnityEngine.Texture texture = InstanceObject.Texture;
		UnityEngine.Color color = (null != InstanceObject.SpriteRenderer) ? InstanceObject.SpriteRenderer.color : UnityEngine.Color.white;
		Library_IndexColorShader.Data.ControlMaterialPalette.Update(	InstanceMaterial,
																		dataLUT,
																		color,
																		InstanceObject.Interpolation,
																		texture,
																		null
																);

		/* Draw */
		base.OnPreviewGUI(rect, background);
		Graphics.DrawTexture(rect, texture, InstanceMaterial);
	}
	#endregion Functions-forPreview

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
//	private readonly static GUIContent TitlePreview = new GUIContent("Preview [Script_IndexColorShader_Sprite]");
	#endregion Enums & Constants
}
