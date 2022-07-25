/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static partial class LibraryEditor_IndexColorShader
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private static void LogError(string messagePrefix, string message)
	{
		LibraryEditor_IndexColorShader.Utility.Log.Error(messagePrefix + ": " + message);
	}

	private static void LogWarning(string messagePrefix, string message)
	{
		LibraryEditor_IndexColorShader.Utility.Log.Warning(messagePrefix + ": " + message);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class Import
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public static bool Exec(	ref Setting setting,
									string nameInputFullPathImage,
									string nameOutputAssetFolderBase,
									bool flagDisplayProgressBar = true
								)
		{
			const string messageLogPrefix = "Main";
			const string titleProgressBar = Library_IndexColorShader.SignatureNameAsset;

			string nameDirectory = "";
			string nameFileBody = "";
			string nameFileExtension = "";
			nameInputFullPathImage = Utility.File.PathNormalize(nameInputFullPathImage);
			string messageLogSuffix = " [" + nameInputFullPathImage + "]";

			const int stepLoad = 2;			/* Load / Decode */
			const int stepConvert = 1;		/* Convert(Palette, Pixel) */
			const int stepOutputBasic = 3;	/* Output-Basic(Palette, Texture, Material) */
			const int stepOutputSprite = 1;	/* Output-Sprite(Prefab) */
			const int stepOutputUI = 2;		/* Output-UI(Material, Prefab) */
			int stepFull =	stepLoad
							+ stepConvert
							+ stepOutputBasic
							+ stepOutputSprite
							+ stepOutputUI;
			int countStep = 0;

//			Utility.Log.Message("Importing Start" + messageLogSuffix, true, false);	/* External-File only, no indent */

			/* Load Image */
			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Loading Image", flagDisplayProgressBar, countStep, stepFull);
			if(false == System.IO.File.Exists(nameInputFullPathImage))
			{	/* Not Found */
				LogError(messageLogPrefix, "File Not Found" + messageLogSuffix);
				goto Exec_ErrorEnd;
			}

			Utility.File.PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, nameInputFullPathImage);
			byte[] dataTexture = System.IO.File.ReadAllBytes(nameInputFullPathImage);
			if(null == dataTexture)
			{
				LogError(messageLogPrefix, "Failed to read file" + messageLogSuffix);
				goto Exec_ErrorEnd;
			}
			countStep++;

			/* Decode Image */
			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Deocoding Image", flagDisplayProgressBar, countStep, stepFull);
			Image.InformationDecode informationDecode = Image.Decode(dataTexture, messageLogSuffix);
			if(null == informationDecode)
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			/* Convert */
			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Converting Palette", flagDisplayProgressBar, countStep, stepFull);
			Color[] palette = Image.PaletteConvert(informationDecode, messageLogSuffix);
			if(null == palette)
			{
				informationDecode.Release();
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Converting Pixel", flagDisplayProgressBar, countStep, stepFull);
			Texture2D textureIndexed = Image.PixelConvert(informationDecode, messageLogSuffix);
			if(null == textureIndexed)
			{
				informationDecode.Release();
				goto Exec_ErrorEnd;
			}
			countStep++;

			/* MEMO: Image has been converted, no-need decoding-information anymore. */
			informationDecode.Release();

			/* Output Assets */
			bool flagIgnoreConfirmOverwrite = false;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Pallete asset", flagDisplayProgressBar, countStep, stepFull);
			Script_IndexColorShader_Palette assetDataPalette;
			if(false == AssetOutputPalette(out assetDataPalette, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, palette))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Texture asset", flagDisplayProgressBar, countStep, stepFull);
			Texture2D assetTextureIndexed;
			if(false == AssetOutputTextureIndexed(out assetTextureIndexed, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, textureIndexed))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Material asset", flagDisplayProgressBar, countStep, stepFull);
			Material assetMaterial;
			if(false == AssetOutputMaterial(out assetMaterial, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, assetTextureIndexed))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Prefab-Sprite asset", flagDisplayProgressBar, countStep, stepFull);
			GameObject assetPrefabSprite;
			if(false == AssetOutputPrefabSprite(out assetPrefabSprite, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, assetMaterial, assetDataPalette))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Material-UI asset", flagDisplayProgressBar, countStep, stepFull);
			Material assetMaterialUI;
			if(false == AssetOutputMaterialUI(out assetMaterialUI, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, assetTextureIndexed))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, "Output Prefab-UI asset", flagDisplayProgressBar, countStep, stepFull);
			GameObject assetPrefabUI;
			if(false == AssetOutputPrefabUI(out assetPrefabUI, ref flagIgnoreConfirmOverwrite, ref setting, nameFileBody, nameOutputAssetFolderBase, assetMaterialUI, assetDataPalette))
			{
				goto Exec_ErrorEnd;
			}
			countStep++;

			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, string.Empty, flagDisplayProgressBar, -1, -1);
