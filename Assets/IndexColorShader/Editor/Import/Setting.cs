/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
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
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupBasic(	bool flagCreateMaterial,
									string nameMaterialSource
								)
				{
					FlagCreateMaterial = flagCreateMaterial;
					MaterialSource = Utility.Asset.AssetLoadPath<Material>(nameMaterialSource);
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
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

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagCreateMaterial, FlagCreateMaterial);
					string text = string.Empty;
					if(null != MaterialSource)
					{
						text = Utility.Asset.PathGetAsset(MaterialSource);
					}
					Utility.Prefs.StringSave(PrefsKeyMaterialSource, text);

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

				private const string TextKeyPrefix = "Basic_";
				private const string TextKeyFlagCreateMaterial = TextKeyPrefix + KeyFlagCreateMaterial;
				private const string TextKeyMaterialSource = TextKeyPrefix + KeyMaterialSource;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagCreateMaterial = PrefsKeyPrefix + KeyFlagCreateMaterial;
				private const string PrefsKeyMaterialSource = PrefsKeyPrefix + KeyMaterialSource;

				private readonly static GroupBasic Default = new GroupBasic(
					true,																		/* FlagCreateMaterial */
					"Assets/IndexColorShader/Material/Default_Palette256" + ExtentionMaterial	/* MaterialSource */
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
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupConfirmOverWrite(	bool flagTexture,
												bool flagPalette,
												bool flagMaterial
											)
				{
					FlagTexture = flagTexture;
					FlagPalette = flagPalette;
					FlagMaterial = flagMaterial;
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

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagTexture, FlagTexture);
					EditorPrefs.SetBool(PrefsKeyFlagPalette, FlagPalette);
					EditorPrefs.SetBool(PrefsKeyFlagMaterial, FlagMaterial);

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagTexture = "FlagTexture";
				private const string KeyFlagPalette = "FlagPalette";
				private const string KeyFlagMaterial = "FlagMaterial";

				private const string TextKeyPrefix = "ConfirmOverWrite_";
				private const string TextKeyFlagTexture = TextKeyPrefix + KeyFlagTexture;
				private const string TextKeyFlagPalette = TextKeyPrefix + KeyFlagPalette;
				private const string TextKeyFlagMaterial = TextKeyPrefix + KeyFlagMaterial;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagTexture = PrefsKeyPrefix + KeyFlagTexture;
				private const string PrefsKeyFlagPalette = PrefsKeyPrefix + KeyFlagPalette;
				private const string PrefsKeyFlagMaterial = PrefsKeyPrefix + KeyFlagMaterial;

				internal readonly static GroupConfirmOverWrite Default = new GroupConfirmOverWrite(
					false,	/* FlagTexture */
					false,	/* FlagPalette */
					false	/* FlagMaterial */
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
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAsset(	string namePrefixTexture,
											string namePrefixPalette,
											string namePrefixMaterial
										)
				{
					NamePrefixTexture = namePrefixTexture;
					NamePrefixPalette = namePrefixPalette;
					NamePrefixMaterial = namePrefixMaterial;
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

					return(true);
				}

				public bool Save()
				{
					Utility.Prefs.StringSave(PrefsKeyNamePrefixTexture, NamePrefixTexture);
					Utility.Prefs.StringSave(PrefsKeyNamePrefixPalette, NamePrefixPalette);
					Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterial, NamePrefixMaterial);

					return(true);
				}

				public void Adjust()
				{
					NamePrefixTexture = Adjust(NamePrefixTexture);
					NamePrefixPalette = Adjust(NamePrefixPalette);
					NamePrefixMaterial = Adjust(NamePrefixMaterial);
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
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyNamePrefixTexture = "NamePrefixTexture";
				private const string KeyNamePrefixPalette = "NamePrefixPalette";
				private const string KeyNamePrefixMaterial = "NamePrefixMaterial";

				private const string TextKeyPrefix = "RuleNameAsset_";
				private const string TextKeyNamePrefixTexture = TextKeyPrefix + KeyNamePrefixTexture;
				private const string TextKeyNamePrefixPalette = TextKeyPrefix + KeyNamePrefixPalette;
				private const string TextKeyNamePrefixMaterial = TextKeyPrefix + KeyNamePrefixMaterial;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNamePrefixTexture = PrefsKeyPrefix + KeyNamePrefixTexture;
				private const string PrefsKeyNamePrefixPalette = PrefsKeyPrefix + KeyNamePrefixPalette;
				private const string PrefsKeyNamePrefixMaterial = PrefsKeyPrefix + KeyNamePrefixMaterial;

				private readonly static GroupRuleNameAsset Default = new GroupRuleNameAsset(
					/* namePrefixTexture */		"",
					/* namePrefixPalette */		"icsp_",
					/* namePrefixMaterial */	"icsm_"
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
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAssetFolder(	string nameFolderTexture,
													string nameFolderPalette,
													string nameFolderMaterial
												)
				{
					NameFolderTexture = nameFolderTexture;
					NameFolderPalette = nameFolderPalette;
					NameFolderMaterial = nameFolderMaterial;
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

					Adjust();

					return(true);
				}

				public bool Save()
				{
					Adjust();

					Utility.Prefs.StringSave(PrefsKeyNameFolderTexture, NameFolderTexture);
					Utility.Prefs.StringSave(PrefsKeyNameFolderPalette, NameFolderPalette);
					Utility.Prefs.StringSave(PrefsKeyNameFolderMaterial, NameFolderMaterial);

					return(true);
				}

				public void Adjust()
				{
					NameFolderTexture = Adjust(NameFolderTexture);
					NameFolderPalette = Adjust(NameFolderPalette);
					NameFolderMaterial = Adjust(NameFolderMaterial);
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

				private const string TextKeyPrefix = "RuleNameAssetFolder_";
				private const string TextKeyNameFolderTexture = TextKeyPrefix + KeyNameFolderTexture;
				private const string TextKeyNameFolderPalette = TextKeyPrefix + KeyNameFolderPalette;
				private const string TextKeyNameFolderMaterial = TextKeyPrefix + KeyNameFolderMaterial;

				private const string PrefsKeyPrefix = Import.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNameFolderTexture = PrefsKeyPrefix + KeyNameFolderTexture;
				private const string PrefsKeyNameFolderPalette = PrefsKeyPrefix + KeyNameFolderPalette;
				private const string PrefsKeyNameFolderMaterial = PrefsKeyPrefix + KeyNameFolderMaterial;

				private readonly static GroupRuleNameAssetFolder Default = new GroupRuleNameAssetFolder(
					"Texture",				/* NameFolderTexture */
					"Palette",				/* NameFolderPalette */
					"Material"				/* NameFolderMaterial */
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
