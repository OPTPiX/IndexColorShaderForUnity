/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Scene : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindMode[] ListMode;

	public GameObject GameObjectModeChangeColor;
	public GameObject GameObjectModeChangeColorB;
	public GameObject GameObjectModeAnimation;
	public GameObject GameObjectModePerformance;

	private int Mode = -1;
	private Script_ColorChange ControlModeColorChange = null;
	private Script_ColorChange ControlModeColorChangeB = null;
	private Script_Animation ControlModeAnimation = null;
	private Script_Performance ControlModePerformance = null;

	public static bool FlagIgnoreInput = true;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
//	void Start()
//	{
//	}

	void Update()
	{
		FlagIgnoreInput = false;

		if(null == ListMode)
		{
			return;
		}
		int countMode = ListMode.Length;
		if(0 >= countMode)
		{
			return;
		}

		if(0 > Mode)
		{	/* Not initialized */
			if(null != GameObjectModeChangeColor)
			{
				ControlModeColorChange = GameObjectModeChangeColor.GetComponent<Script_ColorChange>();
			}
			if(null != GameObjectModeChangeColorB)
			{
				ControlModeColorChangeB = GameObjectModeChangeColorB.GetComponent<Script_ColorChange>();
			}
			if(null != GameObjectModeAnimation)
			{
				ControlModeAnimation = GameObjectModeAnimation.GetComponent<Script_Animation>();
			}
			if(null != GameObjectModePerformance)
			{
				ControlModePerformance = GameObjectModePerformance.GetComponent<Script_Performance>();
			}

			Mode = 0;
			ModeSet(ListMode[Mode]);
		}

		if(false == FlagIgnoreInput)
		{
			/* Check Mode-Change */
			if(true == Input.GetKeyDown(KeyCode.Space))
			{
				int modeNew = ((int)Mode + 1) % countMode;
				ModeSet(ListMode[modeNew]);
				Mode = modeNew;
			}
		}
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	private void ModeSet(KindMode mode)
	{
		GameObject gameObjectStop00 = null;
		GameObject gameObjectStop01 = null;
		GameObject gameObjectStop02 = null;
		GameObject gameObjectRun = null;

		if(null != GameObjectModePerformance)
		{
			ControlModePerformance.SpriteRelease();
		}

		switch(mode)
		{
			case KindMode.COLOR_CHANGE:
				gameObjectStop00 = GameObjectModeChangeColorB;
				gameObjectStop01 = GameObjectModeAnimation;
				gameObjectStop02 = GameObjectModePerformance;
				gameObjectRun = GameObjectModeChangeColor;
				break;

			case KindMode.COLOR_CHANGE_B:
				gameObjectStop00 = GameObjectModeChangeColor;
				gameObjectStop01 = GameObjectModeAnimation;
				gameObjectStop02 = GameObjectModePerformance;
				gameObjectRun = GameObjectModeChangeColorB;
				break;

			case KindMode.ANIMATION:
				gameObjectStop00 = GameObjectModeChangeColor;
				gameObjectStop01 = GameObjectModeChangeColorB;
				gameObjectStop02 = GameObjectModePerformance;
				gameObjectRun = GameObjectModeAnimation;
				break;

			case KindMode.PERFORMANCE:
				gameObjectStop00 = GameObjectModeChangeColorB;
				gameObjectStop01 = GameObjectModeChangeColorB;
				gameObjectStop02 = GameObjectModeAnimation;
				gameObjectRun = GameObjectModePerformance;
				break;

			default:
				return;
		}

		if(null != gameObjectRun)
		{
			gameObjectRun.SetActive(true);
		}
		if(null != gameObjectStop00)
		{
			gameObjectStop00.SetActive(false);
		}
		if(null != gameObjectStop01)
		{
			gameObjectStop01.SetActive(false);
		}
		if(null != gameObjectStop02)
		{
			gameObjectStop02.SetActive(false);
		}

		FlagIgnoreInput = true;
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindMode
	{
		ERROR = -1,

		COLOR_CHANGE = 0,
		COLOR_CHANGE_B,
		ANIMATION,
		PERFORMANCE,

		TERMINATOR
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