//			Utility.Log.Message("Importing Complete" + messageLogSuffix, true, false);	/* External-File only, no indent */

			return(true);

		Exec_ErrorEnd:;
			Utility.Miscellaneous.ProgressBarUpdate(titleProgressBar, string.Empty, flagDisplayProgressBar, -1, -1);
//			Utility.Log.Message("Importing Failed" + messageLogSuffix, true, false);	/* External-File only, no indent */

			return(false);
		}

		internal static bool AssetOutputPalette(	out Script_IndexColorShader_Palette assetDataPalette,
													ref bool flagIgnoreConfirmOverwrite,
													ref Setting setting,
													string nameAsset,
													string nameOutputAssetFolderBase,
													Color[] palette
											)
		{
			assetDataPalette = null;

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderPalette,
														setting.RuleNameAsset.NameGetAssetPalette(nameAsset),
														ExtentionScriptableObject
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetDataPalette = AssetGetPalette(	out flagEnableUpdate,
												ref flagIgnoreConfirmOverwrite,
												ref setting,
												pathAssetFull
											);
			if(null == assetDataPalette)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				/* Write palette */
				if(null != palette)
				{
					/* Clear & Set palettes */
					int countPalette = assetDataPalette.Color.Length;
					for(int i=0; i<countPalette; i++)
					{
						assetDataPalette.Color[i] = Color.clear;
					}

					if(palette.Length < countPalette)
					{
						countPalette = palette.Length;
					}
					for(int i=0; i<countPalette; i++)
					{
						assetDataPalette.Color[i] = palette[i];
//						assetDataPalette.Color[i] = new Vector4(palette[i].r, palette[i].g, palette[i].b, palette[i].a);
					}
				}

				/* Reset Data-Version */
				/* MEMO: For now, Overwrite to the latest version. */
				assetDataPalette.Version = Script_IndexColorShader_Palette.KindVersion.SUPPORT_LATEST;

				/* Dirty data */
				EditorUtility.SetDirty(assetDataPalette);
				AssetDatabase.SaveAssets();
			}

			return(true);
		}
		private static Script_IndexColorShader_Palette AssetGetPalette(	out bool flagEnableUpdate,
																		ref bool flagIgnoreConfirmOverwrite,
																		ref Setting setting,
																		string pathAsset
																	)
		{
			const string messageLogPrefix = "Asset-Create (Data-Palette)";
			flagEnableUpdate = false;

			/* Check existing */
			Script_IndexColorShader_Palette assetDataPalette = AssetDatabase.LoadAssetAtPath<Script_IndexColorShader_Palette>(pathAsset);
			if(null == assetDataPalette)
			{	/* Not Exist (Create New) */
				/* Creagte Asset-Folder */
				string nameAssetFolder = "";
				string nameAssetFileBody = "";
				string nameAssetFileExtension = "";

				Utility.File.PathSplit(out nameAssetFolder, out nameAssetFileBody, out nameAssetFileExtension, pathAsset);
				if(true == string.IsNullOrEmpty(Utility.File.AssetFolderCreate(nameAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameAssetFolder + "\" could not be created");
					return(null);
				}

				/* Create Asset */
				assetDataPalette = ScriptableObject.CreateInstance<Script_IndexColorShader_Palette>();
				AssetDatabase.CreateAsset(assetDataPalette, pathAsset);

				/* Boot up Data */
				assetDataPalette.BootUp();
			}
			else
			{	/* Exist (Overwrite) */
				/* Check overwrite */
				if(false == Utility.File.PermissionGetConfirmDialogueOverwrite(	ref flagIgnoreConfirmOverwrite,
																				setting.ConfirmOverWrite.FlagPalette,
																				pathAsset,
																				"Palette"
																			)
				)
				{	/* Not overwrite (Cancel) */
					/* MEMO: No message */
//					flagEnableUpdate = false;

					return(assetDataPalette);
				}
			}

			flagEnableUpdate = true;

			return(assetDataPalette);
		}

		internal static bool AssetOutputTextureIndexed(	out Texture2D assetTexture,
														ref bool flagIgnoreConfirmOverwrite,
														ref Setting setting,
														string nameAsset,
														string nameOutputAssetFolderBase,
														Texture2D textureIndexed
													)
		{
			assetTexture = null;

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderTexture,
														setting.RuleNameAsset.NameGetAssetTexture(nameAsset),
														ExtentionTextureIndexed
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetTexture = AssetGetTextureImporter(	out flagEnableUpdate,
													ref flagIgnoreConfirmOverwrite,
													ref setting,
													pathAssetFull,
													textureIndexed
												);
			if(null == assetTexture)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				/* Dirty data */
				EditorUtility.SetDirty(assetTexture);
				AssetDatabase.SaveAssets();
			}

			return(assetTexture);
		}
		private static Texture2D AssetGetTextureImporter(	out bool flagEnableUpdate,
															ref bool flagIgnoreConfirmOverwrite,
															ref Setting setting,
															string pathAsset,
															Texture2D textureIndexed
													)
		{
			const string messageLogPrefix = "Asset-Create (Texture-Encode)";
			flagEnableUpdate = false;

			bool optionIsSRGB = false;
			bool optionIsReadable = false;
			bool optionMipmapEnabled = false;
			int optionMaxTextureSize = 2048;
			TextureImporterAlphaSource optionAlphaSource = TextureImporterAlphaSource.FromInput;
			bool optionAlphaIsTransparency = false;
			TextureImporterNPOTScale optionNpotScale = TextureImporterNPOTScale.None;
			TextureImporterType optionTextureType = TextureImporterType.Sprite;
			SpriteImportMode optionSpriteImportMode = SpriteImportMode.Multiple;

			/* Check existing */
			TextureImporter importer = TextureImporter.GetAtPath(pathAsset) as TextureImporter;
			if(null == importer)
			{	/* Not Exist (Create New) */
				/* Creagte Asset-Folder */
				string nameAssetFolder = "";
				string nameAssetFileBody = "";
				string nameAssetFileExtension = "";

				Utility.File.PathSplit(out nameAssetFolder, out nameAssetFileBody, out nameAssetFileExtension, pathAsset);
				if(true == string.IsNullOrEmpty(Utility.File.AssetFolderCreate(nameAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameAssetFolder + "\" could not be created");
					return(null);
				}
			}
			else
			{	/* Exist (Overwrite) */
				/* Check overwrite */
				if(false == Utility.File.PermissionGetConfirmDialogueOverwrite(	ref flagIgnoreConfirmOverwrite,
																				setting.ConfirmOverWrite.FlagTexture,
																				pathAsset,
																				"Texture"
																			)
				)
				{	/* Not overwrite (Cancel) */
					/* MEMO: No message */
//					flagEnableUpdate = false;

					return(Utility.Asset.AssetLoadPath<Texture2D>(pathAsset));
				}

				/* Back up importer's setting */
				optionIsSRGB = importer.sRGBTexture;
				optionIsReadable = importer.isReadable;
				optionMipmapEnabled = importer.mipmapEnabled;
				optionMaxTextureSize = importer.maxTextureSize;
				optionAlphaSource = importer.alphaSource;
				optionAlphaIsTransparency = importer.alphaIsTransparency;
				optionNpotScale = importer.npotScale;
				optionTextureType = importer.textureType;
				optionSpriteImportMode = importer.spriteImportMode;
			}

			flagEnableUpdate = true;

			/* Texture output */
			string pathAssetNative = Utility.File.PathGetAssetNative(pathAsset);
			byte[] dataTexture = textureIndexed.EncodeToPNG();
			if(null == dataTexture)
			{
				LogError(messageLogPrefix, "Texturer \"" + pathAsset + "\" could not encode");
				return(null);
			}
			System.IO.File.WriteAllBytes(pathAssetNative, dataTexture);
			/* MEMO: After storing the Texture-File, cannot be gotten TextureImporter unless import & save once. */
		    AssetDatabase.ImportAsset(pathAsset, ImportAssetOptions.ForceUpdate);
			AssetDatabase.SaveAssets();

			/* Re-get TextureImporter */
			importer = TextureImporter.GetAtPath(pathAsset) as TextureImporter;

			/* Overwrite importer's setting */
			importer.textureShape = TextureImporterShape.Texture2D;
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.filterMode = FilterMode.Point;
			importer.isReadable = optionIsReadable;
			importer.mipmapEnabled = optionMipmapEnabled;
			importer.maxTextureSize = optionMaxTextureSize;
			importer.npotScale = optionNpotScale;
			importer.textureType = optionTextureType;
			importer.spriteImportMode = optionSpriteImportMode;
			importer.sRGBTexture = optionIsSRGB;
			importer.alphaIsTransparency = false;	// optionAlphaIsTransparency;
			importer.alphaSource = optionAlphaSource;

//		    AssetDatabase.ImportAsset(pathAsset, ImportAssetOptions.ForceUpdate);
			EditorUtility.SetDirty(importer);
			importer.SaveAndReimport();
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			/* Get asset */
			Texture2D assetTexture = Utility.Asset.AssetLoadPath<Texture2D>(pathAsset);

			return(assetTexture);
		}

		internal static bool AssetOutputMaterial(	out Material assetMaterial,
													ref bool flagIgnoreConfirmOverwrite,
													ref Setting setting,
													string nameAsset,
													string nameOutputAssetFolderBase,
													Texture2D textureIndexed
											)
		{
			assetMaterial = null;

			if(false == setting.Basic.FlagCreateMaterial)
			{
				return(true);
			}

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderMaterial,
														setting.RuleNameAsset.NameGetAssetMaterial(nameAsset),
														ExtentionMaterial
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetMaterial = AssetGetMaterial(	out flagEnableUpdate,
												ref flagIgnoreConfirmOverwrite,
												pathAssetFull,
												setting.ConfirmOverWrite.FlagMaterial,
												setting.Basic.MaterialSource,
												"Material"
										);
			if(null == assetMaterial)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				/* Set Texture */
				assetMaterial.mainTexture = textureIndexed;

				/* Dirty data */
				EditorUtility.SetDirty(assetMaterial);
				AssetDatabase.SaveAssets();
			}

			return(true);
		}
		private static Material AssetGetMaterial(	out bool flagEnableUpdate,
													ref bool flagIgnoreConfirmOverwrite,
													string pathAsset,
													bool flagConfirmOverwrite,
													Material assetSource,
													string textKindMaterial
												)
		{
			string messageLogPrefix = "Asset-Create " + textKindMaterial;
			flagEnableUpdate = false;

			/* Check existing */
			Material assetMaterial = AssetDatabase.LoadAssetAtPath<Material>(pathAsset);
			if(null == assetMaterial)
			{	/* Not Exist (Create New) */
				/* Creagte Asset-Folder */
				string nameAssetFolder = "";
				string nameAssetFileBody = "";
				string nameAssetFileExtension = "";

				Utility.File.PathSplit(out nameAssetFolder, out nameAssetFileBody, out nameAssetFileExtension, pathAsset);
				if(true == string.IsNullOrEmpty(Utility.File.AssetFolderCreate(nameAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameAssetFolder + "\" could not be created");
					return(null);
				}

				/* Create Asset */
				assetMaterial = new Material(assetSource);
				AssetDatabase.CreateAsset(assetMaterial, pathAsset);
			}
			else
			{	/* Exist (Overwrite) */
				/* Check overwrite */
				if(false == Utility.File.PermissionGetConfirmDialogueOverwrite(	ref flagIgnoreConfirmOverwrite,
																				flagConfirmOverwrite,
																				pathAsset,
																				textKindMaterial
																			)
				)
				{	/* Not overwrite (Cancel) */
					/* MEMO: No message */
//					flagEnableUpdate = true;

					return(assetMaterial);
				}
			}

			flagEnableUpdate = true;

			return(assetMaterial);
		}

		internal static bool AssetOutputPrefabSprite(	out GameObject assetPrefabSprite,
														ref bool flagIgnoreConfirmOverwrite,
														ref Setting setting,
														string nameAsset,
														string nameOutputAssetFolderBase,
														Material material,
														Script_IndexColorShader_Palette dataPalette
												)
		{
			assetPrefabSprite = null;

			if(false == setting.Basic.FlagCreatePrefabSprite)
			{
				return(true);
			}

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderPrefabSprite,
														setting.RuleNameAsset.NameGetAssetPrefabSprite(nameAsset),
														ExtentionPrefab
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetPrefabSprite = AssetGetPrefabSprite(	out flagEnableUpdate,
														ref flagIgnoreConfirmOverwrite,
														ref setting,
														pathAssetFull
												);
			if(null == assetPrefabSprite)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				Script_IndexColorShader_Sprite assetSprite = assetPrefabSprite.GetComponent<Script_IndexColorShader_Sprite>();
				UnityEngine.SpriteRenderer assetSpriteRenderer = assetPrefabSprite.GetComponent<UnityEngine.SpriteRenderer>();

				/* Set other assets: Script_IndexColorShader_Sprite */
				assetSprite.DataPalette = dataPalette;
				assetSprite.Texture = (null != material) ? (Texture2D)material.mainTexture : null;
				assetSprite.MaterialMaster = setting.Basic.MaterialSource;	// material;

				/* Set other assets: UnityEngine.SpriteRenderer */
				assetSpriteRenderer.material = material;
				assetSpriteRenderer.sprite = null;

				/* Dirty data */
				EditorUtility.SetDirty(assetPrefabSprite);
				AssetDatabase.SaveAssets();
			}

			return(true);
		}
		private static GameObject AssetGetPrefabSprite(	out bool flagEnableUpdate,
														ref bool flagIgnoreConfirmOverwrite,
														ref Setting setting,
														string pathAsset
													)
		{
			const string messageLogPrefix = "Asset-Create (Prefab-Sprite)";
			flagEnableUpdate = false;

			/* Check existing : GameObject & Script-Sprite */
			GameObject gameObjectSprite = AssetDatabase.LoadAssetAtPath<GameObject>(pathAsset);
			Script_IndexColorShader_Sprite assetPrefabSprite = null;
			if(null == gameObjectSprite)
			{	/* Not Exist (Create New) */
				/* Creagte Asset-Folder */
				string nameAssetFolder = "";
				string nameAssetFileBody = "";
				string nameAssetFileExtension = "";

				Utility.File.PathSplit(out nameAssetFolder, out nameAssetFileBody, out nameAssetFileExtension, pathAsset);
				if(true == string.IsNullOrEmpty(Utility.File.AssetFolderCreate(nameAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameAssetFolder + "\" could not be created");
					return(null);
				}

				/* Create GameObject */
				gameObjectSprite = new GameObject();
				assetPrefabSprite = gameObjectSprite.AddComponent<Script_IndexColorShader_Sprite>();
				if(null == assetPrefabSprite)
				{
					return(null);
				}

				/* Clean up component */
				AssetResetPrefabSpriteScript(assetPrefabSprite);
			}
			else
			{	/* Exist (Overwrite) */
				/* Check overwrite */
				if(false == Utility.File.PermissionGetConfirmDialogueOverwrite(	ref flagIgnoreConfirmOverwrite,
																				setting.ConfirmOverWrite.FlagPrefabUI,
																				pathAsset,
																				"Prefab-Sprite"
																			)
				)
				{	/* Not overwrite (Cancel) */
					/* MEMO: No message */
//					flagEnableUpdate = false;

					return(gameObjectSprite);
				}

				/* Instantiate & Update */
				GameObject gameObjectUIPrefab = gameObjectSprite;
				gameObjectSprite = UnityEngine.Object.Instantiate(gameObjectUIPrefab);
				assetPrefabSprite = gameObjectSprite.GetComponent<Script_IndexColorShader_Sprite>();
				if(null == assetPrefabSprite)
				{	/*  Component missing */
					assetPrefabSprite = gameObjectSprite.AddComponent<Script_IndexColorShader_Sprite>();

					/* Clean up component */
					AssetResetPrefabSpriteScript(assetPrefabSprite);
				}
			}

			/* Check existing : GameObject & Script-Sprite */
			UnityEngine.SpriteRenderer assetSpriteRenderer = gameObjectSprite.GetComponent<UnityEngine.SpriteRenderer>();
			if(null == assetSpriteRenderer)
			{
				assetSpriteRenderer = gameObjectSprite.AddComponent<UnityEngine.SpriteRenderer>();
				if(null == assetSpriteRenderer)
				{
					return(null);
				}

				AssetResetPrefabSpriteRenderer(assetSpriteRenderer);
			}

			/* Save assets */
			GameObject prefabUI = PrefabUtility.SaveAsPrefabAsset(gameObjectSprite, pathAsset);
			AssetDatabase.SaveAssets();

			/* Destroy Temporary */
			UnityEngine.Object.DestroyImmediate(gameObjectSprite);

			flagEnableUpdate = true;

			return(prefabUI);
		}
		private static void AssetResetPrefabSpriteScript(Script_IndexColorShader_Sprite scriptSprite)
		{
			scriptSprite.Interpolation = Library_IndexColorShader.KindInterpolation.LINEAR;
			scriptSprite.Texture = null;	/* Crear */
			scriptSprite.RectDraw = new Rect(Vector2.zero, -Vector2.one);	/* Rect.zero; */
			scriptSprite.RatePivot = new Vector2(0.5f, 0.5f);
			scriptSprite.PixelsPerUnit = 0.0f;
		}
		private static void AssetResetPrefabSpriteRenderer(UnityEngine.SpriteRenderer renderer)
		{
			/* MEMO: Now, nothing to set. */
		}

		internal static bool AssetOutputMaterialUI(	out Material assetMaterial,
													ref bool flagIgnoreConfirmOverwrite,
													ref Setting setting,
													string nameAsset,
													string nameOutputAssetFolderBase,
													Texture2D textureIndexed
												)
		{
			assetMaterial = null;

			if(false == setting.Basic.FlagCreatePrefabUI)
			{
				return(false);
			}

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderMaterialUI,
														setting.RuleNameAsset.NameGetAssetMaterialUI(nameAsset),
														ExtentionMaterial
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetMaterial = AssetGetMaterial(	out flagEnableUpdate,
												ref flagIgnoreConfirmOverwrite,
												pathAssetFull,
												setting.ConfirmOverWrite.FlagMaterialUI,
												setting.Basic.MaterialSourceUI,
												"Material-UI"
										);
			if(null == assetMaterial)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				/* Set Texture */
				assetMaterial.mainTexture = textureIndexed;

				/* Dirty data */
				EditorUtility.SetDirty(assetMaterial);
				AssetDatabase.SaveAssets();
			}

			return(true);
		}

		internal static bool AssetOutputPrefabUI(	out GameObject assetPrefabUI,
													ref bool flagIgnoreConfirmOverwrite,
													ref Setting setting,
													string nameAsset,
													string nameOutputAssetFolderBase,
													Material material,
													Script_IndexColorShader_Palette dataPalette
											)
		{
			assetPrefabUI = null;

			if(false == setting.Basic.FlagCreatePrefabUI)
			{
				return(true);
			}

			/* Get Assets Full-Path */
			string pathAssetFull = AssetPathGetFull(	nameOutputAssetFolderBase,
														setting.RuleNameAssetFolder.NameFolderPrefabUI,
														setting.RuleNameAsset.NameGetAssetPrefabUI(nameAsset),
														ExtentionPrefab
												);
			if(null == pathAssetFull)
			{
				return(false);
			}

			/* Get asset to save */
			bool flagEnableUpdate;
			assetPrefabUI = AssetGetPrefabUI(	out flagEnableUpdate,
												ref flagIgnoreConfirmOverwrite,
												ref setting,
												pathAssetFull
										);
			if(null == assetPrefabUI)
			{	/* Error */
				return(false);
			}

			/* Update asset */
			if(true == flagEnableUpdate)
			{
				Script_IndexColorShader_UIImage assetUIImage = assetPrefabUI.GetComponent<Script_IndexColorShader_UIImage>();

				/* Set other assets */
				assetUIImage.DataPalette = dataPalette;
				assetUIImage.material = material;

				/* Dirty data */
				EditorUtility.SetDirty(assetPrefabUI);
				AssetDatabase.SaveAssets();
			}

			return(true);
		}
		private static GameObject AssetGetPrefabUI(	out bool flagEnableUpdate,
													ref bool flagIgnoreConfirmOverwrite,
													ref Setting setting,
													string pathAsset
											)
		{
			const string messageLogPrefix = "Asset-Create (Prefab-UI)";
			flagEnableUpdate = false;

			/* Check existing */
			GameObject gameObjectUI = AssetDatabase.LoadAssetAtPath<GameObject>(pathAsset);
			Script_IndexColorShader_UIImage assetPrefabUI = null;
			if(null == gameObjectUI)
			{	/* Not Exist (Create New) */
				/* Creagte Asset-Folder */
				string nameAssetFolder = "";
				string nameAssetFileBody = "";
				string nameAssetFileExtension = "";

				Utility.File.PathSplit(out nameAssetFolder, out nameAssetFileBody, out nameAssetFileExtension, pathAsset);
				if(true == string.IsNullOrEmpty(Utility.File.AssetFolderCreate(nameAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameAssetFolder + "\" could not be created");
					return(null);
				}

				/* Create GameObject */
				gameObjectUI = new GameObject();
				assetPrefabUI = gameObjectUI.AddComponent<Script_IndexColorShader_UIImage>();
				if(null == assetPrefabUI)
				{
					return(null);
				}

				/* Clean up component */
				AssetResetPrefabUIScript(assetPrefabUI);
			}
			else
			{	/* Exist (Overwrite) */
				/* Check overwrite */
				if(false == Utility.File.PermissionGetConfirmDialogueOverwrite(	ref flagIgnoreConfirmOverwrite,
																				setting.ConfirmOverWrite.FlagPrefabUI,
																				pathAsset,
																				"Prefab-UI"
																			)
				)
				{	/* Not overwrite (Cancel) */
					/* MEMO: No message */
//					flagEnableUpdate = false;

					return(gameObjectUI);
				}

				/* Instantiate & Update */
				GameObject gameObjectUIPrefab = gameObjectUI;
				gameObjectUI = UnityEngine.Object.Instantiate(gameObjectUIPrefab);
				assetPrefabUI = gameObjectUI.GetComponent<Script_IndexColorShader_UIImage>();
				if(null == assetPrefabUI)
				{	/*  Component missing */
					assetPrefabUI = gameObjectUI.AddComponent<Script_IndexColorShader_UIImage>();

					/* Clean up component */
					AssetResetPrefabUIScript(assetPrefabUI);
				}
			}

			/* Save assets */
			GameObject prefabUI = PrefabUtility.SaveAsPrefabAsset(gameObjectUI, pathAsset);
			AssetDatabase.SaveAssets();

			/* Destroy Temporary */
			UnityEngine.Object.DestroyImmediate(gameObjectUI);

			flagEnableUpdate = true;

			return(prefabUI);
		}
		private static void AssetResetPrefabUIScript(Script_IndexColorShader_UIImage scriptUIImage)
		{
			scriptUIImage.color = Color.white;
			scriptUIImage.raycastTarget = true;
			scriptUIImage.maskable = true;
			scriptUIImage.Interpolation = Library_IndexColorShader.KindInterpolation.LINEAR;
			scriptUIImage.FlagHideForce = false;
			scriptUIImage.uvRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			scriptUIImage.SizeImageForce = -Vector2.one;
			scriptUIImage.StencilCompare = UnityEngine.Rendering.CompareFunction.Disabled;
			scriptUIImage.StencilID = 0;
		}

		private static string AssetPathGetFull(string nameBase, string nameFolderSub, string nameAsset, string extention)
		{
			if(true == string.IsNullOrEmpty(nameAsset))
			{	/* Error */
				return(null);
			}

			string pathAssetFull = nameBase;
			if(true == string.IsNullOrEmpty(pathAssetFull))
			{
				pathAssetFull = string.Empty;
			}
			if(false == pathAssetFull.EndsWith("/"))
			{
				pathAssetFull += "/";
			}

			if(false == string.IsNullOrEmpty(nameFolderSub))
			{
				pathAssetFull += nameFolderSub;
				if(false == pathAssetFull.EndsWith("/"))
				{
					pathAssetFull += "/";
				}
			}

			pathAssetFull += nameAsset;
			if(false == string.IsNullOrEmpty(extention))
			{
				pathAssetFull += extention;
			}

			return(pathAssetFull);
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		internal const string ExtentionPrefab = ".prefab";
		internal const string ExtentionScriptableObject = ".asset";
		internal const string ExtentionTextureIndexed = ".png";
		internal const string ExtentionMaterial = ".mat";
		internal const string ExtentionSprite = ".png";
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		#endregion Classes, Structs & Interfaces

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}

	public static partial class Utility
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class File
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static bool NamesGetFileDialogLoad(	out string nameDirectory,
														out string nameFileBody,
														out string nameFileExtension,
														string nameDirectoryPrevious,
														string textTitleDialog,
														string filterExtension
													)
			{
				if(true == string.IsNullOrEmpty(nameDirectoryPrevious))
				{
					nameDirectoryPrevious = "";
				}

				/* Choose file */
				string fileNameFullPath = EditorUtility.OpenFilePanel(textTitleDialog, nameDirectoryPrevious, filterExtension);
				if(0 == fileNameFullPath.Length)
				{	/* Cancelled */
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtension = "";

					return(false);
				}

				return(PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, fileNameFullPath));
			}

			public static bool NamesGetFileDialogSave(	out string nameDirectory,
														out string nameFileBody,
														out string nameFileExtension,
														string nameDirectoryPrevious,
														string nameFilePrevious,
														string textTitleDialog,
														string nameExtension
													)
			{
				/* Choose file */
				string fileNameFullPath = EditorUtility.SaveFilePanel(textTitleDialog, nameDirectoryPrevious, nameFilePrevious, nameExtension);
				if(0 == fileNameFullPath.Length)
				{	/* Cancelled */
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtension = "";

					return(false);
				}

				return(PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, fileNameFullPath));
			}

			internal static bool PermissionGetConfirmDialogueOverwrite(	ref bool flagIgnoewConfirm,
																		bool flagSwitchSetting,
																		string nameAsset,
																		string nameTypeAsset
																	)
			{
				if((false == flagSwitchSetting) || (true == flagIgnoewConfirm))
				{	/* No-Confirm */
					return(true);
				}

				bool rv = false;
				int KindResult = EditorUtility.DisplayDialogComplex(	"Asset already exists.",
																		"Do you want to overwrite?\n" + nameAsset,
																		"Yes",
																		"Yes, all \"" + nameTypeAsset +"\"s",
																		"No"
																	);
				switch(KindResult)
				{
					case 0:	/* Yes */
						rv = true;
						break;

					case 1:	/* All */
						flagIgnoewConfirm = false;
						rv = true;
						break;

					case 2:	/* No */
						rv = false;
						break;

				}

				return(rv);
			}

			public static string AssetFolderCreate(string namePath)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					return(null);
				}

				/* MEMO: Originally, way that should not take. Use "System.IO.Directory.CreateDirectory" to create folders. */
				string[] namePathSplit = namePath.Split(TextSplitFolder);
				int count = namePathSplit.Length;
				if(0 >= count)
				{
					return(null);
				}

				/* Reconstruct path */
				int indexTop = (NamePathRootAsset.ToLower() == namePathSplit[0].ToLower()) ? 1 : 0;
				string namePathAsset = string.Copy(NamePathRootAsset);
				string namePathNative = string.Copy(NamePathRootNative);
				string namePathChild = null;
				for(int i=indexTop; i<count; i++)
				{
					namePathChild = namePathSplit[i];
					if(false == string.IsNullOrEmpty(namePathChild))
					{
						namePathNative += "/" + namePathChild;
						namePathAsset += "/" + namePathChild;
					}
				}

				/* Create folder, if not exist. */
				if(false == System.IO.Directory.Exists(namePathNative))
				{
					System.IO.Directory.CreateDirectory(namePathNative);
				}

				namePathAsset += "/";
				return(namePathAsset);
			}

			public static string AssetPathGetSelected(string namePath=null)
			{
				string namePathAsset = "";
				if(true == string.IsNullOrEmpty(namePath))
				{	/* Now Selected Path in "Project" */
					Object objectNow = Selection.activeObject;
					if(null == objectNow)
					{	/* No Selected *//* Error */
						namePathAsset = null;
					}
					else
					{	/* Selected */
						namePathAsset = AssetDatabase.GetAssetPath(objectNow);
					}
				}
				else
				{	/* Specified */
					namePathAsset = System.String.Copy(namePath);
				}

				return(namePathAsset);
			}

			public static bool AssetCheckFolder(string namePath)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					return(false);
				}

				return(AssetDatabase.IsValidFolder(namePath));
			}

			public static bool FileCopyToAsset(string nameAsset, string nameOriginalFileName, bool flagOverCopy)
			{
				System.IO.File.Copy(nameOriginalFileName, nameAsset, flagOverCopy);
				return(true);
			}

			public static bool PathSplit(	out string nameDirectory,
											out string nameFileBody,
											out string nameFileExtention,
											string namePath
										)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtention = "";
					return(false);
				}

				string namePathNormalized = PathNormalize(namePath);
				nameDirectory = PathNormalize(System.IO.Path.GetDirectoryName(namePathNormalized) + "/");
				nameFileBody = System.IO.Path.GetFileNameWithoutExtension(namePathNormalized);
				nameFileExtention = System.IO.Path.GetExtension(namePathNormalized);

				return(true);
			}

			public static string PathNormalize(string namePath)
			{
				string namePathNew = namePath.Replace("\\", "/");	/* "\" -> "/" */
				return(namePathNew);
			}

			public static string PathGetAbsolute(string namePath, string nameBase)
			{
				string nameCurrent = System.Environment.CurrentDirectory;
				System.Environment.CurrentDirectory = nameBase;

				string rv = System.IO.Path.GetFullPath(namePath);
				rv = PathNormalize(rv);

				System.Environment.CurrentDirectory = nameCurrent;
				return(rv);
			}

			public static string PathGetAssetNative(string namePathAsset)
			{
				string namePathNative = string.Copy(NamePathRootNative);
				if(false == string.IsNullOrEmpty(namePathAsset))
				{
					namePathNative += "/" + namePathAsset.Substring(NamePathRootAsset.Length + 1);
					namePathNative = PathNormalize(namePathNative);
				}
				return(namePathNative);
			}

			public static bool PathCheckRoot(string namePath)
			{	/* MEMO: Create another function separately, since possibility that can not be checked with IsPathRooted. */
				return(System.IO.Path.IsPathRooted(namePath));
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private readonly static char[] TextSplitFolder = 
			{
				'/',
				'\\',
			};

			internal readonly static string NamePathRootNative = Application.dataPath;
			internal const string NamePathRootAsset = "Assets";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			#endregion Classes, Structs & Interfaces

			/* ----------------------------------------------- Delegates */
			#region Delegates
			#endregion Delegates
		}

		public static partial class Prefs
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static void StringSave(string prefsKey, string text)
			{
				string text64 = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(text));
				EditorPrefs.SetString(prefsKey, text64);
			}

			public static string StringLoad(string prefsKey, string textDefault)
			{
				string textDefault64 = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(textDefault));
				string text64 = EditorPrefs.GetString(prefsKey, textDefault64);
				return(System.Text.UTF8Encoding.UTF8.GetString(System.Convert.FromBase64String(text64)));
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

		public static partial class Asset
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static _Type AssetLoadPath<_Type>(string path)
				where _Type : class
			{
				_Type asset = AssetDatabase.LoadAssetAtPath(path, typeof(_Type)) as _Type;
				return(asset);
			}

			public static string PathGetAsset(UnityEngine.Object asset)
			{
				return(AssetDatabase.GetAssetPath(asset));
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

		public static partial class Log
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public static System.IO.StreamWriter StreamExternal = null;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static void Error(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "IndexColorShader Error: " + message;
				if(false == flagExternalOnly)
				{
					Debug.LogError(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Warning(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "IndexColorShader Warning: " + message;
				if(false == flagExternalOnly)
				{
					Debug.LogWarning(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Message(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "IndexColorShader Message: " + message;
				if(false == flagExternalOnly)
				{
					Debug.Log(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
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

		public static partial class Miscellaneous
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static void ProgressBarUpdate(string title, string nameTask, bool flagSwitch, int step, int stepFull)
			{
				if(false == flagSwitch)
				{
					return;
				}

				if((-1 == step) || (-1 == stepFull))
				{
					EditorUtility.ClearProgressBar();
					return;
				}

				EditorUtility.DisplayProgressBar(title, nameTask, ((float)step / (float)stepFull));
			}

			public static void GarbageCollect()
			{
				System.GC.Collect();
				System.GC.WaitForPendingFinalizers();
				System.GC.Collect();
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
		#endregion Classes, Structs & Interfaces

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
