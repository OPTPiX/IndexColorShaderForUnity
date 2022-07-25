/**
	Index Color Shader for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Animation_Robo : Script_ColorChange_Robo
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindActionAnimation Action;
	private int StepAction = -1;
	private float TimeElapsed = 0.0f;
	private Vector4 TillingOffset;

	private KindActionAnimation ActionPrevious = KindActionAnimation.ERROR;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions-MonoBehaviour
//	void Start()
//	{
//	}

	new void Update()
	{
		/* ICS is Booted up ? */
		bool flagBootedUpNow;
		Library_IndexColorShader.Data.ControlMaterialPalette controlMaterialPalette = ControlGetMaterialPalette(out flagBootedUpNow);
		if((true == flagBootedUpNow) || (ActionPrevious != Action))
		{	/* Just after booted-up */
			if(null == DataPalette.Color)
			{
				return;
			}

			/* Duplicate Color-Table */
			Palette = Library_IndexColorShader.Palette.ColorCopyDeep(null, DataPalette.Color);

			/* Action Initialize */
			ActionPlay(Action);
		}
		else
		{
			TimeElapsed += Time.deltaTime;
		}

		/* Calculate Colors */
		if(null != DataPaletteOverride)
		{
			if(0 < DataPaletteOverride.Length)
			{
				int countColor = Palette.Length;
				Vector4 colorSource;
				Vector4 colorOverride;
				for(int i=0; i<countColor; i++)
				{
					colorSource = DataPalette.Color[i];
					colorOverride = DataPaletteOverride[i];
					if(Vector4.zero != colorOverride)
					{
						Palette[i] = (colorSource * (1.0f - RateBlendOverride)) + (colorOverride * RateBlendOverride);
					}
				}
			}
		}

		/* Update Action */
		ActionUpdate();

		/* Update Shader-Constants */
		ConstantsUpdateShader(	controlMaterialPalette,
								Interpolation,
								Palette,
								TillingOffset
							);
	}
	#endregion Functions-MonoBehaviour

	/* ----------------------------------------------- Functions */
	#region Functions
	private bool ActionPlay(KindActionAnimation action)
	{
		if(((KindActionAnimation)0 > action) || (KindActionAnimation.TERNIMATOR <= action))
		{
			return(false);
		}

		TimeElapsed = 0.0f;
		ActionPrevious = Action;
		Action = action;
		StepAction = -1;

		return(true);
	}

	private void ActionUpdate()
	{
		if(((KindActionAnimation)0 > Action) || (KindActionAnimation.TERNIMATOR <= Action))
		{
			return;
		}

		int stepActionNext = StepAction + 1;

		float timeFrameNext = DataActionAnimation[(int)Action][stepActionNext].Time;
		if(timeFrameNext > TimeElapsed)
		{
			return;
		}

		StepAction = stepActionNext;
		KindFrameAnimatrion frameAnimation = DataActionAnimation[(int)Action][stepActionNext].Animation;
		if(KindFrameAnimatrion.ERROR == frameAnimation)
		{
			TimeElapsed -= timeFrameNext;

			StepAction = 0;
			stepActionNext = 0;
			frameAnimation = DataActionAnimation[(int)Action][stepActionNext].Animation;
		}

		TillingOffset = DataTilingOffset[(int)frameAnimation];
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindFrameAnimatrion
	{
		ERROR = -1,

		STANDBY_00 = 0,
		STANDBY_01,
		STANDBY_02,
		STANDBY_03,
		STANDBY_04,
		STANDBY_05,
		PUNCH_00,
		PUNCH_01,
		PUNCH_02,
		PUNCH_03,
		PUNCH_04,
		PUNCH_05,
		KICK_00,
		KICK_01,
		KICK_02,
		KICK_03,
		KICK_04,
		KICK_05,
		KICK_06,
		KICK_07,
		KICK_08,
		KICK_09,
		KICK_10,
		KICK_11,
	}
	private readonly static Vector4[] DataTilingOffset = new Vector4[] {
		/* STANDBY_00 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		0.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* STANDBY_01 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		150.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* STANDBY_02 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		300.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* STANDBY_03 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		450.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* STANDBY_04 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		600.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* STANDBY_05 */	new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		750.0f / SizeTextureX,		1.0f - (0.0f / SizeTextureY) - (150.0f / SizeTextureY)		),
		/* PUNCH_00 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		0.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* PUNCH_01 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		150.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* PUNCH_02 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		300.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* PUNCH_03 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		450.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* PUNCH_04 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		600.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* PUNCH_05 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		750.0f / SizeTextureX,		1.0f - (150.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_00 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		0.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_01 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		150.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_02 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		300.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_03 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		450.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_04 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		600.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_05 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		750.0f / SizeTextureX,		1.0f - (300.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_06 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		0.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_07 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		150.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_08 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		300.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_09 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		450.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_10 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		600.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
		/* KICK_11 */		new Vector4(	150.0f / SizeTextureX,		150.0f / SizeTextureY,		750.0f / SizeTextureX,		1.0f - (450.0f / SizeTextureY) - (150.0f / SizeTextureY)	),
	};
	private const float SizeTextureX = 1024.0f;
	private const float SizeTextureY = 1024.0f;

	/* A */
	public enum KindActionAnimation
	{
		ERROR = -1,

		STANDBY = 0,
		PUNCH,
		KICK,

		TERNIMATOR,
	}
	private readonly static DataAnimationFrame[][] DataActionAnimation = new DataAnimationFrame[(int)KindActionAnimation.TERNIMATOR][] {
		new DataAnimationFrame[] {	/* STANDBY */
			new DataAnimationFrame(0.0f / AnimationFPS,		KindFrameAnimatrion.STANDBY_01	),
			new DataAnimationFrame(10.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_02	),
			new DataAnimationFrame(17.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_03	),
			new DataAnimationFrame(25.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_04	),
			new DataAnimationFrame(35.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_05	),
			new DataAnimationFrame(44.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_04	),
			new DataAnimationFrame(50.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_03	),
			new DataAnimationFrame(55.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_02	),
			new DataAnimationFrame(58.0f / AnimationFPS,	KindFrameAnimatrion.STANDBY_01	),
			new DataAnimationFrame(60.0f / AnimationFPS,	KindFrameAnimatrion.ERROR		),	/* Return To Top */
		},
		new DataAnimationFrame[] {	/* PUNCH */
			new DataAnimationFrame(0.0f / AnimationFPS,		KindFrameAnimatrion.STANDBY_00	),
			new DataAnimationFrame(8.0f / AnimationFPS,		KindFrameAnimatrion.PUNCH_01	),
			new DataAnimationFrame(15.0f / AnimationFPS,	KindFrameAnimatrion.PUNCH_04	),
			new DataAnimationFrame(19.0f / AnimationFPS,	KindFrameAnimatrion.PUNCH_03	),
			new DataAnimationFrame(28.0f / AnimationFPS,	KindFrameAnimatrion.PUNCH_02	),
			new DataAnimationFrame(33.0f / AnimationFPS,	KindFrameAnimatrion.PUNCH_01	),
			new DataAnimationFrame(37.0f / AnimationFPS,	KindFrameAnimatrion.PUNCH_00	),
			new DataAnimationFrame(40.0f / AnimationFPS,	KindFrameAnimatrion.ERROR		),	/* Return To Top */
		},
		new DataAnimationFrame[] {	/* KICK */
			new DataAnimationFrame(0.0f / AnimationFPS,		KindFrameAnimatrion.KICK_00		),
			new DataAnimationFrame(6.0f / AnimationFPS,		KindFrameAnimatrion.KICK_01		),
			new DataAnimationFrame(12.0f / AnimationFPS,	KindFrameAnimatrion.KICK_02		),
			new DataAnimationFrame(15.0f / AnimationFPS,	KindFrameAnimatrion.KICK_03		),
			new DataAnimationFrame(18.0f / AnimationFPS,	KindFrameAnimatrion.KICK_04		),
			new DataAnimationFrame(21.0f / AnimationFPS,	KindFrameAnimatrion.KICK_06		),
			new DataAnimationFrame(25.0f / AnimationFPS,	KindFrameAnimatrion.KICK_05		),
			new DataAnimationFrame(30.0f / AnimationFPS,	KindFrameAnimatrion.KICK_08		),
			new DataAnimationFrame(36.0f / AnimationFPS,	KindFrameAnimatrion.KICK_10		),
			new DataAnimationFrame(38.0f / AnimationFPS,	KindFrameAnimatrion.KICK_11		),
			new DataAnimationFrame(40.0f / AnimationFPS,	KindFrameAnimatrion.ERROR		),	/* Return To Top */
		}
	};
	private const float AnimationFPS = 60.0f;
	private const KindFrameAnimatrion KindFrameBase = KindFrameAnimatrion.STANDBY_00;
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public struct DataAnimationFrame
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public float Time;
		public KindFrameAnimatrion Animation;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public DataAnimationFrame(float time, KindFrameAnimatrion animation)
		{
			Time = time;
			Animation = animation;
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
