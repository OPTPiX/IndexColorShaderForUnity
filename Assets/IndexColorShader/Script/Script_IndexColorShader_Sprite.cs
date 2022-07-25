/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

// #define COMPILEOPTION_INHERIT_SPRITEPALETTE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
#if COMPILEOPTION_INHERIT_SPRITEPALETTE
public class Script_IndexColorShader_Sprite : Script_IndexColorShader_SpritePalette
#else
public class Script_IndexColorShader_Sprite : MonoBehaviour
#endif
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* Settings */
#if COMPILEOPTION_INHERIT_SPRITEPALETTE
#else
	public Script_IndexColorShader_Palette DataPalette;
#endif
	public UnityEngine.Material MaterialMaster;
	public Library_IndexColorShader.KindInterpolation Interpolation;
	public UnityEngine.Texture Texture;
	public Rect RectDraw;
	public Vector2 RatePivot;
	public float PixelsPerUnit;

	/* WorkArea */
#if COMPILEOPTION_INHERIT_SPRITEPALETTE
#else
	protected Library_IndexColorShader.Data.ControlMaterialPalette ControlMaterial = null;

	protected SpriteRenderer InstanceRendererSprite = null;
#endif
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return(InstanceRendererSprite);
		}
	}
	protected Sprite InstanceSprite = null;
	protected Material InstanceMaterial = null;

	protected UnityEngine.Texture TexturePrevious = null;
	protected Rect RectDrawPrevious = new Rect(float.NaN, float.NaN, float.NaN, float.NaN);
//	protected Vector2 PivotPrevious = new Vector2(float.NaN, float.NaN);

	protected UnityEngine.Vector4[] InstanceDataColor = null;
	public UnityEngine.Vector4[] LUT
	{
		get
		{
			return(InstanceDataColor);
		}
		set
		{
			InstanceDataColor = value;
		}
	}
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions (MonoBehaviour) */
	#region Functions-MonoBehaviour
//	protected void Start()
//	{
//	}

#if COMPILEOPTION_INHERIT_SPRITEPALETTE
	protected new void Update()
#else
	protected void Update()
