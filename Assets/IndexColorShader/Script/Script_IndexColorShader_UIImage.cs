/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

// #define USE_MATRIXTRANSFORM_COORDINATE
// #define USE_MATRIXTRANSFORM_TEXTURE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
[ExecuteInEditMode]
[System.Serializable]
public partial class Script_IndexColorShader_UIImage : UnityEngine.UI.MaskableGraphic
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* Settings */
	public override UnityEngine.Texture mainTexture
	{
		get
		{
			if(null == material)
			{
				return(null);
			}

			return(material.mainTexture);
		}
	}

	public Script_IndexColorShader_Palette DataPalette;
	public Library_IndexColorShader.KindInterpolation Interpolation;

	public bool FlagHideForce;
	public Rect uvRect;												/* for UI.RawImage conpativility */
	public Vector2 SizeImageForce;									/* Vector2.zero: RectTransform's width,height / -Vector2.one: Texture size */
 
	public UnityEngine.Rendering.CompareFunction StencilCompare;	/* Disable: Auto */
	public int StencilID;											/* -1: Auto / Lower-8bits Valid */

	/* WorkArea */
	protected UnityEngine.CanvasRenderer InstanceRendererCanvas = null;
	public UnityEngine.CanvasRenderer CanvasRenderer
	{
		get
		{
			return(InstanceRendererCanvas);
		}
	}

	protected UnityEngine.Material InstanceMaterialInUse = null;

	protected bool MaskablePrevious = false;
	protected UnityEngine.Rendering.CompareFunction StencilComparePrevious = UnityEngine.Rendering.CompareFunction.Disabled;
	protected int StencilIDPrevious = -1;

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

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

//	void Start()
//	{
//	}

	void Update()
	{
		/* MEMO: To be called "OnPopulateMesh" each loop, keep constantly dirty. */
		SetVerticesDirty();
	}

