/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_Animation : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public ControlRobot[] Robot;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
//	void Start()
//	{
//	}

	void Update()
	{
		if(null == Robot)
		{
			return;
		}

		int countRobot = Robot.Length;
		for(int i=0; i<countRobot; i++)
		{
			Robot[i].ActionUpdate();
			Robot[i].PaletteUpdate();
		}
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	[System.Serializable]
	public class ControlRobot
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public Script_Animation_Robo Instance;
		public Script_Animation_Robo.KindActionAnimation Action;
		public float RateSpeed;

		public Library_IndexColorShader.KindInterpolation Interpolation;
		public Vector2 Zoom;

		internal float RateBlend = 0.0f;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public void ActionUpdate()
		{
			if(null != Instance)
			{
				Instance.Action = Action;
			}
		}

		public void PaletteUpdate()
		{
			if(0.0f > RateSpeed)
			{
				RateBlend = 1.0f;
			}
			else
			{
				RateBlend += RateBlendUnit * RateSpeed;
				while(RateBlendLimit <= RateBlend)
				{
					RateBlend -= RateBlendLimit;
				}
				while(0.0f > RateBlend)
				{
					RateBlend += RateBlendLimit;
				}
			}

			float rateBlendPalette = RateBlend;
			if(RateBlendLimitSingle < rateBlendPalette)
			{
				rateBlendPalette = 1.0f - (rateBlendPalette - RateBlendLimitSingle);
			}

			if(null != Instance)
			{
				Instance.RateBlendOverride = rateBlendPalette;
				Instance.Interpolation = Interpolation;

				Vector2 scale = RateZoomUnit * Zoom;

				Instance.transform.localScale = new Vector3(scale.x, scale.y, 1.0f);
			}
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		private const float RateBlendUnit = 1.0f / 60.0f;
		private const float RateBlendLimitSingle = 1.0f;
		private const float RateBlendLimit = RateBlendLimitSingle * 2.0f;	/* Outward: 0.0f...1.0f / Return: 1.0f...2.0f */

		private readonly static Vector2 RateZoomUnit = new Vector2(150.0f * 2.0f, 150.0f * 2.0f);
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
