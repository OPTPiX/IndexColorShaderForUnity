/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
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
	public const string SignatureVersionAsset = "1.1.0";
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
	public enum KindInterpolation
	{
		NONE = 0,									/* Nearest Neighbor */
		LINEAR,										/* Bi-Linear */
	}
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
			private readonly static int IDMainTexture =  UnityEngine.Shader.PropertyToID("_MainTex");
			private readonly static int IDMainTextureST = UnityEngine.Shader.PropertyToID("_MainTex_ST");
			private readonly static int IDColorTable = UnityEngine.Shader.PropertyToID("_ColorTable");
			private readonly static int IDColor = UnityEngine.Shader.PropertyToID("_Color");
			private readonly static int IDSetting = UnityEngine.Shader.PropertyToID("_Setting");

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
#if false
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
#endif

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
			//! Update (for Renderer)
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
								Library_IndexColorShader.KindInterpolation interpolationTexture,
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

			/* ******************************************************** */
			//! Update (for Material)
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
			public static void Update(	Material instanceMaterial,
										Vector4[] tableColor,
										Color colorDiffuse,
										Library_IndexColorShader.KindInterpolation interpolationTexture,
										Texture texture,
										Vector4? textureTilingOffset
									)
			{
				if(null == instanceMaterial)
				{
					return;
				}

				/* Set Palette */
				if(0 <= IDColorTable)
				{
					instanceMaterial.SetVectorArray(IDColorTable, tableColor);
				}

				/* Set Texture */
				if(null == texture)
				{
					texture = instanceMaterial.mainTexture;
				}
				if(0 <= IDMainTexture)
				{
					instanceMaterial.SetTexture(IDMainTexture, texture);
				}
				if(0 <= IDMainTextureST)
				{
					Vector4 tilingOffset = textureTilingOffset ?? TextureTilingOffsetDefault;
					instanceMaterial.SetVector(IDMainTextureST, tilingOffset);
				}

				/* Set Mesh-Color */
				if(0 <= IDColor)
				{
					instanceMaterial.SetColor(IDColor, colorDiffuse);
				}

				/* Set Setting */
				if(0 <= IDSetting)
				{
					Vector4 setting = new Vector4(64.0f, 64.0f, (float)interpolationTexture, 1.0f);
					if(null != texture)
					{
						setting.x = texture.width;
						setting.y = texture.height;
					}
					instanceMaterial.SetVector(IDSetting, setting);
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public readonly static Vector4 TextureTilingOffsetDefault = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);

			/* MEMO: Old definition, but has been retained for backward compatibility.   */
			/*       The new definition is "Library_IndexColorShader.KindInterpolation". */
			public enum KindInterpolation
			{
				NONE = Library_IndexColorShader.KindInterpolation.NONE,
				LINEAR = Library_IndexColorShader.KindInterpolation.LINEAR,
			}
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
	#endregion Classes, Structs & Interfaces

	public static partial class Control
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		public static bool ManagerBootUpMaterial(int capacity)
		{
			if(null == ManagerMaterial)
			{
				ManagerMaterial = new CacheMaterialStatic();
				if(null == ManagerMaterial)
				{
					return(false);
				}
				if(false == ManagerMaterial.BootUp(capacity))
				{
					return(false);
				}
			}

			return(true);
		}
		#endregion Functions

		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		/* Material Manager */
		public static Library_IndexColorShader.Control.CacheMaterialStatic ManagerMaterial = null;
		#endregion Variables & Properties

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public class CacheMaterialStatic
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public List<InformationData> Data;

			public bool StatusIsBootedUp
			{
				get
				{
					return(null != Data);	/*  ? true : false*/
				}
			}
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public bool BootUp(int capacity)
			{
				if(0 >= capacity)
				{
					capacity = CapacityMaterialDefault;
				}

				Data = new List<InformationData>(capacity);
				Data.Clear();

				return(true);
			}

			public void ShutDown(bool flagDestroyInstance)
			{
				DataPurge(flagDestroyInstance);

				Data = null;
			}

			public void DataPurge(bool flagDestroyInstance)
			{
				if(false == StatusIsBootedUp)
				{
					return;
				}

				int countData = Data.Count;
				for(int i=(countData-1); i>=0; i--)
				{
					DataRelease(i, flagDestroyInstance);
				}
				Data.Clear();
			}

			public void DataAppend(long codeHash, UnityEngine.Material instanecMaterial)
			{
				InformationData informationData = new InformationData(codeHash, instanecMaterial);
				informationData.Count++;
				Data.Add(informationData);
			}

			public void DataRelease(int index, bool flagDestroyInstance=false)
			{
				if(true == flagDestroyInstance)
				{
					UnityEngine.Material material = Data[index].Instance;
					if(null != material)
					{
						Utility.Asset.ObjectDestroy(material);
					}
				}

				Data.RemoveAt(index);
			}

			public int IndexGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						if(null != Data[i].Instance)
						{	/* Valid Instance */
							return(i);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(-1);
			}

			public int IndexGet(UnityEngine.Material instanceMaterial)
			{
				if(null == instanceMaterial)
				{
					return(-1);
				}

				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].Instance == instanceMaterial)
					{
						return(i);
					}
				}

				return(-1);
			}

			public UnityEngine.Material MaterialGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						UnityEngine.Material instanceMaterial = Data[i].Instance;
						if(null != instanceMaterial)
						{	/* Valid Instance */
							return(instanceMaterial);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(null);
			}
			public UnityEngine.Material MaterialGet(	UnityEngine.Texture texture,
														UnityEngine.Shader shader,
														UnityEngine.Rendering.CompareFunction functionCompareStenctil,
														int idStencil,
														bool flagCreateNew
												)
			{
				/* Get shader's hash-code */
				int hashShader = 0;
				if(null == shader)
				{
					return(null);
				}
				hashShader = shader.GetHashCode();

				/* Get texture's hash-code */
				int hashTexture = 0;
				if(null != texture)
				{
					hashTexture = texture.GetHashCode();
				}

				/* Get material's hash-code */
				long codeHash = InformationData.CodeGet(hashShader, hashTexture, functionCompareStenctil, idStencil);
				UnityEngine.Material instanceMaterial = null;
				int indexCacheMaterial = IndexGet(codeHash);
				if(0 > indexCacheMaterial)
				{	/* Not exist */
					if(true == flagCreateNew)
					{
						/* Create new material */
						instanceMaterial = new UnityEngine.Material(shader);
						instanceMaterial.mainTexture = texture;

						/* Append new material */
						DataAppend(codeHash, instanceMaterial);
					}
				}
				else
				{	/* Exist */
					/* Increment reference-count. */
					InformationData dataCache = Data[indexCacheMaterial];
					dataCache.Count++;
					Data[indexCacheMaterial] = dataCache;

					instanceMaterial = dataCache.Instance;
				}

				return(instanceMaterial);
			}

			public void MaterialRelease(UnityEngine.Material instanceMaterial)
			{
				int indexCacheMaterial = IndexGet(instanceMaterial);
				if(0 > indexCacheMaterial)
				{	/* Not exist */
					return;
				}

				/* Decrement reference-count. */
				InformationData dataCache = Data[indexCacheMaterial];
				dataCache.Count--;
				if(0 < dataCache.Count)
				{	/* Still referenced */
					Data[indexCacheMaterial] = dataCache;
				}
				else
				{	/* No referenced */
					DataRelease(indexCacheMaterial, true);
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const int CapacityMaterialDefault = 100;

			public const string NameShaderPrefix = "Custom/IndexColorShader/";
			public static UnityEngine.Shader ShaderDefaultSprite = UnityEngine.Shader.Find(NameShaderPrefix + "Palette256");
			public static UnityEngine.Shader ShaderDefaultUIImage = UnityEngine.Shader.Find(NameShaderPrefix + "UIPalette256");
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public struct InformationData
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal long CodeHash;
				internal int Count;
				internal Material Instance;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal InformationData(long codeHash, UnityEngine.Material instance)
				{
					CodeHash = codeHash;
					Count = 0;
					Instance = instance;
				}

				internal static long CodeGet(int hashShader, int hashTexture, UnityEngine.Rendering.CompareFunction functionCompare, int idStencil)
				{
					long hashShaderLong = (long)hashShader & MaskCodeName;
					long hashCombined = hashShaderLong;
					hashCombined <<= 5;
					hashCombined |= (hashCombined >> 32);
					hashCombined += hashShaderLong;
					hashCombined ^= (long)hashTexture;
					hashCombined &= MaskCodeName;

					long code = (long)hashCombined & MaskCodeName;
					code |= ((long)idStencil & MaskCodeFunctionStencil) << CountShiftCodeFunctionStencil;
					code |= ((long)functionCompare & MaskCodeIDStencil) << CountShiftCodeIDStencil;

					return(code);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				internal const long MaskCodeName = 0x00000000ffffffffL;
				internal const long MaskCodeFunctionStencil = 0x00000000000000ffL;
				internal const long MaskCodeIDStencil = 0x000000000000000fL;

//				internal const int CountShiftCodeName = 0;
				internal const int CountShiftCodeFunctionStencil = 32;
				internal const int CountShiftCodeIDStencil = 40;
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Utility
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class Asset
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static GameObject GameObjectCreate(string name, bool flagActive, GameObject gameObjectParent)
			{
				GameObject gameObject = new GameObject(name);
				if(null != gameObject)
				{
					gameObject.SetActive(flagActive);
					Transform transform = gameObject.transform;
					if(null != gameObjectParent)
					{
						transform.parent = gameObjectParent.transform;
					}
					transform.localPosition = Vector3.zero;
					transform.localEulerAngles = Vector3.zero;
					transform.localScale = Vector3.one;
				}
				return(gameObject);
			}

			public static void ObjectDestroy(UnityEngine.Object instanceObject)
			{
				if(null != instanceObject)
				{
#if UNITY_EDITOR
					if(false == UnityEditor.EditorApplication.isPlaying)
					{
						UnityEngine.Object.DestroyImmediate(instanceObject);
					}
					else
					{
						UnityEngine.Object.Destroy(instanceObject);
					}
#else
					UnityEngine.Object.Destroy(instanceObject);
#endif
				}
			}
			#endregion Functions
		}
		#endregion Classes, Structs & Interfaces
	}
	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
