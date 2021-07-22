/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class MenuItem_IndexColorShader_ImportTexture : EditorWindow
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private static LibraryEditor_IndexColorShader.Import.Setting SettingImport;
	private static Setting SettingOption;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	[MenuItem("Tools/IndexColorShader/Importer")]
	static void OpenWindow()
	{
		EditorWindow.GetWindow<MenuItem_IndexColorShader_ImportTexture>(true, Library_IndexColorShader.SignatureNameAsset + " Import-Settings");
		SettingImport.CleanUp();
		SettingOption.CleanUp();
		SettingImport.Load();
		SettingOption.Load();
	}
	void OnGUI()
	{
		int levelIndent = EditorGUI.indentLevel;

		/* Fold Out: Commons */
		EditorGUILayout.Space();

		SettingOption.FlagFoldOutBasic = EditorGUILayout.Foldout(SettingOption.FlagFoldOutBasic, "Options: Basic");
		if(true == SettingOption.FlagFoldOutBasic)
		{
			FoldOutExecBasic(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.FlagFoldOutConfirmOverWrite = EditorGUILayout.Foldout(SettingOption.FlagFoldOutConfirmOverWrite, "Options: Overwrite Confirm");
		if(true == SettingOption.FlagFoldOutConfirmOverWrite)
		{
			FoldOutExecConfirmOverWrite(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		/* Button: Import */
		EditorGUILayout.Space();

		if(true == GUILayout.Button("Import"))
		{
			/* Single File Import */
			string nameBaseAssetPath = LibraryEditor_IndexColorShader.Utility.File.AssetPathGetSelected();
			if((false == string.IsNullOrEmpty(nameBaseAssetPath)) && (true == LibraryEditor_IndexColorShader.Utility.File.AssetCheckFolder(nameBaseAssetPath)))
			{
				string nameDirectory;
				string nameFileBody;
				string nameFileExtension;
				if(true == LibraryEditor_IndexColorShader.Utility.File.NamesGetFileDialogLoad(	out nameDirectory,
																								out nameFileBody,
																								out nameFileExtension,
																								SettingOption.NameFolderImportPrevious,
																								"Select Image-File to import",
																								"png"
																							)
					)
				{
					string nameFile = LibraryEditor_IndexColorShader.Utility.File.PathNormalize(nameDirectory + "/" + nameFileBody + nameFileExtension);
					SettingOption.NameFolderImportPrevious = nameDirectory;

					SettingOption.Save();
					SettingImport.Save();

					/* Import */
					if(false == LibraryEditor_IndexColorShader.Import.Exec(	ref SettingImport,
																			nameFile,
																			nameBaseAssetPath,
																			true
																		)
						)
					{
						EditorUtility.DisplayDialog(	Library_IndexColorShader.SignatureNameAsset,
														"Import Interrupted! Check Error on Console.",
												 		"OK"
												);
					}

					Close();
				}
			}
			else
			{	/* Error (No selected) */
				EditorUtility.DisplayDialog(	Library_IndexColorShader.SignatureNameAsset,
												"Select Asset-Folder you want to store in before import, on the \"Project\" window.",
												"OK"
										);
			}
		}

		/* Fold Out: Advanced Options */
		EditorGUILayout.Space();

		SettingOption.FlagFoldOutAdvancedOprions = EditorGUILayout.Foldout(SettingOption.FlagFoldOutAdvancedOprions, "Advanced Options");
		if(true == SettingOption.FlagFoldOutAdvancedOprions)
		{
			levelIndent++;
			EditorGUI.indentLevel = levelIndent;

			SettingOption.FlagFoldOutRuleNameAsset = EditorGUILayout.Foldout(SettingOption.FlagFoldOutRuleNameAsset, "Advanced Options: Naming Assets");
			if(true == SettingOption.FlagFoldOutRuleNameAsset)
			{
				FoldOutExecRuleNameAsset(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.FlagFoldOutRuleNameAssetFolder = EditorGUILayout.Foldout(SettingOption.FlagFoldOutRuleNameAssetFolder, "Advanced Options: Naming Asset-Foldes");
			if(true == SettingOption.FlagFoldOutRuleNameAssetFolder)
			{
				FoldOutExecRuleNameAssetFolder(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			levelIndent--;
			EditorGUI.indentLevel = levelIndent;
		}
	}
	private void FoldOutExecBasic(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Basic.FlagCreateMaterial = EditorGUILayout.ToggleLeft("Create Material", SettingImport.Basic.FlagCreateMaterial);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Create a material with textures and shaders assigned.");
		EditorGUI.indentLevel = levelIndent;

		if(true == SettingImport.Basic.FlagCreateMaterial)
		{
			EditorGUILayout.Space();
			SettingImport.Basic.MaterialSource = EditorGUILayout.ObjectField("Source Material", SettingImport.Basic.MaterialSource, typeof(Material), true) as Material;
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
	private void FoldOutExecConfirmOverWrite(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("Confirm when overwrite existing data.");
		EditorGUILayout.Space();

		SettingImport.ConfirmOverWrite.FlagTexture = EditorGUILayout.ToggleLeft("Textures", SettingImport.ConfirmOverWrite.FlagTexture);
		SettingImport.ConfirmOverWrite.FlagPalette = EditorGUILayout.ToggleLeft("Palette", SettingImport.ConfirmOverWrite.FlagPalette);
		SettingImport.ConfirmOverWrite.FlagMaterial = EditorGUILayout.ToggleLeft("Material", SettingImport.ConfirmOverWrite.FlagMaterial);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
	private void FoldOutExecRuleNameAsset(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("So that assets' name does not conflict when stored in AssetBundle,");
		EditorGUILayout.LabelField("    add the data identification string to top of file-name.");
		EditorGUILayout.LabelField("The prohibited characters are \":\", \"/\", \"\\\", \".\", \"*\", \"?\", Space and Tab.");
		EditorGUILayout.Space();

		SettingImport.RuleNameAsset.NamePrefixTexture = EditorGUILayout.TextField("Texture", SettingImport.RuleNameAsset.NamePrefixTexture);
		SettingImport.RuleNameAsset.NamePrefixPalette = EditorGUILayout.TextField("Palette", SettingImport.RuleNameAsset.NamePrefixPalette);
		SettingImport.RuleNameAsset.NamePrefixMaterial = EditorGUILayout.TextField("Material", SettingImport.RuleNameAsset.NamePrefixMaterial);

		SettingImport.RuleNameAsset.Adjust();

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
	private void FoldOutExecRuleNameAssetFolder(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("For each type to output, you can specify name of Asset-Folder.");
		EditorGUILayout.LabelField("The prohibited characters are \":\", \"/\", \"\\\", \".\", \"*\", \"?\", Space and Tab.");
		EditorGUILayout.Space();

		SettingImport.RuleNameAssetFolder.NameFolderTexture = EditorGUILayout.TextField("Texture", SettingImport.RuleNameAssetFolder.NameFolderTexture);
		SettingImport.RuleNameAssetFolder.NameFolderPalette = EditorGUILayout.TextField("Palette", SettingImport.RuleNameAssetFolder.NameFolderPalette);
		SettingImport.RuleNameAssetFolder.NameFolderMaterial = EditorGUILayout.TextField("Material", SettingImport.RuleNameAssetFolder.NameFolderMaterial);

		SettingImport.RuleNameAssetFolder.Adjust();

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	private struct Setting
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public string NameFolderImportPrevious;

		public bool FlagFoldOutBasic;
		public bool FlagFoldOutConfirmOverWrite;

		public bool FlagFoldOutAdvancedOprions;
		public bool FlagFoldOutRuleNameAsset;
		public bool FlagFoldOutRuleNameAssetFolder;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public Setting(	string nameFolderImportPrevious,
						bool flagFoldOutBasic,
						bool flagFoldOutConfirmOverWrite,
						bool flagFoldOutAdvancedOprions,
						bool flagFoldOutRuleNameAsset,
						bool flagFoldOutRuleNameAssetFolder
					)
		{
			NameFolderImportPrevious = nameFolderImportPrevious;

			FlagFoldOutBasic = flagFoldOutBasic;
			FlagFoldOutConfirmOverWrite = flagFoldOutConfirmOverWrite;

			FlagFoldOutAdvancedOprions = flagFoldOutAdvancedOprions;

			FlagFoldOutRuleNameAsset = flagFoldOutRuleNameAsset;
			FlagFoldOutRuleNameAssetFolder = flagFoldOutRuleNameAssetFolder;
		}

		public void CleanUp()
		{
			this = Default;
		}

		public bool Load()
		{
			NameFolderImportPrevious = LibraryEditor_IndexColorShader.Utility.Prefs.StringLoad(PrefsKeyNameFolderImportPrevious, Default.NameFolderImportPrevious);

			FlagFoldOutBasic = EditorPrefs.GetBool(PrefsKeyFlagFoldOutBasic, Default.FlagFoldOutBasic);
			FlagFoldOutConfirmOverWrite = EditorPrefs.GetBool(PrefsKeyFlagFoldOutConfirmOverWrite, Default.FlagFoldOutConfirmOverWrite);

			FlagFoldOutAdvancedOprions = EditorPrefs.GetBool(PrefsKeyFlagFoldOutAdvancedOprions, Default.FlagFoldOutAdvancedOprions);

			FlagFoldOutRuleNameAsset = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAsset, Default.FlagFoldOutRuleNameAsset);
			FlagFoldOutRuleNameAssetFolder = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, Default.FlagFoldOutRuleNameAssetFolder);

			return(true);
		}

		public bool Save()
		{
			LibraryEditor_IndexColorShader.Utility.Prefs.StringSave(PrefsKeyNameFolderImportPrevious, NameFolderImportPrevious);

			EditorPrefs.SetBool(PrefsKeyFlagFoldOutBasic, FlagFoldOutBasic);
			EditorPrefs.SetBool(PrefsKeyFlagFoldOutConfirmOverWrite, FlagFoldOutConfirmOverWrite);

			EditorPrefs.SetBool(PrefsKeyFlagFoldOutAdvancedOprions, FlagFoldOutAdvancedOprions);

			EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAsset, FlagFoldOutRuleNameAsset);
			EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, FlagFoldOutRuleNameAssetFolder);

			return(true);
		}
		#endregion Functions

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		private const string PrefsKeyPrefix = "ICS_ToolImporter_";
		private const string PrefsKeyNameFolderImportPrevious = PrefsKeyPrefix + "NameFolderImportPrevious";
		private const string PrefsKeyFlagFoldOutBasic = PrefsKeyPrefix + "FlagFoldOutBasic";
		private const string PrefsKeyFlagFoldOutConfirmOverWrite = PrefsKeyPrefix + "FlagFoldOutConfirmOverWrite";
		private const string PrefsKeyFlagFoldOutAdvancedOprions = PrefsKeyPrefix + "FlagFoldOutAdvancedOprions";
		private const string PrefsKeyFlagFoldOutRuleNameAsset = PrefsKeyPrefix + "FlagFoldOutRuleNameAsset";
		private const string PrefsKeyFlagFoldOutRuleNameAssetFolder = PrefsKeyPrefix + "FlagFoldOutRuleNameAssetFolder";

		public readonly static Setting Default = new Setting(	string.Empty,	/* NameFolderImportPrevious */
																true,			/* FlagFoldOutBasic */
																false,			/* FlagFoldOutConfirmOverWrite */
																false,			/* FlagFoldOutAdvancedOprions */
																false,			/* FlagFoldOutRuleNameAsset */
																false			/* FlagFoldOutRuleNameAssetFolder */
															);
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
