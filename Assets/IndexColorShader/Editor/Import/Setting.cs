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
	public static partial class Import
	{
		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		internal const string PrefsKeyPrefix = "ICS_ImportSetting_";	/* Common for all settings */
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public partial struct Setting
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public GroupBasic Basic;
			public GroupConfirmOverWrite ConfirmOverWrite;

			/* Advanced Options */
			public GroupRuleNameAsset RuleNameAsset;
			public GroupRuleNameAssetFolder RuleNameAssetFolder;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			/* MEMO: There is no constructor for initialization. */

			public void CleanUp()
			{
				Basic.CleanUp();
				ConfirmOverWrite.CleanUp();
				RuleNameAsset.CleanUp();
				RuleNameAssetFolder.CleanUp();
			}

			public bool Load()
			{
				Basic.Load();
				ConfirmOverWrite.Load();
				RuleNameAsset.Load();
				RuleNameAssetFolder.Load();

				return(true);
			}

			public bool Save()
			{
				Basic.Save();
				ConfirmOverWrite.Save();
				RuleNameAsset.Save();
				RuleNameAssetFolder.Save();

				return(true);
			}

			private static string NameNormalize(string nameBase)
			{
				string nameNormalize = nameBase;
				if(true == string.IsNullOrEmpty(nameNormalize))
				{
					nameNormalize = string.Empty;
				}
				else
				{
					nameNormalize = nameNormalize.Trim();

					/* Spaces */
					nameNormalize = nameNormalize.Replace(" ", "");
					nameNormalize = nameNormalize.Replace("\t", "");

					/* File & Hierarchy Delimiter */
					nameNormalize = nameNormalize.Replace(":", "");
					nameNormalize = nameNormalize.Replace("/", "");
					nameNormalize = nameNormalize.Replace("\\", "");
					nameNormalize = nameNormalize.Replace(".", "");
					nameNormalize = nameNormalize.Replace("*", "");
					nameNormalize = nameNormalize.Replace("?", "");

					/* Line-Feeds */
					nameNormalize = nameNormalize.Replace("\n", "");
					nameNormalize = nameNormalize.Replace("\r", "");

					/* Quotations */
					nameNormalize = nameNormalize.Replace("\"", "");
					nameNormalize = nameNormalize.Replace("\'", "");
				}
				return(nameNormalize);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const string TextKeyPrefix = "";
//			private const string TextKeyDummy = TextKeyPrefix + KeyDummy;

//			private const string PrefsKeyDummy = Import.PrefsKeyPrefix + TextKeyDummy;
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public struct GroupBasic
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagCreateMaterial;
				public Material MaterialSource;

				public bool FlagCreatePrefabSprite;

				public bool FlagCreatePrefabUI;
				public Material MaterialSourceUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupBasic(	bool flagCreateMaterial,
									string nameMaterialSource,
									bool flagCreatePrefabSprite,
									bool flagCreatePrefabUI,
									string nameMaterialSourcePrefabUI
								)
				{
					FlagCreateMaterial = flagCreateMaterial;
					MaterialSource = Utility.Asset.AssetLoadPath<Material>(nameMaterialSource);

					FlagCreatePrefabSprite = flagCreatePrefabSprite;

					FlagCreatePrefabUI = flagCreatePrefabUI;
					MaterialSourceUI = Utility.Asset.AssetLoadPath<Material>(nameMaterialSourcePrefabUI);
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					{	/* "Create Material" */
						FlagCreateMaterial = EditorPrefs.GetBool(PrefsKeyFlagCreateMaterial, Default.FlagCreateMaterial);
						string textDefault = Utility.Asset.PathGetAsset(Default.MaterialSource);
						string text = Utility.Prefs.StringLoad(PrefsKeyMaterialSource, textDefault);
						if(true == string.IsNullOrEmpty(text))
						{
							MaterialSource = null;
						}
						else
						{
							MaterialSource = Utility.Asset.AssetLoadPath<Material>(text);
						}
					}

					{	/* "Create PrefabSprite"  */
						FlagCreatePrefabSprite = EditorPrefs.GetBool(PrefsKeyFlagCreatePrefabSprite, Default.FlagCreatePrefabSprite);
					}

					{	/* "Create Prefab-UI" */
						FlagCreatePrefabUI = EditorPrefs.GetBool(PrefsKeyFlagCreatePrefabUI, Default.FlagCreatePrefabUI);
						string textDefault = Utility.Asset.PathGetAsset(Default.MaterialSourceUI);
						string text = Utility.Prefs.StringLoad(PrefsKeyMaterialSourcePrefabUI, textDefault);
						if(true == string.IsNullOrEmpty(text))
						{
							MaterialSourceUI = null;
						}
						else
						{
							MaterialSourceUI = Utility.Asset.AssetLoadPath<Material>(text);
						}
					}


					return(true);
				}

				public bool Save()
				{
					{	/* "Create Material" */
						EditorPrefs.SetBool(PrefsKeyFlagCreateMaterial, FlagCreateMaterial);
						string text = string.Empty;
						if(null != MaterialSource)
						{
							text = Utility.Asset.PathGetAsset(MaterialSource);
						}
						Utility.Prefs.StringSave(PrefsKeyMaterialSource, text);
					}

					{	/* "Create PrefabSprite"  */
						EditorPrefs.SetBool(PrefsKeyFlagCreatePrefabSprite, FlagCreatePrefabSprite);
					}

					{	/* "Create Prefab-UI" */
						EditorPrefs.SetBool(PrefsKeyFlagCreatePrefabUI, FlagCreatePrefabUI);
						string text = string.Empty;
						if(null != MaterialSourceUI)
						{
							text = Utility.Asset.PathGetAsset(MaterialSourceUI);
						}
						Utility.Prefs.StringSave(PrefsKeyMaterialSourcePrefabUI, text);
					}

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindNoCreateMaterialUnreferenced
				{
					NONE = 0,
					BLENDING,
					BLENDING_CELLMAP,

					TERMINATOR
				}

				private const string KeyFlagCreateMaterial = "FlagCreateMaterial";
				private const string KeyMaterialSource = "MaterialSource";
				private const string KeyFlagCreatePrefabSprite = "FlagCreatePrefabSprite";
				private const string KeyFlagCreatePrefabUI = "FlagCreatePrefabUI";
				private const string KeyMaterialSourcePrefabUI = "MaterialSourcePrefabUI";

				private const string TextKeyPrefix = "Basic_";
				private const string TextKeyFlagCreateMaterial = TextKeyPrefix + KeyFlagCreateMaterial;
				private const string TextKeyMaterialSource = TextKeyPrefix + KeyMaterialSource;
				private const string TextKeyFlagCreatePrefabSprite = TextKeyPrefix + KeyFlagCreatePrefabSprite;
				private const string TextKeyFlagCreatePrefabUI = TextKeyPrefix + KeyFlagCreatePrefabUI;
				private const string TextKeyMaterialSourcePrefabUI = TextKeyPrefix + KeyMaterialSourcePrefabUI;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagCreateMaterial = PrefsKeyPrefix + KeyFlagCreateMaterial;
				private const string PrefsKeyMaterialSource = PrefsKeyPrefix + KeyMaterialSource;
				private const string PrefsKeyFlagCreatePrefabSprite = PrefsKeyPrefix + KeyFlagCreatePrefabSprite;
				private const string PrefsKeyFlagCreatePrefabUI = PrefsKeyPrefix + KeyFlagCreatePrefabUI;
				private const string PrefsKeyMaterialSourcePrefabUI = PrefsKeyPrefix + KeyMaterialSourcePrefabUI;

				private readonly static GroupBasic Default = new GroupBasic(
					true,																			/* FlagCreateMaterial */
					"Assets/IndexColorShader/Material/Default_Palette256" + ExtentionMaterial,		/* MaterialSource */
					true,																			/* FlagCreatePrefabSprite */
					true,																			/* FlagCreatePrefabUI */
					"Assets/IndexColorShader/Material/Default_UIPalette256" + ExtentionMaterial		/* MaterialSourceUI */
				);
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces

				/* ----------------------------------------------- Delegates */
				#region Delegates
				#endregion Delegates
			}

			public struct GroupConfirmOverWrite
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagTexture;
				public bool FlagPalette;
				public bool FlagMaterial;

				public bool FlagPrefabSprite;

				public bool FlagPrefabUI;
				public bool FlagMaterialUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupConfirmOverWrite(	bool flagTexture,
												bool flagPalette,
												bool flagMaterial,
												bool flagPrefabSprite,
												bool flagPrefabUI,
												bool flagMaterialUI
											)
				{
					FlagTexture = flagTexture;
					FlagPalette = flagPalette;
					FlagMaterial = flagMaterial;

					FlagPrefabSprite = flagPrefabSprite;

					FlagPrefabUI = flagPrefabUI;
					FlagMaterialUI = flagMaterialUI;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagTexture = EditorPrefs.GetBool(PrefsKeyFlagTexture, Default.FlagTexture);
					FlagPalette = EditorPrefs.GetBool(PrefsKeyFlagPalette, Default.FlagPalette);
					FlagMaterial = EditorPrefs.GetBool(PrefsKeyFlagMaterial, Default.FlagMaterial);

					FlagPrefabSprite = EditorPrefs.GetBool(PrefsKeyFlagPrefabSprite, Default.FlagPrefabSprite);

					FlagPrefabUI = EditorPrefs.GetBool(PrefsKeyFlagPrefabUI, Default.FlagPrefabUI);
					FlagMaterialUI = EditorPrefs.GetBool(PrefsKeyFlagMaterialUI, Default.FlagMaterialUI);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagTexture, FlagTexture);
					EditorPrefs.SetBool(PrefsKeyFlagPalette, FlagPalette);
					EditorPrefs.SetBool(PrefsKeyFlagMaterial, FlagMaterial);

					EditorPrefs.SetBool(PrefsKeyFlagPrefabSprite, FlagPrefabSprite);

					EditorPrefs.SetBool(PrefsKeyFlagPrefabUI, FlagPrefabUI);
					EditorPrefs.SetBool(PrefsKeyFlagMaterialUI, FlagMaterialUI);

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagTexture = "FlagTexture";
				private const string KeyFlagPalette = "FlagPalette";
				private const string KeyFlagMaterial = "FlagMaterial";
				private const string KeyFlagPrefabSprite = "FlagPrefabSprite";
				private const string KeyFlagPrefabUI = "FlagPrefabUI";
				private const string KeyFlagMaterialUI = "FlagMaterialUI";

				private const string TextKeyPrefix = "ConfirmOverWrite_";
				private const string TextKeyFlagTexture = TextKeyPrefix + KeyFlagTexture;
				private const string TextKeyFlagPalette = TextKeyPrefix + KeyFlagPalette;
				private const string TextKeyFlagMaterial = TextKeyPrefix + KeyFlagMaterial;
				private const string TextKeyFlagPrefabSprite = TextKeyPrefix + KeyFlagPrefabSprite;
				private const string TextKeyFlagPrefabUI = TextKeyPrefix + KeyFlagPrefabUI;
				private const string TextKeyFlagMaterialUI = TextKeyPrefix + KeyFlagMaterialUI;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagTexture = PrefsKeyPrefix + KeyFlagTexture;
				private const string PrefsKeyFlagPalette = PrefsKeyPrefix + KeyFlagPalette;
				private const string PrefsKeyFlagMaterial = PrefsKeyPrefix + KeyFlagMaterial;
				private const string PrefsKeyFlagPrefabSprite = PrefsKeyPrefix + KeyFlagPrefabSprite;
				private const string PrefsKeyFlagPrefabUI = PrefsKeyPrefix + KeyFlagPrefabUI;
				private const string PrefsKeyFlagMaterialUI = PrefsKeyPrefix + KeyFlagMaterialUI;

				internal readonly static GroupConfirmOverWrite Default = new GroupConfirmOverWrite(
					false,	/* FlagTexture */
					false,	/* FlagPalette */
					false,	/* FlagMaterial */
					false,	/* FlagPrefabSprite */
					false,	/* FlagPrefabUI */
					false	/* FlagMaterialUI */
				);
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces

				/* ----------------------------------------------- Delegates */
				#region Delegates
				#endregion Delegates
			}

			public struct GroupRuleNameAsset
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				/* Prefix */
				public string NamePrefixTexture;
				public string NamePrefixPalette;
				public string NamePrefixMaterial;

				public string NamePrefixPrefabSprite;

				public string NamePrefixPrefabUI;
				public string NamePrefixMaterialUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAsset(	string namePrefixTexture,
											string namePrefixPalette,
											string namePrefixMaterial,
											string namePrefixPrefabSprite,
											string namePrefixPrefabUI,
											string namePrefixMaterialUI
										)
				{
					NamePrefixTexture = namePrefixTexture;
					NamePrefixPalette = namePrefixPalette;
					NamePrefixMaterial = namePrefixMaterial;

					NamePrefixPrefabSprite = namePrefixPrefabSprite;

					NamePrefixPrefabUI = namePrefixPrefabUI;
					NamePrefixMaterialUI = namePrefixMaterialUI;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					NamePrefixTexture = Utility.Prefs.StringLoad(PrefsKeyNamePrefixTexture, Default.NamePrefixTexture);
					NamePrefixPalette = Utility.Prefs.StringLoad(PrefsKeyNamePrefixPalette, Default.NamePrefixPalette);
					NamePrefixMaterial = Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterial, Default.NamePrefixMaterial);

					NamePrefixPrefabSprite = Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabSprite, Default.NamePrefixPrefabSprite);

					NamePrefixPrefabUI = Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabUI, Default.NamePrefixPrefabUI);
					NamePrefixMaterialUI = Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterialUI, Default.NamePrefixMaterialUI);

					return(true);
				}

				public bool Save()
				{
					Utility.Prefs.StringSave(PrefsKeyNamePrefixTexture, NamePrefixTexture);
					Utility.Prefs.StringSave(PrefsKeyNamePrefixPalette, NamePrefixPalette);
					Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterial, NamePrefixMaterial);

					Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabSprite, NamePrefixPrefabSprite);

					Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabUI, NamePrefixPrefabUI);
					Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterialUI, NamePrefixMaterialUI);

					return(true);
				}

				public void Adjust()
				{
					NamePrefixTexture = Adjust(NamePrefixTexture);
					NamePrefixPalette = Adjust(NamePrefixPalette);
					NamePrefixMaterial = Adjust(NamePrefixMaterial);

					NamePrefixPrefabSprite = Adjust(NamePrefixPrefabSprite);

					NamePrefixPrefabUI = Adjust(NamePrefixPrefabUI);
					NamePrefixMaterialUI = Adjust(NamePrefixMaterialUI);
				}
				private static string Adjust(string text)
				{
					return(Setting.NameNormalize(text));
				}

				public string NameGetAssetTexture(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixTexture))
					{
						nameNormalize = NamePrefixTexture + nameNormalize;
					}
					return(nameNormalize);
				}
				public string NameGetAssetPalette(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixPalette))
					{
						nameNormalize = NamePrefixPalette + nameNormalize;
					}
					return(nameNormalize);
				}
				public string NameGetAssetMaterial(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixMaterial))
					{
						nameNormalize = NamePrefixMaterial + nameNormalize;
					}
					return(nameNormalize);
				}
				public string NameGetAssetPrefabSprite(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixPrefabSprite))
					{
						nameNormalize = NamePrefixPrefabSprite + nameNormalize;
					}
					return(nameNormalize);
				}
				public string NameGetAssetPrefabUI(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixPrefabUI))
					{
						nameNormalize = NamePrefixPrefabUI + nameNormalize;
					}
					return(nameNormalize);
				}
				public string NameGetAssetMaterialUI(string nameBase)
				{
					string nameNormalize = nameBase;
					if(false == string.IsNullOrEmpty(NamePrefixMaterialUI))
					{
						nameNormalize = NamePrefixMaterialUI + nameNormalize;
					}
					return(nameNormalize);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyNamePrefixTexture = "NamePrefixTexture";
				private const string KeyNamePrefixPalette = "NamePrefixPalette";
				private const string KeyNamePrefixMaterial = "NamePrefixMaterial";
				private const string KeyNamePrefixPrefabSprite = "NamePrefixPrefabSprite";
				private const string KeyNamePrefixPrefabUI = "NamePrefixPrefabUI";
				private const string KeyNamePrefixMaterialUI = "NamePrefixMaterialUI";

				private const string TextKeyPrefix = "RuleNameAsset_";
				private const string TextKeyNamePrefixTexture = TextKeyPrefix + KeyNamePrefixTexture;
				private const string TextKeyNamePrefixPalette = TextKeyPrefix + KeyNamePrefixPalette;
				private const string TextKeyNamePrefixMaterial = TextKeyPrefix + KeyNamePrefixMaterial;
				private const string TextKeyNamePrefixPrefabSprite = TextKeyPrefix + KeyNamePrefixPrefabSprite;
				private const string TextKeyNamePrefixPrefabUI = TextKeyPrefix + KeyNamePrefixPrefabUI;
				private const string TextKeyNamePrefixMaterialUI = TextKeyPrefix + KeyNamePrefixMaterialUI;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNamePrefixTexture = PrefsKeyPrefix + KeyNamePrefixTexture;
				private const string PrefsKeyNamePrefixPalette = PrefsKeyPrefix + KeyNamePrefixPalette;
				private const string PrefsKeyNamePrefixMaterial = PrefsKeyPrefix + KeyNamePrefixMaterial;
				private const string PrefsKeyNamePrefixPrefabSprite = PrefsKeyPrefix + KeyNamePrefixPrefabSprite;
				private const string PrefsKeyNamePrefixPrefabUI = PrefsKeyPrefix + KeyNamePrefixPrefabUI;
				private const string PrefsKeyNamePrefixMaterialUI = PrefsKeyPrefix + KeyNamePrefixMaterialUI;

				private readonly static GroupRuleNameAsset Default = new GroupRuleNameAsset(
					/* NamePrefixTexture */			"",
					/* NamePrefixPalette */			"icsp_",
					/* NamePrefixMaterial */		"icsm_",
					/* NamePrefixPrefabSprite */	"icss_",
					/* NamePrefixPrefabUI */		"icsui_",
					/* NamePrefixMaterialUI */		"icsum_"
				);
				#endregion Enums & Constants
			}

			public struct GroupRuleNameAssetFolder
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				/* Folder Names (Common) */
				public string NameFolderTexture;
				public string NameFolderPalette;
				public string NameFolderMaterial;

				public string NameFolderPrefabSprite;

				public string NameFolderPrefabUI;
				public string NameFolderMaterialUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAssetFolder(	string nameFolderTexture,
													string nameFolderPalette,
													string nameFolderMaterial,
													string nameFolderPrefabSprite,
													string nameFolderPrefabUI,
													string nameFolderMaterialUI
												)
				{
					NameFolderTexture = nameFolderTexture;
					NameFolderPalette = nameFolderPalette;
					NameFolderMaterial = nameFolderMaterial;

					NameFolderPrefabSprite = nameFolderPrefabSprite;

					NameFolderPrefabUI = nameFolderPrefabUI;
					NameFolderMaterialUI = nameFolderMaterialUI;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					NameFolderTexture = Utility.Prefs.StringLoad(PrefsKeyNameFolderTexture, Default.NameFolderTexture);
					NameFolderPalette = Utility.Prefs.StringLoad(PrefsKeyNameFolderPalette, Default.NameFolderPalette);
					NameFolderMaterial = Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterial, Default.NameFolderMaterial);

					NameFolderPrefabSprite = Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabSprite, Default.NameFolderPrefabSprite);

					NameFolderPrefabUI = Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabUI, Default.NameFolderPrefabUI);
					NameFolderMaterialUI = Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterialUI, Default.NameFolderMaterialUI);

					Adjust();

					return(true);
				}

				public bool Save()
				{
					Adjust();

					Utility.Prefs.StringSave(PrefsKeyNameFolderTexture, NameFolderTexture);
					Utility.Prefs.StringSave(PrefsKeyNameFolderPalette, NameFolderPalette);
					Utility.Prefs.StringSave(PrefsKeyNameFolderMaterial, NameFolderMaterial);

					Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabSprite, NameFolderPrefabSprite);

					Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabUI, NameFolderPrefabUI);
					Utility.Prefs.StringSave(PrefsKeyNameFolderMaterialUI, NameFolderMaterialUI);

					return(true);
				}

				public void Adjust()
				{
					NameFolderTexture = Adjust(NameFolderTexture);
					NameFolderPalette = Adjust(NameFolderPalette);
					NameFolderMaterial = Adjust(NameFolderMaterial);

					NameFolderPrefabSprite = Adjust(NameFolderPrefabSprite);

					NameFolderPrefabUI = Adjust(NameFolderPrefabUI);
					NameFolderMaterialUI = Adjust(NameFolderMaterialUI);
				}
				private static string Adjust(string text)
				{
					return(Setting.NameNormalize(text));
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyNameFolderTexture = "NameFolderTexture";
				private const string KeyNameFolderPalette = "NameFolderPalette";
				private const string KeyNameFolderMaterial = "NameFolderMaterial";
				private const string KeyNameFolderPrefabSprite = "NameFolderPrefabSprite";
				private const string KeyNameFolderPrefabUI = "NameFolderPrefabUI";
				private const string KeyNameFolderMaterialUI = "NameFolderMaterialUI";

				private const string TextKeyPrefix = "RuleNameAssetFolder_";
				private const string TextKeyNameFolderTexture = TextKeyPrefix + KeyNameFolderTexture;
				private const string TextKeyNameFolderPalette = TextKeyPrefix + KeyNameFolderPalette;
				private const string TextKeyNameFolderMaterial = TextKeyPrefix + KeyNameFolderMaterial;
				private const string TextKeyNameFolderPrefabSprite = TextKeyPrefix + KeyNameFolderPrefabSprite;
				private const string TextKeyNameFolderPrefabUI = TextKeyPrefix + KeyNameFolderPrefabUI;
				private const string TextKeyNameFolderMaterialUI = TextKeyPrefix + KeyNameFolderMaterialUI;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNameFolderTexture = PrefsKeyPrefix + KeyNameFolderTexture;
				private const string PrefsKeyNameFolderPalette = PrefsKeyPrefix + KeyNameFolderPalette;
				private const string PrefsKeyNameFolderMaterial = PrefsKeyPrefix + KeyNameFolderMaterial;
				private const string PrefsKeyNameFolderPrefabSprite = PrefsKeyPrefix + KeyNameFolderPrefabSprite;
				private const string PrefsKeyNameFolderPrefabUI = PrefsKeyPrefix + KeyNameFolderPrefabUI;
				private const string PrefsKeyNameFolderMaterialUI = PrefsKeyPrefix + KeyNameFolderMaterialUI;

				private readonly static GroupRuleNameAssetFolder Default = new GroupRuleNameAssetFolder(
					"Texture",				/* NameFolderTexture */
					"Palette",				/* NameFolderPalette */
					"Material",				/* NameFolderMaterial */
					"PrefabSprite",			/* NameFolderPrefabSprite */
					"PrefabUI",				/* NameFolderPrefabUI */
					"MaterialUI"			/* NameFolderMaterialUI */
				);
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
	}
}
