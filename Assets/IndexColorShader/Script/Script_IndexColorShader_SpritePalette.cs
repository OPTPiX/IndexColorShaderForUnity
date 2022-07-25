/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Script_IndexColorShader_SpritePalette : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_IndexColorShader_Palette DataPalette;

	protected Library_IndexColorShader.Data.ControlMaterialPalette ControlMaterial = null;

	protected Renderer InstanceRenderer = null;
	protected SpriteRenderer InstanceRendererSprite = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions (MonoBehaviour) */
	#region Functions-MonoBehaviour
//	protected void Start()
//	{
//	}

	protected void Update()
	{
		if(null == DataPalette)
		{
			return;
		}

		/* ICS is Booted up ? */
		bool flagBootedUpNow;
		Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette = ControlGetMaterialPalette(out flagBootedUpNow);

		/* Update Shader-Constants */
		ConstantsUpdateShader(	controlMaterialPalette,
								Library_IndexColorShader.KindInterpolation.LINEAR,
								null,	/* DataPalette.Color */
								null
							);
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	public Library_IndexColorShader.Data.ControlMaterialPalette ControlGetMaterialPalette(out bool flagBootedUpNow)
	{
		flagBootedUpNow = false;

		if((null == InstanceRenderer) && (null == InstanceRendererSprite))
		{
			InstanceRendererSprite = GetComponent<SpriteRenderer>();
			if(null == InstanceRendererSprite)
			{
				InstanceRenderer = GetComponent<MeshRenderer>();
			}

			ControlMaterial = new Library_IndexColorShader.Data.ControlMaterialPalette();
			ControlMaterial.BootUp();

			flagBootedUpNow = true;
		}

		return(ControlMaterial);
	}

	public void ConstantsUpdateShader(	Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette,
										Library_IndexColorShader.KindInterpolation interpolation,
										Vector4[] tableColor,
										Vector4? textureTillingOffset
									)
	{
		if(null == DataPalette)
		{
			return;
		}
		if(null == controlMaterialPalette)
		{
			return;
		}

		Vector4[] dataColor = tableColor;
		if(null == dataColor)
		{
			dataColor = DataPalette.Color;
		}

#if UNITY_EDITOR
		/* MEMO: Check only on Unity-Editor, as be recompiled during running. */
		if(null == dataColor)
		{
			return;
		}

		if((null == InstanceRenderer) && (null == InstanceRendererSprite))
		{
			return;
		}
#endif

		Renderer renderer = null;
		Texture texture = null;
		Color color;
		if(null == InstanceRendererSprite)
		{	/* (Any)Renderer */
			if(null == InstanceRenderer.sharedMaterial)
			{
				return;
			}

			renderer = InstanceRenderer;
			color = InstanceRenderer.sharedMaterial.color;
			texture = InstanceRenderer.sharedMaterial.mainTexture;
		}
		else
		{	/* SpriteRenderer */
			renderer = InstanceRendererSprite;
			color = InstanceRendererSprite.color;
			texture = InstanceRendererSprite.sharedMaterial.mainTexture;
		}

		controlMaterialPalette.Update(	renderer,
										dataColor,
										color,
										interpolation,
										texture,
										textureTillingOffset
									);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public const int CountColorMax = 256;
	#endregion Enums & Constants

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