#endif
	{
		/* ICS is Booted up ? */
//		if(0 == (Status & FlagBitStatus.VALID))
		{	/* Not Valid */
			bool flagBootedUpNow;
			if(false == BootUp(out flagBootedUpNow))
			{	/* Error */
				return;
			}
		}

		/* Check Update */
		if(TexturePrevious != Texture)
		{	/* Change Texture */
			if(false == TextureUpdate())
			{
				return;
			}
		}
//		if((RectDrawPrevious != RectDraw) || (PivotPrevious != RatePivot))
		if(RectDrawPrevious != RectDraw)
		{	/* Change RectDraw */
			if(false == VertexUpdateMesh())
			{
				return;
			}
		}

		/* Update Shader-Constants */
		ConstantsUpdateShader(ControlMaterial, Interpolation, null);
	}

	protected void OnDestroy()
	{
		if(null != InstanceMaterial)
		{
			Library_IndexColorShader.Control.CacheMaterialStatic managerMaterial = Library_IndexColorShader.Control.ManagerMaterial;
			if(null == managerMaterial)
			{
				Library_IndexColorShader.Utility.Asset.ObjectDestroy(InstanceMaterial);
			}
			else
			{
				managerMaterial.MaterialRelease(InstanceMaterial);
			}
		}
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	protected bool BootUp(out bool flagBootedUpNow)
	{
		flagBootedUpNow = false;

		if(false == Library_IndexColorShader.Control.ManagerBootUpMaterial(-1))
		{
			return(false);
		}

		if(null == InstanceDataColor)
		{
			if(false == LUTSet(null))
			{
				return(false);
			}
		}

		if(null == InstanceRendererSprite)
		{
			InstanceRendererSprite = GetComponent<SpriteRenderer>();
			if(null == InstanceRendererSprite)
			{
				return(false);
			}
			flagBootedUpNow = true;
		}

		if(null == ControlMaterial)
		{
			ControlMaterial = new Library_IndexColorShader.Data.ControlMaterialPalette();
			ControlMaterial.BootUp();

			flagBootedUpNow = true;
		}

		if(null == InstanceSprite)
		{
			if(false == ParameterNormalizeSprite())
			{
				return(false);
			}

			Texture2D texture = Texture as Texture2D;
			if(null == texture)
			{
				return(false);
			}

#if false
			/* MEMO: When created to set "SpriteMeshType.FullRect", overhead is small, but the size cannot be changed. */
//			InstanceSprite = UnityEngine.Sprite.Create(texture, RectDraw, RatePivot, PixelsPerUnit, 0, SpriteMeshType.FullRect);
			InstanceSprite = UnityEngine.Sprite.Create(texture, RectDraw, RatePivot);
#else
			/* MEMO: Texture must be made at maximum size.                      */
			/*       If initial size is small, cannot enlarge size any further. */
			Rect rectDraw = new Rect(0.0f, 0.0f, texture.width, texture.height);
			InstanceSprite = UnityEngine.Sprite.Create(texture, rectDraw, RatePivot);
#endif
		}
		if(null == InstanceMaterial)
		{
			InstanceMaterial = Library_IndexColorShader.Control.ManagerMaterial.MaterialGet(	MaterialMaster.mainTexture,
																								MaterialMaster.shader,
																								UnityEngine.Rendering.CompareFunction.Disabled,
																								0,
																								true
																						);
			TextureUpdate();

			InstanceRendererSprite.material = InstanceMaterial;
		}

		return(true);
	}
	protected bool ParameterNormalizeSprite()
	{
		/* Texture */
		if(null == Texture)
		{
			return(false);
		}

		/* Pixels per Unit */
		if(0.0f >= PixelsPerUnit)
		{
			PixelsPerUnit = 100.0f;
		}

		/* RectDraw */
		float sizeTextureX = Texture.width;
		float sizeTextureY = Texture.height;
		if((0.0f > RectDraw.x) || (sizeTextureX <= RectDraw.x))
		{
			RectDraw.x = 0.0f;
		}
		if((0.0f > RectDraw.y) || (sizeTextureY <= RectDraw.y))
		{
			RectDraw.y = 0.0f;
		}
		if(0.0f > RectDraw.width)
		{
			RectDraw.width = sizeTextureX - RectDraw.x;
		}
		if(0.0f > RectDraw.height)
		{
			RectDraw.height = sizeTextureY - RectDraw.y;
		}
		if(sizeTextureX < (RectDraw.x + RectDraw.width))
		{
			RectDraw.width = sizeTextureX - RectDraw.x;
		}
		if(sizeTextureY < (RectDraw.y + RectDraw.height))
		{
			RectDraw.height = sizeTextureY - RectDraw.y;
		}

		/* Pivot */
		RatePivot.x = Mathf.Clamp01(RatePivot.x);
		RatePivot.y = Mathf.Clamp01(RatePivot.y);

		return(true);
	}

#if COMPILEOPTION_INHERIT_SPRITEPALETTE
#else
	public void ConstantsUpdateShader(	Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette,
										Library_IndexColorShader.KindInterpolation interpolation,
										Vector4? textureTillingOffset
									)
	{
		if(null == controlMaterialPalette)
		{
			return;
		}

		Vector4[] dataColor = InstanceDataColor;
		if(null == dataColor)
		{
			if(false == LUTSet(null))
			{
				return;
			}
		}

		/* MEMO: Check only on Unity-Editor, as be recompiled during running. */
		if(null == dataColor)
		{
			return;
		}
		if(0 >= dataColor.Length)
		{
			return;
		}

		if(null == InstanceRendererSprite)
		{
			return;
		}
		InstanceRendererSprite.material = InstanceMaterial;

		/* Set Shader constants */
		controlMaterialPalette.Update(	InstanceRendererSprite,
										dataColor,
										InstanceRendererSprite.color,
										interpolation,
										null,
										textureTillingOffset
									);
	}
#endif

	protected bool TextureUpdate()
	{
		if(null == InstanceMaterial)
		{
			return(false);
		}

		InstanceMaterial.mainTexture = Texture;
		TexturePrevious = Texture;

		return(true);
	}
	protected bool VertexUpdateMesh()
	{
		if(false == ParameterNormalizeSprite())
		{	/* Error */
			return(false);
		}

		RectDrawPrevious = RectDraw;
//		PivotPrevious = RatePivot;

		InstanceRendererSprite.sprite = null;	/* Clear */
		Vector2[] vertexSprite = InstanceSprite.vertices;
		Vector2 vertexLU = new Vector2(RectDraw.x, RectDraw.y);
		Vector2 vertexRD = new Vector2(RectDraw.width, RectDraw.height);
		vertexSprite[0] = vertexLU;
		vertexSprite[1] = vertexRD;
		vertexSprite[2].x = vertexRD.x;		vertexSprite[2].y = vertexLU.y;
		vertexSprite[3].x = vertexLU.x;		vertexSprite[3].y = vertexRD.y;
		InstanceSprite.OverrideGeometry(vertexSprite, InstanceSprite.triangles);

		InstanceRendererSprite.sprite = InstanceSprite;

		return(true);
	}

	/* ******************************************************** */
	//! Set external-LUT(Palette)
	/*!
	@param	tableColor
		external LUT<br>
		null == Set originally-set palette
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set LUT externally.
	LUT must be an array of 256 Vector4(Color). (Vector4[256])
	*/
	public bool LUTSet(UnityEngine.Vector4[] tableColor)
	{
		if(null == tableColor)
		{
			if(null == DataPalette)
			{
				return(false);
			}

			tableColor = DataPalette.Color;
		}

		InstanceDataColor = tableColor;

		return(true);
	}

	/* ******************************************************** */
	//! Get LUT(Palette)
	/*!
	@param	flagInUse
		true == Get currently in use<br>
		false == Get originally-set
	@param	flagCreateInstance
		true == Create new data instance (array)<br>
		false == Get source-data itself
		Default: false
	@param	flagCreateClean
		*) Valid only when "flagCreateInstance == true".<br>
		true == Make LUT completely-transparent (all zero).<br>
		false == Copy source data.<br>
		Default: false
	@retval	Return-Value
		LUT (Vector4[256])<br>
		null == Failure (Error)

	Get LUT (Palette) data.<br>
	When "flagCreateInstance == false" will directly modify the contents, note that modifying returned Vector4-array.<br>
	To reflect the returned LUT, use function "LUTSet".
	*/
	public Vector4[] LUTGet(bool flagInUse, bool flagCreateInstance=false, bool flagCreateClean=false)
	{
		Vector4[] tableColorSource = null;
		if(false == flagInUse)
		{
			if(null == DataPalette)
			{
				return(null);
			}
			tableColorSource = DataPalette.Color;
		}
		else
		{
			if(null == InstanceDataColor)
			{
				return(null);
			}
			tableColorSource = InstanceDataColor;
		}
		if(null == tableColorSource)
		{
			return(null);
		}

		if(false == flagCreateInstance)
		{
			return(tableColorSource);
		}

		int countColor = tableColorSource.Length;
		Vector4[] tableColor = new Vector4[countColor];
		if(false == flagCreateClean)
		{	/* Copy LUT */
			for(int i=0; i<countColor; i++)
			{
				tableColor[i] = tableColorSource[i];
			}
		}
		else
		{	/* Clear */
			for(int i=0; i<countColor; i++)
			{
				tableColor[i] = Color.clear;
			}
		}

		return(tableColor);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
#if COMPILEOPTION_INHERIT_SPRITEPALETTE
#else
	public const int CountColorMax = 256;
#endif
	#endregion Enums & Constants

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