//	void LateUpdate()
//	{
//	}

	protected override void OnDestroy()
	{
		if(null != InstanceMaterialInUse)
		{
			Library_IndexColorShader.Utility.Asset.ObjectDestroy(InstanceMaterialInUse);
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Override-Functions */
	#region Override-Functions
	protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vertexHelper)
	{
		Texture texture = mainTexture;
		if(null == texture)
		{	/* Material is missing */
			return;
		}
		if(null == InstanceDataColor)
		{	/* Palette is missing */
			if(false == LUTSet(null))
			{
				return;
			}
		}

		/* Renderer Initiallize */
		if(null == InstanceRendererCanvas)
		{	/* Not yet get  */
			InstanceRendererCanvas = gameObject.GetComponent<CanvasRenderer>();
			if(null == InstanceRendererCanvas)
			{	/* Failure (Not exist renderer) */
				return;
			}
		}

		/* Calculate Transform-Matrix */
		Matrix4x4 matrixTransform = Matrix4x4.identity;	/* Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale); */

		/* Draw mesh */
		vertexHelper.Clear();
		if(false == FlagHideForce)
		{
			/* ICS is Booted up ? */
			if(null == InstanceRendererCanvas)
			{
				InstanceRendererCanvas = GetComponent<CanvasRenderer>();
				if(null == InstanceRendererCanvas)
				{
					return;
				}
			}

			/* Update Shader-Constants */
			ConstantsUpdateShader(	Interpolation,
									InstanceDataColor,
									new Vector4(uvRect.width, uvRect.height, uvRect.x, uvRect.y)
								);

			int indexVertex = 0;
			indexVertex = MeshPopulate(vertexHelper, indexVertex);
			if(0 > indexVertex)
			{	/* Error */
				vertexHelper.Clear();

				return;
			}
		}
	}

	protected int MeshPopulate(UnityEngine.UI.VertexHelper vertexHelper, int indexVertex)
	{
		Texture texture = mainTexture;
		if(null == texture)
		{
			return(-1);
		}

		/* Solve sprite-size */
		Vector3 sizeTexture = SizeImageForce;
		if((0.0f > sizeTexture.x) || (0.0f > sizeTexture.y))
		{	/* Minus : (mainTexture.width, mainTexture.height) */
			sizeTexture = new Vector2(texture.width, texture.height);

			/* MEMO: Write it back for convenience on UnityEditor. */
			SizeImageForce = sizeTexture;
		}
		else
		{
			if((0.0f >= sizeTexture.x) || (0.0f >= sizeTexture.y))
			{	/* Zero : (RectTransform.width, RectTransform.height) */
				sizeTexture = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);

				/* MEMO: Write both back to zero (to avoid failure). */
				SizeImageForce = Vector2.zero;
			}
		}

		Vector3 sizeSprite = sizeTexture;
		Vector3 coordinateSprite = sizeSprite * -0.5f;	/* -(sizeSprite / 2.0f) */

		/* Create vertex-data */
		UIVertex dataVertex = UIVertex.simpleVert;
		Vector3 coordinate  = coordinateSprite;
		Vector3 uv = Vector3.zero;

		{	/* LU */
//			coordinate.x = coordinateSprite.x;
//			coordinate.y = coordinateSprite.y;
			dataVertex.position = coordinate;

			dataVertex.color = color;

//			uv.x = 0.0f;
//			uv.y = 0.0f;
			dataVertex.uv0 = uv;
		}
		vertexHelper.AddVert(dataVertex);

		{	/* RU */
			coordinate.x = coordinateSprite.x + sizeSprite.x;
//			coordinate.y = coordinateSprite.y;
			dataVertex.position = coordinate;

//			dataVertex.color = color;

			uv.x = 1.0f;
//			uv.y = 0.0f;
			dataVertex.uv0 = uv;
		}
		vertexHelper.AddVert(dataVertex);

		{	/* RD */
//			coordinate.x = coordinateSprite.x + sizeSprite.x;
			coordinate.y = coordinateSprite.y + sizeSprite.y;
			dataVertex.position = coordinate;

//			dataVertex.color = color;

//			uv.x = 1.0f;
			uv.y = 1.0f;
			dataVertex.uv0 = uv;
		}
		vertexHelper.AddVert(dataVertex);

		{	/* LD */
			coordinate.x = coordinateSprite.x;
//			coordinate.y = coordinateSprite.y + sizeSprite.y;
			dataVertex.position = coordinate;

//			dataVertex.color = color;

			uv.x = 0.0f;
//			uv.y = 1.0f;
			dataVertex.uv0 = uv;
		}
		vertexHelper.AddVert(dataVertex);

		/* Set vertex-indices (triangles) */
		vertexHelper.AddTriangle(indexVertex + 0, indexVertex + 1, indexVertex + 2);
		vertexHelper.AddTriangle(indexVertex + 2, indexVertex + 3, indexVertex + 0);
		indexVertex += 4;

		return(indexVertex);
	}

	public void ConstantsUpdateShader(	Library_IndexColorShader.KindInterpolation interpolation,
										Vector4[] tableColor,
										Vector4? textureTillingOffset
									)
	{
		if(null == tableColor)
		{
			return;
		}
		if(0 >= tableColor.Length)
		{
			return;
		}

		if(null == InstanceMaterialInUse)
		{
			return;
		}

		if(null != InstanceRendererCanvas)
		{
			Library_IndexColorShader.Data.ControlMaterialPalette.Update(	InstanceMaterialInUse,
																			tableColor,
																			color,
																			interpolation,
																			null,	/* InstanceMaterialInUse.mainTexture, */
																			textureTillingOffset
																		);
		}
	}

	public override UnityEngine.Material GetModifiedMaterial(UnityEngine.Material materialBase)
	{
		/* MEMO: MaterialPropertyBlock is not available for UI.                               */
		/*       Therefore, force creation of a new material without using "MaterialManager". */
		if(null == InstanceMaterialInUse)
		{
			if(null == material)
			{
				return(null);
			}

			InstanceMaterialInUse = new UnityEngine.Material(material);
			if(null == InstanceMaterialInUse)
			{
				return(null);
			}
		}

		/* Set Changing parameters */
		bool flagModifyMaterial = false;
		if(MaskablePrevious != maskable)
		{	/* State changed */
			flagModifyMaterial = true;
		}
		else
		{
			if(true == maskable)
			{
				if(	(StencilComparePrevious != StencilCompare)
					|| (StencilIDPrevious != StencilID)
				)
				{	/* Stencil-parameter changed */
					flagModifyMaterial = true;
				}
			}
		}
		if(false == flagModifyMaterial)
		{	/* No changed */
			return(InstanceMaterialInUse);
		}

		/* Determine parameters */
		UnityEngine.Rendering.CompareFunction stencilCompare;
		int stencilID;

		MaskablePrevious = maskable;
		StencilComparePrevious = StencilCompare;
		StencilIDPrevious = StencilID;

		if(false == maskable)
		{	/* UnMaskable */
			stencilCompare = UnityEngine.Rendering.CompareFunction.Always;
			stencilID = 0;
		}
		else
		{	/* Maskable */
			stencilCompare = StencilComparePrevious;
			stencilID = StencilIDPrevious;
			
			if(UnityEngine.Rendering.CompareFunction.Disabled == stencilCompare)
			{	/* Default */
				stencilCompare = UnityEngine.Rendering.CompareFunction.Equal;
			}
			if(0 > stencilID)
			{	/* Auto ID */
				UnityEngine.Transform canvasRoot = UnityEngine.UI.MaskUtilities.FindRootSortOverrideCanvas(transform);
				stencilID = UnityEngine.UI.MaskUtilities.GetStencilDepth(transform, canvasRoot);
			}
		}

		return(InstanceMaterialInUse);
	}
	#endregion Override-Functions

	/* ----------------------------------------------- Functions */
	#region Functions

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
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces
}
