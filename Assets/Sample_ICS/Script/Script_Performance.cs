/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Performance : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public int CountSprite;
	public GameObject GameObjectFullColor;
	public GameObject GameObjectPalette;
	public GameObject GameObjectPaletteIntegrated;
	public Script_IndexColorShader_Palette DataPaletteIntegrated;

	private KindMode Mode = KindMode.FULL_COLOR;
	private bool FlagNowRunning = false;

	private GameObject[] GameObjectRootDuplicate;

	private Library_IndexColorShader.Data.ControlMaterialPalette[] ControlMaterial = null;
	private MeshRenderer[] InstanceRenderer = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
//	void Start()
//	{
//	}

	void Update()
	{
		if(null == GameObjectRootDuplicate)
		{
			GameObjectRootDuplicate = new GameObject[CountSprite];
		}

		if(false == FlagNowRunning)
		{
			Mode = KindMode.FULL_COLOR;
			SpriteDuplicatate(GameObjectGetDuplicateSource(Mode));
		}
		FlagNowRunning = true;

		/* Check Mode-Change */
		if(true == Input.GetKeyDown(KeyCode.Return))
		{
			Mode = (KindMode)(((int)Mode + 1) % (int)KindMode.TERMINATOR);
			SpriteDuplicatate(GameObjectGetDuplicateSource(Mode));

			/* BootUp for each mode */
			switch(Mode)
			{
				case KindMode.FULL_COLOR:
					break;
				case KindMode.PALETTE:
					break;
				case KindMode.PALETTE_INTEGRATED:
					ModeBootUpPaletteIntegrated();
					break;
			}
		}

		/* Update for each mode */
		switch(Mode)
		{
			case KindMode.FULL_COLOR:
				break;
			case KindMode.PALETTE:
				break;
			case KindMode.PALETTE_INTEGRATED:
				ModeUpdatePaletteIntegrated();
				break;
		}
	}

	private void OnDisable()
	{
		FlagNowRunning = false;
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	internal void SpriteRelease()
	{
		if(null != GameObjectRootDuplicate)
		{
			for(int i=0; i<CountSprite; i++)
			{
				if(null != GameObjectRootDuplicate[i])
				{
					Destroy(GameObjectRootDuplicate[i]);
				}
				GameObjectRootDuplicate[i] = null;
			}
		}

		ControlMaterial = null;
		InstanceRenderer = null;
	}
	private GameObject GameObjectGetDuplicateSource(KindMode mode)
	{
		switch(mode)
		{
			case KindMode.FULL_COLOR:
				return(GameObjectFullColor);
			case KindMode.PALETTE:
				return(GameObjectPalette);
			case KindMode.PALETTE_INTEGRATED:
				return(GameObjectPaletteIntegrated);
		}

		return(null);
	}
	private void SpriteDuplicatate(GameObject gameObjectSource)
	{
		if(null == gameObjectSource)
		{
			return;
		}

		SpriteRelease();

		Vector3 position = new Vector3();
		for(int i=0; i<CountSprite; i++)
		{
			GameObjectRootDuplicate[i] = Instantiate(gameObjectSource);

			position.x = Random.Range(-640.0f, 640.0f);
			position.y = Random.Range(-360.0f, 360.0f);
			position.z = gameObjectSource.transform.localPosition.z;

			GameObjectRootDuplicate[i].transform.localPosition = position;

			GameObjectRootDuplicate[i].SetActive(true);
		}
	}

	private void ModeBootUpPaletteIntegrated()
	{
		if(null == GameObjectRootDuplicate)
		{
			return;
		}

		int countGameObject = GameObjectRootDuplicate.Length;
		if(0 >= countGameObject)
		{
			return;
		}

		ControlMaterial = new Library_IndexColorShader.Data.ControlMaterialPalette[countGameObject];
		InstanceRenderer = new MeshRenderer[countGameObject];
		GameObject gameObject;
		for(int i=0; i<countGameObject; i++)
		{
			gameObject = GameObjectRootDuplicate[i];
			if(null != gameObject)
			{
				InstanceRenderer[i] = gameObject.GetComponent<MeshRenderer>();
				ControlMaterial[i] = new Library_IndexColorShader.Data.ControlMaterialPalette();
				ControlMaterial[i].BootUp();
			}
		}
	}
	private void ModeUpdatePaletteIntegrated()
	{
		if((null == InstanceRenderer) || (null == ControlMaterial))
		{
			return;
		}
		if(null == DataPaletteIntegrated)
		{
			return;
		}

		int countGameObject = ControlMaterial.Length;
		Library_IndexColorShader.Data.ControlMaterialPalette controlMaterial;
		for(int i=0; i<countGameObject; i++)
		{
			controlMaterial = ControlMaterial[i];
			if(null != controlMaterial)
			{
				controlMaterial.Update(	InstanceRenderer[i],
										DataPaletteIntegrated.Color,
										Color.white,
										Library_IndexColorShader.Data.ControlMaterialPalette.KindInterpolation.NONE,
										null,
										null
									);
			}
		}
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private enum KindMode
	{
		FULL_COLOR = 0,
		PALETTE,
		PALETTE_INTEGRATED,

		TERMINATOR,
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
